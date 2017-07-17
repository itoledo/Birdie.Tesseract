using System.Text;
using tvn.cosine.ai.common;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.exceptions;
using tvn.cosine.ai.learning.framework;
using tvn.cosine.ai.util;

namespace tvn.cosine.ai.learning.inductive
{
    public class DecisionTree : IToString
    {
        private string attributeName;

        // each node modelled as a hash of attribute_value/decisiontree
        private IMap<string, DecisionTree> nodes;

        protected DecisionTree()
        { }

        public DecisionTree(string attributeName)
        {
            this.attributeName = attributeName;
            nodes = Factory.CreateMap<string, DecisionTree>();
        }

        public virtual void addLeaf(string attributeValue, string decision)
        {
            nodes.Put(attributeValue, new ConstantDecisonTree(decision));
        }

        public virtual void addNode(string attributeValue, DecisionTree tree)
        {
            nodes.Put(attributeValue, tree);
        }

        public virtual object predict(Example e)
        {
            string attrValue = e.getAttributeValueAsString(attributeName);
            if (nodes.ContainsKey(attrValue))
            {
                return nodes.Get(attrValue).predict(e);
            }
            else
            {
                throw new RuntimeException("no node exists for attribute value " + attrValue);
            }
        }

        public static DecisionTree getStumpFor(DataSet ds, string attributeName,
                string attributeValue, string returnValueIfMatched,
                IQueue<string> unmatchedValues, string returnValueIfUnmatched)
        {
            DecisionTree dt = new DecisionTree(attributeName);
            dt.addLeaf(attributeValue, returnValueIfMatched);
            foreach (string unmatchedValue in unmatchedValues)
            {
                dt.addLeaf(unmatchedValue, returnValueIfUnmatched);
            }
            return dt;
        }

        public static IQueue<DecisionTree> getStumpsFor(DataSet ds,
                string returnValueIfMatched, string returnValueIfUnmatched)
        {
            IQueue<string> attributes = ds.getNonTargetAttributes();
            IQueue<DecisionTree> trees = Factory.CreateQueue<DecisionTree>();
            foreach (string attribute in attributes)
            {
                IQueue<string> values = ds.getPossibleAttributeValues(attribute);
                foreach (string value in values)
                {
                    IQueue<string> unmatchedValues = Util.removeFrom(ds.getPossibleAttributeValues(attribute), value);

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
            return ToString(1, new StringBuilder());
        }

        public virtual string ToString(int depth, StringBuilder buf)
        {
            if (attributeName != null)
            {
                buf.Append(Util.ntimes("\t", depth));
                buf.Append(Util.ntimes("***", 1));
                buf.Append(attributeName + " \n");
                foreach (string attributeValue in nodes.GetKeys())
                {
                    buf.Append(Util.ntimes("\t", depth + 1));
                    buf.Append("+" + attributeValue);
                    buf.Append("\n");
                    DecisionTree child = nodes.Get(attributeValue);
                    buf.Append(child.ToString(depth + 1, new StringBuilder()));
                }
            }

            return buf.ToString();
        }
    }
}
