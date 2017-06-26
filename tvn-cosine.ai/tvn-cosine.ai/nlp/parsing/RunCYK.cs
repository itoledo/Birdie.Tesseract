 namespace aima.core.nlp.parsing;

 
import java.util.Arrays;
 

import aima.core.nlp.data.grammars.ProbCNFGrammarExamples;
import aima.core.nlp.parsing.grammars.ProbCNFGrammar;

/**
 * A simple runner class to test out one parsing scenario on CYK.
 * @author Jonathon
 *
 */
public class RunCYK {

	
	public static void main(string[] args) {
		Console.WriteLine("Running...");
		ProbCNFGrammar exampleG = ProbCNFGrammarExamples.buildTrivialGrammar();
		CYK cyk = new CYK();
		List<string> words = new List<string>(Arrays.asList("the","man","liked","a","woman"));
		float[][][] probTable = cyk.parse(words, exampleG);
		cyk.printProbTable(probTable, words, exampleG);
		Console.WriteLine("Done!");
	}
}
