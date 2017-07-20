using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.common.exceptions;
using tvn.cosine.ai.util.math;

namespace tvn_cosine.ai.test.unit.util.math
{
    [TestClass]
    public class MixedRadixNumberTest
    { 
        [TestMethod]
        [ExpectedException(typeof(IllegalArgumentException))]
        public void testInvalidRadices()
        {
            new MixedRadixNumber(100, new int[] { 1, 0, -1 });
        }


        [TestMethod]
        [ExpectedException(typeof(IllegalArgumentException))]
        public void testInvalidMaxValue()
        {
            new MixedRadixNumber(100, new int[] { 3, 3, 3 });
        }


        [TestMethod]
        [ExpectedException(typeof(IllegalArgumentException))]
        public void testInvalidInitialValuesValue1()
        {
            new MixedRadixNumber(new int[] { 0, 0, 4 }, new int[] { 3, 3, 3 });
        }


        [TestMethod]
        [ExpectedException(typeof(IllegalArgumentException))]
        public void testInvalidInitialValuesValue2()
        {
            new MixedRadixNumber(new int[] { 1, 2, -1 }, new int[] { 3, 3, 3 });
        }


        [TestMethod]
        [ExpectedException(typeof(IllegalArgumentException))]
        public void testInvalidInitialValuesValue3()
        {
            new MixedRadixNumber(new int[] { 1, 2, 3, 1 }, new int[] { 3, 3, 3 });
        }

        [TestMethod]
        public void testAllowedMaxValue()
        {
            Assert.AreEqual(15, (new MixedRadixNumber(0,
                    new int[] { 2, 2, 2, 2 }).getMaxAllowedValue()));
            Assert.AreEqual(80, (new MixedRadixNumber(0,
                    new int[] { 3, 3, 3, 3 }).getMaxAllowedValue()));
            Assert.AreEqual(5, (new MixedRadixNumber(0, new int[] { 3, 2 })
                    .getMaxAllowedValue()));
            Assert.AreEqual(35, (new MixedRadixNumber(0,
                    new int[] { 3, 3, 2, 2 }).getMaxAllowedValue()));
            Assert.AreEqual(359, (new MixedRadixNumber(0, new int[] { 3, 4, 5,
                6 }).getMaxAllowedValue()));
            Assert.AreEqual(359, (new MixedRadixNumber(0, new int[] { 6, 5, 4,
                3 }).getMaxAllowedValue()));
            Assert.AreEqual(359,
                    (new MixedRadixNumber(new int[] { 5, 4, 3, 2 }, new int[] { 6,
                        5, 4, 3 }).getMaxAllowedValue()));
        }

        [TestMethod]
        public void testIncrement()
        {
            MixedRadixNumber mrn = new MixedRadixNumber(0, new int[] { 3, 2 });
            int i = 0;
            while (mrn.increment())
            {
                i++;
            }
            Assert.AreEqual(i, mrn.getMaxAllowedValue());
        }

        [TestMethod]
        public void testDecrement()
        {
            MixedRadixNumber mrn = new MixedRadixNumber(5, new int[] { 3, 2 });
            int i = 0;
            while (mrn.decrement())
            {
                i++;
            }
            Assert.AreEqual(i, mrn.getMaxAllowedValue());
            i = 0;
            while (mrn.increment())
            {
                i++;
            }
            while (mrn.decrement())
            {
                i--;
            }
            Assert.AreEqual(i, mrn.intValue());
        }

