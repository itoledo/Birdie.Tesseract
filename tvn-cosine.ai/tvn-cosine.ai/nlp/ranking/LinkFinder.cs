using System.Collections.Generic;

namespace tvn.cosine.ai.nlp.ranking
{
    /**
     * 
     * @author Jonathon Belotti (thundergolfer)
     *
     */
    public interface LinkFinder
    {

        /**
         * Take a Page object and return its outlinks as a list of strings. The Page
         * object must therefore possess the information to determine what it links
         * to.
         * 
         * @param page
         * @return
         */
        IList<string> getOutlinks(Page page);

        /**
         * Take a Page object and return its inlinks (who links to it) as a list of
         * strings.
         * 
         * @param page
         * @param pageTable
         * @return
         */
        IList<string> getInlinks(Page page, IDictionary<string, Page> pageTable);

    }

}
