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

        public static FiniteProbabilityModel<bool> getUmbrellaWorldTransitionModel()
        {
            return getUmbrellaWorldModel();
        }

        public static FiniteProbabilityModel<bool> getUmbrellaWorldSensorModel()
        {
            return getUmbrellaWorldModel();
        }

        public static FiniteProbabilityModel<bool> getUmbrellaWorldModel()
        {
            // Prior belief state
            FiniteNode<bool> rain_tm1 = new FullCPTNode<bool>(ExampleRV.RAIN_tm1_RV, new double[] { 0.5, 0.5 });
            // Transition Model
            FiniteNode<bool> rain_t = new FullCPTNode<bool>(ExampleRV.RAIN_t_RV, new double[] {
				// R_t-1 = true, R_t = true
				0.7,
				// R_t-1 = true, R_t = false
				0.3,
				// R_t-1 = false, R_t = true
				0.3,
				// R_t-1 = false, R_t = false
				0.7 }, rain_tm1);
            // Sensor Model 

            FiniteNode<bool> umbrealla_t = new FullCPTNode<bool>(ExampleRV.UMBREALLA_t_RV,
                    new double[] {
						// R_t = true, U_t = true
						0.9,
						// R_t = true, U_t = false
						0.1,
						// R_t = false, U_t = true
						0.2,
						// R_t = false, U_t = false
						0.8 }, rain_t);

            return new FiniteBayesModel<bool>(new BayesNet<bool>(rain_tm1));
        }

        public static IDictionary<RandomVariable, RandomVariable> getUmbrellaWorld_Xt_to_Xtm1_Map()
        {
            IDictionary<RandomVariable, RandomVariable> tToTm1StateVarMap = new Dictionary<RandomVariable, RandomVariable>();
            tToTm1StateVarMap.Add(ExampleRV.RAIN_t_RV, ExampleRV.RAIN_tm1_RV);

            return tToTm1StateVarMap;
        }
    } 
}
