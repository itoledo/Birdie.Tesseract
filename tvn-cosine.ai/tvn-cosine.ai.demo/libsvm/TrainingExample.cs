using System.Collections.Generic;
using System.IO;
using tvn.cosine.ai.libsvm;
using tvn.cosine.text;

namespace tvn_cosine.ai.demo.libsvm
{
    class TrainingExample
    {
        private svm_parameter param;        // set by parse_command_line
        private svm_problem prob;           // set by read_problem
        private svm_model model;
        private string input_file_name;     // set by parse_command_line
        private string model_file_name;     // set by parse_command_line
        private string error_msg;
        private int cross_validation;
        private int nr_fold;

        private static svm_print_interface svm_print_null = new NONEsvm_print_interface();

        private static void exit_with_help()
        {
            System.Console.WriteLine(
             "Usage: svm_train [options] training_set_file [model_file]\n"
            + "options:\n"
            + "-s svm_type : set type of SVM (default 0)\n"
            + "	0 -- C-SVC\n"
            + "	1 -- nu-SVC\n"
            + "	2 -- one-class SVM\n"
            + "	3 -- epsilon-SVR\n"
            + "	4 -- nu-SVR\n"
            + "-t kernel_type : set type of kernel function (default 2)\n"
            + "	0 -- linear: u'*v\n"
            + "	1 -- polynomial: (gamma*u'*v + coef0)^degree\n"
            + "	2 -- radial basis function: exp(-gamma*|u-v|^2)\n"
            + "	3 -- sigmoid: tanh(gamma*u'*v + coef0)\n"
            + "	4 -- precomputed kernel (kernel values in training_set_file)\n"
            + "-d degree : set degree in kernel function (default 3)\n"
            + "-g gamma : set gamma in kernel function (default 1/num_features)\n"
            + "-r coef0 : set coef0 in kernel function (default 0)\n"
            + "-c cost : set the parameter C of C-SVC, epsilon-SVR, and nu-SVR (default 1)\n"
            + "-n nu : set the parameter nu of nu-SVC, one-class SVM, and nu-SVR (default 0.5)\n"
            + "-p epsilon : set the epsilon in loss function of epsilon-SVR (default 0.1)\n"
            + "-m cachesize : set cache memory size in MB (default 100)\n"
            + "-e epsilon : set tolerance of termination criterion (default 0.001)\n"
            + "-h shrinking : whether to use the shrinking heuristics, 0 or 1 (default 1)\n"
            + "-b probability_estimates : whether to train a SVC or SVR model for probability estimates, 0 or 1 (default 0)\n"
            + "-wi weight : set the parameter C of class i to weight*C, for C-SVC (default 1)\n"
            + "-v n : n-fold cross validation mode\n"
            + "-q : quiet mode (no outputs)\n"
            );
            System.Console.ReadLine();
            System.Environment.Exit(1);
        }

        private void do_cross_validation()
        {
            int i;
            int total_correct = 0;
            double total_error = 0;
            double sumv = 0, sumy = 0, sumvv = 0, sumyy = 0, sumvy = 0;
            double[] target = new double[prob.l];

            svm.svm_cross_validation(prob, param, nr_fold, target);
            if (param.svm_type == svm_parameter.EPSILON_SVR ||
               param.svm_type == svm_parameter.NU_SVR)
            {
                for (i = 0; i < prob.l; i++)
                {
                    double y = prob.y[i];
                    double v = target[i];
                    total_error += (v - y) * (v - y);
                    sumv += v;
                    sumy += y;
                    sumvv += v * v;
                    sumyy += y * y;
                    sumvy += v * y;
                }
                System.Console.WriteLine("Cross Validation Mean squared error = " + total_error / prob.l + "\n");
                System.Console.WriteLine("Cross Validation Squared correlation coefficient = " +
                    ((prob.l * sumvy - sumv * sumy) * (prob.l * sumvy - sumv * sumy)) /
                    ((prob.l * sumvv - sumv * sumv) * (prob.l * sumyy - sumy * sumy)) + "\n"
                    );
            }
            else
            {
                for (i = 0; i < prob.l; i++)
                    if (target[i] == prob.y[i])
                        ++total_correct;
                System.Console.WriteLine("Cross Validation Accuracy = " + 100.0 * total_correct / prob.l + "%\n");
            }
        }

        private void run(params string[] argv)
        {
            parse_command_line(argv);
            read_problem();
            error_msg = svm.svm_check_parameter(prob, param);

            if (error_msg != null)

            {
                System.Console.WriteLine("ERROR: " + error_msg + "\n");
                System.Environment.Exit(1);
            }

            if (cross_validation != 0)

            {
                do_cross_validation();
            }
            else

            {
                model = svm.svm_train(prob, param);
                svm.svm_save_model(model_file_name, model);
            }
        }

        public static void Main(params string[] argv)
        {
            string[] args = new string[] { "heart_scale.data" };
            TrainingExample t = new TrainingExample();
            t.run(args);
        }

        private static double atof(string s)
        {
            double d = TextFactory.ParseDouble(s);
            if (double.IsNaN(d) || double.IsInfinity(d))
            {
                System.Console.WriteLine("NaN or Infinity in input\n");
                System.Environment.Exit(1);
            }
            return (d);
        }

        private static int atoi(string s)
        {
            return TextFactory.ParseInt(s);
        }

