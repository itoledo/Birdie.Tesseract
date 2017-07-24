using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.datastructures;

namespace tvn_cosine.ai.test.unit.util.datastructure
{
    [TestClass]
    public class TableTest
    {
        private Table<string, string, int?> table;

        [TestInitialize]
        public void setUp()
        {
            IQueue<string> rowHeaders = Factory.CreateQueue<string>();
            IQueue<string> columnHeaders = Factory.CreateQueue<string>();

            rowHeaders.Add("row1");
            rowHeaders.Add("ravi");
            rowHeaders.Add("peter");

            columnHeaders.Add("col1");
            columnHeaders.Add("iq");
            columnHeaders.Add("age");
            table = new Table<string, string, int?>(rowHeaders, columnHeaders);

        }

        [TestMethod]
        public void testTableInitialization()
        {
            Assert.IsNull(table.get("ravi", "iq"));
            table.set("ravi", "iq", 50);
            int? i = table.get("ravi", "iq");
            Assert.AreEqual(50, i);
        }

        [TestMethod]
        public void testNullAccess()
        {
            // No value yet assigned
            Assert.IsNull(table.get("row1", "col2"));
            table.set("row1", "col1", 1);
            Assert.AreEqual(1, (int)table.get("row1", "col1"));
            // Check null returned if column does not exist
            Assert.IsNull(table.get("row1", "col2"));
            // Check null returned if row does not exist
            Assert.IsNull(table.get("row2", "col1"));
        } 
    } 
}
