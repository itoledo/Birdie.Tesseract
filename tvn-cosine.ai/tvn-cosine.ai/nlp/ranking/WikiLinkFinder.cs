 namespace aima.core.nlp.ranking;

 
 
 
 
 
 

/**
 * 
 * @author Jonathon Belotti (thundergolfer)
 *
 */
public class WikiLinkFinder : LinkFinder {

	// TODO
	// Make more intelligent link search
	public List<string> getOutlinks(Page page) {

		String content = page.getContent();
		List<string> outLinks = new List<string>();
		// search content for all href="x" outlinks
		List<string> allMatches = new List<string>();
		Matcher m = Pattern.compile("href=\"(/wiki/.*?)\"").matcher(content);
		while (m.find()) {
			allMatches.Add(m.group());
		}
		for (int i = 0; i < allMatches.Count; ++i) {
			String match = allMatches.get(i);
			string[] tokens = match.split("\"");
			String location = tokens[1].toLowerCase(); // also, tokens[0] = the
														// text before the first
														// quote,
														// and tokens[2] is the
														// text after the second
														// quote
			outLinks.Add(location);
		}

		return outLinks;
	}

	 
	public List<string> getInlinks(Page target, IDictionary<String, Page> pageTable) {

		String location = target.getLocation().toLowerCase(); // make comparison
																// case
																// insensitive
		List<string> inlinks = new List<string>(); // initialise a list for
														// the inlinks

		// go through all pages and if they link back to target then add that
		// page's location to the target's inlinks
		Iterator<string> keySetIterator = pageTable.Keys.iterator();
		while (keySetIterator.hasNext()) {
			Page p = pageTable.get(keySetIterator.next());
			for (int i = 0; i < p.getOutlinks().Count; ++i) {
				String pForward = p.getOutlinks().get(i).toLowerCase().replace('\\', '/');
				String pBackward = p.getOutlinks().get(i).toLowerCase().replace('/', '\\');
				if (pForward .Equals(location) || pBackward .Equals(location)) {
					inlinks.Add(p.getLocation().toLowerCase());
					break;
				}
			}
		}
		return inlinks;
	}

}
