namespace tvn.cosine.ai.logic.propositional.kb
{
    public class KnowledgeBase
    {
        private IQueue<Sentence> sentences = Factory.CreateQueue<>();
        private ConjunctionOfClauses asCNF = new ConjunctionOfClauses(Collections.emptySet());
        private ISet<PropositionSymbol> symbols = Factory.CreateSet<>();
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
            if (!(sentences.contains(aSentence)))
            {
                sentences.Add(aSentence);
                asCNF = asCNF.extend(ConvertToConjunctionOfClauses.convert(aSentence).getClauses());
                symbols.addAll(SymbolCollector.getSymbolsFrom(aSentence));
            }
        }

        /**
         * Each time the agent program is called, it TELLS the knowledge base what
         * it perceives.
         * 
         * @param percepts
         *            what the agent perceives
         */
        public void tellAll(String[] percepts)
        {
            for (string percept : percepts)
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
            return sentences.size();
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
            return asCNF.getClauses();
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
            return sentences.isEmpty() ? "" : asSentence().ToString();
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
