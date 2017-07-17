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
            System.out.println("Running...");
            ProbCNFGrammar exampleG = ProbCNFGrammarExamples.buildTrivialGrammar();
            CYK cyk = new CYK();
            List<String> words = new ArrayList<>(Arrays.asList("the", "man", "liked", "a", "woman"));
            float[][][] probTable = cyk.parse(words, exampleG);
            cyk.printProbTable(probTable, words, exampleG);
            System.out.println("Done!");
        }
    }
}
