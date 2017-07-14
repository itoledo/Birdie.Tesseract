using System;
using System.Collections.Generic;
using tvn.cosine.ai.logic.fol;
using tvn.cosine.ai.logic.fol.domain;
using tvn.cosine.ai.logic.fol.parsing;
using tvn.cosine.ai.logic.fol.parsing.ast;

namespace TvnTestConsoleApp.demo.logic
{
    class UnifierDemo
    {
        public static void Main(params string[] args)
        {
            unifierDemo(); 

            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }

        private static void unifierDemo()
        {
            FOLParser parser = new FOLParser(DomainFactory.knowsDomain());
            Unifier unifier = new Unifier();
            IDictionary<Variable, Term> theta = new Dictionary<Variable, Term>();

            Sentence query = parser.parse("Knows(John,x)");
            Sentence johnKnowsJane = parser.parse("Knows(y,Mother(y))");

            Console.WriteLine("------------");
            Console.WriteLine("Unifier Demo");
            Console.WriteLine("------------");
            IDictionary<Variable, Term> subst = unifier.unify(query, johnKnowsJane, theta);
            Console.Write("Unify '" + query + "' with '" + johnKnowsJane + "' to get the substitution {{");

            bool first = true;
            foreach (var row in subst)
            {
                if (first)
                    first = false;
                else
                    Console.Write(", ");

                Console.Write(row.Key);
                Console.Write("=");
                Console.Write(row.Value);
            }
            Console.WriteLine("}}.\n"); 
        }
    }
}
