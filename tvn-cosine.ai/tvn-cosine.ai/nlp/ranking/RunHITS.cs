 namespace aima.core.nlp.ranking;

 
 

/**
 * 
 * @author Jonathon Belotti (thundergolfer)
 *
 */
public class RunHITS {

	public static void main(string[] args) {
		List<Page> result;
		// build page table
		IDictionary<String, Page> pageTable = PagesDataset.loadDefaultPages();
		// Create HITS Ranker
		HITS hits = new HITS(pageTable);
		// run hits
		Console.WriteLine("Ranking...");
		result = hits.hits("man is");
		// report results
		Console.WriteLine("Ranking Finished.");
		hits.report(result);
	}
}
