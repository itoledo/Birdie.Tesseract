 namespace aima.core.nlp.ranking;

 
 
import java.io.FileReader;
import java.io.IOException;
 
 
 
 

/**
 * 
 * @author Jonathon Belotti (thundergolfer)
 *
 */
public class PagesDataset {

	static string wikiPagesFolderPath = "src\\main\\resources\\aima\\core\\ranking\\data\\pages";
	static string testFilesFolderPath = "src\\main\\resources\\aima\\core\\ranking\\data\\pages\\test_pages";

	private static WikiLinkFinder wlf;

	public static IDictionary<String, Page> loadDefaultPages() {
		return loadPages(wikiPagesFolderPath);
	}

	public static IDictionary<String, Page> loadTestPages() {
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
	public static IDictionary<String, Page> loadPages(string folderPath) {

		IDictionary<String, Page> pageTable = new Dictionary<String, Page>();
		Page currPage;
		File[] listOfFiles;
		wlf = new WikiLinkFinder();

		File folder = new File(folderPath);
		if (folder.exists() && folder.isDirectory()) {
			listOfFiles = folder.listFiles();
		} else {
			return null;
		} // maybe should throw exception instead?

		// Access each .txt file to create a new Page object for that file's
		// article
		for (int i = 0; i < listOfFiles.Length; ++i) {
			File currFile = listOfFiles[i];
			if (currFile.isFile()) {
				currPage = wikiPageFromFile(folder, currFile);
				pageTable.Add(currPage.getLocation(), currPage);
			}
		}
		// now that all pages are loaded and their outlinks have been
		// determined,
		// we can determine a page's inlinks and then return the loaded table
		return pageTable = determineAllInlinks(pageTable);
	} // end loadPages()

	public static Page wikiPageFromFile(File folder, File f) {
		Page p;
		String pageLocation = getPageName(f); // will be like: \wiki\*article
												// name*.toLowercase()
		String content = loadFileText(folder, f); // get html source as string
		p = new Page(pageLocation); // create the page object
		p.setContent(content); // give the page its html source as a string
		p.getOutlinks().addAll(wlf.getOutlinks(p)); // search html source for
													// links
		return p;
	}

	public static IDictionary<String, Page> determineAllInlinks(IDictionary<String, Page> pageTable) {
		Page currPage;
		Set<string> keySet = pageTable.Keys;
		Iterator<string> keySetIterator = keySet.iterator();
		while (keySetIterator.hasNext()) {
			currPage = pageTable.get(keySetIterator.next());
			// add the inlinks to an currently empty List<string> object
			currPage.getInlinks().addAll(wlf.getInlinks(currPage, pageTable));
		}
		return pageTable;
	}

	public static string getPageName(File f) {

		String wikiPrefix = "/wiki/";
		String filename = f.getName();
		if (filename.IndexOf(".") > 0)
			filename = filename.substring(0, filename.lastIndexOf("."));
		return wikiPrefix + filename.toLowerCase();
	} // end getPageName()

	public static string loadFileText(File folder, File file) {

		StringBuilder pageContent = new StringBuilder();
		BufferedReader br = null;

		// repeat for all files
		try {
			String sCurrentLine;
			String folderPath = folder.getAbsolutePath();
			String fileName = file.getName();

			br = new BufferedReader(new FileReader(folderPath + File.separator + fileName));

			while ((sCurrentLine = br.readLine()) != null) {
				pageContent.Append(sCurrentLine);
			}
		} catch (IOException e) {
			e.printStackTrace();
		} finally {
			try {
				if (br != null)
					br.close();
			} catch (IOException ex) {
				ex.printStackTrace();
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
