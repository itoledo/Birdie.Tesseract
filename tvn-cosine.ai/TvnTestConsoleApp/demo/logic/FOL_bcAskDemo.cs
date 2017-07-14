using System;
using tvn.cosine.ai.logic.fol.inference;

namespace TvnTestConsoleApp.demo.logic
{
    class FOL_bcAskDemo
    {
        public static void Main(params string[] args)
        {
            fOL_bcAskDemo();

            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }

        private static void fOL_bcAskDemo()
        {
            Console.WriteLine("----------------------------");
            Console.WriteLine("Backward Chain, Kings Demo 1");
            Console.WriteLine("----------------------------");
            Util.kingsDemo1(new FOLBCAsk());
            Console.WriteLine("----------------------------");
            Console.WriteLine("Backward Chain, Kings Demo 2");
            Console.WriteLine("----------------------------");
            Util.kingsDemo2(new FOLBCAsk());
            Console.WriteLine("----------------------------");
            Console.WriteLine("Backward Chain, Weapons Demo");
            Console.WriteLine("----------------------------");
            Util.weaponsDemo(new FOLBCAsk());
        }
    }
}
