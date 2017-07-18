namespace tvn_cosine.ai.test.unit.util.datastructure
{
    public class XYLocationTest
    {

        @Test
        public void testXYLocationAtributeSettingOnConstruction()
        {
            XYLocation loc = new XYLocation(3, 4);
            Assert.assertEquals(3, loc.getXCoOrdinate());
            Assert.assertEquals(4, loc.getYCoOrdinate());
        }

        @Test
        public void testEquality()
        {
            XYLocation loc1 = new XYLocation(3, 4);
            XYLocation loc2 = new XYLocation(3, 4);
            Assert.assertEquals(loc1, loc2);
        }
    }
}
