using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tvn.cosine.ai.util.datastructure
{
    public class Table<RowHeaderType, ColumnHeaderType, ValueType>
    {
        private IList<RowHeaderType> rowHeaders;
        private IList<ColumnHeaderType> columnHeaders;
        private IDictionary<RowHeaderType, IDictionary<ColumnHeaderType, ValueType>> rows;

        /// <summary>
        /// Constructs a Table with the specified row and column headers.
        /// </summary>
        /// <param name="rowHeaders">a list of row headers</param>
        /// <param name="columnHeaders">a list of column headers</param>
        public Table(IList<RowHeaderType> rowHeaders,
                     IList<ColumnHeaderType> columnHeaders)
        {

            this.rowHeaders = rowHeaders;
            this.columnHeaders = columnHeaders;
            this.rows = new Dictionary<RowHeaderType, IDictionary<ColumnHeaderType, ValueType>>();
            foreach (RowHeaderType rowHeader in rowHeaders)
            {
                rows[rowHeader] = new Dictionary<ColumnHeaderType, ValueType>();
            }
        }

        /// <summary>
        /// Maps the specified row and column to the specified value in the table.
        /// Neither the row nor the column nor the value can be <see cref="null"/> 
        /// The value can be retrieved by calling th get method with a
        /// row and column that is equal to the original row and column.
        /// </summary>
        /// <param name="r">the table row</param>
        /// <param name="c">the table column</param>
        /// <param name="v">the value</param>
        public void Set(RowHeaderType r, ColumnHeaderType c, ValueType v)
        {
            rows[r].Add(c, v);
        }

        /// <summary>
        /// Returns the value to which the specified row and column is mapped in this table.
        /// </summary>
        /// <param name="r">a row in the table</param>
        /// <param name="c">a column in the table</param>
        /// <returns>the value to which the row and column is mapped in this table; null if the row and column is not mapped to any values in this table.</returns>
        public ValueType Get(RowHeaderType r, ColumnHeaderType c)
        {
            IDictionary<ColumnHeaderType, ValueType> rowValues = rows[r];
            return rowValues == null ? default(ValueType) : rowValues[c];

        }

        class Row<R>
        {
            private IDictionary<ColumnHeaderType, ValueType> cells;

            public Row()
            {

                this.cells = new Dictionary<ColumnHeaderType, ValueType>();
            }

            public IDictionary<ColumnHeaderType, ValueType> Cells()
            {
                return this.cells;
            }

        }

        class Cell<ValueHeaderType>
        {
            private ValueHeaderType value;

            public Cell()
            {
                value = default(ValueHeaderType);
            }

            public Cell(ValueHeaderType value)
            {
                this.value = value;
            }

            public void set(ValueHeaderType value)
            {
                this.value = value;
            }

            public ValueHeaderType Value()
            {
                return value;
            }

        }
    }
}
