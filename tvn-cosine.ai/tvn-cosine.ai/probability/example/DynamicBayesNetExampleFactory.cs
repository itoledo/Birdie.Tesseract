using System.Collections.Generic;
using tvn.cosine.ai.probability.bayes;
using tvn.cosine.ai.probability.bayes.impl;

namespace tvn.cosine.ai.probability.example
{
    /**
     * 
     * @author Ciaran O'Reilly
     *
     */
    public class DynamicBayesNetExampleFactory
    {
        /**
         * Return a Dynamic Bayesian Network of the Umbrella World Network.
         * 
         * @return a Dynamic Bayesian Network of the Umbrella World Network.
         */
        public static DynamicBayesianNetwork<string> getUmbrellaWorldNetwork()
        {
            FiniteNode<string> prior_rain_tm1 = new FullCPTNode<string>(ExampleRV.RAIN_tm1_RV, new double[] { 0.5, 0.5 });

            BayesNet<string> priorNetwork = new BayesNet<string>(prior_rain_tm1);

            // Prior belief state
            FiniteNode<string> rain_tm1 = new FullCPTNode<string>(ExampleRV.RAIN_tm1_RV, new double[] { 0.5, 0.5 });
            // Transition Model
            FiniteNode<string> rain_t = new FullCPTNode<string>(ExampleRV.RAIN_t_RV, new double[] {
				// R_t-1 = true, R_t = true
				0.7,
				// R_t-1 = true, R_t = false
				0.3,
				// R_t-1 = false, R_t = true
				0.3,
				// R_t-1 = false, R_t = false
				0.7 }, rain_tm1);
            // Sensor Model 

            FiniteNode<string> umbrealla_t = new FullCPTNode<string>(ExampleRV.UMBREALLA_t_RV,
                    new double[] {
						// R_t = true, U_t = true
						0.9,
						// R_t = true, U_t = false
						0.1,
						// R_t = false, U_t = true
						0.2,
						// R_t = false, U_t = false
						0.8 }, rain_t);

            IDictionary<RandomVariable, RandomVariable> X_0_to_X_1 = new Dictionary<RandomVariable, RandomVariable>();
            X_0_to_X_1.Add(ExampleRV.RAIN_tm1_RV, ExampleRV.RAIN_t_RV);
            ISet<RandomVariable> E_1 = new HashSet<RandomVariable>();
            E_1.Add(ExampleRV.UMBREALLA_t_RV);

            return new DynamicBayesNet<string>(priorNetwork, X_0_to_X_1, E_1, rain_tm1);
        }
    }
}
