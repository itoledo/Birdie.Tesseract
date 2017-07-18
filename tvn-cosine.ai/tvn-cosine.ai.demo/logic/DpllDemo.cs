﻿namespace tvn_cosine.ai.demo.logic
{
    public class DpllDemo
    {
        private static DPLLSatisfiable dpll = new DPLLSatisfiable();



        public static void main(String[] args)
        {

            displayDPLLSatisfiableStatus("A & B");
            displayDPLLSatisfiableStatus("A & ~A");
            displayDPLLSatisfiableStatus("(A | ~A) & (A | B)");
        }

        public static void displayDPLLSatisfiableStatus(String query)
        {
            PLParser parser = new PLParser();
            if (dpll.dpllSatisfiable(parser.parse(query)))
            {
                System.out.println(query + " is  (DPLL) satisfiable");
            }
            else
            {
                System.out.println(query + " is NOT (DPLL)  satisfiable");
            }
        }
    }

}
