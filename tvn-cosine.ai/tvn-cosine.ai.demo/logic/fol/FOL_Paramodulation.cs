using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.logic.fol.domain;
using tvn.cosine.ai.logic.fol.inference;
using tvn.cosine.ai.logic.fol.kb.data;
using tvn.cosine.ai.logic.fol.parsing;
using tvn.cosine.ai.logic.fol.parsing.ast;

namespace tvn_cosine.ai.demo.logic.fol
{
    public class FOL_Paramodulation : FolDemoBase
    {
        static void Main(params string[] args)
        {
            fOL_Paramodulation();
        }

        static void fOL_Paramodulation()
        {
            System.Console.WriteLine("-------------------");
            System.Console.WriteLine("Paramodulation Demo");
            System.Console.WriteLine("-------------------");

            FOLDomain domain = new FOLDomain();
            domain.addConstant("A");
            domain.addConstant("B");
            domain.addPredicate("P");
            domain.addPredicate("Q");
            domain.addPredicate("R");
            domain.addFunction("F");

            FOLParser parser = new FOLParser(domain);

            IQueue<Literal> lits = Factory.CreateQueue<Literal>();
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

            System.Console.WriteLine("Paramodulate '" + c1 + "' with '" + c2 + "' to give");
            System.Console.WriteLine(paras.ToString());
            System.Console.WriteLine("");
        }
    }
}
