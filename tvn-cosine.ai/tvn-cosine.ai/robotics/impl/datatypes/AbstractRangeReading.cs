 namespace aima.core.robotics.impl.datatypes;

import aima.core.robotics.datatypes.IMclRangeReading;

/**
 * This class : a single linear reading of a range.<br/>
 * In addition to the range it stores an {@link Angle} by which the heading was rotated for the measurement of the range.
 * 
 * @author Arno von Borries
 * @author Jan Phillip Kretzschmar
 * @author Andreas Walscheid
 *
 */
public abstract class AbstractRangeReading : IMclRangeReading<AbstractRangeReading,Angle> {
	
	private readonly double value;
	private readonly Angle angle;
	
	/**
	 * Constructor for a range reading with an angle assumed to be zero.
	 * @param value the actual range that was measured.
	 */
	public AbstractRangeReading(double value) {
		this.value = value;
		this.angle = Angle.ZERO_ANGLE;
	}
	
	/**
	 * @param value the actual range that was measured.
	 * @param angle the angle by which the heading was rotated for the measurement.
	 */
	public AbstractRangeReading(double value,Angle angle) {
		this.value = value;
		this.angle = angle;
	}

	/**
	 * Returns the range that was measured.
	 * @return the range that was measured.
	 */
	public double getValue() {
		return value;
	}
	
	 
	public Angle getAngle() {
		return angle;
	}
	
	 
	public override string ToString() {
		return String.format("%.2f", value) + "@" + String.format("%.2f", angle.getDegreeValue()) + "\u00BA";
	}
}
