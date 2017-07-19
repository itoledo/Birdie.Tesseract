using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.logic.propositional.inference;
using tvn.cosine.ai.logic.propositional.kb.data;
using tvn.cosine.ai.logic.propositional.parsing;
using tvn.cosine.ai.logic.propositional.parsing.ast;
using tvn.cosine.ai.logic.propositional.visitors;

namespace tvn.cosine.ai.logic.propositional.kb
{
    public class KnowledgeBase
    {
        private IQueue<Sentence> sentences = Factory.CreateQueue<Sentence>();
        private ConjunctionOfClauses _asCNF = new ConjunctionOfClauses(Factory.CreateSet<Clause>());
        private ISet<PropositionSymbol> symbols = Factory.CreateSet<PropositionSymbol>();
        private PLParser parser = new PLParser();


        /**
         * Adds the specified sentence to the knowledge base.
         * 
         * @param aSentence
         *            a fact to be added to the knowledge base.
         */
        public void tell(string aSentence)
        {
            tell((Sentence)parser.parse(aSentence));

        }

        /**
         * Adds the specified sentence to the knowledge base.
         * 
         * @param aSentence
         *            a fact to be added to the knowledge base.
         */
        public void tell(Sentence aSentence)
        {
            if (!(sentences.Contains(aSentence)))
            {
                sentences.Add(aSentence);
                _asCNF = _asCNF.extend(ConvertToConjunctionOfClauses.convert(aSentence).getClauses());
                symbols.AddAll(SymbolCollector.getSymbolsFrom(aSentence));
            }
        }

        /**
         * Each time the agent program is called, it TELLS the knowledge base what
         * it perceives.
         * 
         * @param percepts
         *            what the agent perceives
         */
        public void tellAll(string[] percepts)
        {
            foreach (string percept in percepts)
            {
                tell(percept);
            }

        }

        /**
         * Returns the number of sentences in the knowledge base.
         * 
         * @return the number of sentences in the knowledge base.
         */
        public int size()
        {
            return sentences.Size();
        }

        /**
         * Returns the list of sentences in the knowledge base chained together as a
         * single sentence.
         * 
         * @return the list of sentences in the knowledge base chained together as a
         *         single sentence.
         */
        public Sentence asSentence()
        {
            return Sentence.newConjunction(sentences);
        }

        /**
         * 
         * @return a Conjunctive Normal Form (CNF) representation of the Knowledge Base.
         */
        public ISet<Clause> asCNF()
        {
            return _asCNF.getClauses();
        }

        /**
         * 
         * @return a unique set of the symbols currently contained in the Knowledge Base.
         */
        public ISet<PropositionSymbol> getSymbols()
        {
            return symbols;
        }

        /**
         * Returns the answer to the specified question using the TT-Entails
         * algorithm.
         * 
         * @param queryString
         *            a question to ASK the knowledge base
         * 
         * @return the answer to the specified question using the TT-Entails
         *         algorithm.
         */
        public bool askWithTTEntails(string queryString)
        {
            PLParser parser = new PLParser();

            Sentence alpha = parser.parse(queryString);

            return new TTEntails().ttEntails(this, alpha);
        }


        public override string ToString()
        {
            return sentences.IsEmpty() ? "" : asSentence().ToString();
        }

        /**
         * Returns the list of sentences in the knowledge base.
         * 
         * @return the list of sentences in the knowledge base.
         */
        public IQueue<Sentence> getSentences()
        {
            return sentences;
        }
    }
}
