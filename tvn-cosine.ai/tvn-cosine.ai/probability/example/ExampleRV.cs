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
        public static readonly RandVar<bool> DICE_1_RV = new RandVar<bool>("Dice1", new FiniteIntegerDomain(1, 2, 3, 4, 5, 6));
        public static readonly RandVar<bool> DICE_2_RV = new RandVar<bool>("Dice2", new FiniteIntegerDomain(1, 2, 3, 4, 5, 6));
        //
        public static readonly RandVar<bool> TOOTHACHE_RV = new RandVar<bool>("Toothache", new BooleanDomain());
        public static readonly RandVar<bool> CAVITY_RV = new RandVar<bool>("Cavity", new BooleanDomain());
        public static readonly RandVar<bool> CATCH_RV = new RandVar<bool>("Catch", new BooleanDomain());
        //
        public static readonly RandVar<bool> WEATHER_RV = new RandVar<bool>("Weather", new ArbitraryTokenDomain<string>("sunny", "rain", "cloudy", "snow"));
        //
        public static readonly RandVar<bool> MENINGITIS_RV = new RandVar<bool>("Meningitis", new BooleanDomain());
        public static readonly RandVar<bool> STIFF_NECK_RV = new RandVar<bool>("StiffNeck", new BooleanDomain());
        //
        public static readonly RandVar<bool> BURGLARY_RV = new RandVar<bool>("Burglary", new BooleanDomain());
        public static readonly RandVar<bool> EARTHQUAKE_RV = new RandVar<bool>("Earthquake", new BooleanDomain());
        public static readonly RandVar<bool> ALARM_RV = new RandVar<bool>("Alarm", new BooleanDomain());
        public static readonly RandVar<bool> JOHN_CALLS_RV = new RandVar<bool>("JohnCalls", new BooleanDomain());
        public static readonly RandVar<bool> MARY_CALLS_RV = new RandVar<bool>("MaryCalls", new BooleanDomain());
        //
        public static readonly RandVar<bool> CLOUDY_RV = new RandVar<bool>("Cloudy", new BooleanDomain());
        public static readonly RandVar<bool> SPRINKLER_RV = new RandVar<bool>("Sprinkler", new BooleanDomain());
        public static readonly RandVar<bool> RAIN_RV = new RandVar<bool>("Rain", new BooleanDomain());
        public static readonly RandVar<bool> WET_GRASS_RV = new RandVar<bool>("WetGrass", new BooleanDomain());
        //
        public static readonly RandVar<bool> RAIN_tm1_RV = new RandVar<bool>("Rain_t-1", new BooleanDomain());
        public static readonly RandVar<bool> RAIN_t_RV = new RandVar<bool>("Rain_t", new BooleanDomain());
        public static readonly RandVar<bool> UMBREALLA_t_RV = new RandVar<bool>("Umbrella_t", new BooleanDomain());
    }
}
