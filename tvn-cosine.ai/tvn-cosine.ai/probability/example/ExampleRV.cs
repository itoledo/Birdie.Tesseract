using tvn.cosine.ai.probability.domain;
using tvn.cosine.ai.probability.util;

namespace tvn.cosine.ai.probability.example
{
    /**
     * Predefined example Random Variables from AIMA3e.
     * 
     * @author Ciaran O'Reilly
     */
    public class ExampleRV
    {
        //
        public static readonly RandVar<string> DICE_1_RV = new RandVar<string>("Dice1", new FiniteIntegerDomain(1, 2, 3, 4, 5, 6));
        public static readonly RandVar<string> DICE_2_RV = new RandVar<string>("Dice2", new FiniteIntegerDomain(1, 2, 3, 4, 5, 6));
        //
        public static readonly RandVar<string> TOOTHACHE_RV = new RandVar<string>("Toothache", new BooleanDomain());
        public static readonly RandVar<string> CAVITY_RV = new RandVar<string>("Cavity", new BooleanDomain());
        public static readonly RandVar<string> CATCH_RV = new RandVar<string>("Catch", new BooleanDomain());
        //
        public static readonly RandVar<string> WEATHER_RV = new RandVar<string>("Weather", new ArbitraryTokenDomain<string>("sunny", "rain", "cloudy", "snow"));
        //
        public static readonly RandVar<string> MENINGITIS_RV = new RandVar<string>("Meningitis", new BooleanDomain());
        public static readonly RandVar<string> STIFF_NECK_RV = new RandVar<string>("StiffNeck", new BooleanDomain());
        //
        public static readonly RandVar<string> BURGLARY_RV = new RandVar<string>("Burglary", new BooleanDomain());
        public static readonly RandVar<string> EARTHQUAKE_RV = new RandVar<string>("Earthquake", new BooleanDomain());
        public static readonly RandVar<string> ALARM_RV = new RandVar<string>("Alarm", new BooleanDomain());
        public static readonly RandVar<string> JOHN_CALLS_RV = new RandVar<string>("JohnCalls", new BooleanDomain());
        public static readonly RandVar<string> MARY_CALLS_RV = new RandVar<string>("MaryCalls", new BooleanDomain());
        //
        public static readonly RandVar<string> CLOUDY_RV = new RandVar<string>("Cloudy", new BooleanDomain());
        public static readonly RandVar<string> SPRINKLER_RV = new RandVar<string>("Sprinkler", new BooleanDomain());
        public static readonly RandVar<string> RAIN_RV = new RandVar<string>("Rain", new BooleanDomain());
        public static readonly RandVar<string> WET_GRASS_RV = new RandVar<string>("WetGrass", new BooleanDomain());
        //
        public static readonly RandVar<string> RAIN_tm1_RV = new RandVar<string>("Rain_t-1", new BooleanDomain());
        public static readonly RandVar<string> RAIN_t_RV = new RandVar<string>("Rain_t", new BooleanDomain());
        public static readonly RandVar<string> UMBREALLA_t_RV = new RandVar<string>("Umbrella_t", new BooleanDomain());
    }
}
