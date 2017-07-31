using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.collections;
using tvn.cosine.collections.api;
using tvn.cosine.ai.learning.framework;
using tvn.cosine.ai.learning.framework.api;
using tvn.cosine.ai.learning.inductive;
using tvn.cosine.ai.learning.learners;

namespace tvn_cosine.ai.test.unit.learning.learners
{
    [TestClass]
    public class EnsembleLearningTest
    {

        private static readonly string YES = "Yes";

        [TestMethod]
        public void testAdaBoostEnablesCollectionOfStumpsToClassifyDataSetAccurately()


        {
            DataSet ds = DataSetFactory.getRestaurantDataSet();
            ICollection<DecisionTree> stumps = DecisionTree.getStumpsFor(ds, YES, "No");
            ICollection<ILearner> learners = CollectionFactory.CreateQueue<ILearner>();
            foreach (object stump in stumps)
            {
                DecisionTree sl = (DecisionTree)stump;
                StumpLearner stumpLearner = new StumpLearner(sl, "No");
                learners.Add(stumpLearner);
            }
            AdaBoostLearner learner = new AdaBoostLearner(learners, ds);
            learner.train(ds);
            int[] result = learner.Test(ds);
            Assert.AreEqual(12, result[0]);
            Assert.AreEqual(0, result[1]);
        }
    }
}
