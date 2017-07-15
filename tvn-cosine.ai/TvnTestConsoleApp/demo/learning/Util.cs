using System;
using System.Collections.Generic;
using System.Text;
using tvn.cosine.ai.common;
using tvn.cosine.ai.environment.cellworld;
using tvn.cosine.ai.learning.reinforcement.agent;
using tvn.cosine.ai.learning.reinforcement.example;
using tvn.cosine.ai.probability.example;

namespace TvnTestConsoleApp.demo.learning
{
    internal static class Util
    {
        internal static void output_utility_learning_rates(
            ReinforcementAgent<Cell<double>, CellWorldAction> reinforcementAgent,
            int numRuns, int numTrialsPerRun, int rmseTrialsToReport,
            int reportEveryN)
        {

            if (rmseTrialsToReport > (numTrialsPerRun / reportEveryN))
            {
                throw new ArgumentException("Requesting to report too many RMSE trials, max allowed for args is " + (numTrialsPerRun / reportEveryN));
            }

            CellWorld<double> cw = CellWorldFactory.CreateCellWorldForFig17_1();
            CellWorldEnvironment cwe = new CellWorldEnvironment(
                    cw.getCellAt(1, 1),
                    cw.GetCells(),
                    MDPFactory.createTransitionProbabilityFunctionForFigure17_1(cw),
                    new DefaultRandom());

            cwe.addAgent(reinforcementAgent);

            IDictionary<int, IList<IDictionary<Cell<double>, double>>> runs = new Dictionary<int, IList<IDictionary<Cell<double>, double>>>();
            for (int r = 0; r < numRuns; r++)
            {
                reinforcementAgent.reset();
                IList<IDictionary<Cell<double>, double>> trials = new List<IDictionary<Cell<double>, double>>();
                for (int t = 0; t < numTrialsPerRun; t++)
                {
                    cwe.executeTrial();
                    if (0 == t % reportEveryN)
                    {
                        IDictionary<Cell<double>, double> u = reinforcementAgent.getUtility();
                        if (!u.ContainsKey(cw.getCellAt(1, 1)))
                        {
                            throw new Exception("Bad Utility State Encountered: r=" + r
                                            + ", t=" + t + ", u=" + u);
                        }
                        trials.Add(u);
                    }
                }
                runs[r] = trials;
            }

            StringBuilder v4_3 = new StringBuilder();
            StringBuilder v3_3 = new StringBuilder();
            StringBuilder v1_3 = new StringBuilder();
            StringBuilder v1_1 = new StringBuilder();
            StringBuilder v3_2 = new StringBuilder();
            StringBuilder v2_1 = new StringBuilder();
            for (int t = 0; t < (numTrialsPerRun / reportEveryN); t++)
            {
                // Use the last run
                IDictionary<Cell<double>, double> u = runs[numRuns - 1][t];
                v4_3.Append((u.ContainsKey(cw.getCellAt(4, 3)) ? u[cw.getCellAt(4, 3)] : 0.0) + "\t");
                v3_3.Append((u.ContainsKey(cw.getCellAt(3, 3)) ? u[cw.getCellAt(3, 3)] : 0.0) + "\t");
                v1_3.Append((u.ContainsKey(cw.getCellAt(1, 3)) ? u[cw.getCellAt(1, 3)] : 0.0) + "\t");
                v1_1.Append((u.ContainsKey(cw.getCellAt(1, 1)) ? u[cw.getCellAt(1, 1)] : 0.0) + "\t");
                v3_2.Append((u.ContainsKey(cw.getCellAt(3, 2)) ? u[cw.getCellAt(3, 2)] : 0.0) + "\t");
                v2_1.Append((u.ContainsKey(cw.getCellAt(2, 1)) ? u[cw.getCellAt(2, 1)] : 0.0) + "\t");
            }

            StringBuilder rmseValues = new StringBuilder();
            for (int t = 0; t < rmseTrialsToReport; t++)
            {
                // Calculate the Root Mean Square Error for utility of 1,1
                // for this trial# across all runs
                double xSsquared = 0;
                for (int r = 0; r < numRuns; r++)
                {
                    IDictionary<Cell<double>, double> u = runs[r][t];
                    if (!u.ContainsKey(cw.getCellAt(1, 1)))
                    {
                        throw new Exception("U(1,1,) is not present: r=" + r + ", t=" + t
                                        + ", runs.size=" + runs.Count
                                        + ", runs(r).size()=" + runs[r].Count
                                        + ", u=" + u);
                    }
                    xSsquared += Math.Pow(0.705 - u[cw.getCellAt(1, 1)], 2);
                }
                double rmse = Math.Sqrt(xSsquared / runs.Count);
                rmseValues.Append(rmse);
                rmseValues.Append("\t");
            }

            Console.WriteLine("Note: You may copy and paste the following lines into a spreadsheet to generate graphs of learning rate and RMS error in utility:");
            Console.WriteLine("(4,3)" + "\t" + v4_3);
            Console.WriteLine("(3,3)" + "\t" + v3_3);
            Console.WriteLine("(1,3)" + "\t" + v1_3);
            Console.WriteLine("(1,1)" + "\t" + v1_1);
            Console.WriteLine("(3,2)" + "\t" + v3_2);
            Console.WriteLine("(2,1)" + "\t" + v2_1);
            Console.WriteLine("RMSeiu" + "\t" + rmseValues);
        }
    }
}
