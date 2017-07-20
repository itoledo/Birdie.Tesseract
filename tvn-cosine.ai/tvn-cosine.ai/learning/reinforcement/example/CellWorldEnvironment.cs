using tvn.cosine.ai.agent;
using tvn.cosine.ai.agent.impl;
using tvn.cosine.ai.common;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.exceptions;
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
    public class CellWorldEnvironment : EnvironmentBase
    {
        private Cell<double> startingCell = null;
        private ISet<Cell<double>> allStates = Factory.CreateSet<Cell<double>>();
        private TransitionProbabilityFunction<Cell<double>, CellWorldAction> tpf;
        private IRandom r = null;
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
         *            a IRandom used to sample actions that are actually to be
         *            executed based on the transition probabilities for actions.
         */
        public CellWorldEnvironment(Cell<double> startingCell,
                ISet<Cell<double>> allStates,
                TransitionProbabilityFunction<Cell<double>, CellWorldAction> tpf,
                IRandom r)
        {
            this.startingCell = startingCell;
            this.allStates.AddAll(allStates);
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
            for (int i = 0; i < n;++i)
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
            foreach (IAgent a in agents)
            {
                a.SetAlive(true);
                currentState.setAgentLocation(a, startingCell);
            }
            StepUntilDone();
        }


        public override void executeAction(IAgent agent, IAction action)
        {
            if (!action.IsNoOp())
            {
                Cell<double> s = currentState.getAgentLocation(agent);
                double probabilityChoice = r.NextDouble();
                double total = 0;
                bool set = false;
                foreach (Cell<double> sDelta in allStates)
                {
                    total += tpf.probability(sDelta, s, (CellWorldAction)action);
                    if (total > 1.0)
                    {
                        throw new IllegalStateException("Bad probability calculation.");
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
                    throw new IllegalStateException("Failed to simulate the action=" + action + " correctly from s=" + s);
                }
            }
        }


        public override IPercept getPerceptSeenBy(IAgent anAgent)
        {
            return currentState.getPerceptFor(anAgent);
        }
    }
}
