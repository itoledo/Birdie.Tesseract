using System;
using System.Collections.Generic;
using System.IO;
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
    public class PagesDataset
    {
        static string wikiPagesFolderPath = "src\\main\\resources\\aima\\core\\ranking\\data\\pages";
        static string testFilesFolderPath = "src\\main\\resources\\aima\\core\\ranking\\data\\pages\\test_pages";

        private static WikiLinkFinder wlf;

        public static IDictionary<string, Page> loadDefaultPages()
        {
            return loadPages(wikiPagesFolderPath);
        }

        public static IDictionary<string, Page> loadTestPages()
        {
            return loadPages(testFilesFolderPath);
        }

        /**
         * Access a folder of .txt files containing wikipedia html source, and give
         * back a hashtable of pages, which each page having it's correct inlink
         * list and outlink list.
         * 
         * @param folderPath
         * @return a hashtable of Page objects, accessed by article name (which is a
         *         location for wikipedia: \wiki\*article name*)
         */
        public static IDictionary<string, Page> loadPages(string folderPath)
        {
            IDictionary<string, Page> pageTable = new Dictionary<string, Page>();
            Page currPage;
            string[] listOfFiles;
            wlf = new WikiLinkFinder();

            if (System.IO.Directory.Exists(folderPath))
            {
                listOfFiles = System.IO.Directory.GetFiles(folderPath);
            }
            else
            {
                return null;
            } // maybe should throw exception instead?

            // Access each .txt file to create a new Page object for that file's
            // article
            for (int i = 0; i < listOfFiles.Length; i++)
            {
                FileInfo currFile = new System.IO.FileInfo(listOfFiles[i]);
                currPage = wikiPageFromFile(listOfFiles[i], currFile);
                pageTable.Add(currPage.getLocation(), currPage);
            }
            // now that all pages are loaded and their outlinks have been
            // determined,
            // we can determine a page's inlinks and then return the loaded table
            return pageTable = determineAllInlinks(pageTable);
        } // end loadPages()

        public static Page wikiPageFromFile(string folder, FileInfo f)
        {
            Page p;
            string pageLocation = getPageName(f); // will be like: \wiki\*article
                                                  // name*.toLowercase()
            string content = loadFileText(folder, f); // get html source as string
            p = new Page(pageLocation); // create the page object
            p.setContent(content); // give the page its html source as a string

            foreach (var v in wlf.getOutlinks(p))
            p.getOutlinks().Add(v); // search html source for
                                                        // links
            return p;
        }

        public static IDictionary<string, Page> determineAllInlinks(IDictionary<string, Page> pageTable)
        { 
            foreach (var currPage in pageTable.Values)  
            {
                // add the inlinks to an currently empty List<string> object
                foreach (var v in wlf.getInlinks(currPage, pageTable))
                currPage.getInlinks().Add(v);
            }
            return pageTable;
        }

        public static string getPageName(FileInfo f)
        {
            string wikiPrefix = "/wiki/";
            string filename = f.Name;
            if (filename.IndexOf(".") > 0)
                filename = filename.Substring(0, filename.LastIndexOf("."));
            return wikiPrefix + filename.ToLower();
        } // end getPageName()

        public static string loadFileText(string folder, FileInfo file)
        {
            StringBuilder pageContent = new StringBuilder();

            // repeat for all files
            using (var br = new System.IO.StreamReader(file.FullName))
            {
                while (!br.EndOfStream)
                {
                    pageContent.Append(br.ReadLine());
                }
            }
            return pageContent.ToString();
        } // end loadFileText()

        // TODO:
        // Be able to automatically retrieve an arbitrary number of
        // wikipaedia pages and create a hashtable of Pages from them.

        // TODO:
        // Be able to automatically retreive an arbitraru number of webpages
        // that are in a network conducive to application of the HITS algorithm
    }

}
