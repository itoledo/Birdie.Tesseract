using tvn.cosine.ai.probability.bayes;
using tvn.cosine.ai.probability.bayes.impl;

namespace tvn.cosine.ai.probability.example
{
    /**
     * 
     * @author Ciaran O'Reilly
     *
     */
    public class BayesNetExampleFactory
    {
        public static BayesianNetwork<bool> construct2FairDiceNetwor()
        {
            FiniteNode<bool> dice1 = new FullCPTNode<bool>(ExampleRV.DICE_1_RV, 
                new double[] {  1.0 / 6.0, 1.0 / 6.0, 1.0 / 6.0, 1.0 / 6.0, 1.0 / 6.0, 1.0 / 6.0 });
            FiniteNode<bool> dice2 = new FullCPTNode<bool>(ExampleRV.DICE_2_RV, 
                new double[] { 1.0 / 6.0, 1.0 / 6.0, 1.0 / 6.0, 1.0 / 6.0, 1.0 / 6.0,  1.0 / 6.0 });

            return new BayesNet<bool>(dice1, dice2);
        }

        public static BayesianNetwork<bool> constructToothacheCavityCatchNetwork()
        {
            FiniteNode<bool> cavity = new FullCPTNode<bool>(ExampleRV.CAVITY_RV, new double[] { 0.2, 0.8 });

            FiniteNode<bool> toothache = new FullCPTNode<bool>(ExampleRV.TOOTHACHE_RV,
                    new double[] {
						// C=true, T=true
						0.6,
						// C=true, T=false
						0.4,
						// C=false, T=true
						0.1,
						// C=false, T=false
						0.9

                    }, cavity);


            FiniteNode<bool> catchN = new FullCPTNode<bool>(ExampleRV.CATCH_RV, new double[] {
				// C=true, Catch=true
				0.9,
				// C=true, Catch=false
				0.1,
				// C=false, Catch=true
				0.2,
				// C=false, Catch=false
				0.8 }, cavity);

            return new BayesNet<bool>(cavity);
        }

        public static BayesianNetwork<bool> constructToothacheCavityCatchWeatherNetwork()
        {
            FiniteNode<bool> cavity = new FullCPTNode<bool>(ExampleRV.CAVITY_RV, new double[] {
                0.2, 0.8 });

            FiniteNode<bool> toothache = new FullCPTNode<bool>(ExampleRV.TOOTHACHE_RV,
                    new double[] {
						// C=true, T=true
						0.6,
						// C=true, T=false
						0.4,
						// C=false, T=true
						0.1,
						// C=false, T=false
						0.9

                    }, cavity);

            FiniteNode<bool> catchN = new FullCPTNode<bool>(ExampleRV.CATCH_RV, new double[] {
				// C=true, Catch=true
				0.9,
				// C=true, Catch=false
				0.1,
				// C=false, Catch=true
				0.2,
				// C=false, Catch=false
				0.8 }, cavity);
            FiniteNode<bool> weather = new FullCPTNode<bool>(ExampleRV.WEATHER_RV,
                    new double[] {
						// sunny
						0.6,
						// rain
						0.1,
						// cloudy
						0.29,
						// snow
						0.01 });

            return new BayesNet<bool>(cavity, weather);
        }

        public static BayesianNetwork<bool> constructMeningitisStiffNeckNetwork()
        {
            FiniteNode<bool> meningitis = new FullCPTNode<bool>(ExampleRV.MENINGITIS_RV,
                    new double[] { 1.0 / 50000.0, 1.0 - (1.0 / 50000.0) });

            FiniteNode<bool> stiffneck = new FullCPTNode<bool>(ExampleRV.STIFF_NECK_RV,
                    new double[] {
						// M=true, S=true
						0.7,
						// M=true, S=false
						0.3,
						// M=false, S=true
						0.009986199723994478,
						// M=false, S=false
						0.9900138002760055

                    }, meningitis);
            return new BayesNet<bool>(meningitis);
        }

        public static BayesianNetwork<bool> constructBurglaryAlarmNetwork()
        {
            FiniteNode<bool> burglary = new FullCPTNode<bool>(ExampleRV.BURGLARY_RV,
                    new double[] { 0.001, 0.999 });
            FiniteNode<bool> earthquake = new FullCPTNode<bool>(ExampleRV.EARTHQUAKE_RV,
                    new double[] { 0.002, 0.998 });
            FiniteNode<bool> alarm = new FullCPTNode<bool>(ExampleRV.ALARM_RV, new double[] {
				// B=true, E=true, A=true
				0.95,
				// B=true, E=true, A=false
				0.05,
				// B=true, E=false, A=true
				0.94,
				// B=true, E=false, A=false
				0.06,
				// B=false, E=true, A=true
				0.29,
				// B=false, E=true, A=false
				0.71,
				// B=false, E=false, A=true
				0.001,
				// B=false, E=false, A=false
				0.999 }, burglary, earthquake);

            FiniteNode<bool> johnCalls = new FullCPTNode<bool>(ExampleRV.JOHN_CALLS_RV,
                    new double[] {
						// A=true, J=true
						0.90,
						// A=true, J=false
						0.10,
						// A=false, J=true
						0.05,
						// A=false, J=false
						0.95 }, alarm);

            FiniteNode<bool> maryCalls = new FullCPTNode<bool>(ExampleRV.MARY_CALLS_RV,
                    new double[] {
						// A=true, M=true
						0.70,
						// A=true, M=false
						0.30,
						// A=false, M=true
						0.01,
						// A=false, M=false
						0.99 }, alarm);

            return new BayesNet<bool>(burglary, earthquake);
        }

        public static BayesianNetwork<bool> constructCloudySprinklerRainWetGrassNetwork()
        {
            FiniteNode<bool> cloudy = new FullCPTNode<bool>(ExampleRV.CLOUDY_RV, new double[] {
                0.5, 0.5 });
            FiniteNode<bool> sprinkler = new FullCPTNode<bool>(ExampleRV.SPRINKLER_RV,
                    new double[] {
						// Cloudy=true, Sprinkler=true
						0.1,
						// Cloudy=true, Sprinkler=false
						0.9,
						// Cloudy=false, Sprinkler=true
						0.5,
						// Cloudy=false, Sprinkler=false
						0.5 }, cloudy);
            FiniteNode<bool> rain = new FullCPTNode<bool>(ExampleRV.RAIN_RV, new double[] {
				// Cloudy=true, Rain=true
				0.8,
				// Cloudy=true, Rain=false
				0.2,
				// Cloudy=false, Rain=true
				0.2,
				// Cloudy=false, Rain=false
				0.8 }, cloudy);

            FiniteNode<bool> wetGrass = new FullCPTNode<bool>(ExampleRV.WET_GRASS_RV,
                    new double[] {
						// Sprinkler=true, Rain=true, WetGrass=true
						.99,
						// Sprinkler=true, Rain=true, WetGrass=false
						.01,
						// Sprinkler=true, Rain=false, WetGrass=true
						.9,
						// Sprinkler=true, Rain=false, WetGrass=false
						.1,
						// Sprinkler=false, Rain=true, WetGrass=true
						.9,
						// Sprinkler=false, Rain=true, WetGrass=false
						.1,
						// Sprinkler=false, Rain=false, WetGrass=true
						0.0,
						// Sprinkler=false, Rain=false, WetGrass=false
						1.0 }, sprinkler, rain);

            return new BayesNet<bool>(cloudy);
        }
    }
}