        [TestMethod]
        public void testCurrentNumberalValue()
        {
            MixedRadixNumber mrn;
            //
            mrn = new MixedRadixNumber(0, new int[] { 3, 3, 2, 2 });
            Assert.AreEqual(0, mrn.getCurrentNumeralValue(0));
            Assert.AreEqual(0, mrn.getCurrentNumeralValue(1));
            Assert.AreEqual(0, mrn.getCurrentNumeralValue(2));
            Assert.AreEqual(0, mrn.getCurrentNumeralValue(3));
            //
            mrn = new MixedRadixNumber(35, new int[] { 3, 3, 2, 2 });
            Assert.AreEqual(2, mrn.getCurrentNumeralValue(0));
            Assert.AreEqual(2, mrn.getCurrentNumeralValue(1));
            Assert.AreEqual(1, mrn.getCurrentNumeralValue(2));
            Assert.AreEqual(1, mrn.getCurrentNumeralValue(3));
            //
            mrn = new MixedRadixNumber(25, new int[] { 3, 3, 2, 2 });
            Assert.AreEqual(1, mrn.getCurrentNumeralValue(0));
            Assert.AreEqual(2, mrn.getCurrentNumeralValue(1));
            Assert.AreEqual(0, mrn.getCurrentNumeralValue(2));
            Assert.AreEqual(1, mrn.getCurrentNumeralValue(3));
            //
            mrn = new MixedRadixNumber(17, new int[] { 3, 3, 2, 2 });
            Assert.AreEqual(2, mrn.getCurrentNumeralValue(0));
            Assert.AreEqual(2, mrn.getCurrentNumeralValue(1));
            Assert.AreEqual(1, mrn.getCurrentNumeralValue(2));
            Assert.AreEqual(0, mrn.getCurrentNumeralValue(3));
            //
            mrn = new MixedRadixNumber(8, new int[] { 3, 3, 2, 2 });
            Assert.AreEqual(2, mrn.getCurrentNumeralValue(0));
            Assert.AreEqual(2, mrn.getCurrentNumeralValue(1));
            Assert.AreEqual(0, mrn.getCurrentNumeralValue(2));
            Assert.AreEqual(0, mrn.getCurrentNumeralValue(3));
            //
            mrn = new MixedRadixNumber(359, new int[] { 3, 4, 5, 6 });
            Assert.AreEqual(2, mrn.getCurrentNumeralValue(0));
            Assert.AreEqual(3, mrn.getCurrentNumeralValue(1));
            Assert.AreEqual(4, mrn.getCurrentNumeralValue(2));
            Assert.AreEqual(5, mrn.getCurrentNumeralValue(3));
            //
            mrn = new MixedRadixNumber(359, new int[] { 6, 5, 4, 3 });
            Assert.AreEqual(5, mrn.getCurrentNumeralValue(0));
            Assert.AreEqual(4, mrn.getCurrentNumeralValue(1));
            Assert.AreEqual(3, mrn.getCurrentNumeralValue(2));
            Assert.AreEqual(2, mrn.getCurrentNumeralValue(3));
        }

        [TestMethod]
        public void testCurrentValueFor()
        {
            MixedRadixNumber mrn;
            //
            mrn = new MixedRadixNumber(0, new int[] { 3, 3, 2, 2 });
            Assert.AreEqual(0, mrn.getCurrentValueFor(new int[] { 0, 0, 0, 0 }));
            //
            mrn = new MixedRadixNumber(35, new int[] { 3, 3, 2, 2 });
            Assert.AreEqual(35,
                    mrn.getCurrentValueFor(new int[] { 2, 2, 1, 1 }));
            //
            mrn = new MixedRadixNumber(25, new int[] { 3, 3, 2, 2 });
            Assert.AreEqual(25,
                    mrn.getCurrentValueFor(new int[] { 1, 2, 0, 1 }));
            //
            mrn = new MixedRadixNumber(17, new int[] { 3, 3, 2, 2 });
            Assert.AreEqual(17,
                    mrn.getCurrentValueFor(new int[] { 2, 2, 1, 0 }));
            //
            mrn = new MixedRadixNumber(8, new int[] { 3, 3, 2, 2 });
            Assert.AreEqual(8, mrn.getCurrentValueFor(new int[] { 2, 2, 0, 0 }));
            //
            mrn = new MixedRadixNumber(359, new int[] { 3, 4, 5, 6 });
            Assert.AreEqual(359,
                    mrn.getCurrentValueFor(new int[] { 2, 3, 4, 5 }));
            //
            mrn = new MixedRadixNumber(359, new int[] { 6, 5, 4, 3 });
            Assert.AreEqual(359,
                    mrn.getCurrentValueFor(new int[] { 5, 4, 3, 2 }));
        }

