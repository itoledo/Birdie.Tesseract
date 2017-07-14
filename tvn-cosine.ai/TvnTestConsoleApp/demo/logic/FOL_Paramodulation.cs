using System;
using System.Collections.Generic;
using tvn.cosine.ai.logic.fol.domain;
using tvn.cosine.ai.logic.fol.inference;
using tvn.cosine.ai.logic.fol.kb.data;
using tvn.cosine.ai.logic.fol.parsing;
using tvn.cosine.ai.logic.fol.parsing.ast;

namespace TvnTestConsoleApp.demo.logic
{
    class FOL_OTTERDemo
    {
        public static void Main(params string[] args)
        {
            fOL_Paramodulation();

            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }

        private static void fOL_Paramodulation()
        {
            Console.WriteLine("-------------------");
            Console.WriteLine("Paramodulation Demo");
            Console.WriteLine("-------------------");

            FOLDomain domain = new FOLDomain();
            domain.addConstant("A");
            domain.addConstant("B");
            domain.addPredicate("P");
            domain.addPredicate("Q");
            domain.addPredicate("R");
            domain.addFunction("F");

            FOLParser parser = new FOLParser(domain);

            IList<Literal> lits = new List<Literal>();
            AtomicSentence a1 = (AtomicSentence)parser.parse("P(F(x,B),x)");
            AtomicSentence a2 = (AtomicSentence)parser.parse("Q(x)");
            lits.Add(new Literal(a1));
            lits.Add(new Literal(a2));

            Clause c1 = new Clause(lits);

            lits.Clear();
            a1 = (AtomicSentence)parser.parse("F(A,y) = y");
            a2 = (AtomicSentence)parser.parse("R(y)");
            lits.Add(new Literal(a1));
            lits.Add(new Literal(a2));

            Clause c2 = new Clause(lits);

            Paramodulation paramodulation = new Paramodulation();
            ISet<Clause> paras = paramodulation.apply(c1, c2);

            Console.WriteLine("Paramodulate '" + c1 + "' with '" + c2 + "' to give");
            Console.WriteLine(paras.ToString());
            Console.WriteLine("");
        }
    }
}
