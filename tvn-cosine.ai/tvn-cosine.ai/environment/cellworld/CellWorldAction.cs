using System.Collections.Generic;
using tvn.cosine.ai.agent;

namespace tvn.cosine.ai.environment.cellworld
{
    /// <summary> 
    /// Artificial Intelligence A Modern Approach (3rd Edition): page 645. <para />  
    /// The actions in every state are Up, Down, Left, and Right. <para /> 
    ///  
    /// Note: Moving 'North' causes y to increase by 1, 'Down' y to decrease by
    /// 1, 'Left' x to decrease by 1, and 'Right' x to increase by 1 within a Cell
    /// World.
    /// </summary>
    public class CellWorldAction : IAction
    {
        private static readonly ISet<CellWorldAction> _actions;
        public static readonly CellWorldAction Up;
        public static readonly CellWorldAction Down;
        public static readonly CellWorldAction Left;
        public static readonly CellWorldAction Right;
        public static readonly CellWorldAction None;

        static CellWorldAction()
        {
            _actions = new HashSet<CellWorldAction>();
            Up = new CellWorldAction();
            Down = new CellWorldAction();
            Left = new CellWorldAction();
            Right = new CellWorldAction();
            None = new CellWorldAction();

            _actions.Add(Up);
            _actions.Add(Down);
            _actions.Add(Left);
            _actions.Add(Right);
            _actions.Add(None);
        }

        /// <summary>
        /// A set of the actual actions.
        /// </summary>
        /// <returns>a set of the actual actions.</returns>
        public static ISet<CellWorldAction> Actions()
        {
            return _actions;
        }

        public bool IsNoOp()
        {
            if (None.Equals(this))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// The result on the x position of applying this action.
        /// </summary>
        /// <param name="currentX">the current x position.</param>
        /// <returns>the result on the x position of applying this action.</returns>
        public int GetXResult(int currentX)
        {
            int newX = currentX;

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

        /// <summary>
        /// The result on the y position of applying this action.
        /// </summary>
        /// <param name="currentY">the current y position.</param>
        /// <returns>the result on the y position of applying this action.</returns>
        public int GetYResult(int currentY)
        {
            int newY = currentY;

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


        /// <summary>
        /// The first right angled action related to this action.
        /// </summary>
        /// <returns>the first right angled action related to this action.</returns>
        public CellWorldAction GetFirstRightAngledAction()
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

        /// <summary>
        /// The second right angled action related to this action.
        /// </summary>
        /// <returns>the second right angled action related to this action.</returns>
        public CellWorldAction GetSecondRightAngledAction()
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