        private void parse_command_line(params string[] argv)
        {
            int i;
            svm_print_interface print_func = null;  // default printing to stdout

            param = new svm_parameter();
            // default values
            param.svm_type = svm_parameter.C_SVC;
            param.kernel_type = svm_parameter.RBF;
            param.degree = 3;
            param.gamma = 0;    // 1/num_features
            param.coef0 = 0;
            param.nu = 0.5;
            param.cache_size = 100;
            param.C = 1;
            param.eps = 1e-3;
            param.p = 0.1;
            param.shrinking = 1;
            param.probability = 0;
            param.nr_weight = 0;
            param.weight_label = new int[0];
            param.weight = new double[0];
            cross_validation = 0;

            // parse options
            for (i = 0; i < argv.Length; i++)
            {
                if (argv[i][0] != '-') break;
                if (++i >= argv.Length)
                    exit_with_help();
                switch (argv[i - 1][1])
                {
                    case 's':
                        param.svm_type = atoi(argv[i]);
                        break;
                    case 't':
                        param.kernel_type = atoi(argv[i]);
                        break;
                    case 'd':
                        param.degree = atoi(argv[i]);
                        break;
                    case 'g':
                        param.gamma = atof(argv[i]);
                        break;
                    case 'r':
                        param.coef0 = atof(argv[i]);
                        break;
                    case 'n':
                        param.nu = atof(argv[i]);
                        break;
                    case 'm':
                        param.cache_size = atof(argv[i]);
                        break;
                    case 'c':
                        param.C = atof(argv[i]);
                        break;
                    case 'e':
                        param.eps = atof(argv[i]);
                        break;
                    case 'p':
                        param.p = atof(argv[i]);
                        break;
                    case 'h':
                        param.shrinking = atoi(argv[i]);
                        break;
                    case 'b':
                        param.probability = atoi(argv[i]);
                        break;
                    case 'q':
                        print_func = svm_print_null;
                        i--;
                        break;
                    case 'v':
                        cross_validation = 1;
                        nr_fold = atoi(argv[i]);
                        if (nr_fold < 2)
                        {
                            System.Console.WriteLine("n-fold cross validation: n must >= 2\n");
                            exit_with_help();
                        }
                        break;
                    case 'w':
                        ++param.nr_weight;
                        {
                            int[] old = param.weight_label;
                            param.weight_label = new int[param.nr_weight];
                            System.Array.Copy(old, 0, param.weight_label, 0, param.nr_weight - 1);
                        }

                        {
                            double[] old = param.weight;
                            param.weight = new double[param.nr_weight];
                            System.Array.Copy(old, 0, param.weight, 0, param.nr_weight - 1);
                        }

                        param.weight_label[param.nr_weight - 1] = atoi(argv[i - 1].Substring(2));
                        param.weight[param.nr_weight - 1] = atof(argv[i]);
                        break;
                    default:
                        System.Console.WriteLine("Unknown option: " + argv[i - 1] + "\n");
                        exit_with_help();
                        break;
                }
            }

            svm.svm_set_print_string_function(print_func);

            // determine filenames

            if (i >= argv.Length)
                exit_with_help();

            input_file_name = argv[i];

            if (i < argv.Length - 1)
                model_file_name = argv[i + 1];
            else
            {
                int p = argv[i].LastIndexOf('/');
                ++p;    // whew...
                model_file_name = argv[i].Substring(p) + ".model";
            }
        }

        // read in a problem (in svmlight format)

        private void read_problem()
        {
            StreamReader fp = new StreamReader(input_file_name);
            List<double> vy = new List<double>();
            List<svm_node[]> vx = new List<svm_node[]>();
            int max_index = 0;

            while (true)
            {
                string line = fp.ReadLine();
                if (line == null) break;

                int counter = 0;
                var st = line.Split(new[] { ' ', '\t', '\n', '\r', '\f', ':' });

                vy.Add(atof(st[counter++]));
                int m = (st.Length - counter) / 2;
                svm_node[] x = new svm_node[m];
                for (int j = 0; j < m; j++)
                {
                    x[j] = new svm_node();
                    x[j].index = atoi(st[counter++]);
                    x[j].value = atof(st[counter++]);
                }
                if (m > 0) max_index = System.Math.Max(max_index, x[m - 1].index);
                vx.Add(x);
            }

            prob = new svm_problem();
            prob.l = vy.Count;
            prob.x = new svm_node[prob.l][];
            for (int i = 0; i < prob.l; i++)
                prob.x[i] = vx[i];
            prob.y = new double[prob.l];
            for (int i = 0; i < prob.l; i++)
                prob.y[i] = vy[i];

            if (param.gamma == 0 && max_index > 0)
                param.gamma = 1.0 / max_index;

            if (param.kernel_type == svm_parameter.PRECOMPUTED)
                for (int i = 0; i < prob.l; i++)
                {
                    if (prob.x[i][0].index != 0)
                    {
                        System.Console.WriteLine("Wrong kernel matrix: first column must be 0:sample_serial_number\n");
                        System.Environment.Exit(1);
                    }
                    if ((int)prob.x[i][0].value <= 0 || (int)prob.x[i][0].value > max_index)
                    {
                        System.Console.WriteLine("Wrong input format: sample_serial_number out of range\n");
                        System.Environment.Exit(1);
                    }
                }

            fp.Close();
        }
    }
}
