using tvn.cosine.text;

namespace tvn.cosine.ai.libsvm
{
    public class svm_predict
    {
        private static double atof(string s)
        {
            return TextFactory.ParseDouble(s);
        }

        private static int atoi(string s)
        {
            return TextFactory.ParseInt(s);
        }

        private static void predict(System.IO.StreamReader input, System.IO.StreamWriter output, svm_model model, int predict_probability)
        {
            int correct = 0;
            int total = 0;
            double error = 0;
            double sumv = 0, sumy = 0, sumvv = 0, sumyy = 0, sumvy = 0;

            int svm_type = svm.svm_get_svm_type(model);
            int nr_class = svm.svm_get_nr_class(model);
            double[] prob_estimates = null;

            if (predict_probability == 1)
            {
                if (svm_type == svm_parameter.EPSILON_SVR ||
                   svm_type == svm_parameter.NU_SVR)
                {
                    System.Console.WriteLine("Prob. model for test data: target value = predicted value + z,\nz: Laplace distribution e^(-|z|/sigma)/(2sigma),sigma=" + svm.svm_get_svr_probability(model) + "\n");
                }
                else
                {
                    int[] labels = new int[nr_class];
                    svm.svm_get_labels(model, labels);
                    prob_estimates = new double[nr_class];
                    output.Write("labels");
                    for (int j = 0; j < nr_class; j++)
                        output.Write(" " + labels[j]);
                    output.Write("\n");
                }
            }
            while (true)
            {
                string line = input.ReadLine();
                if (line == null) break;

                var st = line.Split(new[] { ' ', '\t', '\n', '\r', '\f', ':' });
                int counter = 0;

                double target = atof(st[counter++]);
                int m = (st.Length - counter) / 2;
                svm_node[] x = new svm_node[m];
                for (int j = 0; j < m; j++)
                {
                    x[j] = new svm_node();
                    x[j].index = atoi(st[counter++]);
                    x[j].value = atof(st[counter++]);
                }

                double v;
                if (predict_probability == 1 && (svm_type == svm_parameter.C_SVC || svm_type == svm_parameter.NU_SVC))
                {
                    v = svm.svm_predict_probability(model, x, prob_estimates);
                    output.Write(v + " ");
                    for (int j = 0; j < nr_class; j++)
                        output.Write(prob_estimates[j] + " ");
                    output.Write("\n");
                }
                else
                {
                    v = svm.svm_predict(model, x);
                    output.Write(v + "\n");
                }

                if (v == target)
                    ++correct;
                error += (v - target) * (v - target);
                sumv += v;
                sumy += target;
                sumvv += v * v;
                sumyy += target * target;
                sumvy += v * target;
                ++total;
            }
            if (svm_type == svm_parameter.EPSILON_SVR ||
               svm_type == svm_parameter.NU_SVR)
            {
                System.Console.WriteLine("Mean squared error = " + error / total + " (regression)\n");
                System.Console.WriteLine("Squared correlation coefficient = " +
                                 ((total * sumvy - sumv * sumy) * (total * sumvy - sumv * sumy)) /
                                 ((total * sumvv - sumv * sumv) * (total * sumyy - sumy * sumy)) +
                                 " (regression)\n");
            }
            else
                System.Console.WriteLine("Accuracy = " + (double)correct / total * 100 +
                     "% (" + correct + "/" + total + ") (classification)\n");
        }

        private static void exit_with_help()
        {
            System.Console.WriteLine("usage: svm_predict [options] test_file model_file output_file\n"
            + "options:\n"
            + "-b probability_estimates: whether to predict probability estimates, 0 or 1 (default 0); one-class SVM not supported yet\n");
            System.Environment.Exit(1);
        }

        public static void Main(params string[] argv)
        {
            int i, predict_probability = 0;

            // parse options
            for (i = 0; i < argv.Length; i++)

            {
                if (argv[i][0] != '-') break;
                ++i;
                switch (argv[i - 1][1])
                {
                    case 'b':
                        predict_probability = atoi(argv[i]);
                        break;
                    default:
                        System.Console.WriteLine("Unknown option: " + argv[i - 1] + "\n");
                        exit_with_help();
                        break;
                }
            }
            if (i >= argv.Length - 2)

                exit_with_help();

            System.IO.StreamReader input = new System.IO.StreamReader(argv[i]);
            System.IO.StreamWriter output = new System.IO.StreamWriter(argv[i + 2]);
            svm_model model = svm.svm_load_model(argv[i + 1]);
            if (predict_probability == 1)
            {
                if (svm.svm_check_probability_model(model) == 0)
                {
                    System.Console.WriteLine("Model does not support probabiliy estimates\n");
                    System.Environment.Exit(1);
                }
            }
            else
            {
                if (svm.svm_check_probability_model(model) != 0)
                {
                    System.Console.WriteLine("Model supports probability estimates, but disabled in prediction.\n");
                }
            }
            predict(input, output, model, predict_probability);
            input.Close();
            output.Close();
        }
    }

}
