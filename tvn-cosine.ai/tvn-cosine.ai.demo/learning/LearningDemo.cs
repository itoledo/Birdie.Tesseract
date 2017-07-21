using System.Text;
using tvn.cosine.ai.common;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.exceptions;
using tvn.cosine.ai.environment.cellworld;
using tvn.cosine.ai.learning.framework;
using tvn.cosine.ai.learning.inductive;
using tvn.cosine.ai.learning.learners;
using tvn.cosine.ai.learning.neural;
using tvn.cosine.ai.learning.reinforcement.agent;
using tvn.cosine.ai.learning.reinforcement.example;
using tvn.cosine.ai.probability.example;
using tvn.cosine.ai.probability.mdp.impl;
using tvn.cosine.ai.util;

namespace tvn_cosine.ai.demo.learning
{
    public class LearningDemo
    {
        public static void Main(params string[] args)
        {
            // Chapter 21
            passiveADPAgentDemo();
            passiveTDAgentDemo();
            //qLearningAgentDemo();

            // Chapter 18

            backPropogationDemo();
            perceptronDemo();
            decisionListDemo();
            ensembleLearningDemo();
            decisionTreeDemo();

        }

        public static void decisionTreeDemo()
        {
            System.Console.WriteLine(Util.ntimes("*", 100));
            System.Console.WriteLine("\nDecisionTree Demo - Inducing a DecisionList from the Restaurant DataSet\n ");
            System.Console.WriteLine(Util.ntimes("*", 100));
            try
            {
                DataSet ds = DataSetFactory.getRestaurantDataSet();
                DecisionTreeLearner learner = new DecisionTreeLearner();
                learner.train(ds);
                System.Console.WriteLine("The Induced Decision Tree is ");
                System.Console.WriteLine(learner.getDecisionTree());
                int[] result = learner.test(ds);

                System.Console.WriteLine("\nThis Decision Tree classifies the data set with "
                                + result[0]
                                + " successes"
                                + " and "
                                + result[1]
                                + " failures");
                System.Console.WriteLine("\n");
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Decision Tree Demo Failed  ");
                throw e;
            }
        }

        public static void decisionListDemo()
        {
            try
            {
                System.Console.WriteLine(Util.ntimes("*", 100));
                System.Console.WriteLine("DecisionList Demo - Inducing a DecisionList from the Restaurant DataSet\n ");
                System.Console.WriteLine(Util.ntimes("*", 100));
                DataSet ds = DataSetFactory.getRestaurantDataSet();
                DecisionListLearner learner = new DecisionListLearner("Yes", "No", new DLTestFactory());
                learner.train(ds);
                System.Console.WriteLine("The Induced DecisionList is");
                System.Console.WriteLine(learner.getDecisionList());
                int[] result = learner.test(ds);

                System.Console.WriteLine("\nThis Decision List classifies the data set with "
                                + result[0]
                                + " successes"
                                + " and "
                                + result[1]
                                + " failures");
                System.Console.WriteLine("\n");

            }
            catch (Exception)
            {
                System.Console.WriteLine("Decision ListDemo Failed");
            }
        }

        public static void ensembleLearningDemo()
        {
            System.Console.WriteLine(Util.ntimes("*", 100));
            System.Console.WriteLine("\n Ensemble Decision Demo - Weak Learners co operating to give Superior decisions ");
            System.Console.WriteLine(Util.ntimes("*", 100));
            try
            {
                DataSet ds = DataSetFactory.getRestaurantDataSet();
                IQueue<DecisionTree> stumps = DecisionTree.getStumpsFor(ds, "Yes", "No");
                IQueue<Learner> learners = Factory.CreateQueue<Learner>();

                System.Console.WriteLine("\nStump Learners vote to decide in this algorithm");
                foreach (object stump in stumps)
                {
                    DecisionTree sl = (DecisionTree)stump;
                    StumpLearner stumpLearner = new StumpLearner(sl, "No");
                    learners.Add(stumpLearner);
                }
                AdaBoostLearner learner = new AdaBoostLearner(learners, ds);
                learner.train(ds);
                int[] result = learner.test(ds);
                System.Console
                    .WriteLine("\nThis Ensemble Learner  classifies the data set with "
                            + result[0]
                            + " successes"
                            + " and "
                            + result[1]
                            + " failures");
                System.Console.WriteLine("\n");

            }
            catch (Exception e)
            {
                throw e;

            }
        }

