using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.logic.fol;
using tvn.cosine.ai.logic.fol.domain;
using tvn.cosine.ai.logic.fol.inference;
using tvn.cosine.ai.logic.fol.inference.proof;
using tvn.cosine.ai.logic.fol.kb;
using tvn.cosine.ai.logic.fol.kb.data;
using tvn.cosine.ai.logic.fol.parsing;
using tvn.cosine.ai.logic.fol.parsing.ast;

namespace tvn_cosine.ai.demo.logic
{
    public class FolDemo
    {
        public static void Main(params string[] args)
        {
            // unifierDemo();
            //  fOL_fcAskDemo();
            // fOL_bcAskDemo();
            fOL_CNFConversion();


            // fOL_TFMResolutionDemo();

            //	fOL_Demodulation();
            //	fOL_Paramodulation();
            //	fOL_OTTERDemo();
            //	fOL_ModelEliminationDemo();

        }

        private static void unifierDemo()
        {
            FOLParser parser = new FOLParser(DomainFactory.knowsDomain());
            Unifier unifier = new Unifier();
            IMap<Variable, Term> theta = Factory.CreateInsertionOrderedMap<Variable, Term>();

            Sentence query = parser.parse("Knows(John,x)");
            Sentence johnKnowsJane = parser.parse("Knows(y,Mother(y))");

            System.Console.WriteLine("------------");
            System.Console.WriteLine("Unifier Demo");
            System.Console.WriteLine("------------");
            IMap<Variable, Term> subst = unifier.unify(query, johnKnowsJane, theta);
            System.Console.WriteLine("Unify '" + query + "' with '" + johnKnowsJane + "' to get the substitution " + subst + ".");
            System.Console.WriteLine("");
        }

        private static void fOL_fcAskDemo()
        {
            System.Console.WriteLine("---------------------------");
            System.Console.WriteLine("Forward Chain, Kings Demo 1");
            System.Console.WriteLine("---------------------------");
            kingsDemo1(new FOLFCAsk());
            System.Console.WriteLine("---------------------------");
            System.Console.WriteLine("Forward Chain, Kings Demo 2");
            System.Console.WriteLine("---------------------------");
            kingsDemo2(new FOLFCAsk());
            System.Console.WriteLine("---------------------------");
            System.Console.WriteLine("Forward Chain, Weapons Demo");
            System.Console.WriteLine("---------------------------");
            weaponsDemo(new FOLFCAsk());
        }

        private static void fOL_bcAskDemo()
        {
            System.Console.WriteLine("----------------------------");
            System.Console.WriteLine("Backward Chain, Kings Demo 1");
            System.Console.WriteLine("----------------------------");
            kingsDemo1(new FOLBCAsk());
            System.Console.WriteLine("----------------------------");
            System.Console.WriteLine("Backward Chain, Kings Demo 2");
            System.Console.WriteLine("----------------------------");
            kingsDemo2(new FOLBCAsk());
            System.Console.WriteLine("----------------------------");
            System.Console.WriteLine("Backward Chain, Weapons Demo");
            System.Console.WriteLine("----------------------------");
            weaponsDemo(new FOLBCAsk());
        }

        private static void fOL_CNFConversion()
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

        private static void fOL_TFMResolutionDemo()
        {
            System.Console.WriteLine("----------------------------");
            System.Console.WriteLine("TFM Resolution, Kings Demo 1");
            System.Console.WriteLine("----------------------------");
            kingsDemo1(new FOLTFMResolution());
            System.Console.WriteLine("----------------------------");
            System.Console.WriteLine("TFM Resolution, Kings Demo 2");
            System.Console.WriteLine("----------------------------");
            kingsDemo2(new FOLTFMResolution());
            System.Console.WriteLine("----------------------------");
            System.Console.WriteLine("TFM Resolution, Weapons Demo");
            System.Console.WriteLine("----------------------------");
            weaponsDemo(new FOLTFMResolution());
            System.Console.WriteLine("---------------------------------");
            System.Console.WriteLine("TFM Resolution, Loves Animal Demo");
            System.Console.WriteLine("---------------------------------");
            lovesAnimalDemo(new FOLTFMResolution());
            System.Console.WriteLine("---------------------------------------");
            System.Console.WriteLine("TFM Resolution, ABC Equality Axiom Demo");
            System.Console.WriteLine("---------------------------------------");
            abcEqualityAxiomDemo(new FOLTFMResolution());
        }

