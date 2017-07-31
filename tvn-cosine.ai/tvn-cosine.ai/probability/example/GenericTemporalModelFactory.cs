using tvn.cosine.collections;
using tvn.cosine.collections.api;
using tvn.cosine.ai.probability.api;
using tvn.cosine.ai.probability.bayes;
using tvn.cosine.ai.probability.bayes.api;
using tvn.cosine.ai.probability.bayes.model;

namespace tvn.cosine.ai.probability.example
{
    public class GenericTemporalModelFactory
    {
        public static IFiniteProbabilityModel getUmbrellaWorldTransitionModel()
        {
            return getUmbrellaWorldModel();
        }

        public static IFiniteProbabilityModel getUmbrellaWorldSensorModel()
        {
            return getUmbrellaWorldModel();
        }

        public static IFiniteProbabilityModel getUmbrellaWorldModel()
        {
            // Prior belief state
            IFiniteNode rain_tm1 = new FullCPTNode(ExampleRV.RAIN_tm1_RV,
                    new double[] { 0.5, 0.5 });
            // Transition Model
            IFiniteNode rain_t = new FullCPTNode(ExampleRV.RAIN_t_RV, new double[] {
				// R_t-1 = true, R_t = true
				0.7,
				// R_t-1 = true, R_t = false
				0.3,
				// R_t-1 = false, R_t = true
				0.3,
				// R_t-1 = false, R_t = false
				0.7 }, rain_tm1);
            // Sensor Model 
            IFiniteNode umbrealla_t = new FullCPTNode(ExampleRV.UMBREALLA_t_RV,
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

        public static IMap<IRandomVariable, IRandomVariable> getUmbrellaWorld_Xt_to_Xtm1_Map()
        {
            IMap<IRandomVariable, IRandomVariable> tToTm1StateVarMap = CollectionFactory.CreateInsertionOrderedMap<IRandomVariable, IRandomVariable>();
            tToTm1StateVarMap.Put(ExampleRV.RAIN_t_RV, ExampleRV.RAIN_tm1_RV);

            return tToTm1StateVarMap;
        }
    } 
}
