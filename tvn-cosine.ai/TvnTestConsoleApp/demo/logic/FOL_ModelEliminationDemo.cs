using System;
using tvn.cosine.ai.logic.fol.inference;

namespace TvnTestConsoleApp.demo.logic
{
    class FOL_ModelEliminationDemo
    {
        public static void Main(params string[] args)
        {
            fOL_ModelEliminationDemo();

            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }

        private static void fOL_ModelEliminationDemo()
        {
            Console.WriteLine("-------------------------------");
            Console.WriteLine("Model Elimination, Kings Demo 1");
            Console.WriteLine("-------------------------------");
            Util.kingsDemo1(new FOLModelElimination());
            Console.WriteLine("-------------------------------");
            Console.WriteLine("Model Elimination, Kings Demo 2");
            Console.WriteLine("-------------------------------");
            Util.kingsDemo2(new FOLModelElimination());
            Console.WriteLine("-------------------------------");
            Console.WriteLine("Model Elimination, Weapons Demo");
            Console.WriteLine("-------------------------------");
            Util.weaponsDemo(new FOLModelElimination());
            Console.WriteLine("------------------------------------");
            Console.WriteLine("Model Elimination, Loves Animal Demo");
            Console.WriteLine("------------------------------------");
            Util.lovesAnimalDemo(new FOLModelElimination());
            Console.WriteLine("------------------------------------------");
            Console.WriteLine("Model Elimination, ABC Equality Axiom Demo");
            Console.WriteLine("-------------------------------------------");
            Util.abcEqualityAxiomDemo(new FOLModelElimination());
        }
    }
}
