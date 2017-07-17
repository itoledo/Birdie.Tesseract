namespace tvn.cosine.ai.nlp.ranking
{
    public class RunHITS
    {

        public static void main(String[] args)
        {
            List<Page> result;
            // build page table
            Map<String, Page> pageTable = PagesDataset.loadDefaultPages();
            // Create HITS Ranker
            HITS hits = new HITS(pageTable);
            // run hits
            System.out.println("Ranking...");
            result = hits.hits("man is");
            // report results
            System.out.println("Ranking Finished.");
            hits.report(result);
        }
    }
}
