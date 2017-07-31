using tvn.cosine.collections;
using tvn.cosine.collections.api;
using tvn.cosine.ai.logic.fol;
using tvn.cosine.ai.logic.fol.domain;
using tvn.cosine.ai.logic.fol.parsing;
using tvn.cosine.ai.logic.fol.parsing.ast;

namespace tvn_cosine.ai.demo.logic.fol
{
    public class UnifierDemo
    {
        static void Main(params string[] args)
        {
            unifierDemo();
        }

        private static void unifierDemo()
        {
            FOLParser parser = new FOLParser(DomainFactory.knowsDomain());
            Unifier unifier = new Unifier();
            IMap<Variable, Term> theta = CollectionFactory.CreateInsertionOrderedMap<Variable, Term>();

            Sentence query = parser.parse("Knows(John,x)");
            Sentence johnKnowsJane = parser.parse("Knows(y,Mother(y))");

            System.Console.WriteLine("------------");
            System.Console.WriteLine("Unifier Demo");
            System.Console.WriteLine("------------");
            IMap<Variable, Term> subst = unifier.unify(query, johnKnowsJane, theta);
            System.Console.WriteLine("Unify '" + query + "' with '" + johnKnowsJane + "' to get the substitution " + subst + ".");
            System.Console.WriteLine("");
        }
    }
}
