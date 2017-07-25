using tvn.cosine.ai.common;
using tvn.cosine.ai.common.api;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.collections.api;
using tvn.cosine.ai.common.text;
using tvn.cosine.ai.common.text.api;

namespace tvn.cosine.ai.common.datastructures
{ 
    public class Table<RowHeaderType, ColumnHeaderType, ValueType> : IStringable
    {
        private ICollection<RowHeaderType> rowHeaders;
        private ICollection<ColumnHeaderType> columnHeaders;
        private IMap<RowHeaderType, IMap<ColumnHeaderType, ValueType>> rows;

        /**
         * Constructs a Table with the specified row and column headers.
         * 
         * @param rowHeaders
         *            a list of row headers
         * @param columnHeaders
         *            a list of column headers
         */
        public Table(ICollection<RowHeaderType> rowHeaders,
                     ICollection<ColumnHeaderType> columnHeaders)
        {
            this.rowHeaders = rowHeaders;
            this.columnHeaders = columnHeaders;
            this.rows = CollectionFactory.CreateInsertionOrderedMap<RowHeaderType, IMap<ColumnHeaderType, ValueType>>();
            foreach (RowHeaderType rowHeader in rowHeaders)
            {
                rows.Put(rowHeader, CollectionFactory.CreateInsertionOrderedMap<ColumnHeaderType, ValueType>());
            }
        }

        /**
         * Maps the specified row and column to the specified value in the table.
         * Neither the row nor the column nor the value can be <code>null</code> <br>
         * The value can be retrieved by calling the <code>get</code> method with a
         * row and column that is equal to the original row and column.
         * 
         * @param r
         *            the table row
         * @param c
         *            the table column
         * @param v
         *            the value
         * 
         * @throws NullPointerException
         *             if the row, column, or value is <code>null</code>.
         */
        public void set(RowHeaderType r, ColumnHeaderType c, ValueType v)
        { 
            rows.Get(r).Put(c, v);
        }

        /**
         * Returns the value to which the specified row and column is mapped in this
         * table.
         * 
         * @param r
         *            a row in the table
         * @param c
         *            a column in the table
         * 
         * @return the value to which the row and column is mapped in this table;
         *         <code>null</code> if the row and column is not mapped to any
         *         values in this table.
         * 
         * @throws NullPointerException
         *             if the row or column is <code>null</code>.
         */
        public ValueType get(RowHeaderType r, ColumnHeaderType c)
        {
            IMap<ColumnHeaderType, ValueType> rowValues = rows.Get(r);
            return rowValues == null ? default(ValueType) : rowValues.Get(c);

        }

        public override string ToString()
        {
            IStringBuilder buf = TextFactory.CreateStringBuilder();
            foreach (RowHeaderType r in rowHeaders)
            {
                foreach (ColumnHeaderType c in columnHeaders)
                {
                    buf.Append(get(r, c));
                    buf.Append(" ");
                }
                buf.Append("\n");
            }
            return buf.ToString();
        }

        class Row<R>
        {
            private IMap<ColumnHeaderType, ValueType> _cells;

            public Row()
            {

                this._cells = CollectionFactory.CreateInsertionOrderedMap<ColumnHeaderType, ValueType>();
            }

            public IMap<ColumnHeaderType, ValueType> cells()
            {
                return this._cells;
            }

        }

        class Cell<ValueHeaderType>
        {
            private ValueHeaderType _value;

            public Cell()
            {
                _value = default(ValueHeaderType);
            }

            public Cell(ValueHeaderType value)
            {
                this._value = value;
            }

            public void set(ValueHeaderType value)
            {
                this._value = value;
            }

            public ValueHeaderType value()
            {
                return _value;
            }
        }
    }
}
