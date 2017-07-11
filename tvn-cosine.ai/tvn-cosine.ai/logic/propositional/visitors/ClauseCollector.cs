﻿using System;
using System.Collections.Generic;
using tvn.cosine.ai.logic.propositional.kb.data;
using tvn.cosine.ai.logic.propositional.parsing.ast;

namespace tvn.cosine.ai.logic.propositional.visitors
{
    /**
     * Utility class for collecting clauses from CNF Sentences.
     * 
     * @author Ravi Mohan
     * @author Ciaran O'Reilly
     */
    public class ClauseCollector : BasicGatherer<Clause>
    {

        /**
         * Collect a set of clauses from a list of given sentences.
         * 
         * @param cnfSentences
         *            a list of CNF sentences from which to collect clauses.
         * @return a set of all contained clauses.
         * @throws IllegalArgumentException
         *             if any of the given sentences are not in CNF.
         */
        public static ISet<Clause> getClausesFrom(params Sentence[] cnfSentences)
        {
            ISet<Clause> result = new HashSet<Clause>();

            ClauseCollector clauseCollector = new ClauseCollector();
            foreach (Sentence cnfSentence in cnfSentences)
            {
                result = cnfSentence.accept(clauseCollector, result);
            }

            return result;
        }

        public override ISet<Clause> visitPropositionSymbol(PropositionSymbol s, ISet<Clause> arg)
        {
            // a positive unit clause
            Literal positiveLiteral = new Literal(s);
            arg.Add(new Clause(positiveLiteral));

            return arg;
        }

        public override ISet<Clause> visitUnarySentence(ComplexSentence s, ISet<Clause> arg)
        {

            if (!s.getSimplerSentence(0).isPropositionSymbol())
            {
                throw new Exception("Sentence is not in CNF: " + s);
            }

            // a negative unit clause
            Literal negativeLiteral = new Literal((PropositionSymbol)s.getSimplerSentence(0), false);
            arg.Add(new Clause(negativeLiteral));

            return arg;
        }

        public override ISet<Clause> visitBinarySentence(ComplexSentence s, ISet<Clause> arg)
        {

            if (s.isAndSentence())
            {
                s.getSimplerSentence(0).accept(this, arg);
                s.getSimplerSentence(1).accept(this, arg);
            }
            else if (s.isOrSentence())
            {
                List<Literal> literals = new List<Literal>(LiteralCollector.getLiterals(s));
                arg.Add(new Clause(literals));
            }
            else
            {
                throw new ArgumentException("Sentence is not in CNF: " + s);
            }

            return arg;
        }

        //
        // PRIVATE
        //
        private class LiteralCollector : BasicGatherer<Literal>
        { 
            public static ISet<Literal> getLiterals(Sentence disjunctiveSentence)
            {
                ISet<Literal> result = new HashSet<Literal>();

                LiteralCollector literalCollector = new LiteralCollector();
                result = disjunctiveSentence.accept(literalCollector, result);

                return result;
            }

            public override ISet<Literal> visitPropositionSymbol(PropositionSymbol s, ISet<Literal> arg)
            {
                // a positive literal
                Literal positiveLiteral = new Literal(s);
                arg.Add(positiveLiteral);

                return arg;
            }

            public override ISet<Literal> visitUnarySentence(ComplexSentence s, ISet<Literal> arg)
            {

                if (!s.getSimplerSentence(0).isPropositionSymbol())
                {
                    throw new Exception("Sentence is not in CNF: " + s);
                }

                // a negative literal
                Literal negativeLiteral = new Literal((PropositionSymbol)s.getSimplerSentence(0), false);

                arg.Add(negativeLiteral);

                return arg;
            }

            public override ISet<Literal> visitBinarySentence(ComplexSentence s, ISet<Literal> arg)
            {
                if (s.isOrSentence())
                {
                    s.getSimplerSentence(0).accept(this, arg);
                    s.getSimplerSentence(1).accept(this, arg);
                }
                else
                {
                    throw new ArgumentException("Sentence is not in CNF: " + s);
                }
                return arg;
            }
        }
    }

}