using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using tvn.cosine.ai.learning.framework;
using tvn.cosine.ai.learning.inductive;
using tvn.cosine.ai.learning.learners;

namespace tvn_cosine.ai.test.learning.learners
{
    [TestClass]
    public class EnsembleLearningTest
    {
        private const string YES = "Yes";

        [TestMethod]
        public void testAdaBoostEnablesCollectionOfStumpsToClassifyDataSetAccurately()
        {
            DataSet ds = DataSetFactory.getRestaurantDataSet();
            IList<DecisionTree> stumps = DecisionTree.getStumpsFor(ds, YES, "No");
            IList<Learner> learners = new List<Learner>();
            foreach (DecisionTree stump in stumps)
            { 
                StumpLearner stumpLearner = new StumpLearner(stump, "No");
                learners.Add(stumpLearner);
            }
            AdaBoostLearner learner = new AdaBoostLearner(learners, ds);
            learner.train(ds);
            int[] result = learner.test(ds);
            Assert.AreEqual(12, result[0]);
            Assert.AreEqual(0, result[1]);
        }
    }
}
