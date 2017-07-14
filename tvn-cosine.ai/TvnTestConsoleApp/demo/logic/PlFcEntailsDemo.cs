using System;
using tvn.cosine.ai.logic.propositional.inference;
using tvn.cosine.ai.logic.propositional.kb;
using tvn.cosine.ai.logic.propositional.parsing.ast;

namespace TvnTestConsoleApp.demo.logic
{
    class PlFcEntailsDemo
    {
        private static PLFCEntails plfce = new PLFCEntails();

        public static void Main(params string[] args)
        {
            Console.WriteLine("\nPlFcEntailsDemo\n");
            KnowledgeBase kb = new KnowledgeBase();
            kb.tell("P => Q");
            kb.tell("L & M => P");
            kb.tell("B & L => M");
            kb.tell("A & P => L");
            kb.tell("A & B => L");
            kb.tell("A");
            kb.tell("B");

            Console.WriteLine("Example from  page 220 of AIMA 2nd Edition");
            Console.WriteLine("KnowledgeBsse consists of sentences");
            Console.WriteLine("P => Q");
            Console.WriteLine("L & M => P");
            Console.WriteLine("B & L => M");
            Console.WriteLine("A & P => L");
            Console.WriteLine("A & B => L");
            Console.WriteLine("A");
            Console.WriteLine("B");

            displayPLFCEntailment(kb, "Q");

            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }

        private static void displayPLFCEntailment(KnowledgeBase kb, string q)
        {
            Console.WriteLine("Running PLFCEntailment on knowledge base" + " with query " + q + " gives " + plfce.plfcEntails(kb, new PropositionSymbol(q)));
        }
    }
}
