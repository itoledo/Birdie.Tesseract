using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tvn.cosine.ai.agent;
using tvn.cosine.ai.agent.impl;
using tvn.cosine.ai.search.nondeterministic;

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
        private NondeterministicProblem<object, IAction> problem;
        private System.Func<IPercept, object> ptsFunction;
        private Plan contingencyPlan;
        private Stack<object> stack = new Stack<object>();

        public NondeterministicVacuumAgent(System.Func<IPercept, object> ptsFunction)
        {
            setPerceptToStateFunction(ptsFunction);
        }

        /**
         * Returns the search problem for this agent.
         * 
         * @return the search problem for this agent.
         */
        public NondeterministicProblem<object, IAction> getProblem()
        {
            return problem;
        }

        /**
         * Sets the search problem for this agent to solve.
         * 
         * @param problem
         *            the search problem for this agent to solve.
         */
        public void setProblem(NondeterministicProblem<object, IAction> problem)
        {
            this.problem = problem;
            init();
        }

        /**
         * Returns the percept to state function of this agent.
         * 
         * @return the percept to state function of this agent.
         */
        public System.Func<IPercept, object> getPerceptToStateFunction()
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
        public void setPerceptToStateFunction(System.Func<IPercept, object> ptsFunction)
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
                throw new System.Exception("Contingency plan not set.");
            }
            return this.contingencyPlan;
        }

        /**
         * Execute an action from the contingency plan
         * 
         * @param percept a percept.
         * @return an action from the contingency plan.
         */
        public override IAction Execute(IPercept percept)
        {
            // check if goal state
            VacuumEnvironmentState state = (VacuumEnvironmentState)this.getPerceptToStateFunction()(percept);
            if (state.getLocationState(VacuumEnvironment.LOCATION_A) == VacuumEnvironment.LocationState.Clean
                    && state.getLocationState(VacuumEnvironment.LOCATION_B) == VacuumEnvironment.LocationState.Clean)
            {
                return DynamicAction.NO_OP;
            }
            // check stack size
            if (this.stack.Count < 1)
            {
                if (this.contingencyPlan.Count < 1)
                {
                    return DynamicAction.NO_OP;
                }
                else
                { 
                    this.stack.Push(this.getContingencyPlan()[0]);
                    this.getContingencyPlan().RemoveAt(0);
                }
            }
            // pop...
            object currentStep = this.stack.Peek();
            // push...
            if (currentStep is IAction)
            {
                return (IAction)this.stack.Pop();
            } // case: next step is a plan
            else if (currentStep is Plan)
            {
                Plan newPlan = (Plan)currentStep;
                if (newPlan.Count > 0)
                {
                    this.stack.Push(newPlan[0]);
                    newPlan.RemoveAt(0);
                }
                else
                {
                    this.stack.Pop();
                }
                return this.Execute(percept);
            } // case: next step is an if-then
            else if (currentStep is IfStateThenPlan<object>)
            {
                IfStateThenPlan<object> conditional = (IfStateThenPlan<object>)this.stack.Pop();
                this.stack.Push(conditional.ifStateMatches(percept));
                return this.Execute(percept);
            } // case: ignore next step if null
            else if (currentStep == null)
            {
                this.stack.Pop();
                return this.Execute(percept);
            }
            else
            {
                throw new System.Exception("Unrecognized contingency plan step.");
            }
        }

        //
        // PRIVATE METHODS
        //
        private void init()
        {
            SetAlive(true);
            stack.Clear();
            AndOrSearch<object, IAction> andOrSearch = new AndOrSearch<object, IAction>();
            this.contingencyPlan = andOrSearch.search(this.problem);
        }
    }

}
