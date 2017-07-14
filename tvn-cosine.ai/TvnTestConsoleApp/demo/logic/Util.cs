using System;
using System.Collections.Generic;
using tvn.cosine.ai.logic.fol;
using tvn.cosine.ai.logic.fol.inference;
using tvn.cosine.ai.logic.fol.inference.proof;
using tvn.cosine.ai.logic.fol.kb;
using tvn.cosine.ai.logic.fol.parsing.ast;

namespace TvnTestConsoleApp.demo.logic
{
    public static class Util
    {

        public static void kingsDemo1(InferenceProcedure ip)
        {
            StandardizeApartIndexicalFactory.flush();

            FOLKnowledgeBase kb = FOLKnowledgeBaseFactory.createKingsKnowledgeBase(ip);

            string kbStr = kb.ToString();

            IList<Term> terms = new List<Term>();
            terms.Add(new Constant("John"));
            Predicate query = new Predicate("Evil", terms);

            InferenceResult answer = kb.ask(query);

            Console.WriteLine("Kings Knowledge Base:");
            Console.WriteLine(kbStr);
            Console.WriteLine("Query: " + query);
            foreach (Proof p in answer.getProofs())
            {
                Console.Write(ProofPrinter.printProof(p));
                Console.WriteLine("");
            }
        }

        public static void kingsDemo2(InferenceProcedure ip)
        {
            StandardizeApartIndexicalFactory.flush();

            FOLKnowledgeBase kb = FOLKnowledgeBaseFactory.createKingsKnowledgeBase(ip);

            string kbStr = kb.ToString();

            IList<Term> terms = new List<Term>();
            terms.Add(new Variable("x"));
            Predicate query = new Predicate("King", terms);

            InferenceResult answer = kb.ask(query);

            Console.WriteLine("Kings Knowledge Base:");
            Console.WriteLine(kbStr);
            Console.WriteLine("Query: " + query);
            foreach (Proof p in answer.getProofs())
            {
                Console.Write(ProofPrinter.printProof(p));
            }
        }

        public static void weaponsDemo(InferenceProcedure ip)
        {
            StandardizeApartIndexicalFactory.flush();

            FOLKnowledgeBase kb = FOLKnowledgeBaseFactory.createWeaponsKnowledgeBase(ip);

            string kbStr = kb.ToString();

            IList<Term> terms = new List<Term>();
            terms.Add(new Variable("x"));
            Predicate query = new Predicate("Criminal", terms);

            InferenceResult answer = kb.ask(query);

            Console.WriteLine("Weapons Knowledge Base:");
            Console.WriteLine(kbStr);
            Console.WriteLine("Query: " + query);
            foreach (Proof p in answer.getProofs())
            {
                Console.Write(ProofPrinter.printProof(p));
                Console.WriteLine("");
            }
        }


        public static void lovesAnimalDemo(InferenceProcedure ip)
        {
            StandardizeApartIndexicalFactory.flush();

            FOLKnowledgeBase kb = FOLKnowledgeBaseFactory.createLovesAnimalKnowledgeBase(ip);

            string kbStr = kb.ToString();

            IList<Term> terms = new List<Term>();
            terms.Add(new Constant("Curiosity"));
            terms.Add(new Constant("Tuna"));
            Predicate query = new Predicate("Kills", terms);

            InferenceResult answer = kb.ask(query);

            Console.WriteLine("Loves Animal Knowledge Base:");
            Console.WriteLine(kbStr);
            Console.WriteLine("Query: " + query);
            foreach (Proof p in answer.getProofs())
            {
                Console.Write(ProofPrinter.printProof(p));
                Console.WriteLine("");
            }
        }

        public static void abcEqualityAxiomDemo(InferenceProcedure ip)
        {
            StandardizeApartIndexicalFactory.flush();

            FOLKnowledgeBase kb = FOLKnowledgeBaseFactory.createABCEqualityKnowledgeBase(ip, true);

            string kbStr = kb.ToString();

            TermEquality query = new TermEquality(new Constant("A"), new Constant("C"));

            InferenceResult answer = kb.ask(query);

            Console.WriteLine("ABC Equality Axiom Knowledge Base:");
            Console.WriteLine(kbStr);
            Console.WriteLine("Query: " + query);
            foreach (Proof p in answer.getProofs())
            {
                Console.Write(ProofPrinter.printProof(p));
                Console.WriteLine("");
            }
        }

        public static void abcEqualityNoAxiomDemo(InferenceProcedure ip)
        {
            StandardizeApartIndexicalFactory.flush();

            FOLKnowledgeBase kb = FOLKnowledgeBaseFactory
                    .createABCEqualityKnowledgeBase(ip, false);

            string kbStr = kb.ToString();

            TermEquality query = new TermEquality(new Constant("A"), new Constant("C"));

            InferenceResult answer = kb.ask(query);

            Console.WriteLine("ABC Equality No Axiom Knowledge Base:");
            Console.WriteLine(kbStr);
            Console.WriteLine("Query: " + query);
            foreach (Proof p in answer.getProofs())
            {
                Console.Write(ProofPrinter.printProof(p));
                Console.WriteLine("");
            }
        }
    }
}
