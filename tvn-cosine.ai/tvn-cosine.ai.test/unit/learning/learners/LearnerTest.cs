﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.collections;
using tvn.cosine.collections.api;
using tvn.cosine.ai.learning.framework;
using tvn.cosine.ai.learning.inductive;
using tvn.cosine.ai.learning.learners;
using tvn_cosine.ai.test.unit.learning.framework;
using tvn_cosine.ai.test.unit.learning.inductive;

namespace tvn_cosine.ai.test.unit.learning.learners
{
    [TestClass]
    public class LearnerTest
    {

        [TestMethod]
        public void testMajorityLearner()
        {
            MajorityLearner learner = new MajorityLearner();
            DataSet ds = DataSetFactory.getRestaurantDataSet();
            learner.Train(ds);
            int[] result = learner.Test(ds);
            Assert.AreEqual(6, result[0]);
            Assert.AreEqual(6, result[1]);
        }

        [TestMethod]
        public void testDefaultUsedWhenTrainingDataSetHasNoExamples()


        {
            // tests RecursionBaseCase#1
            DataSet ds = DataSetFactory.getRestaurantDataSet();
            DecisionTreeLearner learner = new DecisionTreeLearner();

            DataSet ds2 = ds.emptyDataSet();
            Assert.AreEqual(0, ds2.size());

            learner.Train(ds2);
            Assert.AreEqual("Unable To Classify",
                    learner.Predict(ds.getExample(0)));
        }

        [TestMethod]
        public void testClassificationReturnedWhenAllExamplesHaveTheSameClassification()


        {
            // tests RecursionBaseCase#2
            DataSet ds = DataSetFactory.getRestaurantDataSet();
            DecisionTreeLearner learner = new DecisionTreeLearner();

            DataSet ds2 = ds.emptyDataSet();

            // all 3 examples have the same classification (willWait = yes)
            ds2.add(ds.getExample(0));
            ds2.add(ds.getExample(2));
            ds2.add(ds.getExample(3));

            learner.Train(ds2);
            Assert.AreEqual("Yes", learner.Predict(ds.getExample(0)));
        }

        [TestMethod]
        public void testMajorityReturnedWhenAttributesToExamineIsEmpty()


        {
            // tests RecursionBaseCase#2
            DataSet ds = DataSetFactory.getRestaurantDataSet();
            DecisionTreeLearner learner = new DecisionTreeLearner();

            DataSet ds2 = ds.emptyDataSet();

            // 3 examples have classification = "yes" and one ,"no"
            ds2.add(ds.getExample(0));
            ds2.add(ds.getExample(1));// "no"
            ds2.add(ds.getExample(2));
            ds2.add(ds.getExample(3));
            ds2.setSpecification(new MockDataSetSpecification("will_wait"));

            learner.Train(ds2);
            Assert.AreEqual("Yes", learner.Predict(ds.getExample(1)));
        }

        [TestMethod]
        public void testInducedTreeClassifiesDataSetCorrectly()
        {
            DataSet ds = DataSetFactory.getRestaurantDataSet();
            DecisionTreeLearner learner = new DecisionTreeLearner();
            learner.Train(ds);
            int[] result = learner.Test(ds);
            Assert.AreEqual(12, result[0]);
            Assert.AreEqual(0, result[1]);
        }

        [TestMethod]
        public void testDecisionListLearnerReturnsNegativeDLWhenDataSetEmpty()


        {
            // tests first base case of DL Learner
            DecisionListLearner learner = new DecisionListLearner("Yes", "No",
                        new MockDLTestFactory(null));
            DataSet ds = DataSetFactory.getRestaurantDataSet();
            DataSet empty = ds.emptyDataSet();
            learner.Train(empty);
            Assert.AreEqual("No", learner.Predict(ds.getExample(0)));
            Assert.AreEqual("No", learner.Predict(ds.getExample(1)));
            Assert.AreEqual("No", learner.Predict(ds.getExample(2)));
        }

        [TestMethod]
        public void testDecisionListLearnerReturnsFailureWhenTestsEmpty()


        {
            // tests second base case of DL Learner
            DecisionListLearner learner = new DecisionListLearner("Yes", "No", new MockDLTestFactory(CollectionFactory.CreateQueue<tvn.cosine.ai.learning.inductive.DecisionListTest>()));
            DataSet ds = DataSetFactory.getRestaurantDataSet();
            learner.Train(ds);
            Assert.AreEqual(DecisionListLearner.FAILURE,
                    learner.Predict(ds.getExample(0)));
        }

        [TestMethod]
        public void testDecisionListTestRunOnRestaurantDataSet()
        {
            DataSet ds = DataSetFactory.getRestaurantDataSet();
            DecisionListLearner learner = new DecisionListLearner("Yes", "No",

                        new DecisionListTestFactory());
            learner.Train(ds);

            int[] result = learner.Test(ds);
            Assert.AreEqual(12, result[0]);
            Assert.AreEqual(0, result[1]);
        }

        [TestMethod]
        public void testCurrentBestLearnerOnRestaurantDataSet()
        {
            DataSet ds = DataSetFactory.getRestaurantDataSet();
            CurrentBestLearner learner = new CurrentBestLearner("Yes");
            learner.Train(ds);

            int[] result = learner.Test(ds);
            Assert.AreEqual(12, result[0]);
            Assert.AreEqual(0, result[1]);
        }
    }

}
