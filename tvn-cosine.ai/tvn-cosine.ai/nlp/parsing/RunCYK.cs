using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.collections.api;
using tvn.cosine.ai.nlp.data.grammars;
using tvn.cosine.ai.nlp.parsing.grammars;

namespace tvn.cosine.ai.nlp.parsing
{
    /**
     * A simple runner class to test out one parsing scenario on CYK. 
     */
    public class RunCYK
    {
        public static void main(params string[] args)
        {
            System.Console.WriteLine("Running...");
            ProbCNFGrammar exampleG = ProbCNFGrammarExamples.buildTrivialGrammar();
            CYK cyk = new CYK();
            ICollection<string> words = CollectionFactory.CreateQueue<string>(new[] { "the", "man", "liked", "a", "woman" });
            float[,,] probTable = cyk.parse(words, exampleG);
            cyk.printProbTable(probTable, words, exampleG);
            System.Console.WriteLine("Done!");
        }
    }
}
