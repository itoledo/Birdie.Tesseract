using System.Collections.Generic;
using tvn.cosine.ai.probability.bayes;
using tvn.cosine.ai.probability.bayes.impl;
using tvn.cosine.ai.probability.bayes.model;

namespace tvn.cosine.ai.probability.example
{
    /**
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public class GenericTemporalModelFactory
    {

        public static FiniteProbabilityModel<string> getUmbrellaWorldTransitionModel()
        {
            return getUmbrellaWorldModel();
        }

        public static FiniteProbabilityModel<string> getUmbrellaWorldSensorModel()
        {
            return getUmbrellaWorldModel();
        }

        public static FiniteProbabilityModel<string> getUmbrellaWorldModel()
        {
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

            return new FiniteBayesModel<string>(new BayesNet<string>(rain_tm1));
        }

        public static IDictionary<RandomVariable, RandomVariable> getUmbrellaWorld_Xt_to_Xtm1_Map()
        {
            IDictionary<RandomVariable, RandomVariable> tToTm1StateVarMap = new Dictionary<RandomVariable, RandomVariable>();
            tToTm1StateVarMap.Add(ExampleRV.RAIN_t_RV, ExampleRV.RAIN_tm1_RV);

            return tToTm1StateVarMap;
        }
    } 
}
