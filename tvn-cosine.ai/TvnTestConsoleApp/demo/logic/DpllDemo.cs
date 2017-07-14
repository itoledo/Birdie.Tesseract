using System;
using tvn.cosine.ai.logic.propositional.inference;
using tvn.cosine.ai.logic.propositional.parsing;

namespace TvnTestConsoleApp.demo.logic
{
    class DpllDemo
    {
        public static void Main(params string[] args)
        {
            Console.WriteLine(tvn.cosine.ai.util.Util.ntimes("*", 100));
            Console.WriteLine("\n DPLLSatisfiableStatus ");
            Console.WriteLine(tvn.cosine.ai.util.Util.ntimes("*", 100));

            displayDPLLSatisfiableStatus("A & B");
            displayDPLLSatisfiableStatus("A & ~A");
            displayDPLLSatisfiableStatus("(A | ~A) & (A | B)");

            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }

        private static DPLLSatisfiable dpll = new DPLLSatisfiable();
        public static void displayDPLLSatisfiableStatus(string query)
        {
            PLParser parser = new PLParser();
            if (dpll.dpllSatisfiable(parser.parse(query)))
            {
                Console.WriteLine(query + " is  (DPLL) satisfiable");
            }
            else
            {
                Console.WriteLine(query + " is NOT (DPLL)  satisfiable");
            }
        }
    }
}
