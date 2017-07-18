using tvn.cosine.ai.agent;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.util;

namespace tvn.cosine.ai.environment.wumpusworld
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 270.<br>
     * <br>
     *
     * <pre>
     * <code>
     * function HYBRID-WUMPUS-AGENT(percept) returns an action
     *   inputs: percept, a list, [stench, breeze, glitter, bump, scream]
     *   persistent: KB, a knowledge base, initially the temporal "wumpus physics"
     *               t, a counter, initially 0, indicating time
     *               plan, an action sequence, initially empty
     *
     *   TELL(KB, MAKE-PERCEPT-SENTENCE(percept, t))
     *   TELL the KB the temporal "physics" sentences for time t
     *   safe <- {[x, y] : ASK(KB, OK<sup>t</sup><sub>x,y</sub>) = true}
     *   if ASK(KB, Glitter<sup>t</sup>) = true then
     *      plan <- [Grab] + PLAN-ROUTE(current, {[1,1]}, safe) + [Climb]
     *   if plan is empty then
     *      unvisited <- {[x, y] : ASK(KB, L<sup>t'</sup><sub>x,y</sub>) = false for all t' &le; t}
     *      plan <- PLAN-ROUTE(current, unvisited &cap; safe, safe)
     *   if plan is empty and ASK(KB, HaveArrow<sup>t</sup>) = true then
     *      possible_wumpus <- {[x, y] : ASK(KB, ~W<sub>x,y</sub>) = false}
     *      plan <- PLAN-SHOT(current, possible_wumpus, safe)
     *   if plan is empty then //no choice but to take a risk
     *      not_unsafe <- {[x, y] : ASK(KB, ~OK<sup>t</sup><sub>x,y</sub>) = false}
     *      plan <- PLAN-ROUTE(current, unvisited &cap; not_unsafe, safe)
     *   if plan is empty then
     *      plan <- PLAN-ROUTE(current, {[1,1]}, safe) + [Climb]
     *   action <- POP(plan)
     *   TELL(KB, MAKE-ACTION-SENTENCE(action, t))
     *   t <- t+1
     *   return action
     *
     * --------------------------------------------------------------------------------
     *
     * function PLAN-ROUTE(current, goals, allowed) returns an action sequence
     *   inputs: current, the agent's current position
     *           goals, a set of squares; try to plan a route to one of them
     *           allowed, a set of squares that can form part of the route
     *
     *   problem <- ROUTE-PROBLEM(current, goals, allowed)
     *   return A*-GRAPH-SEARCH(problem)
     * </code>
     * </pre>
     *
     * Figure 7.20 A hybrid agent program for the wumpus world. It uses a
     * propositional knowledge base to infer the state of the world, and a
     * combination of problem-solving search and domain-specific code to decide what
     * actions to take.<br><br>
     *
     * This is a tuned version of the {@link HybridWumpusAgent}. It uses a model cave
     * not only for routing but also for position and visited location tracking.
     * The knowledge base grows significant slower than in the original version
     * and response times are much faster.
     *
     * @author Ruediger Lunde
     */
    public class EfficientHybridWumpusAgent : HybridWumpusAgent
    {
        private WumpusCave modelCave;
        private ISet<Room> visitedRooms = Factory.CreateSet<Room>();

        public EfficientHybridWumpusAgent(int caveXDim, int caveYDim, AgentPosition start)
                : this(caveXDim, caveYDim, start, new DPLLSatisfiable(), null)
        { }

        public EfficientHybridWumpusAgent(int caveXDim, int caveYDim, AgentPosition start,
            DPLL satSolver,
            EnvironmentViewNotifier notifier)
            : this(caveXDim, caveYDim, start,
                new WumpusKnowledgeBase(caveXDim, caveYDim, start, satSolver), notifier)
        { }

        public EfficientHybridWumpusAgent(int caveXDim, int caveYDim,
            AgentPosition start, WumpusKnowledgeBase kb,
            EnvironmentViewNotifier notifier)
            : base(caveXDim, caveYDim, start, kb, notifier)
        {
            getKB().disableNavSentences(); // Optimization: Verbosity of produced sentences is reduced.
            modelCave = new WumpusCave(caveXDim, caveYDim);
            visitedRooms.Add(currentPosition.getRoom());
        }

        /**
         * function HYBRID-WUMPUS-AGENT(percept) returns an action<br>
         *
         * @param percept
         *            a list, [stench, breeze, glitter, bump, scream]
         *
         * @return an action the agent should take.
         */
        public override Action execute(Percept percept)
        {

            // TELL(KB, MAKE-PERCEPT-SENTENCE(percept, t))
            getKB().makePerceptSentence((WumpusPercept)percept, t);
            // TELL the KB the temporal "physics" sentences for time t
            // Optimization: The agent is aware of it's position - the KB can profit from that!
            getKB().tellTemporalPhysicsSentences(t, currentPosition);

            ISet<Room> safe ;
            ISet<Room> unvisited;

            // Optimization: Do not ask anything during plan execution (different from pseudo-code)
            if (plan.isEmpty())
            {
                notifyViews("Reasoning (t=" + t + ", Percept=" + percept + ", Pos=" + currentPosition + ") ...");
                // safe <- {[x, y] : ASK(KB, OK<sup>t</sup><sub>x,y</sub>) = true}
                safe = getKB().askSafeRooms(t, visitedRooms);
                notifyViews("Ask safe -> " + safe);
            }

            // if ASK(KB, Glitter<sup>t</sup>) = true then
            // Optimization: Use percept (condition can only be true if plan is empty).
            if (plan.isEmpty() && ((WumpusPercept)percept).isGlitter())
            {
                // plan <- [Grab] + PLAN-ROUTE(current, {[1,1]}, safe) + [Climb]
                ISet<Room> goals = Factory.CreateSet<Room>();
                goals.Add(modelCave.getStart().getRoom());
                plan.Add(WumpusAction.GRAB);
                plan.AddAll(planRouteToRooms(goals, safe));
                plan.Add(WumpusAction.CLIMB);
            }

            // if plan is empty then
            if (plan.isEmpty())
            {
                // unvisited <- {[x, y] : ASK(KB, L<sup>t'</sup><sub>x,y</sub>) = false for all t' &le; t}
                // Optimization: Agent remembers visited locations, no need to ask.
                unvisited = SetOps.difference(modelCave.getAllRooms(), visitedRooms);
                // plan <- PLAN-ROUTE(current, unvisited &cap; safe, safe)
                plan.AddAll(planRouteToRooms(unvisited, safe));
            }

            // if plan is empty and ASK(KB, HaveArrow<sup>t</sup>) = true then
            if (plan.isEmpty() && getKB().askHaveArrow(t))
            {
                // possible_wumpus <- {[x, y] : ASK(KB, ~W<sub>x,y</sub>) = false}
                ISet<Room> possibleWumpus = getKB().askPossibleWumpusRooms(t);
                notifyViews("Ask possible Wumpus positions -> " + possibleWumpus);
                // plan <- PLAN-SHOT(current, possible_wumpus, safe)
                plan.AddAll(planShot(possibleWumpus, safe));
            }

            // if plan is empty then //no choice but to take a risk
            if (plan.isEmpty())
            {
                // not_unsafe <- {[x, y] : ASK(KB, ~OK<sup>t</sup><sub>x,y</sub>) = false}
                // Optimization: Do not check visited rooms again.
                ISet<Room> notUnsafe = getKB().askNotUnsafeRooms(t, visitedRooms);
                notifyViews("Ask not unsafe -> " + notUnsafe);
                // plan <- PLAN-ROUTE(current, unvisited &cap; not_unsafe, safe)
                // Correction: Last argument must be not_unsafe!
                plan.AddAll(planRouteToRooms(unvisited, notUnsafe));
            }

            // if plan is empty then
            if (plan.isEmpty())
            {
                notifyViews("Going home.");
                // plan PLAN-ROUTE(current, {[1,1]}, safe) + [Climb]
                ISet<Room> goal = Factory.CreateSet<>();
                goal.Add(modelCave.getStart().getRoom());
                plan.AddAll(planRouteToRooms(goal, safe));
                plan.Add(WumpusAction.CLIMB);
            }
            // action <- POP(plan)
            WumpusAction action = plan.Remove();
            // TELL(KB, MAKE-ACTION-SENTENCE(action, t))
            getKB().makeActionSentence(action, t);
            // t <- t+1
            t = t + 1;
            updateAgentPosition(action);
            visitedRooms.Add(currentPosition.getRoom());
            // return action
            return action;
        }

        /**
         * Returns a sequence of actions using A* Search.
         *
         * @param goals
         *            a set of agent positions; try to plan a route to one of them
         * @param allowed
         *            a set of squares that can form part of the route
         *
         * @return the best sequence of actions that the agent have to do to reach a
         *         goal from the current position.
         */
        public IQueue<WumpusAction> planRoute(Set<AgentPosition> goals, ISet<Room> allowed)
        {
            modelCave.setAllowed(allowed);
            Problem<AgentPosition, WumpusAction> problem 
                = new GeneralProblem<AgentPosition, WumpusAction>(currentPosition,
                    WumpusFunctions.createActionsFunction(modelCave),
                    WumpusFunctions.createResultFunction(modelCave), 
                    goals.contains);
            SearchForActions<AgentPosition, WumpusAction> search =
                    new AStarSearch<>(new GraphSearch<>(), new ManhattanHeuristicFunction(goals));
            Optional<IQueue<WumpusAction>> actions = search.findActions(problem);

            return actions.isPresent() ? actions.Get() : Collections.emptyList();
        }

        /**
         * Uses the model cave to update the current agent position.
         */
        private void updateAgentPosition(WumpusAction action)
        {
            modelCave.setAllowed(modelCave.getAllRooms());
            switch (action)
            {
                case FORWARD:
                    currentPosition = modelCave.moveForward(currentPosition);
                    break;
                case TURN_LEFT:
                    currentPosition = modelCave.turnLeft(currentPosition);
                    break;
                case TURN_RIGHT:
                    currentPosition = modelCave.turnRight(currentPosition);
                    break;
            }
        }
    }
}
