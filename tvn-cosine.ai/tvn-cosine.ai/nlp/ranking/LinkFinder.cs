namespace tvn.cosine.ai.nlp.ranking
{
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
        IQueue<string> getOutlinks(Page page);

        /**
         * Take a Page object and return its inlinks (who links to it) as a list of
         * strings.
         * 
         * @param page
         * @param pageTable
         * @return
         */
        IQueue<string> getInlinks(Page page, Map<string, Page> pageTable);

    }
}
