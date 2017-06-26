 namespace aima.core.learning.reinforcement.example;

import aima.core.environment.cellworld.Cell;
import aima.core.learning.reinforcement.PerceptStateReward;

/**
 * An implementation of the PerceptStateReward interface for the cell world
 * environment. Note: The getCell() and setCell() methods allow a single percept
 * to be instantiated per agent within the environment. However, if an agent
 * tracks its perceived percepts it will need to explicitly copy the relevant
 * information.
 * 
 * @author oreilly
 * 
 */
public class CellWorldPercept : PerceptStateReward<Cell<double>> {
	private Cell<double> cell = null;

	/**
	 * Constructor.
	 * 
	 * @param cell
	 *            the cell within the environment that the percept refers to.
	 */
	public CellWorldPercept(Cell<double> cell) {
		this.cell = cell;
	}

	/**
	 * 
	 * @return the cell within the environment that the percept refers to.
	 */
	public Cell<double> getCell() {
		return cell;
	}

	/**
	 * Set the cell within the environment that the percept refers to.
	 * 
	 * @param cell
	 *            the cell within the environment that the percept refers to.
	 */
	public void setCell(Cell<double> cell) {
		this.cell = cell;
	}

	//
	// START-PerceptStateReward

	 
	public double reward() {
		return cell.getContent().doubleValue();
	}

	 
	public Cell<double> state() {
		return cell;
	}

	// END-PerceptStateReward
	//
}