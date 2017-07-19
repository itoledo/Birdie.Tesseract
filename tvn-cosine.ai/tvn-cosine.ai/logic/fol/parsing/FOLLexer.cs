﻿using System.Text;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.logic.common;
using tvn.cosine.ai.logic.fol.domain;

namespace tvn.cosine.ai.logic.fol.parsing
{
    public class FOLLexer : Lexer
    {
        private FOLDomain domain;
        private ISet<string> connectors, quantifiers;

        public FOLLexer(FOLDomain domain)
        {
            this.domain = domain;

            connectors = Factory.CreateSet<string>();
            connectors.Add(Connectors.NOT);
            connectors.Add(Connectors.AND);
            connectors.Add(Connectors.OR);
            connectors.Add(Connectors.IMPLIES);
            connectors.Add(Connectors.BICOND);

            quantifiers = Factory.CreateSet<string>();
            quantifiers.Add(Quantifiers.FORALL);
            quantifiers.Add(Quantifiers.EXISTS);
        }

        public FOLDomain getFOLDomain()
        {
            return domain;
        }

        public override Token nextToken()
        {
            int startPosition = getCurrentPositionInInput();
            if (lookAhead(1) == '(')
            {
                consume();
                return new Token(LogicTokenTypes.LPAREN, "(", startPosition);

            }
            else if (lookAhead(1) == ')')
            {
                consume();
                return new Token(LogicTokenTypes.RPAREN, ")", startPosition);

            }
            else if (lookAhead(1) == ',')
            {
                consume();
                return new Token(LogicTokenTypes.COMMA, ",", startPosition);

            }
            else if (identifierDetected())
            {
                // System.Console.WriteLine("identifier detected");
                return identifier();
            }
            else if (null != lookAhead(1) && char.IsWhiteSpace(lookAhead(1).Value))
            {
                consume();
                return nextToken();
            }
            else if (null == lookAhead(1))
            {
                return new Token(LogicTokenTypes.EOI, "EOI", startPosition);
            }
            else
            {
                throw new LexerException("Lexing error on character " + lookAhead(1) + " at position " + getCurrentPositionInInput(), getCurrentPositionInInput());
            }
        }

        private Token identifier()
        {
            int startPosition = getCurrentPositionInInput();
            StringBuilder sbuf = new StringBuilder();
            while ((null != lookAhead(1) && Character.isJavaIdentifierPart(lookAhead(1).Value)) || partOfConnector())
            {
                sbuf.Append(lookAhead(1));
                consume();
            }
            string readString = sbuf.ToString();
            // System.Console.WriteLine(readString);
            if (connectors.Contains(readString))
            {
                return new Token(LogicTokenTypes.CONNECTIVE, readString, startPosition);
            }
            else if (quantifiers.Contains(readString))
            {
                return new Token(LogicTokenTypes.QUANTIFIER, readString, startPosition);
            }
            else if (domain.getPredicates().Contains(readString))
            {
                return new Token(LogicTokenTypes.PREDICATE, readString, startPosition);
            }
            else if (domain.getFunctions().Contains(readString))
            {
                return new Token(LogicTokenTypes.FUNCTION, readString, startPosition);
            }
            else if (domain.getConstants().Contains(readString))
            {
                return new Token(LogicTokenTypes.CONSTANT, readString, startPosition);
            }
            else if (isVariable(readString))
            {
                return new Token(LogicTokenTypes.VARIABLE, readString, startPosition);
            }
            else if (readString.Equals("="))
            {
                return new Token(LogicTokenTypes.EQUALS, readString, startPosition);
            }
            else
            {
                throw new LexerException("Lexing error on character " + lookAhead(1) + " at position " + getCurrentPositionInInput(), getCurrentPositionInInput());
            }
        }

        private bool isVariable(string s)
        {
            return (char.IsLower(s[0]));
        }

        private bool identifierDetected()
        {
            return (null != lookAhead(1) && Character.isJavaIdentifierStart(lookAhead(1).Value)) || partOfConnector();
        }

        private bool partOfConnector()
        {
            return (lookAhead(1) == '=') || (lookAhead(1) == '<') || (lookAhead(1) == '>');
        }
    }
}
