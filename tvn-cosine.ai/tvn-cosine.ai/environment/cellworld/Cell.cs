namespace tvn.cosine.ai.environment.cellworld
{
    /// <summary>
    /// Artificial Intelligence A Modern Approach (3rd Edition): page 645. <para /> 
    /// A representation of a Cell in the environment detailed in Figure 17.1.
    /// </summary>
    /// <typeparam name="C">the content type of the cell.</typeparam>
    public class Cell<C>
    {
        private readonly int x;
        private readonly int y;
        private C content;

        /// <summary>
        /// Construct a Cell.
        /// </summary>
        /// <param name="x">the x position of the cell.</param>
        /// <param name="y">the y position of the cell.</param>
        /// <param name="content">the initial content of the cell.</param>
        public Cell(int x, int y, C content)
        {
            this.x = x;
            this.y = y;
            this.content = content;
        }

        /// <summary>
        /// The x position of the cell.
        /// </summary>
        /// <returns>the x position of the cell.</returns>
        public int getX()
        {
            return x;
        }

        /// <summary>
        /// The y position of the cell.
        /// </summary>
        /// <returns>the y position of the cell.</returns>
        public int getY()
        {
            return y;
        }

        /// <summary>
        /// The content of the cell
        /// </summary>
        /// <returns>the content of the cell</returns>
        public C getContent()
        {
            return content;
        }

        /// <summary>
        /// Set the cell's content.
        /// </summary>
        /// <param name="content">the content to be placed in the cell.</param>
        public void setContent(C content)
        {
            this.content = content;
        }

        public override string ToString()
        {
            return string.Format("<x={0}, y={1}, content={2}", x, y, content);
        }

        public override bool Equals(object obj)
        {
            if (obj != null
             && GetType() == obj.GetType())
            {
                Cell<C> other = (Cell<C>)obj;
                return x.Equals(other.x)
                    && y.Equals(other.y)
                    && content.Equals(other.content);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return x + 23 + y + 31 * content.GetHashCode();
        }
    }
}
