using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.collections.api;

namespace tvn.cosine.ai.learning.neural
{
    public class IrisNNDataSet : NNDataSet
    {
        public override void setTargetColumns()
        {
            // assumed that data from file has been pre processed
            // TODO this should be
            // somewhere else,in the
            // super class.
            // Type != class Aargh! I want more
            // powerful type systems
            targetColumnNumbers = CollectionFactory.CreateQueue<int>();
            int size = nds.Get(0).Size();
            targetColumnNumbers.Add(size - 1); // last column
            targetColumnNumbers.Add(size - 2); // last but one column
            targetColumnNumbers.Add(size - 3); // and the one before that
        }
    }

}
