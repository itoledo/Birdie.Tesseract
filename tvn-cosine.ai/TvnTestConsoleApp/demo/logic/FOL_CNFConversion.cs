using System;
using tvn.cosine.ai.logic.fol;
using tvn.cosine.ai.logic.fol.domain;
using tvn.cosine.ai.logic.fol.kb.data;
using tvn.cosine.ai.logic.fol.parsing;
using tvn.cosine.ai.logic.fol.parsing.ast;

namespace TvnTestConsoleApp.demo.logic
{
    class FOL_CNFConversion
    {
        public static void Main(params string[] args)
        {
            fOL_CNFConversion();

            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }

        private static void fOL_CNFConversion()
        {
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("Conjuctive Normal Form for First Order Logic Demo");
            Console.WriteLine("-------------------------------------------------");
            FOLDomain domain = DomainFactory.lovesAnimalDomain();
            FOLParser parser = new FOLParser(domain);

            Sentence origSentence = parser .parse("FORALL x (FORALL y (Animal(y) => Loves(x, y)) => EXISTS y Loves(y, x))");

            CNFConverter cnfConv = new CNFConverter(parser);

            CNF cnf = cnfConv.convertToCNF(origSentence);

            Console.WriteLine("Convert '" + origSentence + "' to CNF.");
            Console.WriteLine("CNF=" + cnf.ToString());
            Console.WriteLine("");
        }
    }
}
