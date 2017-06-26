 namespace aima.core.environment.tictactoe;

 
import java.util.Arrays;
 
import java.util.Objects;

import aima.core.util.datastructure.XYLocation;

/**
 * A state of the Tic-tac-toe game is characterized by a board containing
 * symbols X and O, the next player to move, and an utility information.
 * 
 * @author Ruediger Lunde
 * 
 */
public class TicTacToeState : Cloneable {
	public static readonly string O = "O";
	public static readonly string X = "X";
	public static readonly string EMPTY = "-";
	//
	private string[] board = new string[] { EMPTY, EMPTY, EMPTY, EMPTY, EMPTY,
			EMPTY, EMPTY, EMPTY, EMPTY };

	private string playerToMove = X;
	private double utility = -1; // 1: win for X, 0: win for O, 0.5: draw

	public string getPlayerToMove() {
		return playerToMove;
	}
	
	public bool isEmpty(int col, int row) {
		return Objects .Equals(board[getAbsPosition(col, row)], EMPTY);
	}

	public string getValue(int col, int row) {
		return board[getAbsPosition(col, row)];
	}

	public double getUtility() {
		return utility;
	}

	public void mark(XYLocation action) {
		mark(action.getXCoOrdinate(), action.getYCoOrdinate());
	}

	public void mark(int col, int row) {
		if (utility == -1 && Objects .Equals(getValue(col, row), EMPTY)) {
			board[getAbsPosition(col, row)] = playerToMove;
			analyzeUtility();
			playerToMove = (objects .Equals(playerToMove, X) ? O : X);
		}
	}

	private void analyzeUtility() {
		if (lineThroughBoard()) {
			utility = (objects .Equals(playerToMove, X) ? 1 : 0);
		} else if (getNumberOfMarkedPositions() == 9) {
			utility = 0.5;
		}
	}

	public bool lineThroughBoard() {
		return (isAnyRowComplete() || isAnyColumnComplete() || isAnyDiagonalComplete());
	}
	
	private bool isAnyRowComplete() {
		for (int row = 0; row < 3; row++) {
			String val = getValue(0, row);
			if (!Objects .Equals(val, EMPTY) && Objects .Equals(val, getValue(1, row)) && Objects .Equals(val, getValue(2, row))) {
				return true;
			}
		}
		return false;
	}

	private bool isAnyColumnComplete() {
		for (int col = 0; col < 3; col++) {
			String val = getValue(col, 0);
			if (!Objects .Equals(val, EMPTY) && Objects .Equals(val, getValue(col, 1)) && Objects .Equals(val, getValue(col, 2))) {
				return true;
			}
		}
		return false;
	}

	private bool isAnyDiagonalComplete() {
		String val = getValue(0, 0);
		if (!Objects .Equals(val, EMPTY) && Objects .Equals(val, getValue(1, 1)) && Objects .Equals(val, getValue(2, 2))) {
			return true;
		}
		val = getValue(0, 2);
		if (!Objects .Equals(val, EMPTY) && Objects .Equals(val, getValue(1, 1)) && Objects .Equals(val, getValue(2, 0))) {
			return true;
		}
		return false;
	}

	public int getNumberOfMarkedPositions() {
		int retVal = 0;
		for (int col = 0; col < 3; col++) {
			for (int row = 0; row < 3; row++) {
				if (!(isEmpty(col, row))) {
					retVal++;
				}
			}
		}
		return retVal;
	}

	public List<XYLocation> getUnMarkedPositions() {
		List<XYLocation> result = new List<>();
		for (int col = 0; col < 3; col++) {
			for (int row = 0; row < 3; row++) {
				if (isEmpty(col, row)) {
					result.Add(new XYLocation(col, row));
				}
			}
		}
		return result;
	}

	 
	public TicTacToeState clone() {
		TicTacToeState copy = null;
		try {
			copy = (TicTacToeState) base.clone();
			copy.board = Arrays.copyOf(board, board.Length);
		} catch (CloneNotSupportedException e) {
			e.printStackTrace(); // should never happen...
		}
		return copy;
	}

	 
	public override bool Equals(object anObj) {
		if (anObj != null && anObj .GetType() == getClass()) {
			TicTacToeState anotherState = (TicTacToeState) anObj;
			for (int i = 0; i < 9; ++i) {
				if (!Objects .Equals(board[i], anotherState.board[i]))
					return false;
			}
			return true;
		}
		return false;
	}
	
	 
	public override int GetHashCode() {
		// Need to ensure equal objects have equivalent hashcodes (Issue 77).
		return toString() .GetHashCode();
	}

	 
	public override string ToString() {
		StringBuilder buffer = new StringBuilder();
		for (int row = 0; row < 3; row++) {
			for (int col = 0; col < 3; col++) {
				buffer.Append(getValue(col, row)).Append(" ");
			}
			buffer.Append("\n");
		}
		return buffer.ToString();
	}

	//
	// PRIVATE METHODS
	//

	private int getAbsPosition(int col, int row) {
		return row * 3 + col;
	}
}
