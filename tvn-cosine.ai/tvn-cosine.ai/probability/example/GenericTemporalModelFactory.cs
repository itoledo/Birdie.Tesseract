using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.probability.bayes;
using tvn.cosine.ai.probability.bayes.impl;
using tvn.cosine.ai.probability.bayes.model;

namespace tvn.cosine.ai.probability.example
{
    public class GenericTemporalModelFactory
    {
        public static FiniteProbabilityModel getUmbrellaWorldTransitionModel()
        {
            return getUmbrellaWorldModel();
        }

        public static FiniteProbabilityModel getUmbrellaWorldSensorModel()
        {
            return getUmbrellaWorldModel();
        }

        public static FiniteProbabilityModel getUmbrellaWorldModel()
        {
            // Prior belief state
            FiniteNode rain_tm1 = new FullCPTNode(ExampleRV.RAIN_tm1_RV,
                    new double[] { 0.5, 0.5 });
            // Transition Model
            FiniteNode rain_t = new FullCPTNode(ExampleRV.RAIN_t_RV, new double[] {
				// R_t-1 = true, R_t = true
				0.7,
				// R_t-1 = true, R_t = false
				0.3,
				// R_t-1 = false, R_t = true
				0.3,
				// R_t-1 = false, R_t = false
				0.7 }, rain_tm1);
            // Sensor Model 
            FiniteNode umbrealla_t = new FullCPTNode(ExampleRV.UMBREALLA_t_RV,
                    new double[] {
						// R_t = true, U_t = true
						0.9,
						// R_t = true, U_t = false
						0.1,
						// R_t = false, U_t = true
						0.2,
						// R_t = false, U_t = false
						0.8 }, rain_t);

            return new FiniteBayesModel(new BayesNet(rain_tm1));
        }

        public static IMap<RandomVariable, RandomVariable> getUmbrellaWorld_Xt_to_Xtm1_Map()
        {
            IMap<RandomVariable, RandomVariable> tToTm1StateVarMap = Factory.CreateInsertionOrderedMap<RandomVariable, RandomVariable>();
            tToTm1StateVarMap.Put(ExampleRV.RAIN_t_RV, ExampleRV.RAIN_tm1_RV);

            return tToTm1StateVarMap;
        }
    } 
}
