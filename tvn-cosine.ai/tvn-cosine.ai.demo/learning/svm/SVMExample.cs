using tvn.cosine.ai.learning.framework;
using tvn.cosine.ai.learning.learners.svm;

namespace tvn_cosine.ai.demo.learning.svm
{
    public class SVMExample
    {
        public static void Main(params string[] args)
        {
            svm_parameter param = new svm_parameter();
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
            param.probability = 1;
            param.nr_weight = 0;
            param.weight_label = new int[0];
            param.weight = new double[0];
            svm_problem problem = DataSetFactory.LoadSVMFile("heart_scale.data", param);
            svm_model model = SupportVectorMachine.svm_train(problem, param);

            int errors = 0;
            for (int i = 0; i < problem.y.Length; ++i)
            {
                double[] prob = new double[2];
                var newPred = SupportVectorMachine.svm_predict_probability(model, problem.x[i], prob);
                var prediction = SupportVectorMachine.svm_predict(model, problem.x[i]);
                if (prediction != problem.y[i])
                {
                    ++errors;

                    System.Console.ForegroundColor = System.ConsoleColor.Red;
                }
                else
                {
                    System.Console.ForegroundColor = System.ConsoleColor.White;
                }
                System.Console.WriteLine("Expected: {0} || Returned: {1}", problem.y[i], prediction);
                System.Console.WriteLine("1: {0}% || 2: {1}\n", prob[0] * 100D, prob[1] * 100D);
            }

            double perc = ((double)errors / problem.y.Length) * 100D;
            System.Console.WriteLine("Errors {0} at {1}%", errors, perc);
            System.Console.ReadLine();
        }
    }
}
