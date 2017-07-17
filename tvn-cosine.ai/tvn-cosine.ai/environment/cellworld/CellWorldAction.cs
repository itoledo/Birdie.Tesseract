using tvn.cosine.ai.agent;
using tvn.cosine.ai.common.collections;

namespace tvn.cosine.ai.environment.cellworld
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 645.<br>
     * <br>
     * 
     * The actions in every state are Up, Down, Left, and Right.<br>
     * <br>
     * <b>Note:<b> Moving 'North' causes y to increase by 1, 'Down' y to decrease by
     * 1, 'Left' x to decrease by 1, and 'Right' x to increase by 1 within a Cell
     * World.
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public class CellWorldAction : Action
    {
        private static readonly CellWorldAction Up = new CellWorldAction();
        private static readonly CellWorldAction Down = new CellWorldAction();
        private static readonly CellWorldAction Left = new CellWorldAction();
        private static readonly CellWorldAction Right = new CellWorldAction();
        private static readonly CellWorldAction None = new CellWorldAction();

        private static readonly ISet<CellWorldAction> _actions;

        static CellWorldAction()
        {
            _actions = Factory.CreateSet<CellWorldAction>();
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
            return Factory.CreateReadOnlySet<CellWorldAction>(_actions);
        }

        public bool isNoOp()
        {
            if (None == this)
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

            if (Left == this)
            {
                newX--;
            }
            else if (Right == this)
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

            if (Up == this)
            {
                newY++;
            }
            else if (Down == this)
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

            if (Up == this
             || Down == this)
            {
                a = Left;
            }
            else if (Left == this
                 || Right == this)
            {
                a = Down;
            }
            else if (None == this)
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

            if (Up == this
             || Down == this)
            {
                a = Right;
            }
            else if (Left == this
                 || Right == this)
            {
                a = Up;
            }
            else if (None == this)
            {
                a = None;
            }

            return a;
        }
    }
}
