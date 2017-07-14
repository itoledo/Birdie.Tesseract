using System;
using tvn.cosine.ai.logic.propositional.inference;
using tvn.cosine.ai.logic.propositional.kb;
using tvn.cosine.ai.logic.propositional.kb.data;
using tvn.cosine.ai.logic.propositional.visitors;

namespace TvnTestConsoleApp.demo.logic
{
    class WalkSatDemo
    {
        public static void Main(params string[] args)
        {
            Console.WriteLine("\nWalkSatDemo\n");
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
            Console.WriteLine(kb.ToString());

            WalkSAT walkSAT = new WalkSAT();
            Model m = walkSAT.walkSAT(ConvertToConjunctionOfClauses.convert(kb.asSentence()).getClauses(), 0.5, 1000);
            if (m == null)
            {
                Console.WriteLine("failure");
            }
            else
            {
                m.print();
            }

            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }
    }
}