        public static void perceptronDemo()
        {
            try
            {
                System.Console.WriteLine(Util.ntimes("*", 100));
                System.Console.WriteLine("\n Perceptron Demo - Running Perceptron on Iris data Set with 10 epochs of learning ");
                System.Console.WriteLine(Util.ntimes("*", 100));
                DataSet irisDataSet = DataSetFactory.getIrisDataSet();
                Numerizer numerizer = new IrisDataSetNumerizer();
                NNDataSet innds = new IrisNNDataSet();

                innds.createExamplesFromDataSet(irisDataSet, numerizer);

                Perceptron perc = new Perceptron(3, 4);

                perc.trainOn(innds, 10);

                innds.refreshDataset();
                int[] result = perc.testOnDataSet(innds);
                System.Console.WriteLine(result[0] + " right, " + result[1] + " wrong");
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public static void backPropogationDemo()
        {
            try
            {
                System.Console.WriteLine(Util.ntimes("*", 100));
                System.Console
                    .WriteLine("\n BackpropagationDemo  - Running BackProp on Iris data Set with 10 epochs of learning ");
                System.Console.WriteLine(Util.ntimes("*", 100));

                DataSet irisDataSet = DataSetFactory.getIrisDataSet();
                Numerizer numerizer = new IrisDataSetNumerizer();
                NNDataSet innds = new IrisNNDataSet();

                innds.createExamplesFromDataSet(irisDataSet, numerizer);

                NNConfig config = new NNConfig();
                config.setConfig(FeedForwardNeuralNetwork.NUMBER_OF_INPUTS, 4);
                config.setConfig(FeedForwardNeuralNetwork.NUMBER_OF_OUTPUTS, 3);
                config.setConfig(FeedForwardNeuralNetwork.NUMBER_OF_HIDDEN_NEURONS,
                        6);
                config.setConfig(FeedForwardNeuralNetwork.LOWER_LIMIT_WEIGHTS, -2.0);
                config.setConfig(FeedForwardNeuralNetwork.UPPER_LIMIT_WEIGHTS, 2.0);

                FeedForwardNeuralNetwork ffnn = new FeedForwardNeuralNetwork(config);
                ffnn.setTrainingScheme(new BackPropLearning(0.1, 0.9));

                ffnn.trainOn(innds, 10);

                innds.refreshDataset();
                int[] result = ffnn.testOnDataSet(innds);
                System.Console.WriteLine(result[0] + " right, " + result[1] + " wrong");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void passiveADPAgentDemo()
        {
            System.Console.WriteLine("=======================");
            System.Console.WriteLine("DEMO: Passive-ADP-Agent");
            System.Console.WriteLine("=======================");
            System.Console.WriteLine("Figure 21.3");
            System.Console.WriteLine("-----------");

            CellWorld<double> cw = CellWorldFactory.createCellWorldForFig17_1();
            CellWorldEnvironment cwe = new CellWorldEnvironment(
                    cw.getCellAt(1, 1),
                    cw.getCells(),
                    MDPFactory.createTransitionProbabilityFunctionForFigure17_1(cw),
                    new DefaultRandom());

            IMap<Cell<double>, CellWorldAction> fixedPolicy = Factory.CreateInsertionOrderedMap<Cell<double>, CellWorldAction>();
            fixedPolicy.Put(cw.getCellAt(1, 1), CellWorldAction.Up);
            fixedPolicy.Put(cw.getCellAt(1, 2), CellWorldAction.Up);
            fixedPolicy.Put(cw.getCellAt(1, 3), CellWorldAction.Right);
            fixedPolicy.Put(cw.getCellAt(2, 1), CellWorldAction.Left);
            fixedPolicy.Put(cw.getCellAt(2, 3), CellWorldAction.Right);
            fixedPolicy.Put(cw.getCellAt(3, 1), CellWorldAction.Left);
            fixedPolicy.Put(cw.getCellAt(3, 2), CellWorldAction.Up);
            fixedPolicy.Put(cw.getCellAt(3, 3), CellWorldAction.Right);
            fixedPolicy.Put(cw.getCellAt(4, 1), CellWorldAction.Left);

            PassiveADPAgent<Cell<double>, CellWorldAction> padpa = new PassiveADPAgent<Cell<double>, CellWorldAction>(
                    fixedPolicy, cw.getCells(), cw.getCellAt(1, 1),
                    MDPFactory.createActionsFunctionForFigure17_1(cw),
                    new ModifiedPolicyEvaluation<Cell<double>, CellWorldAction>(10, 1.0));

            cwe.AddAgent(padpa);

            output_utility_learning_rates(padpa, 20, 100, 100, 1);

            System.Console.WriteLine("=========================");
        }

        public static void passiveTDAgentDemo()
        {
            System.Console.WriteLine("======================");
            System.Console.WriteLine("DEMO: Passive-TD-Agent");
            System.Console.WriteLine("======================");
            System.Console.WriteLine("Figure 21.5");
            System.Console.WriteLine("-----------");

            CellWorld<double> cw = CellWorldFactory.createCellWorldForFig17_1();
            CellWorldEnvironment cwe = new CellWorldEnvironment(
                    cw.getCellAt(1, 1),
                    cw.getCells(),
                    MDPFactory.createTransitionProbabilityFunctionForFigure17_1(cw),
                    new DefaultRandom());

            IMap<Cell<double>, CellWorldAction> fixedPolicy = Factory.CreateInsertionOrderedMap<Cell<double>, CellWorldAction>();
            fixedPolicy.Put(cw.getCellAt(1, 1), CellWorldAction.Up);
            fixedPolicy.Put(cw.getCellAt(1, 2), CellWorldAction.Up);
            fixedPolicy.Put(cw.getCellAt(1, 3), CellWorldAction.Right);
            fixedPolicy.Put(cw.getCellAt(2, 1), CellWorldAction.Left);
            fixedPolicy.Put(cw.getCellAt(2, 3), CellWorldAction.Right);
            fixedPolicy.Put(cw.getCellAt(3, 1), CellWorldAction.Left);
            fixedPolicy.Put(cw.getCellAt(3, 2), CellWorldAction.Up);
            fixedPolicy.Put(cw.getCellAt(3, 3), CellWorldAction.Right);
            fixedPolicy.Put(cw.getCellAt(4, 1), CellWorldAction.Left);

            PassiveTDAgent<Cell<double>, CellWorldAction> ptda = new PassiveTDAgent<Cell<double>, CellWorldAction>(
                    fixedPolicy, 0.2, 1.0);

            cwe.AddAgent(ptda);

            output_utility_learning_rates(ptda, 20, 500, 100, 1);

            System.Console.WriteLine("=========================");
        }

        //public static void qLearningAgentDemo()
        //{
        //    System.Console.WriteLine("======================");
        //    System.Console.WriteLine("DEMO: Q-Learning-Agent");
        //    System.Console.WriteLine("======================");

        //    CellWorld<double> cw = CellWorldFactory.createCellWorldForFig17_1();
        //    CellWorldEnvironment cwe = new CellWorldEnvironment(
        //            cw.getCellAt(1, 1),
        //            cw.getCells(),
        //            MDPFactory.createTransitionProbabilityFunctionForFigure17_1(cw),
        //            new DefaultRandom());

        //    QLearningAgent<Cell<double>, CellWorldAction> qla = new QLearningAgent<Cell<double>, CellWorldAction>(
        //            MDPFactory.createActionsFunctionForFigure17_1(cw),
        //            CellWorldAction.None, 0.2, 1.0, 5,
        //            2.0);

        //    cwe.addAgent(qla);

        //    output_utility_learning_rates(qla, 20, 10000, 500, 20);

        //    System.Console.WriteLine("=========================");
        //}

        //
        // PRIVATE METHODS
        //
        private static void output_utility_learning_rates(
                ReinforcementAgent<Cell<double>, CellWorldAction> reinforcementAgent,
                int numRuns, int numTrialsPerRun, int rmseTrialsToReport,
                int reportEveryN)
        {

            if (rmseTrialsToReport > (numTrialsPerRun / reportEveryN))
            {
                throw new IllegalArgumentException(
                        "Requesting to report too many RMSE trials, max allowed for args is "
                                + (numTrialsPerRun / reportEveryN));
            }

            CellWorld<double> cw = CellWorldFactory.createCellWorldForFig17_1();
            CellWorldEnvironment cwe = new CellWorldEnvironment(
                    cw.getCellAt(1, 1),
                    cw.getCells(),
                    MDPFactory.createTransitionProbabilityFunctionForFigure17_1(cw),
                    new DefaultRandom());

            cwe.AddAgent(reinforcementAgent);

            IMap<int, IQueue<IMap<Cell<double>, double>>> runs = Factory.CreateInsertionOrderedMap<int, IQueue<IMap<Cell<double>, double>>>();
            for (int r = 0; r < numRuns; r++)
            {
                reinforcementAgent.reset();
                IQueue<IMap<Cell<double>, double>> trials = Factory.CreateQueue<IMap<Cell<double>, double>>();
                for (int t = 0; t < numTrialsPerRun; t++)
                {
                    cwe.executeTrial();
                    if (0 == t % reportEveryN)
                    {
                        IMap<Cell<double>, double> u = reinforcementAgent
                                .getUtility();
                        //if (null == u.Get(cw.getCellAt(1, 1)))
                        //{
                        //    throw new IllegalStateException(
                        //            "Bad Utility State Encountered: r=" + r
                        //                    + ", t=" + t + ", u=" + u);
                        //}
                        trials.Add(u);
                    }
                }
                runs.Put(r, trials);
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
                IMap<Cell<double>, double> u = runs.Get(numRuns - 1).Get(t);
                v4_3.Append((u.ContainsKey(cw.getCellAt(4, 3)) ? u.Get(cw
                        .getCellAt(4, 3)) : 0.0) + "\t");
                v3_3.Append((u.ContainsKey(cw.getCellAt(3, 3)) ? u.Get(cw
                        .getCellAt(3, 3)) : 0.0) + "\t");
                v1_3.Append((u.ContainsKey(cw.getCellAt(1, 3)) ? u.Get(cw
                        .getCellAt(1, 3)) : 0.0) + "\t");
                v1_1.Append((u.ContainsKey(cw.getCellAt(1, 1)) ? u.Get(cw
                        .getCellAt(1, 1)) : 0.0) + "\t");
                v3_2.Append((u.ContainsKey(cw.getCellAt(3, 2)) ? u.Get(cw
                        .getCellAt(3, 2)) : 0.0) + "\t");
                v2_1.Append((u.ContainsKey(cw.getCellAt(2, 1)) ? u.Get(cw
                        .getCellAt(2, 1)) : 0.0) + "\t");
            }

            StringBuilder rmseValues = new StringBuilder();
            for (int t = 0; t < rmseTrialsToReport; t++)
            {
                // Calculate the Root Mean Square Error for utility of 1,1
                // for this trial# across all runs
                double xSsquared = 0;
                for (int r = 0; r < numRuns; r++)
                {
                    IMap<Cell<double>, double> u = runs.Get(r).Get(t);
                    double val1_1 = u.Get(cw.getCellAt(1, 1));
                    //if (null == val1_1)
                    //{
                    //    throw new IllegalStateException(
                    //            "U(1,1,) is not present: r=" + r + ", t=" + t
                    //                    + ", runs.size=" + runs.Size()
                    //                    + ", runs(r).Size()=" + runs.Get(r).Size()
                    //                    + ", u=" + u);
                    //}
                    xSsquared += System.Math.Pow(0.705 - val1_1, 2);
                }
                double rmse = System.Math.Sqrt(xSsquared / runs.Size());
                rmseValues.Append(rmse);
                rmseValues.Append("\t");
            }

            System.Console
                .WriteLine("Note: You may copy and paste the following lines into a spreadsheet to generate graphs of learning rate and RMS error in utility:");
            System.Console.WriteLine("(4,3)" + "\t" + v4_3);
            System.Console.WriteLine("(3,3)" + "\t" + v3_3);
            System.Console.WriteLine("(1,3)" + "\t" + v1_3);
            System.Console.WriteLine("(1,1)" + "\t" + v1_1);
            System.Console.WriteLine("(3,2)" + "\t" + v3_2);
            System.Console.WriteLine("(2,1)" + "\t" + v2_1);
            System.Console.WriteLine("RMSeiu" + "\t" + rmseValues);
        }
    }
}
