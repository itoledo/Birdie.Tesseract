using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tvn.cosine.ai.agent;
using tvn.cosine.ai.agent.impl;
using tvn.cosine.ai.environment.wumpusworld.action;
using tvn.cosine.ai.search.framework;
using tvn.cosine.ai.search.framework.agent;
using tvn.cosine.ai.search.framework.problem;
using tvn.cosine.ai.search.framework.qsearch;
using tvn.cosine.ai.search.informed;

namespace tvn.cosine.ai.environment.wumpusworld
{
    ///////**
    ////// * Artificial Intelligence A Modern Approach (3rd Edition): page 270. 
    ////// *  
    ////// * 
    ////// * <pre>
    ////// * <code>
    ////// * function HYBRID-WUMPUS-AGENT(percept) returns an action
    ////// *   inputs: percept, a list, [stench, breeze, glitter, bump, scream]
    ////// *   persistent: KB, a knowledge base, initially the atemporal "wumpus physics"
    ////// *               t, a counter, initially 0, indicating time
    ////// *               plan, an action sequence, initially empty
    ////// * 
    ////// *   TELL(KB, MAKE-PERCEPT-SENTENCE(percept, t))
    ////// *   TELL the KB the temporal "physics" sentences for time t
    ////// *   safe <- {[x, y] : ASK(KB, OK<sup>t</sup><sub>x,y</sub>) = true}
    ////// *   if ASK(KB, Glitter<sup>t</sup>) = true then
    ////// *      plan <- [Grab] + PLAN-ROUTE(current, {[1,1]}, safe) + [Climb]
    ////// *   if plan is empty then
    ////// *      unvisited <- {[x, y] : ASK(KB, L<sup>t'</sup><sub>x,y</sub>) = false for all t' &le; t}
    ////// *      plan <- PLAN-ROUTE(current, unvisited &cap; safe, safe)
    ////// *   if plan is empty and ASK(KB, HaveArrow<sup>t</sup>) = true then
    ////// *      possible_wumpus <- {[x, y] : ASK(KB, ~W<sub>x,y</sub>) = false}
    ////// *      plan <- PLAN-SHOT(current, possible_wumpus, safe)
    ////// *   if plan is empty then //no choice but to take a risk
    ////// *      not_unsafe <- {[x, y] : ASK(KB, ~OK<sup>t</sup><sub>x,y</sub>) = false}
    ////// *      plan <- PLAN-ROUTE(current, unvisited &cap; not_unsafe, safe)
    ////// *   if plan is empty then
    ////// *      plan <- PLAN-ROUTE(current, {[1,1]}, safe) + [Climb]
    ////// *   action <- POP(plan)
    ////// *   TELL(KB, MAKE-ACTION-SENTENCE(action, t))
    ////// *   t <- t+1
    ////// *   return action
    ////// * 
    ////// * --------------------------------------------------------------------------------
    ////// * 
    ////// * function PLAN-ROUTE(current, goals, allowed) returns an action sequence
    ////// *   inputs: current, the agent's current position
    ////// *           goals, a set of squares; try to plan a route to one of them
    ////// *           allowed, a set of squares that can form part of the route 
    ////// *    
    ////// *   problem <- ROUTE-PROBLEM(current, goals, allowed)
    ////// *   return A*-GRAPH-SEARCH(problem)
    ////// * </code>
    ////// * </pre>
    ////// * 
    ////// * Figure 7.20 A hybrid agent program for the wumpus world. It uses a
    ////// * propositional knowledge base to infer the state of the world, and a
    ////// * combination of problem-solving search and domain-specific code to decide what
    ////// * actions to take
    ////// * 
    ////// * @author Federico Baron
    ////// * @author Alessandro Daniele
    ////// * @author Ciaran O'Reilly
    ////// * @author Ruediger Lunde
    ////// */
    //////public class HybridWumpusAgent : AbstractAgent
    //////{

    //////    // persistent: KB, a knowledge base, initially the atemporal
    //////    // "wumpus physics"
    //////    //TODO: implement ww kb
    //////     private WumpusKnowledgeBase kb = null;
    //////    // t, a counter, initially 0, indicating time
    //////    private int t = 0;
    //////    // plan, an action sequence, initially empty
    //////    private IQueue<Action> plan = new FifoQueue<Action>(); // FIFOQueue

