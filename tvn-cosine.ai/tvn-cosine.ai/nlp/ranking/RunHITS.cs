using tvn.cosine.ai.common.collections.api;

namespace tvn.cosine.ai.nlp.ranking
{
    public class RunHITS
    {

        public static void main(params string[] args)
        {
            ICollection<Page> result;
            // build page table
            IMap<string, Page> pageTable = PagesDataset.loadDefaultPages();
            // Create HITS Ranker
            HITS hits = new HITS(pageTable);
            // run hits
            System.Console.WriteLine("Ranking...");
            result = hits.hits("man is");
            // report results
            System.Console.WriteLine("Ranking Finished.");
            hits.report(result);
        }
    }
}
