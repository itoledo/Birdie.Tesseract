using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tvn.cosine.ai.search.adversarial
{
    /**
  * Implements an iterative deepening Minimax search with alpha-beta pruning and
  * action ordering. Maximal computation time is specified in seconds. The
  * algorithm is implemented as template method and can be configured and tuned
  * by subclassing.
  *
  * @param <S> Type which is used for states in the game.
  * @param <A> Type which is used for actions in the game.
  * @param <P> Type which is used for players in the game.
  * @author Ruediger Lunde
  */
    public class IterativeDeepeningAlphaBetaSearch<S, A, P> : AdversarialSearch<S, A>
    {
        public const string METRICS_NODES_EXPANDED = "nodesExpanded";
        public const string METRICS_MAX_DEPTH = "maxDepth";

        protected Game<S, A, P> game;
        protected double utilMax;
        protected double utilMin;
        protected int currDepthLimit;
        private bool heuristicEvaluationUsed;    // indicates that non-terminal
                                                 // nodes
                                                 // have been evaluated.
        private Timer timer;
        private bool logEnabled;

        private IDictionary<string, double> metrics = new Dictionary<string, double>();

        /**
         * Creates a new search object for a given game.
         *
         * @param game    The game.
         * @param utilMin Utility value of worst state for this player. Supports
         *                evaluation of non-terminal states and early termination in
         *                situations with a safe winner.
         * @param utilMax Utility value of best state for this player. Supports
         *                evaluation of non-terminal states and early termination in
         *                situations with a safe winner.
         * @param time    Maximal computation time in seconds.
         */
        public static IterativeDeepeningAlphaBetaSearch<STATE, ACTION, PLAYER>
            createFor<STATE, ACTION, PLAYER>(Game<STATE, ACTION, PLAYER> game, double utilMin, double utilMax, int time)
        {
            return new IterativeDeepeningAlphaBetaSearch<STATE, ACTION, PLAYER>(game, utilMin, utilMax, time);
        }

        /**
         * Creates a new search object for a given game.
         *
         * @param game    The game.
         * @param utilMin Utility value of worst state for this player. Supports
         *                evaluation of non-terminal states and early termination in
         *                situations with a safe winner.
         * @param utilMax Utility value of best state for this player. Supports
         *                evaluation of non-terminal states and early termination in
         *                situations with a safe winner.
         * @param time    Maximal computation time in seconds.
         */
        public IterativeDeepeningAlphaBetaSearch(Game<S, A, P> game, double utilMin, double utilMax, int time)
        {
            this.game = game;
            this.utilMin = utilMin;
            this.utilMax = utilMax;
            this.timer = new Timer(time);
        }

        public void setLogEnabled(bool b)
        {
            logEnabled = b;
        }

        /**
         * Template method controlling the search. It is based on iterative
         * deepening and tries to make to a good decision in limited time. Credit
         * goes to Behi Monsio who had the idea of ordering actions by utility in
         * subsequent depth-limited search runs.
         */
        public A makeDecision(S state)
        {
            metrics = new Dictionary<string, double>();
            StringBuilder logText = null;
            P player = game.getPlayer(state);
            List<A> results = orderActions(state, game.getActions(state), player, 0);
            timer.start();
            currDepthLimit = 0;
            do
            {
                incrementDepthLimit();
                if (logEnabled)
                    logText = new StringBuilder("depth " + currDepthLimit + ": ");
                heuristicEvaluationUsed = false;
                ActionStore newResults = new ActionStore();
                foreach (A action in results)
                {
                    double value = minValue(game.getResult(state, action), player, double.NegativeInfinity, double.PositiveInfinity, 1);
                    if (timer.timeOutOccurred())
                        break; // exit from action loop
                    newResults.add(action, value);
                    if (logEnabled)
                        logText.Append(action)
                               .Append("->")
                               .Append(value)
                               .Append(" ");
                }
                if (logEnabled)
                    Console.WriteLine(logText);

                if (newResults.size() > 0)
                {
                    results = newResults.actions;
                    if (!timer.timeOutOccurred())
                    {
                        if (hasSafeWinner(newResults.utilValues[0]))
                            break; // exit from iterative deepening loop
                        else if (newResults.size() > 1
                                && isSignificantlyBetter(newResults.utilValues[0], newResults.utilValues[1]))
                            break; // exit from iterative deepening loop
                    }
                }
            } while (!timer.timeOutOccurred() && heuristicEvaluationUsed);
            return results[0];
        }

        // returns an utility value
        public double maxValue(S state, P player, double alpha, double beta, int depth)
        {
            updateMetrics(depth);
            if (game.isTerminal(state) || depth >= currDepthLimit || timer.timeOutOccurred())
            {
                return eval(state, player);
            }
            else
            {
                double value = double.NegativeInfinity;
                foreach (A action in orderActions(state, game.getActions(state), player, depth))
                {
                    value = Math.Max(value, minValue(game.getResult(state, action), //
                            player, alpha, beta, depth + 1));
                    if (value >= beta)
                        return value;
                    alpha = Math.Max(alpha, value);
                }
                return value;
            }
        }

        // returns an utility value
        public double minValue(S state, P player, double alpha, double beta, int depth)
        {
            updateMetrics(depth);
            if (game.isTerminal(state) || depth >= currDepthLimit || timer.timeOutOccurred())
            {
                return eval(state, player);
            }
            else
            {
                double value = double.PositiveInfinity;
                foreach (A action in orderActions(state, game.getActions(state), player, depth))
                {
                    value = Math.Min(value, maxValue(game.getResult(state, action), //
                            player, alpha, beta, depth + 1));
                    if (value <= alpha)
                        return value;
                    beta = Math.Min(beta, value);
                }
                return value;
            }
        }

        private void updateMetrics(int depth)
        {
            ++metrics[METRICS_NODES_EXPANDED];
            metrics.Add(METRICS_MAX_DEPTH, Math.Max(metrics[METRICS_MAX_DEPTH], depth));
        }

        /**
         * Returns some statistic data from the last search.
         */
        public IDictionary<string, double> getMetrics()
        {
            return metrics;
        }

        /**
         * Primitive operation which is called at the beginning of one depth limited
         * search step. This implementation increments the current depth limit by
         * one.
         */
        protected void incrementDepthLimit()
        {
            currDepthLimit++;
        }

        /**
         * Primitive operation which is used to stop iterative deepening search in
         * situations where a clear best action exists. This implementation returns
         * always false.
         */
        protected bool isSignificantlyBetter(double newUtility, double utility)
        {
            return false;
        }

        /**
         * Primitive operation which is used to stop iterative deepening search in
         * situations where a safe winner has been identified. This implementation
         * returns true if the given value (for the currently preferred action
         * result) is the highest or lowest utility value possible.
         */
        protected bool hasSafeWinner(double resultUtility)
        {
            return resultUtility <= utilMin || resultUtility >= utilMax;
        }

        /**
         * Primitive operation, which estimates the value for (not necessarily
         * terminal) states. This implementation returns the utility value for
         * terminal states and <code>(utilMin + utilMax) / 2</code> for non-terminal
         * states. When overriding, first call the super implementation!
         */
        protected double eval(S state, P player)
        {
            if (game.isTerminal(state))
            {
                return game.getUtility(state, player);
            }
            else
            {
                heuristicEvaluationUsed = true;
                return (utilMin + utilMax) / 2;
            }
        }

        /**
         * Primitive operation for action ordering. This implementation preserves
         * the original order (provided by the game).
         */
        public List<A> orderActions(S state, List<A> actions, P player, int depth)
        {
            return actions;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        // nested helper classes

        class Timer
        {
            private long duration;
            private DateTime startTime;

            public Timer(int maxSeconds)
            {
                this.duration = 1000 * maxSeconds;
            }

            public void start()
            {
                startTime = System.DateTime.Now;
            }

            public bool timeOutOccurred()
            {
                return (DateTime.Now - startTime).TotalMilliseconds > duration;
            }
        }

        /**
         * Orders actions by utility.
         */
        class ActionStore
        {
            public List<A> actions = new List<A>();
            public List<double> utilValues = new List<double>();

            public void add(A action, double utilValue)
            {
                int idx = 0;
                while (idx < actions.Count && utilValue <= utilValues[idx])
                    idx++;
                actions.Insert(idx, action);
                utilValues.Insert(idx, utilValue);
            }

            public int size()
            {
                return actions.Count;
            }
        }
    }
}
