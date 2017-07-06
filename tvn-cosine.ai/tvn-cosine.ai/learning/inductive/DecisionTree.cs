using System;
using System.Collections.Generic;
using System.Text;
using tvn.cosine.ai.learning.framework;
using tvn.cosine.ai.util;

namespace tvn.cosine.ai.learning.inductive
{
    /**
     * @author Ravi Mohan
     * 
     */
    public class DecisionTree
    {
        private string attributeName;

        // each node modelled as a hash of attribute_value/decisiontree
        private IDictionary<string, DecisionTree> nodes;

        protected DecisionTree()
        {

        }

        public DecisionTree(string attributeName)
        {
            this.attributeName = attributeName;
            nodes = new Dictionary<string, DecisionTree>();

        }

        public virtual void addLeaf(string attributeValue, string decision)
        {
            nodes.Add(attributeValue, new ConstantDecisonTree(decision));
        }

        public virtual void addNode(string attributeValue, DecisionTree tree)
        {
            nodes.Add(attributeValue, tree);
        }

        public virtual object predict(Example e)
        {
            string attrValue = e.getAttributeValueAsString(attributeName);
            if (nodes.ContainsKey(attrValue))
            {
                return nodes[attrValue].predict(e);
            }
            else
            {
                throw new Exception("no node exists for attribute value " + attrValue);
            }
        }

        public static DecisionTree getStumpFor(DataSet ds, string attributeName,
                string attributeValue, string returnValueIfMatched,
                List<string> unmatchedValues, string returnValueIfUnmatched)
        {
            DecisionTree dt = new DecisionTree(attributeName);
            dt.addLeaf(attributeValue, returnValueIfMatched);
            foreach (string unmatchedValue in unmatchedValues)
            {
                dt.addLeaf(unmatchedValue, returnValueIfUnmatched);
            }
            return dt;
        }

        public static List<DecisionTree> getStumpsFor(DataSet ds, string returnValueIfMatched, string returnValueIfUnmatched)
        {
            List<string> attributes = ds.getNonTargetAttributes();
            List<DecisionTree> trees = new List<DecisionTree>();
            foreach (string attribute in attributes)
            {
                List<string> values = ds.getPossibleAttributeValues(attribute);
                foreach (string value in values)
                {
                    List<string> unmatchedValues = Util.removeFrom(ds.getPossibleAttributeValues(attribute), value);

                    DecisionTree tree = getStumpFor(ds, attribute, value,
                            returnValueIfMatched, unmatchedValues,
                            returnValueIfUnmatched);
                    trees.Add(tree);

                }
            }
            return trees;
        }

        /**
         * @return Returns the attributeName.
         */
        public virtual string getAttributeName()
        {
            return attributeName;
        }

        public override string ToString()
        {
            return toString(1, new StringBuilder());
        }

        public virtual string toString(int depth, StringBuilder buf)
        {
            if (attributeName != null)
            {
                buf.Append(Util.ntimes("\t", depth));
                buf.Append(Util.ntimes("***", 1));
                buf.Append(attributeName + " \n");
                foreach (string attributeValue in nodes.Keys)
                {
                    buf.Append(Util.ntimes("\t", depth + 1));
                    buf.Append("+" + attributeValue);
                    buf.Append("\n");
                    DecisionTree child = nodes[attributeValue];
                    buf.Append(child.toString(depth + 1, new StringBuilder()));
                }
            }

            return buf.ToString();
        }
    }

}