    //////    /**
    //////     * function HYBRID-WUMPUS-AGENT(percept) returns an action 
    //////     * 
    //////     * @param percept
    //////     *            a list, [stench, breeze, glitter, bump, scream]
    //////     * 
    //////     * @return an action the agent should take.
    //////     */
    //////    public override Action execute(Percept percept)
    //////    {

    //////        // TELL(KB, MAKE-PERCEPT-SENTENCE(percept, t))
    //////        kb.makePerceptSentence((AgentPercept)percept, t);
    //////        // TELL the KB the temporal "physics" sentences for time t
    //////        kb.tellTemporalPhysicsSentences(t);

    //////        AgentPosition current = kb.askCurrentPosition(t);

    //////        // safe <- {[x, y] : ASK(KB, OK<sup>t</sup><sub>x,y</sub>) = true}
    //////        ISet<Room> safe = kb.askSafeRooms(t);

    //////        // if ASK(KB, Glitter<sup>t</sup>) = true then
    //////        if (kb.askGlitter(t))
    //////        {
    //////            // plan <- [Grab] + PLAN-ROUTE(current, {[1,1]}, safe) + [Climb]
    //////            ISet<Room> goals = new HashSet<Room>();
    //////            goals.Add(new Room(1, 1));

    //////            plan.add(new Grab());
    //////            foreach (var v in planRoute(current, goals, safe))
    //////                plan.add(v);
    //////            plan.add(new Climb());
    //////        }

    //////        // if plan is empty then
    //////        // unvisited <- {[x, y] : ASK(KB, L<sup>t'</sup><sub>x,y</sub>) = false
    //////        // for all t' &le; t}
    //////        ISet<Room> unvisited = kb.askUnvisitedRooms(t);
    //////        if (plan.isEmpty())
    //////        {
    //////            // plan <- PLAN-ROUTE(current, unvisited &cap; safe, safe)
    //////            foreach (var v in planRoute(current, new HashSet<Room>(unvisited.Intersect(safe)), safe))
    //////                plan.add(v);
    //////        }

    //////        // if plan is empty and ASK(KB, HaveArrow<sup>t</sup>) = true then
    //////        if (plan.isEmpty() && kb.askHaveArrow(t))
    //////        {
    //////            // possible_wumpus <- {[x, y] : ASK(KB, ~W<sub>x,y</sub>) = false}
    //////            ISet<Room> possibleWumpus = kb.askPossibleWumpusRooms(t);
    //////            // plan <- PLAN-SHOT(current, possible_wumpus, safe)
    //////            foreach (var v in planShot(current, possibleWumpus, safe))
    //////                plan.add(v);
    //////        }

    //////        // if plan is empty then //no choice but to take a risk
    //////        if (plan.isEmpty())
    //////        {
    //////            // not_unsafe <- {[x, y] : ASK(KB, ~OK<sup>t</sup><sub>x,y</sub>) =
    //////            // false}
    //////            ISet<Room> notUnsafe = kb.askNotUnsafeRooms(t);
    //////            // plan <- PLAN-ROUTE(current, unvisited &cap; not_unsafe, safe)
    //////            foreach (var v in planRoute(current, new HashSet<Room>(unvisited.Intersect(notUnsafe)), safe))
    //////                plan.add(v);
    //////        }

    //////        // if plan is empty then
    //////        if (plan.isEmpty())
    //////        {
    //////            // plan PLAN-ROUTE(current, {[1,1]}, safe) + [Climb]
    //////            ISet<Room> start = new HashSet<Room>();
    //////            start.Add(new Room(1, 1));
    //////            foreach (var v in planRoute(current, start, safe))
    //////                plan.add(v);
    //////            plan.add(new Climb());
    //////        }
    //////        // action <- POP(plan)
    //////        Action action = plan.remove();
    //////        // TELL(KB, MAKE-ACTION-SENTENCE(action, t))
    //////        kb.makeActionSentence(action, t);
    //////        // t <- t+1
    //////        t = t + 1;
    //////        // return action
    //////        return action;
    //////    }

