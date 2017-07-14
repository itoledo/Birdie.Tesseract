using System;
using System.Collections.Generic;
using tvn.cosine.ai.logic.fol;
using tvn.cosine.ai.logic.fol.domain;
using tvn.cosine.ai.logic.fol.inference.proof;
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
            Console.WriteLine("Unify '{0}' with '{1}' to get the substitution {2}.\n", query, johnKnowsJane,  subst.CustomDictionaryWriterToString()); 
        }
    }
}
