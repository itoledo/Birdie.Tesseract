using System;
using tvn.cosine.ai.logic.fol.inference;

namespace TvnTestConsoleApp.demo.logic
{
    class FOL_Paramodulation__
    {
        public static void Main(params string[] args)
        {
            fOL_OTTERDemo();

            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }

        private static void fOL_OTTERDemo()
        {
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("OTTER Like Theorem Prover, Kings Demo 1");
            Console.WriteLine("---------------------------------------");
            Util.kingsDemo1(new FOLOTTERLikeTheoremProver());
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("OTTER Like Theorem Prover, Kings Demo 2");
            Console.WriteLine("---------------------------------------");
            Util.kingsDemo2(new FOLOTTERLikeTheoremProver());
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("OTTER Like Theorem Prover, Weapons Demo");
            Console.WriteLine("---------------------------------------");
            Util.weaponsDemo(new FOLOTTERLikeTheoremProver());
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("OTTER Like Theorem Prover, Loves Animal Demo");
            Console.WriteLine("--------------------------------------------");
            Util.lovesAnimalDemo(new FOLOTTERLikeTheoremProver());
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("OTTER Like Theorem Prover, ABC Equality Axiom Demo");
            Console.WriteLine("--------------------------------------------------");
            Util.abcEqualityAxiomDemo(new FOLOTTERLikeTheoremProver(false));
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("OTTER Like Theorem Prover, ABC Equality No Axiom Demo");
            Console.WriteLine("-----------------------------------------------------");
            Util.abcEqualityNoAxiomDemo(new FOLOTTERLikeTheoremProver(true));
        }
    }
}
