namespace aima.core.search.basic.support;

using java.util.function.ToDoubleFunction;

/**
 * The Schedule for Simulated-Annealing.
 * 
 * @author Ravi Mohan
 * @author Anurag Rai
 */
public class BasicSchedule implements ToDoubleFunction<Integer> {

	private readonly int k, limit;
	private readonly double lam;

	// default constructor
	public BasicSchedule() {
		// base value
		this.k = 20;
		this.lam = 0.045;
		// time limit
		this.limit = 100;
	}

	public BasicSchedule(int k, double lam, int limit) {
		this.k = k;
		this.lam = lam;
		this.limit = limit;
	}

	/**
	 * Get a decreasing temperature value as the input parameter t increases.
	 * 
	 * @param t
	 *            the number of time steps that have gone by.
	 * @return the temperature at the given time step.
	 */
	 
	public double applyAsDouble(Integer t) {
		if (t < limit) {
			double res = k * Math.exp((-1) * lam * t);
			return res;
		} else {
			return 0.0;
		}
	}
}