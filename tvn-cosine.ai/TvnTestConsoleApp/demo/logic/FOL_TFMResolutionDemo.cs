using System;
using tvn.cosine.ai.logic.fol.inference;

namespace TvnTestConsoleApp.demo.logic
{
    class FOL_TFMResolutionDemo
    {
        public static void Main(params string[] args)
        {
            fOL_TFMResolutionDemo();

            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }

        private static void fOL_TFMResolutionDemo()
        {
            Console.WriteLine("----------------------------");
            Console.WriteLine("TFM Resolution, Kings Demo 1");
            Console.WriteLine("----------------------------");
            Util.kingsDemo1(new FOLTFMResolution());
            Console.WriteLine("----------------------------");
            Console.WriteLine("TFM Resolution, Kings Demo 2");
            Console.WriteLine("----------------------------");
            Util.kingsDemo2(new FOLTFMResolution());
            Console.WriteLine("----------------------------");
            Console.WriteLine("TFM Resolution, Weapons Demo");
            Console.WriteLine("----------------------------");
            Util.weaponsDemo(new FOLTFMResolution());
            Console.WriteLine("---------------------------------");
            Console.WriteLine("TFM Resolution, Loves Animal Demo");
            Console.WriteLine("---------------------------------");
            Util.lovesAnimalDemo(new FOLTFMResolution());
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("TFM Resolution, ABC Equality Axiom Demo");
            Console.WriteLine("---------------------------------------");
            Util.abcEqualityAxiomDemo(new FOLTFMResolution());
        }
    }
}
