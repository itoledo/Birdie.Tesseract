namespace tvn.cosine.ai.nlp.ranking
{
    public class WikiLinkFinder : LinkFinder
    {

        // TODO
        // Make more intelligent link search
        public IQueue<string> getOutlinks(Page page)
        {

            string content = page.getContent();
            IQueue<string> outLinks = Factory.CreateQueue<string>();
            // search content for all href="x" outlinks
            IQueue<string> allMatches = Factory.CreateQueue<string>();
            Matcher m = Pattern.compile("href=\"(/wiki/.*?)\"").matcher(content);
            while (m.find())
            {
                allMatches.Add(m.group());
            }
            for (int i = 0; i < allMatches.size(); i++)
            {
                string match = allMatches.Get(i);
                String[] tokens = match.split("\"");
                string location = tokens[1].toLowerCase(); // also, tokens[0] = the
                                                           // text before the first
                                                           // quote,
                                                           // and tokens[2] is the
                                                           // text after the second
                                                           // quote
                outLinks.Add(location);
            }

            return outLinks;
        }

         
    public IQueue<string> getInlinks(Page target, IMap<string, Page> pageTable)
        {

            string location = target.getLocation().toLowerCase(); // make comparison
                                                                  // case
                                                                  // insensitive
            IQueue<string> inlinks = Factory.CreateQueue<string>(); // initialise a list for
                                                            // the inlinks

            // go through all pages and if they link back to target then add that
            // page's location to the target's inlinks
            Iterator<string> keySetIterator = pageTable.GetKeys().iterator();
            while (keySetIterator.hasNext())
            {
                Page p = pageTable.Get(keySetIterator.next());
                for (int i = 0; i < p.getOutlinks().size(); i++)
                {
                    string pForward = p.getOutlinks().Get(i).toLowerCase().replace('\\', '/');
                    string pBackward = p.getOutlinks().Get(i).toLowerCase().replace('/', '\\');
                    if (pForward.Equals(location) || pBackward.Equals(location))
                    {
                        inlinks.Add(p.getLocation().toLowerCase());
                        break;
                    }
                }
            }
            return inlinks;
        }

    }
}
