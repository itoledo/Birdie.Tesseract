using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tvn.cosine.ai.util.datastructure;

namespace tvn.cosine.ai.environment.tictactoe
{
    /**
     * A state of the Tic-tac-toe game is characterized by a board containing
     * symbols X and O, the next player to move, and an utility information.
     * 
     * @author Ruediger Lunde
     * 
     */
    public class TicTacToeState
    {

        public const string O = "O";
        public const string X = "X";
        public const string EMPTY = "-";
        //
        private string[] board = new string[] { EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY };

        private string playerToMove = X;
        private double utility = -1; // 1: win for X, 0: win for O, 0.5: draw

        public string getPlayerToMove()
        {
            return playerToMove;
        }

        public bool isEmpty(int col, int row)
        {
            return board[getAbsPosition(col, row)].Equals(EMPTY);
        }

        public string getValue(int col, int row)
        {
            return board[getAbsPosition(col, row)];
        }

        public double getUtility()
        {
            return utility;
        }

        public void mark(XYLocation action)
        {
            mark(action.X, action.Y);
        }

        public void mark(int col, int row)
        {
            if (utility == -1 && getValue(col, row).Equals(EMPTY))
            {
                board[getAbsPosition(col, row)] = playerToMove;
                analyzeUtility();
                playerToMove = (playerToMove.Equals(X) ? O : X);
            }
        }

        private void analyzeUtility()
        {
            if (lineThroughBoard())
            {
                utility = (playerToMove.Equals(X) ? 1 : 0);
            }
            else if (getNumberOfMarkedPositions() == 9)
            {
                utility = 0.5;
            }
        }

        public bool lineThroughBoard()
        {
            return (isAnyRowComplete() || isAnyColumnComplete() || isAnyDiagonalComplete());
        }

        private bool isAnyRowComplete()
        {
            for (int row = 0; row < 3; row++)
            {
                string val = getValue(0, row);
                if (!val.Equals(EMPTY)
                  && val.Equals(getValue(1, row))
                  && val.Equals(getValue(2, row)))
                {
                    return true;
                }
            }
            return false;
        }

        private bool isAnyColumnComplete()
        {
            for (int col = 0; col < 3; col++)
            {
                string val = getValue(col, 0);
                if (!val.Equals(EMPTY)
                  && val.Equals(getValue(col, 1))
                  && val.Equals(getValue(col, 2)))
                {
                    return true;
                }
            }
            return false;
        }

        private bool isAnyDiagonalComplete()
        {
            string val = getValue(0, 0);
            if (!val.Equals(EMPTY)
             && val.Equals(getValue(1, 1))
             && val.Equals(getValue(2, 2)))
            {
                return true;
            }
            val = getValue(0, 2);
            if (!val.Equals(EMPTY)
              && val.Equals(getValue(1, 1))
              && val.Equals(getValue(2, 0)))
            {
                return true;
            }
            return false;
        }

        public int getNumberOfMarkedPositions()
        {
            int retVal = 0;
            for (int col = 0; col < 3; col++)
            {
                for (int row = 0; row < 3; row++)
                {
                    if (!(isEmpty(col, row)))
                    {
                        retVal++;
                    }
                }
            }
            return retVal;
        }

        public IList<XYLocation> getUnMarkedPositions()
        {
            IList<XYLocation> result = new List<XYLocation>();
            for (int col = 0; col < 3; col++)
            {
                for (int row = 0; row < 3; row++)
                {
                    if (isEmpty(col, row))
                    {
                        result.Add(new XYLocation(col, row));
                    }
                }
            }
            return result;
        }

        public TicTacToeState Clone()
        {
            TicTacToeState copy = null;

            copy = (TicTacToeState)base.MemberwiseClone();
            System.Array.Copy(board, copy.board, board.Length);

            return copy;
        }

        public override bool Equals(object anObj)
        {
            if (anObj != null && anObj.GetType() == GetType())
            {
                TicTacToeState anotherState = (TicTacToeState)anObj;
                for (int i = 0; i < 9; ++i)
                {
                    if (!board[i].Equals(anotherState.board[i]))
                        return false;
                }
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            // Need to ensure equal objects have equivalent hashcodes (Issue 77).
            return ToString().GetHashCode();
        }
         
    public override string ToString()
        {
            StringBuilder buffer = new StringBuilder();
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    buffer.Append(getValue(col, row)).Append(" ");
                }
                buffer.Append("\n");
            }
            return buffer.ToString();
        }

        //
        // PRIVATE METHODS
        //

        private int getAbsPosition(int col, int row)
        {
            return row * 3 + col;
        }
    }

}
