using System;
using tvn.cosine.ai.logic.propositional.kb;

namespace TvnTestConsoleApp.demo.logic
{
    class TTEntailsDemo
    {
        public static void Main(params string[] args)
        {
            KnowledgeBase kb = new KnowledgeBase();
            kb.tell("B12 <=> P11 | P13 | P22 | P02");
            kb.tell("B21 <=> P20 | P22 | P31 | P11");
            kb.tell("B01 <=> P00 | P02 | P11");
            kb.tell("B10 <=> P11 | P20 | P00");
            kb.tell("~B21");
            kb.tell("~B12");
            kb.tell("B10");
            kb.tell("B01");

            Console.WriteLine("\nTTEntailsDemo\n");
            Console.WriteLine(kb.ToString());

            displayTTEntails(kb, "P00");
            displayTTEntails(kb, "~P00");

            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }
        private static void displayTTEntails(KnowledgeBase kb, string s)
        {
            Console.WriteLine(" ttentails (\"" + s + "\" ) returns " + kb.askWithTTEntails(s));
        }
    }
}
