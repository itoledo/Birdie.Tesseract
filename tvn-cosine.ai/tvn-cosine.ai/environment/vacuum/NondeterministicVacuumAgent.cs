using tvn.cosine.ai.agent;
using tvn.cosine.ai.agent.impl;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.exceptions;
using tvn.cosine.ai.search.nondeterministic;
using tvn.cosine.ai.util;

namespace tvn.cosine.ai.environment.vacuum
{
    /**
     * This agent traverses the NondeterministicVacuumEnvironment using a
     * contingency plan. See page 135, AIMA3e.
     * 
     * @author Andrew Brown
     */
    public class NondeterministicVacuumAgent : AbstractAgent
    {
        private NondeterministicProblem<object, Action> problem;
        private Function<Percept, object> ptsFunction;
        private Plan contingencyPlan;
        private IQueue<object> stack = Factory.CreateLifoQueue<object>();

        public NondeterministicVacuumAgent(Function<Percept, object> ptsFunction)
        {
            setPerceptToStateFunction(ptsFunction);
        }

        /**
         * Returns the search problem for this agent.
         * 
         * @return the search problem for this agent.
         */
        public NondeterministicProblem<object, Action> getProblem()
        {
            return problem;
        }

        /**
         * Sets the search problem for this agent to solve.
         * 
         * @param problem
         *            the search problem for this agent to solve.
         */
        public void setProblem(NondeterministicProblem<object, Action> problem)
        {
            this.problem = problem;
            init();
        }

        /**
         * Returns the percept to state function of this agent.
         * 
         * @return the percept to state function of this agent.
         */
        public Function<Percept, object> getPerceptToStateFunction()
        {
            return ptsFunction;
        }

        /**
         * Sets the percept to state functino of this agent.
         * 
         * @param ptsFunction
         *            a function which returns the problem state associated with a
         *            given Percept.
         */
        public void setPerceptToStateFunction(Function<Percept, object> ptsFunction)
        {
            this.ptsFunction = ptsFunction;
        }

        /**
         * Return the agent contingency plan
         * 
         * @return the plan the agent uses to clean the vacuum world
         */
        public Plan getContingencyPlan()
        {
            if (this.contingencyPlan == null)
            {
                throw new RuntimeException("Contingency plan not set.");
            }
            return this.contingencyPlan;
        }

        /**
         * Execute an action from the contingency plan
         * 
         * @param percept a percept.
         * @return an action from the contingency plan.
         */
        public override Action execute(Percept percept)
        {
            // check if goal state
            VacuumEnvironmentState state = (VacuumEnvironmentState)this
                    .getPerceptToStateFunction()(percept);
            if (state.getLocationState(VacuumEnvironment.LOCATION_A) == VacuumEnvironment.LocationState.Clean
                    && state.getLocationState(VacuumEnvironment.LOCATION_B) == VacuumEnvironment.LocationState.Clean)
            {
                return NoOpAction.NO_OP;
            }
            // check stack size
            if (this.stack.Size() < 1)
            {
                if (this.contingencyPlan.Size() < 1)
                {
                    return NoOpAction.NO_OP;
                }
                else
                {
                    this.stack.Add(this.getContingencyPlan().Pop());
                }
            }
            // pop...
            object currentStep = this.stack.Peek();
            // push...
            if (currentStep is Action)
            {
                return (Action)this.stack.Pop();
            } // case: next step is a plan
            else if (currentStep is Plan)
            {
                Plan newPlan = (Plan)currentStep;
                if (newPlan.Size() > 0)
                {
                    this.stack.Add(newPlan.Pop());
                }
                else
                {
                    this.stack.Pop();
                }
                return this.execute(percept);
            } // case: next step is an if-then
            else if (currentStep is IfStateThenPlan)
            {
                IfStateThenPlan conditional = (IfStateThenPlan)this.stack.Pop();
                this.stack.Add(conditional.ifStateMatches(percept));
                return this.execute(percept);
            } // case: ignore next step if null
            else if (currentStep == null)
            {
                this.stack.Pop();
                return this.execute(percept);
            }
            else
            {
                throw new RuntimeException("Unrecognized contingency plan step.");
            }
        }

        //
        // PRIVATE METHODS
        //
        private void init()
        {
            setAlive(true);
            stack.Clear();
            AndOrSearch<object, Action> andOrSearch = new AndOrSearch<object, Action>();
            this.contingencyPlan = andOrSearch.search(this.problem);
        }
    }

}