        [TestMethod]
        public void testSetCurrentValueFor()
        {
            MixedRadixNumber mrn;
            //
            mrn = new MixedRadixNumber(0, new int[] { 3, 3, 2, 2 });
            mrn.setCurrentValueFor(new int[] { 0, 0, 0, 0 });
            Assert.AreEqual(0, mrn.intValue());
            Assert.AreEqual(0, mrn.getCurrentNumeralValue(0));
            Assert.AreEqual(0, mrn.getCurrentNumeralValue(1));
            Assert.AreEqual(0, mrn.getCurrentNumeralValue(2));
            Assert.AreEqual(0, mrn.getCurrentNumeralValue(3));
            //
            mrn = new MixedRadixNumber(0, new int[] { 3, 3, 2, 2 });
            mrn.setCurrentValueFor(new int[] { 2, 2, 1, 1 });
            Assert.AreEqual(35, mrn.intValue());
            Assert.AreEqual(2, mrn.getCurrentNumeralValue(0));
            Assert.AreEqual(2, mrn.getCurrentNumeralValue(1));
            Assert.AreEqual(1, mrn.getCurrentNumeralValue(2));
            Assert.AreEqual(1, mrn.getCurrentNumeralValue(3));
            //
            mrn = new MixedRadixNumber(0, new int[] { 3, 3, 2, 2 });
            mrn.setCurrentValueFor(new int[] { 1, 2, 0, 1 });
            Assert.AreEqual(25, mrn.intValue());
            Assert.AreEqual(1, mrn.getCurrentNumeralValue(0));
            Assert.AreEqual(2, mrn.getCurrentNumeralValue(1));
            Assert.AreEqual(0, mrn.getCurrentNumeralValue(2));
            Assert.AreEqual(1, mrn.getCurrentNumeralValue(3));
            //
            mrn = new MixedRadixNumber(0, new int[] { 3, 3, 2, 2 });
            mrn.setCurrentValueFor(new int[] { 2, 2, 1, 0 });
            Assert.AreEqual(17, mrn.intValue());
            Assert.AreEqual(2, mrn.getCurrentNumeralValue(0));
            Assert.AreEqual(2, mrn.getCurrentNumeralValue(1));
            Assert.AreEqual(1, mrn.getCurrentNumeralValue(2));
            Assert.AreEqual(0, mrn.getCurrentNumeralValue(3));
            //
            mrn = new MixedRadixNumber(0, new int[] { 3, 3, 2, 2 });
            mrn.setCurrentValueFor(new int[] { 2, 2, 0, 0 });
            Assert.AreEqual(8, mrn.intValue());
            Assert.AreEqual(2, mrn.getCurrentNumeralValue(0));
            Assert.AreEqual(2, mrn.getCurrentNumeralValue(1));
            Assert.AreEqual(0, mrn.getCurrentNumeralValue(2));
            Assert.AreEqual(0, mrn.getCurrentNumeralValue(3));
            //
            mrn = new MixedRadixNumber(0, new int[] { 3, 4, 5, 6 });
            mrn.setCurrentValueFor(new int[] { 2, 3, 4, 5 });
            Assert.AreEqual(359, mrn.intValue());
            Assert.AreEqual(2, mrn.getCurrentNumeralValue(0));
            Assert.AreEqual(3, mrn.getCurrentNumeralValue(1));
            Assert.AreEqual(4, mrn.getCurrentNumeralValue(2));
            Assert.AreEqual(5, mrn.getCurrentNumeralValue(3));
            //
            mrn = new MixedRadixNumber(0, new int[] { 6, 5, 4, 3 });
            mrn.setCurrentValueFor(new int[] { 5, 4, 3, 2 });
            Assert.AreEqual(359, mrn.intValue());
            Assert.AreEqual(5, mrn.getCurrentNumeralValue(0));
            Assert.AreEqual(4, mrn.getCurrentNumeralValue(1));
            Assert.AreEqual(3, mrn.getCurrentNumeralValue(2));
            Assert.AreEqual(2, mrn.getCurrentNumeralValue(3));
        }
    }

}
