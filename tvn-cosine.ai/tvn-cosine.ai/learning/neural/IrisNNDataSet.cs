using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tvn.cosine.ai.learning.neural
{
    public class IrisNNDataSet : NNDataSet
    {
        public void setTargetColumns()
        {
            // assumed that data from file has been pre processed
            // TODO this should be
            // somewhere else,in the
            // super class.
            // Type != class Aargh! I want more
            // powerful type systems
            targetColumnNumbers = Factory.CreateQueue<Integer>();
            int size = nds.get(0).size();
            targetColumnNumbers.add(size - 1); // last column
            targetColumnNumbers.add(size - 2); // last but one column
            targetColumnNumbers.add(size - 3); // and the one before that
        }
    }

}
