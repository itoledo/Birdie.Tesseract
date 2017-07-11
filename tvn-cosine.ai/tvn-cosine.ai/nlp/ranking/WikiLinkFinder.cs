using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace tvn.cosine.ai.nlp.ranking
{
    /**
     * 
     * @author Jonathon Belotti (thundergolfer)
     *
     */
    public class WikiLinkFinder : LinkFinder
    {
        // TODO
        // Make more intelligent link search
        public IList<string> getOutlinks(Page page)
        {
            string content = page.getContent();
            List<string> outLinks = new List<string>();
            // search content for all href="x" outlinks
            List<string> allMatches = new List<string>();
            var m = new Regex("href=\"(/wiki/.*?)\"").Matches(content);
            foreach (var x in m)
            {
                allMatches.Add(x.ToString());
            }
            for (int i = 0; i < allMatches.Count; i++)
            {
                string match = allMatches[i];
                string[] tokens = match.Split('\"');
                string location = tokens[1].ToLower(); // also, tokens[0] = the
                                                       // text before the first
                                                       // quote,
                                                       // and tokens[2] is the
                                                       // text after the second
                                                       // quote
                outLinks.Add(location);
            }

            return outLinks;
        }

        public IList<string> getInlinks(Page target, IDictionary<string, Page> pageTable)
        {

            string location = target.getLocation().ToLower(); // make comparison
                                                              // case
                                                              // insensitive
            List<String> inlinks = new List<string>(); // initialise a list for
                                                       // the inlinks

            // go through all pages and if they link back to target then add that
            // page's location to the target's inlinks
            foreach (var p in pageTable.Values)
            {
                for (int i = 0; i < p.getOutlinks().Count; i++)
                {
                    string pForward = p.getOutlinks()[i].ToLower().Replace('\\', '/');
                    string pBackward = p.getOutlinks()[i].ToLower().Replace('/', '\\');
                    if (pForward.Equals(location) || pBackward.Equals(location))
                    {
                        inlinks.Add(p.getLocation().ToLower());
                        break;
                    }
                }
            }
            return inlinks;
        }

    }

}