    //////    /**
    //////     * Returns a sequence of actions using A* Search.
    //////     * 
    //////     * @param current
    //////     *            the agent's current position
    //////     * @param goals
    //////     *            a set of squares; try to plan a route to one of them
    //////     * @param allowed
    //////     *            a set of squares that can form part of the route
    //////     * 
    //////     * @return the best sequence of actions that the agent have to do to reach a
    //////     *         goal from the current position.
    //////     */
    //////    public IList<Action> planRoute(AgentPosition current, ISet<Room> goals, ISet<Room> allowed)
    //////    {

    //////        // Every square represent 4 possible positions for the agent, it could
    //////        // be in different orientations. For every square in allowed and goals
    //////        // sets we add 4 squares.
    //////        ISet<AgentPosition> allowedPositions = new HashSet<AgentPosition>();
    //////        foreach (Room allowedRoom in allowed)
    //////        {
    //////            int x = allowedRoom.getX();
    //////            int y = allowedRoom.getY();

    //////            allowedPositions.Add(new AgentPosition(x, y, AgentPosition.Orientation.FACING_WEST));
    //////            allowedPositions.Add(new AgentPosition(x, y, AgentPosition.Orientation.FACING_EAST));
    //////            allowedPositions.Add(new AgentPosition(x, y, AgentPosition.Orientation.FACING_NORTH));
    //////            allowedPositions.Add(new AgentPosition(x, y, AgentPosition.Orientation.FACING_SOUTH));
    //////        }
    //////        ISet<AgentPosition> goalPositions = new HashSet<AgentPosition>();
    //////        foreach (Room goalRoom in goals)
    //////        {
    //////            int x = goalRoom.getX();
    //////            int y = goalRoom.getY();

    //////            goalPositions.Add(new AgentPosition(x, y, AgentPosition.Orientation.FACING_WEST));
    //////            goalPositions.Add(new AgentPosition(x, y, AgentPosition.Orientation.FACING_EAST));
    //////            goalPositions.Add(new AgentPosition(x, y, AgentPosition.Orientation.FACING_NORTH));
    //////            goalPositions.Add(new AgentPosition(x, y, AgentPosition.Orientation.FACING_SOUTH));
    //////        }

    //////        WumpusCave cave = new WumpusCave(kb.getCaveXDimension(), kb.getCaveYDimension(), allowedPositions);

    //////        GoalTest<AgentPosition> goalTest = goalPositions.Contains;

    //////        IProblem<AgentPosition, Action> problem = new GeneralProblem<AgentPosition, Action>(current,
    //////                WumpusFunctionFunctions.createActionsFunction(cave),
    //////                WumpusFunctionFunctions.createResultFunction(), goalTest);

    //////        HeuristicEvaluationFunction<Node<AgentPosition, Action>> h = (node) =>
    //////        {
    //////            System.Func<int, int, int, int, int> evaluateManhattanDistanceOf = (x1, y1, x2, y2) =>
    //////                {
    //////                    return System.Math.Abs(x1 - x2) + System.Math.Abs(y1 - y2);
    //////                };

    //////            AgentPosition pos = node.getState();
    //////            int nearestGoalDist = int.MaxValue;
    //////            foreach (Room g in goals)
    //////            {
    //////                int tmp = evaluateManhattanDistanceOf(pos.getX(), pos.getY(), g.getX(), g.getY());

    //////                if (tmp < nearestGoalDist)
    //////                {
    //////                    nearestGoalDist = tmp;
    //////                }
    //////            };

    //////            return (double)nearestGoalDist;
    //////        };

    //////        SearchForActions<AgentPosition, Action> search = new AStarSearch<AgentPosition, Action>(new GraphSearch<AgentPosition, Action>(), h);
    //////        SearchAgent<AgentPosition, Action> agent;
    //////        IList<Action> actions;

