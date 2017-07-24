using tvn.cosine.ai.logic.fol;
using tvn.cosine.ai.logic.fol.domain;
using tvn.cosine.ai.logic.fol.kb.data;
using tvn.cosine.ai.logic.fol.parsing;
using tvn.cosine.ai.logic.fol.parsing.ast;

namespace tvn_cosine.ai.demo.logic.fol
{
    public class FOL_CNFConversion : FolDemoBase
    {
        static void Main(params string[] args)
        {
            fOL_CNFConversion();
        }

        static void fOL_CNFConversion()
        {
            System.Console.WriteLine("-------------------------------------------------");
            System.Console.WriteLine("Conjuctive Normal Form for First Order Logic Demo");
            System.Console.WriteLine("-------------------------------------------------");
            FOLDomain domain = DomainFactory.lovesAnimalDomain();
            FOLParser parser = new FOLParser(domain);

            Sentence origSentence = parser.parse("FORALL x (FORALL y (Animal(y) => Loves(x, y)) => EXISTS y Loves(y, x))");

            CNFConverter cnfConv = new CNFConverter(parser);

            CNF cnf = cnfConv.convertToCNF(origSentence);

            System.Console.WriteLine("Convert '" + origSentence + "' to CNF.");
            System.Console.WriteLine("CNF=" + cnf.ToString());
            System.Console.WriteLine("");
        }
    }
}