        private static void fOL_Demodulation()
        {
            System.Console.WriteLine("-----------------");
            System.Console.WriteLine("Demodulation Demo");
            System.Console.WriteLine("-----------------");
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

            Predicate expression = (Predicate)parser
                    .parse("P(A,F(B,G(A,H(B)),C),D)");
            TermEquality assertion = (TermEquality)parser.parse("B = E");

            Demodulation demodulation = new Demodulation();
            Predicate altExpression = (Predicate)demodulation.apply(assertion,
                    expression);

            System.Console.WriteLine("Demodulate '" + expression + "' with '" + assertion
                    + "' to give");
            System.Console.WriteLine(altExpression.ToString());
            System.Console.WriteLine("and again to give");
            System.Console.WriteLine(demodulation.apply(assertion, altExpression)
                    .ToString());
            System.Console.WriteLine("");
        }

        private static void fOL_Paramodulation()
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

        private static void fOL_OTTERDemo()
        {
            System.Console.WriteLine("---------------------------------------");
            System.Console.WriteLine("OTTER Like Theorem Prover, Kings Demo 1");
            System.Console.WriteLine("---------------------------------------");
            kingsDemo1(new FOLOTTERLikeTheoremProver());
            System.Console.WriteLine("---------------------------------------");
            System.Console.WriteLine("OTTER Like Theorem Prover, Kings Demo 2");
            System.Console.WriteLine("---------------------------------------");
            kingsDemo2(new FOLOTTERLikeTheoremProver());
            System.Console.WriteLine("---------------------------------------");
            System.Console.WriteLine("OTTER Like Theorem Prover, Weapons Demo");
            System.Console.WriteLine("---------------------------------------");
            weaponsDemo(new FOLOTTERLikeTheoremProver());
            System.Console.WriteLine("--------------------------------------------");
            System.Console.WriteLine("OTTER Like Theorem Prover, Loves Animal Demo");
            System.Console.WriteLine("--------------------------------------------");
            lovesAnimalDemo(new FOLOTTERLikeTheoremProver());
            System.Console
                .WriteLine("--------------------------------------------------");
            System.Console
                .WriteLine("OTTER Like Theorem Prover, ABC Equality Axiom Demo");
            System.Console
                .WriteLine("--------------------------------------------------");
            abcEqualityAxiomDemo(new FOLOTTERLikeTheoremProver(false));
            System.Console
                .WriteLine("-----------------------------------------------------");
            System.Console
                .WriteLine("OTTER Like Theorem Prover, ABC Equality No Axiom Demo");
            System.Console
                .WriteLine("-----------------------------------------------------");
            abcEqualityNoAxiomDemo(new FOLOTTERLikeTheoremProver(true));
        }

        private static void fOL_ModelEliminationDemo()
        {
            System.Console.WriteLine("-------------------------------");
            System.Console.WriteLine("Model Elimination, Kings Demo 1");
            System.Console.WriteLine("-------------------------------");
            kingsDemo1(new FOLModelElimination());
            System.Console.WriteLine("-------------------------------");
            System.Console.WriteLine("Model Elimination, Kings Demo 2");
            System.Console.WriteLine("-------------------------------");
            kingsDemo2(new FOLModelElimination());
            System.Console.WriteLine("-------------------------------");
            System.Console.WriteLine("Model Elimination, Weapons Demo");
            System.Console.WriteLine("-------------------------------");
            weaponsDemo(new FOLModelElimination());
            System.Console.WriteLine("------------------------------------");
            System.Console.WriteLine("Model Elimination, Loves Animal Demo");
            System.Console.WriteLine("------------------------------------");
            lovesAnimalDemo(new FOLModelElimination());
            System.Console.WriteLine("------------------------------------------");
            System.Console.WriteLine("Model Elimination, ABC Equality Axiom Demo");
            System.Console.WriteLine("-------------------------------------------");
            abcEqualityAxiomDemo(new FOLModelElimination());
        }

        private static void kingsDemo1(InferenceProcedure ip)
        {
            StandardizeApartIndexicalFactory.flush();

            FOLKnowledgeBase kb = FOLKnowledgeBaseFactory.createKingsKnowledgeBase(ip);

            string kbStr = kb.ToString();

            IQueue<Term> terms = Factory.CreateQueue<Term>();
            terms.Add(new Constant("John"));
            Predicate query = new Predicate("Evil", terms);

            InferenceResult answer = kb.ask(query);

            System.Console.WriteLine("Kings Knowledge Base:");
            System.Console.WriteLine(kbStr);
            System.Console.WriteLine("Query: " + query);
            foreach (Proof p in answer.getProofs())
            {
                System.Console.Write(ProofPrinter.printProof(p));
                System.Console.WriteLine("");
            }
        }

