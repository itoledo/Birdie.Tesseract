 namespace aima.core.learning.neural;

 

/**
 * A holder for config data for neural networks and possibly for other learning
 * systems.
 * 
 * @author Ravi Mohan
 * 
 */
public class NNConfig {
	private readonly Dictionary<String, Object> hash;

	public NNConfig(Dictionary<String, Object> hash) {
		this.hash = hash;
	}

	public NNConfig() {
		this.hash = new Dictionary<String, Object>();
	}

	public double getParameterAsDouble(string key) {

		return (Double) hash.get(key);
	}

	public int getParameterAsInteger(string key) {

		return (Integer) hash.get(key);
	}

	public void setConfig(string key, double value) {
		hash.Add(key, value);
	}

	public void setConfig(string key, int value) {
		hash.Add(key, value);
	}
}
