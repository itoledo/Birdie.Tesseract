using System;
using System.Collections.Generic;
using tvn.cosine.ai.agent;
using tvn.cosine.ai.agent.impl;
using tvn.cosine.ai.environment.cellworld;
using tvn.cosine.ai.probability.mdp;

namespace tvn.cosine.ai.learning.reinforcement.example
{
    /**
     * Implementation of the Cell World Environment, supporting the execution of
     * trials for reinforcement learning agents.
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public class CellWorldEnvironment : AbstractEnvironment
    {
        private Cell<double> startingCell = null;
        private ISet<Cell<double>> allStates = new HashSet<Cell<double>>();
        private TransitionProbabilityFunction<Cell<double>, CellWorldAction> tpf;
        private Random r = null;
        private CellWorldEnvironmentState currentState = new CellWorldEnvironmentState();

        /**
         * Constructor.
         * 
         * @param startingCell
         *            the cell that agent(s) are to start from at the beginning of
         *            each trial within the environment.
         * @param allStates
         *            all the possible states in this environment.
         * @param tpf
         *            the transition probability function that simulates how the
         *            environment is meant to behave in response to an agent action.
         * @param r
         *            a Randomizer used to sample actions that are actually to be
         *            executed based on the transition probabilities for actions.
         */
        public CellWorldEnvironment(Cell<double> startingCell,
                ISet<Cell<double>> allStates,
                TransitionProbabilityFunction<Cell<double>, CellWorldAction> tpf,
                Random r)
        {
            this.startingCell = startingCell;
            foreach (var v in allStates)
                this.allStates.Add(v);
            this.tpf = tpf;
            this.r = r;
        }

        /**
         * Execute N trials.
         * 
         * @param n
         *            the number of trials to execute.
         */
        public void executeTrials(int n)
        {
            for (int i = 0; i < n; ++i)
            {
                executeTrial();
            }
        }

        /**
         * Execute a single trial.
         */
        public void executeTrial()
        {
            currentState.reset();
            foreach (Agent a in agents)
            {
                a.setAlive(true);
                currentState.setAgentLocation(a, startingCell);
            }
            stepUntilDone();
        }

        public override void executeAction(Agent agent, ai.agent.Action action)
        {
            if (!action.isNoOp())
            {
                Cell<double> s = currentState.getAgentLocation(agent);
                double probabilityChoice = r.NextDouble();
                double total = 0;
                bool set = false;
                foreach (Cell<double> sDelta in allStates)
                {
                    total += tpf(sDelta, s, (CellWorldAction)action);
                    if (total > 1.0)
                    {
                        throw new Exception("Bad probability calculation.");
                    }
                    if (total > probabilityChoice)
                    {
                        currentState.setAgentLocation(agent, sDelta);
                        set = true;
                        break;
                    }
                }
                if (!set)
                {
                    throw new Exception("Failed to simulate the action=" + action + " correctly from s=" + s);
                }
            }
        }

        public override Percept getPerceptSeenBy(Agent anAgent)
        {
            return currentState.getPerceptFor(anAgent);
        }
    }
}
