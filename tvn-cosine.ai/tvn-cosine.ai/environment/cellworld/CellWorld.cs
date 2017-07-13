using System.Collections.Generic;

namespace tvn.cosine.ai.environment.cellworld
{
    /// <summary>
    /// Artificial Intelligence A Modern Approach (3rd Edition): page 645. <para /> 
    ///   
    /// A representation for the environment depicted in figure 17.1. <para /> 
    /// Note: the x and y coordinates are always positive integers starting at 1. <para /> 
    /// Note: If looking at a rectangle - the coordinate (x=1, y=1) will be the
    /// bottom left hand corner. 
    /// </summary>
    /// <typeparam name="C">the type of content for the Cells in the world.</typeparam>
    public class CellWorld<C>
    {
        private readonly ISet<Cell<C>> cells;
        private readonly IDictionary<int, IDictionary<int, Cell<C>>> cellLookup;

        /// <summary>
        /// Construct a Cell World with size xDimension * y Dimension cells, all with
        /// their values set to a default content value.
        /// </summary>
        /// <param name="width">the size of the x dimension.</param>
        /// <param name="height">the size of the y dimension.</param>
        /// <param name="defaultCellContent">the default content to assign to each cell created.</param>
        public CellWorld(int width, int height, C defaultCellContent)
        {
            cells = new HashSet<Cell<C>>();
            cellLookup = new Dictionary<int, IDictionary<int, Cell<C>>>();

            for (int x = 1; x <= width; x++)
            {
                IDictionary<int, Cell<C>> xCol = new Dictionary<int, Cell<C>>();
                for (int y = 1; y <= height; y++)
                {
                    Cell<C> c = new Cell<C>(x, y, defaultCellContent);
                    cells.Add(c);
                    xCol.Add(y, c);
                }
                cellLookup.Add(x, xCol);
            }
        }

        /// <summary>
        /// All the cells in this world.
        /// </summary>
        /// <returns>all the cells in this world.</returns>
        public ISet<Cell<C>> GetCells()
        {
            return cells;
        }

        /// <summary>
        /// Determine what cell would be moved into if the specified action is
        /// performed in the specified cell. Normally, this will be the cell adjacent
        /// in the appropriate direction. However, if there is no cell in the
        /// adjacent direction of the action then the outcome of the action is to
        /// stay in the same cell as the action was performed in.
        /// </summary>
        /// <param name="cellLocation">the cell location from which the action is to be performed.</param>
        /// <param name="action">the action to perform (Up, Down, Left, or Right).</param>
        /// <returns>the Cell an agent would end up in if they performed the specified action from the specified cell location.</returns>
        public Cell<C> Result(Cell<C> cellLocation, CellWorldAction action)
        {
            Cell<C> sDelta = getCellAt(action.GetXResult(cellLocation.getX()), action.GetYResult(cellLocation.getY()));
            if (null == sDelta)
            {
                // Default to no effect
                // (i.e. bumps back in place as no adjoining cell).
                sDelta = cellLocation;
            }

            return sDelta;
        }

        /// <summary>
        /// Remove the cell at the specified location from this Cell World. This
        /// allows you to introduce barriers into different location.
        /// </summary>
        /// <param name="x">the x dimension of the cell to be removed.</param>
        /// <param name="y">the y dimension of the cell to be removed.</param>
        public void RemoveCell(int x, int y)
        {
            IDictionary<int, Cell<C>> xCol = cellLookup[x];
            if (null != xCol)
            {
                var v = xCol[y];
                xCol.Remove(y);
                cells.Remove(v);
            }
        }

        /// <summary>
        /// Get the cell at the specified x and y locations.
        /// </summary>
        /// <param name="x">the x dimension of the cell to be retrieved.</param>
        /// <param name="y">the y dimension of the cell to be retrieved.</param>
        /// <returns>the cell at the specified x,y location, null if no cell exists at this location.</returns>
        public Cell<C> getCellAt(int x, int y)
        {
            if (cellLookup.ContainsKey(x))
            {
                if (cellLookup[x].ContainsKey(y))
                {
                    return cellLookup[x][y];
                }
            }

            return null;
        }
    }
}
