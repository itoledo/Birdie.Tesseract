using System;
using System.Collections.Generic;
using tvn.cosine.ai.learning.framework;
using tvn.cosine.ai.learning.inductive;
using tvn.cosine.ai.util;

namespace tvn.cosine.ai.learning.learners
{
    /**
     * @author Ravi Mohan
     * @author Mike Stampone
     */
    public class DecisionTreeLearner : Learner
    {
        private DecisionTree tree;

        private string defaultValue;

        public DecisionTreeLearner()
        {
            this.defaultValue = "Unable To Classify";

        }

        // used when you have to test a non induced tree (eg: for testing)
        public DecisionTreeLearner(DecisionTree tree, string defaultValue)
        {
            this.tree = tree;
            this.defaultValue = defaultValue;
        }

        //
        // START-Learner

        /**
         * Induces the decision tree from the specified set of examples
         * 
         * @param ds
         *            a set of examples for constructing the decision tree
         */
        public virtual void train(DataSet ds)
        {
            IList<string> attributes = ds.getNonTargetAttributes();
            this.tree = decisionTreeLearning(ds, attributes,
                    new ConstantDecisonTree(defaultValue));
        }

        public virtual string predict(Example e)
        {
            return (string)tree.predict(e);
        }

        public virtual int[] test(DataSet ds)
        {
            int[] results = new int[] { 0, 0 };

            foreach (Example e in ds.examples)
            {
                if (e.targetValue().Equals(tree.predict(e)))
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
         * Returns the decision tree of this decision tree learner
         * 
         * @return the decision tree of this decision tree learner
         */
        public virtual DecisionTree getDecisionTree()
        {
            return tree;
        }

        //
        // PRIVATE METHODS
        //

        private DecisionTree decisionTreeLearning(DataSet ds, IList<String> attributeNames, ConstantDecisonTree defaultTree)
        {
            if (ds.size() == 0)
            {
                return defaultTree;
            }
            if (allExamplesHaveSameClassification(ds))
            {
                return new ConstantDecisonTree(ds.getExample(0).targetValue());
            }
            if (attributeNames.Count == 0)
            {
                return majorityValue(ds);
            }
            string chosenAttribute = chooseAttribute(ds, attributeNames);

            DecisionTree tree = new DecisionTree(chosenAttribute);
            ConstantDecisonTree m = majorityValue(ds);

            IList<string> values = ds.getPossibleAttributeValues(chosenAttribute);
            foreach (string v in values)
            {
                DataSet filtered = ds.matchingDataSet(chosenAttribute, v);
                IList<string> newAttribs = Util.removeFrom(attributeNames, chosenAttribute);
                DecisionTree subTree = decisionTreeLearning(filtered, newAttribs, m);
                tree.addNode(v, subTree);

            }

            return tree;
        }

        private ConstantDecisonTree majorityValue(DataSet ds)
        {
            Learner learner = new MajorityLearner();
            learner.train(ds);
            return new ConstantDecisonTree(learner.predict(ds.getExample(0)));
        }

        private string chooseAttribute(DataSet ds, IList<string> attributeNames)
        {
            double greatestGain = 0.0;
            string attributeWithGreatestGain = attributeNames[0];
            foreach (string attr in attributeNames)
            {
                double gain = ds.calculateGainFor(attr);
                if (gain > greatestGain)
                {
                    greatestGain = gain;
                    attributeWithGreatestGain = attr;
                }
            }

            return attributeWithGreatestGain;
        }
         
        private bool allExamplesHaveSameClassification(DataSet ds)
        {
            string classification = ds.getExample(0).targetValue();
            foreach (Example element in ds)
            {
                if (!(element.targetValue().Equals(classification)))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
