using System;
using tvn.cosine.ai.logic.fol.inference;

namespace TvnTestConsoleApp.demo.logic
{
    class FOL_fcAskDemo
    {
        public static void Main(params string[] args)
        {
            fOL_fcAskDemo();

            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }

        private static void fOL_fcAskDemo()
        {
            Console.WriteLine("---------------------------");
            Console.WriteLine("Forward Chain, Kings Demo 1");
            Console.WriteLine("---------------------------");
            Util.kingsDemo1(new FOLFCAsk());
            Console.WriteLine("---------------------------");
            Console.WriteLine("Forward Chain, Kings Demo 2");
            Console.WriteLine("---------------------------");
            Util.kingsDemo2(new FOLFCAsk());
            Console.WriteLine("---------------------------");
            Console.WriteLine("Forward Chain, Weapons Demo");
            Console.WriteLine("---------------------------");
            Util.weaponsDemo(new FOLFCAsk());
        }
    }
}
