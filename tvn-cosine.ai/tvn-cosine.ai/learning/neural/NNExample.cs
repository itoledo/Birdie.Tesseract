using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.collections.api;
using tvn.cosine.ai.util.math;

namespace tvn.cosine.ai.learning.neural
{
    public class NNExample
    {
        private readonly ICollection<double> normalizedInput, normalizedTarget;

        public NNExample(ICollection<double> normalizedInput, ICollection<double> normalizedTarget)
        {
            this.normalizedInput = normalizedInput;
            this.normalizedTarget = normalizedTarget;
        }

        public NNExample copyExample()
        {
            ICollection<double> newInput = CollectionFactory.CreateQueue<double>();
            ICollection<double> newTarget = CollectionFactory.CreateQueue<double>();
            foreach (double d in normalizedInput)
            {
                newInput.Add(d);
            }
            foreach (double d in normalizedTarget)
            {
                newTarget.Add(d);
            }
            return new NNExample(newInput, newTarget);
        }

        public Vector getInput()
        {
            Vector v = new Vector(normalizedInput);
            return v;

        }

        public Vector getTarget()
        {
            Vector v = new Vector(normalizedTarget);
            return v;

        }

        public bool isCorrect(Vector prediction)
        {
            /*
             * compares the index having greatest value in target to indec having
             * greatest value in prediction. Ifidentical, correct
             */
            return getTarget().indexHavingMaxValue() == prediction.indexHavingMaxValue();
        }
    } 
}