    //////        agent = new SearchAgent<AgentPosition, Action>(problem, search);
    //////        actions = agent.getActions();
    //////        // Search agent can return a NoOp if already at goal,
    //////        // in the context of this agent we will just return
    //////        // no actions.
    //////        if (actions.Count == 1 && actions[0].isNoOp())
    //////        {
    //////            actions = new List<Action>();
    //////        }

    //////        return actions;
    //////    }

    //////    /**
    //////     * 
    //////     * @param current
    //////     *            the agent's current position
    //////     * @param possibleWumpus
    //////     *            a set of squares where we don't know that there isn't the
    //////     *            wumpus.
    //////     * @param allowed
    //////     *            a set of squares that can form part of the route
    //////     * 
    //////     * @return the sequence of actions to reach the nearest square that is in
    //////     *         line with a possible wumpus position. The last action is a shot.
    //////     */
    //////    public IList<Action> planShot(AgentPosition current, ISet<Room> possibleWumpus, ISet<Room> allowed)
    //////    {
    //////        ISet<AgentPosition> shootingPositions = new HashSet<AgentPosition>();

    //////        foreach (Room p in possibleWumpus)
    //////        {
    //////            int x = p.getX();
    //////            int y = p.getY();

    //////            for (int i = 1; i <= kb.getCaveXDimension(); i++)
    //////            {
    //////                if (i < x)
    //////                {
    //////                    shootingPositions.Add(new AgentPosition(i, y, AgentPosition.Orientation.FACING_EAST));
    //////                }
    //////                if (i > x)
    //////                {
    //////                    shootingPositions.Add(new AgentPosition(i, y, AgentPosition.Orientation.FACING_WEST));
    //////                }
    //////                if (i < y)
    //////                {
    //////                    shootingPositions.Add(new AgentPosition(x, i, AgentPosition.Orientation.FACING_NORTH));
    //////                }
    //////                if (i > y)
    //////                {
    //////                    shootingPositions.Add(new AgentPosition(x, i, AgentPosition.Orientation.FACING_SOUTH));
    //////                }
    //////            }
    //////        }

    //////        // Can't have a shooting position from any of the rooms the wumpus could
    //////        // reside
    //////        foreach (Room p in possibleWumpus)
    //////        {
    //////            shootingPositions.Remove(new AgentPosition(p.getX(), p.getY(), AgentPosition.Orientation.FACING_EAST));
    //////            shootingPositions.Remove(new AgentPosition(p.getX(), p.getY(), AgentPosition.Orientation.FACING_NORTH));
    //////            shootingPositions.Remove(new AgentPosition(p.getX(), p.getY(), AgentPosition.Orientation.FACING_SOUTH));
    //////            shootingPositions.Remove(new AgentPosition(p.getX(), p.getY(), AgentPosition.Orientation.FACING_WEST));
    //////        }

    //////        ISet<Room> shootingPositionsArray = new HashSet<Room>();
    //////        foreach (var tmp in shootingPositions)
    //////        {
    //////            shootingPositionsArray.Add(new Room(tmp.getX(), tmp.getY()));
    //////        }

    //////        IList<Action> actions = planRoute(current, shootingPositionsArray, allowed);

    //////        AgentPosition newPos = current;
    //////        if (actions.Count > 0)
    //////        {
    //////            newPos = ((Forward)actions[actions.Count - 1]).getToPosition();
    //////        }

    //////        while (!shootingPositions.Contains(newPos))
    //////        {
    //////            TurnLeft tLeft = new TurnLeft(newPos.getOrientation());
    //////            newPos = new AgentPosition(newPos.getX(), newPos.getY(), tLeft.getToOrientation());
    //////            actions.Add(tLeft);
    //////        }

    //////        actions.Add(new Shoot());
    //////        return actions;
    //////    }

    //////    //
    //////    // SUPPORTING CODE
    //////    //
    //////    public HybridWumpusAgent()
    //////                : this(4)
    //////    {
    //////        // i.e. default is a 4x4 world as depicted in figure 7.2

    //////    }

    //////    public HybridWumpusAgent(int caveXandYDimensions)
    //////    { 
    //////           kb = new WumpusKnowledgeBase(caveXandYDimensions);
    //////    }
    //////}
}
