using System;
using tvn.cosine.ai.logic.propositional.inference;
using tvn.cosine.ai.logic.propositional.kb;
using tvn.cosine.ai.logic.propositional.parsing;

namespace TvnTestConsoleApp.demo.logic
{
    class PlResolutionDemo
    {
        private static PLResolution plr = new PLResolution();

        public static void Main(params string[] args)
        {
            KnowledgeBase kb = new KnowledgeBase();
            string fact = "(B11 => ~P11) & B11)";
            kb.tell(fact);
            Console.WriteLine("\nPlResolutionDemo\n");
            Console.WriteLine("adding " + fact + "to knowldegebase");

            displayResolutionResults(kb, "~B11");

            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }


        private static void displayResolutionResults(KnowledgeBase kb, string query)
        {
            PLParser parser = new PLParser();
            Console.WriteLine("Running plResolution of query " + query + " on knowledgeBase  gives " + plr.plResolution(kb, parser.parse(query)));
        }
    }
}
