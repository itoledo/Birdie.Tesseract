using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tvn.cosine.ai.nlp.ranking
{
    /**
     * 
     * @author Jonathon Belotti (thundergolfer)
     *
     */
    public class RunHITS
    {

        public static void main(String[] args)
        {
            IList<Page> result;
            // build page table
            IDictionary<string, Page> pageTable = PagesDataset.loadDefaultPages();
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

}