        private static void kingsDemo2(InferenceProcedure ip)
        {
            StandardizeApartIndexicalFactory.flush();

            FOLKnowledgeBase kb = FOLKnowledgeBaseFactory
                    .createKingsKnowledgeBase(ip);

            string kbStr = kb.ToString();

            IQueue<Term> terms = Factory.CreateQueue<Term>();
            terms.Add(new Variable("x"));
            Predicate query = new Predicate("King", terms);

            InferenceResult answer = kb.ask(query);

            System.Console.WriteLine("Kings Knowledge Base:");
            System.Console.WriteLine(kbStr);
            System.Console.WriteLine("Query: " + query);
            foreach (Proof p in answer.getProofs())
            {
                System.Console.Write(ProofPrinter.printProof(p));
            }
        }

        private static void weaponsDemo(InferenceProcedure ip)
        {
            StandardizeApartIndexicalFactory.flush();

            FOLKnowledgeBase kb = FOLKnowledgeBaseFactory
                    .createWeaponsKnowledgeBase(ip);

            string kbStr = kb.ToString();

            IQueue<Term> terms = Factory.CreateQueue<Term>();
            terms.Add(new Variable("x"));
            Predicate query = new Predicate("Criminal", terms);

            InferenceResult answer = kb.ask(query);

            System.Console.WriteLine("Weapons Knowledge Base:");
            System.Console.WriteLine(kbStr);
            System.Console.WriteLine("Query: " + query);
            foreach (Proof p in answer.getProofs())
            {
                System.Console.Write(ProofPrinter.printProof(p));
                System.Console.WriteLine("");
            }
        }

        private static void lovesAnimalDemo(InferenceProcedure ip)
        {
            StandardizeApartIndexicalFactory.flush();

            FOLKnowledgeBase kb = FOLKnowledgeBaseFactory
                    .createLovesAnimalKnowledgeBase(ip);

            string kbStr = kb.ToString();

            IQueue<Term> terms = Factory.CreateQueue<Term>();
            terms.Add(new Constant("Curiosity"));
            terms.Add(new Constant("Tuna"));
            Predicate query = new Predicate("Kills", terms);

            InferenceResult answer = kb.ask(query);

            System.Console.WriteLine("Loves Animal Knowledge Base:");
            System.Console.WriteLine(kbStr);
            System.Console.WriteLine("Query: " + query);
            foreach (Proof p in answer.getProofs())
            {
                System.Console.Write(ProofPrinter.printProof(p));
                System.Console.WriteLine("");
            }
        }

        private static void abcEqualityAxiomDemo(InferenceProcedure ip)
        {
            StandardizeApartIndexicalFactory.flush();

            FOLKnowledgeBase kb = FOLKnowledgeBaseFactory
                    .createABCEqualityKnowledgeBase(ip, true);

            string kbStr = kb.ToString();

            TermEquality query = new TermEquality(new Constant("A"), new Constant(
                    "C"));

            InferenceResult answer = kb.ask(query);

            System.Console.WriteLine("ABC Equality Axiom Knowledge Base:");
            System.Console.WriteLine(kbStr);
            System.Console.WriteLine("Query: " + query);
            foreach (Proof p in answer.getProofs())
            {
                System.Console.Write(ProofPrinter.printProof(p));
                System.Console.WriteLine("");
            }
        }

        private static void abcEqualityNoAxiomDemo(InferenceProcedure ip)
        {
            StandardizeApartIndexicalFactory.flush();

            FOLKnowledgeBase kb = FOLKnowledgeBaseFactory
                    .createABCEqualityKnowledgeBase(ip, false);

            string kbStr = kb.ToString();

            TermEquality query = new TermEquality(new Constant("A"), new Constant(
                    "C"));

            InferenceResult answer = kb.ask(query);

            System.Console.WriteLine("ABC Equality No Axiom Knowledge Base:");
            System.Console.WriteLine(kbStr);
            System.Console.WriteLine("Query: " + query);
            foreach (Proof p in answer.getProofs())
            {
                System.Console.Write(ProofPrinter.printProof(p));
                System.Console.WriteLine("");
            }
        }
    }
}
