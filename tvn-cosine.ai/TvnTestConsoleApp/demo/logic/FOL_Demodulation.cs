using System;
using tvn.cosine.ai.logic.fol.domain;
using tvn.cosine.ai.logic.fol.inference;
using tvn.cosine.ai.logic.fol.parsing;
using tvn.cosine.ai.logic.fol.parsing.ast;

namespace TvnTestConsoleApp.demo.logic
{
    class FOL_Demodulation
    {
        public static void Main(params string[] args)
        {
            fOL_Demodulation();

            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }

        private static void fOL_Demodulation()
        {
            Console.WriteLine("-----------------");
            Console.WriteLine("Demodulation Demo");
            Console.WriteLine("-----------------");
            FOLDomain domain = new FOLDomain();
            domain.addConstant("A");
            domain.addConstant("B");
            domain.addConstant("C");
            domain.addConstant("D");
            domain.addConstant("E");
            domain.addPredicate("P");
            domain.addFunction("F");
            domain.addFunction("G");
            domain.addFunction("H");
            domain.addFunction("J");

            FOLParser parser = new FOLParser(domain);

            Predicate expression = (Predicate)parser.parse("P(A,F(B,G(A,H(B)),C),D)");
            TermEquality assertion = (TermEquality)parser.parse("B = E");

            Demodulation demodulation = new Demodulation();
            Predicate altExpression = (Predicate)demodulation.apply(assertion, expression);

            Console.WriteLine("Demodulate '" + expression + "' with '" + assertion + "' to give");
            Console.WriteLine(altExpression.ToString());
            Console.WriteLine("and again to give");
            Console.WriteLine(demodulation.apply(assertion, altExpression).ToString());
            Console.WriteLine("");
        }
    }
}
