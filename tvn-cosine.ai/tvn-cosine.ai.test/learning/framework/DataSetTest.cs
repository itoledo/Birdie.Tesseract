using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.learning.framework;
using tvn.cosine.ai.learning.neural;
using System.Collections.Generic;
using System.IO;
using tvn.cosine.ai.util.datastructure;

namespace tvn_cosine.ai.test.learning.framework
{
    [TestClass]
    public class DataSetTest
    {
        private const string YES = "Yes";
         
        [TestMethod]
        public void testNormalizationOfFileBasedDataProducesCorrectMeanStdDevAndNormalizedValues()
        {
            RabbitEyeDataSet reds = new RabbitEyeDataSet();
            reds.createNormalizedDataFromFile("rabbiteyes");

            IList<double> means = reds.getMeans();
            Assert.AreEqual(2, means.Count);
            Assert.AreEqual(244.771, means[0], 0.001);
            Assert.AreEqual(145.505, means[1], 0.001);

            IList<double> stdev = reds.getStdevs();
            Assert.AreEqual(2, stdev.Count);
            Assert.AreEqual(213.554, stdev[0], 0.001);
            Assert.AreEqual(65.776, stdev[1], 0.001);

            IList<IList<double>> normalized = reds.getNormalizedData();
            Assert.AreEqual(70, normalized.Count);

            // check first value
            Assert.AreEqual(-1.0759, normalized[0][0], 0.001);
            Assert.AreEqual(-1.882, normalized[0][1], 0.001);

            // check last Value
            Assert.AreEqual(2.880, normalized[69][0], 0.001);
            Assert.AreEqual(1.538, normalized[69][1], 0.001);
        }

        [TestMethod]
        public void testExampleFormation()
        {
            RabbitEyeDataSet reds = new RabbitEyeDataSet();
            reds.createExamplesFromFile("rabbiteyes");
            Assert.AreEqual(70, reds.howManyExamplesLeft());
            reds.getExampleAtRandom();
            Assert.AreEqual(69, reds.howManyExamplesLeft());
            reds.getExampleAtRandom();
            Assert.AreEqual(68, reds.howManyExamplesLeft());
        }

        [TestMethod]
        public void testLoadsDatasetFile()
        {
            DataSet ds = DataSetFactory.getRestaurantDataSet();
            Assert.AreEqual(12, ds.Count);

            Example first = ds.getExample(0);
            Assert.AreEqual(YES, first.getAttributeValueAsString("alternate"));
            Assert.AreEqual("$$$", first.getAttributeValueAsString("price"));
            Assert.AreEqual("0-10", first.getAttributeValueAsString("wait_estimate"));
            Assert.AreEqual(YES, first.getAttributeValueAsString("will_wait"));
            Assert.AreEqual(YES, first.targetValue());
        }


        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void testThrowsExceptionForNonExistentFile()
        {
            DataSetFactory.FromFile("nonexistent", null, null);
        }

        [TestMethod]
        public void testLoadsIrisDataSetWithNumericAndStringAttributes()
        {
            DataSet ds = DataSetFactory.getIrisDataSet();
            Example first = ds.getExample(0);
            Assert.AreEqual("5.1", first.getAttributeValueAsString("sepal_length"));
        }

        [TestMethod]
        public void testNonDestructiveRemoveExample()
        {
            DataSet ds1 = DataSetFactory.getRestaurantDataSet();
            DataSet ds2 = ds1.removeExample(ds1.getExample(0));
            Assert.AreEqual(12, ds1.Count);
            Assert.AreEqual(11, ds2.Count);
        }

        [TestMethod]
        public void testNumerizesAndDeNumerizesIrisDataSetExample1()


        {
            DataSet ds = DataSetFactory.getIrisDataSet();
            Example first = ds.getExample(0);
            Numerizer n = new IrisDataSetNumerizer();
            Pair<IList<double>, IList<double>> io = n.numerize(first);

            Assert.AreEqual(new List<double>(new[] { 5.1, 3.5, 1.4, 0.2 }), io.First);
            Assert.AreEqual(new List<double>(new[] { 0.0, 0.0, 1.0 }), io.Second);

            String plant_category = n.denumerize(new List<double>(new[] { 0.0, 0.0, 1.0 }));
            Assert.AreEqual("setosa", plant_category);
        }

        [TestMethod]
        public void testNumerizesAndDeNumerizesIrisDataSetExample2() 
        {
            DataSet ds = DataSetFactory.getIrisDataSet();
            Example first = ds.getExample(51);
            Numerizer n = new IrisDataSetNumerizer();
            Pair<IList<double>, IList<double>> io = n.numerize(first);

            Assert.AreEqual(new List<double>(new[] { 6.4, 3.2, 4.5, 1.5 }), io.First);
            Assert.AreEqual(new List<double>(new[] { 0.0, 1.0, 0.0 }), io.Second);

            String plant_category = n.denumerize(new List<double>(new[] { 0.0, 1.0, 0.0 }));
            Assert.AreEqual("versicolor", plant_category);
        }

        [TestMethod]
        public void testNumerizesAndDeNumerizesIrisDataSetExample3() 
        {
            DataSet ds = DataSetFactory.getIrisDataSet();
            Example first = ds.getExample(100);
            Numerizer n = new IrisDataSetNumerizer();
            Pair<IList<double>, IList<double>> io = n.numerize(first);

            Assert.AreEqual(new List<double>(new[] { 6.3, 3.3, 6.0, 2.5 }), io.First);
            Assert.AreEqual(new List<double>(new[] { 1.0, 0.0, 0.0 }), io.Second);

            String plant_category = n.denumerize(new List<double>(new[] { 1.0, 0.0, 0.0 }));
            Assert.AreEqual("virginica", plant_category);
        }
    }
}
