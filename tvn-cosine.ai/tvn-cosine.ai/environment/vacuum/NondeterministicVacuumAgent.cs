using System;
using System.Collections.Generic;
using tvn.cosine.ai.agent;
using tvn.cosine.ai.agent.impl;
using tvn.cosine.ai.search.nondeterministic;

namespace tvn.cosine.ai.environment.vacuum
{
    /// <summary>
    /// This agent traverses the NondeterministicVacuumEnvironment using a contingency plan. See page 135, AIMA3e.
    /// </summary>
    public class NondeterministicVacuumAgent<S, A> : AbstractAgent
    {
        private NondeterministicProblem<S, A> problem;
        private PerceptToStateFunction<Percept, object> ptsFunction;
        private Plan contingencyPlan;
        private List<object> stack = new List<object>();

        public NondeterministicVacuumAgent(PerceptToStateFunction<Percept, object> ptsFunction)
        {
            setPerceptToStateFunction(ptsFunction);
        }

        /// <summary>
        /// Returns the search problem for this agent.
        /// </summary>
        /// <returns>the search problem for this agent.</returns>
        public NondeterministicProblem<S, A> getProblem()
        {
            return problem;
        }

        /// <summary>
        /// Sets the search problem for this agent to solve.
        /// </summary>
        /// <param name="problem">the search problem for this agent to solve.</param>
        public void setProblem(NondeterministicProblem<S, A> problem)
        {
            this.problem = problem;
            init();
        }

        /// <summary>
        /// Returns the percept to state function of this agent.
        /// </summary>
        /// <returns></returns>
        public PerceptToStateFunction<Percept, object> getPerceptToStateFunction()
        {
            return ptsFunction;
        }


        /// <summary>
        /// Sets the percept to state functino of this agent.
        /// </summary>
        /// <param name="ptsFunction">the percept to state function of this agent.</param>
        public void setPerceptToStateFunction(PerceptToStateFunction<Percept, object> ptsFunction)
        {
            this.ptsFunction = ptsFunction;
        }

        /// <summary>
        /// Return the agent contingency plan
        /// </summary>
        /// <returns>the plan the agent uses to clean the vacuum world</returns>
        public Plan getContingencyPlan()
        {
            if (this.contingencyPlan == null)
            {
                throw new Exception("Contingency plan not set.");
            }
            return this.contingencyPlan;
        }

        /// <summary>
        /// Execute an action from the contingency plan
        /// </summary>
        /// <param name="percept">percept a percept.</param>
        /// <returns>an action from the contingency plan.</returns>
        public override agent.Action execute(Percept percept)
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
                    var first = this.getContingencyPlan()[0];
                    this.getContingencyPlan().RemoveAt(0);
                    this.stack.Add(first);
                }
            }
            // pop...
            object currentStep = this.stack[0];
            // push...
            if (currentStep is agent.Action)
            {
                this.stack.RemoveAt(0);
                return (agent.Action)currentStep;
            } // case: next step is a plan
            else if (currentStep is Plan)
            {
                Plan newPlan = (Plan)currentStep;
                if (newPlan.Count > 0)
                {
                    var first = newPlan[0];
                    newPlan.RemoveAt(0);
                    this.stack.Add(first);
                }
                else
                {
                    this.stack.RemoveAt(0);
                }
                return this.execute(percept);
            } // case: next step is an if-then
            else if (currentStep is IfStateThenPlan)
            {
                this.stack.RemoveAt(0);
                IfStateThenPlan conditional = (IfStateThenPlan)currentStep;
                this.stack.Add(conditional.ifStateMatches(percept));
                return this.execute(percept);
            } // case: ignore next step if null
            else if (currentStep == null)
            {
                this.stack.RemoveAt(0);
                return this.execute(percept);
            }
            else
            {
                throw new Exception("Unrecognized contingency plan step.");
            }
        }
         
        private void init()
        {
            setAlive(true);
            stack.Clear();
            AndOrSearch<S, A> andOrSearch = new AndOrSearch<S, A>();
            this.contingencyPlan = andOrSearch.search(this.problem);
        }
    } 
}
