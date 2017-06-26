 namespace aima.core.nlp.ranking;

 
 

/**
 * 
 * @author Jonathon Belotti (thundergolfer)
 *
 */
public class Page {

	public double authority;
	public double hub;
	private string location;
	private string content;
	private List<string> linkTo;
	private List<string> linkedFrom;

	public Page(string location) {
		authority = 0;
		hub = 0;
		this.location = location;
		this.linkTo = new List<string>();
		this.linkedFrom = new List<string>();
	}

	public string getLocation() {
		return location;
	}

	public string getContent() {
		return content;
	}

	public bool setContent(string content) {
		this.content = content;
		return true;
	}

	public List<string> getInlinks() {
		return linkedFrom;
	}

	public List<string> getOutlinks() {
		return linkTo;
	}
}
