
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tvn.cosine.ai.agent;

namespace tvn.cosine.ai.environment.cellworld
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 645. 
     *  
     * 
     * The actions in every state are Up, Down, Left, and Right. 
     *  
     * <b>Note:<b> Moving 'North' causes y to increase by 1, 'Down' y to decrease by
     * 1, 'Left' x to decrease by 1, and 'Right' x to increase by 1 within a Cell
     * World.
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public class CellWorldAction : IAction
    {
        private static readonly ISet<CellWorldAction> _actions = new HashSet<CellWorldAction>();
        public static readonly CellWorldAction Up = new CellWorldAction();
        public static readonly CellWorldAction Down = new CellWorldAction();
        public static readonly CellWorldAction Left = new CellWorldAction();
        public static readonly CellWorldAction Right = new CellWorldAction();
        public static readonly CellWorldAction None = new CellWorldAction();

        static CellWorldAction()
        {
            _actions.Add(Up);
            _actions.Add(Down);
            _actions.Add(Left);
            _actions.Add(Right);
            _actions.Add(None);
        }

        /**
         * 
         * @return a set of the actual actions.
         */
        public static ISet<CellWorldAction> actions()
        {
            return _actions;
        }

        //
        // START-Action 
        public bool IsNoOp()
        {
            if (None.Equals(this))
            {
                return true;
            }
            return false;
        }
        // END-Action
        //

        /**
         * 
         * @param curX
         *            the current x position.
         * @return the result on the x position of applying this action.
         */
        public int getXResult(int curX)
        {
            int newX = curX;

            if (this.Equals(Left))
            {
                newX--;
            }
            else if (this.Equals(Right))
            {
                newX++;
            }

            return newX;
        }

        /**
         * 
         * @param curY
         *            the current y position.
         * @return the result on the y position of applying this action.
         */
        public int getYResult(int curY)
        {
            int newY = curY;

            if (this.Equals(Up))
            {
                newY++;
            }
            else if (this.Equals(Down))
            {
                newY--;
            }

            return newY;
        }

        /**
         * 
         * @return the first right angled action related to this action.
         */
        public CellWorldAction getFirstRightAngledAction()
        {
            CellWorldAction a = null;

            if (this.Equals(Up)
             || this.Equals(Down))
            {
                a = Left;
            }
            else if (this.Equals(Left)
               || this.Equals(Right))
            {
                a = Down;
            }
            else if (this.Equals(None))
            {
                a = None;
            }

            return a;
        }

        /**
         * 
         * @return the second right angled action related to this action.
         */
        public CellWorldAction getSecondRightAngledAction()
        {
            CellWorldAction a = null;

            if (this.Equals(Up)
             || this.Equals(Down))
            {
                a = Right;
            }
            else if (this.Equals(Left)
                  || this.Equals(Right))
            {
                a = Up;
            }
            else if (this.Equals(None))
            {
                a = None;
            }

            return a;
        }
    }

}
