using System;
using System.Collections.Generic;
using tvn.cosine.ai.learning.framework;
using tvn.cosine.ai.learning.inductive;

namespace tvn.cosine.ai.learning.learners
{
    /**
     * @author Ravi Mohan
     * @author Mike Stampone
     */
    public class DecisionListLearner : Learner
    {
        public const string FAILURE = "Failure";
        private DecisionList decisionList;
        private string positive, negative;
        private DLTestFactory testFactory;

        public DecisionListLearner(string positive, string negative, DLTestFactory testFactory)
        {
            this.positive = positive;
            this.negative = negative;
            this.testFactory = testFactory;
        }

        //
        // START-Learner

        /**
         * Induces the decision list from the specified set of examples
         * 
         * @param ds
         *            a set of examples for constructing the decision list
         */
        public void train(DataSet ds)
        {
            this.decisionList = decisionListLearning(ds);
        }

        public string predict(Example e)
        {
            if (decisionList == null)
            {
                throw new Exception("learner has not been trained with dataset yet!");
            }
            return decisionList.predict(e);
        }

        public int[] test(DataSet ds)
        {
            int[] results = new int[] { 0, 0 };

            foreach (Example e in ds.examples)
            {
                if (e.targetValue().Equals(decisionList.predict(e)))
                {
                    results[0] = results[0] + 1;
                }
                else
                {
                    results[1] = results[1] + 1;
                }
            }
            return results;
        }

        // END-Learner
        //

        /**
         * Returns the decision list of this decision list learner
         * 
         * @return the decision list of this decision list learner
         */
        public DecisionList getDecisionList()
        {
            return decisionList;
        }

        //
        // PRIVATE METHODS
        //
        private DecisionList decisionListLearning(DataSet ds)
        {
            if (ds.size() == 0)
            {
                return new DecisionList(positive, negative);
            }
            IList<DLTest> possibleTests = testFactory.createDLTestsWithAttributeCount(ds, 1);
            DLTest test = getValidTest(possibleTests, ds);
            if (test == null)
            {
                return new DecisionList(null, FAILURE);
            }
            // at this point there is a test that classifies some subset of examples
            // with the same target value
            DataSet matched = test.matchedExamples(ds);
            DecisionList list = new DecisionList(positive, negative);
            list.add(test, matched.getExample(0).targetValue());
            return list.mergeWith(decisionListLearning(test.unmatchedExamples(ds)));
        }

        private DLTest getValidTest(IList<DLTest> possibleTests, DataSet ds)
        {
            foreach (DLTest test in possibleTests)
            {
                DataSet matched = test.matchedExamples(ds);
                if (!(matched.size() == 0))
                {
                    if (allExamplesHaveSameTargetValue(matched))
                    {
                        return test;
                    }
                }

            }
            return null;
        }

        private bool allExamplesHaveSameTargetValue(DataSet matched)
        {
            // assumes at least i example in dataset
            string targetValue = matched.getExample(0).targetValue();
            foreach (Example e in matched.examples)
            {
                if (!(e.targetValue().Equals(targetValue)))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
