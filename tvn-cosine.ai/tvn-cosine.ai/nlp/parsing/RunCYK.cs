using System;
using System.Collections.Generic;
using tvn.cosine.ai.nlp.data.grammers;
using tvn.cosine.ai.nlp.parsing.grammers;

namespace tvn.cosine.ai.nlp.parsing
{
    /**
     * A simple runner class to test out one parsing scenario on CYK.
     * @author Jonathon
     *
     */
    public class RunCYK
    {
        public static void main(String[] args)
        {
            Console.WriteLine("Running...");
            ProbCNFGrammar exampleG = ProbCNFGrammarExamples.buildTrivialGrammar();
            CYK cyk = new CYK();
            List<string> words = new List<string>(new[] { "the", "man", "liked", "a", "woman" });
            float[,,] probTable = cyk.parse(words, exampleG);
            cyk.printProbTable(probTable, words, exampleG);
            Console.WriteLine("Done!");
        }
    }

}
