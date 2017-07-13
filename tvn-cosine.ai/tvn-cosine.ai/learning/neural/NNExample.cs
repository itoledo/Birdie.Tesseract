using System.Collections.Generic;
using tvn.cosine.ai.util.math;

namespace tvn.cosine.ai.learning.neural
{
    /**
     * @author Ravi Mohan
     * 
     */
    public class NNExample
    {
        private readonly IList<double> normalizedInput, normalizedTarget;

        public NNExample(IList<double> normalizedInput, IList<double> normalizedTarget)
        {
            this.normalizedInput = normalizedInput;
            this.normalizedTarget = normalizedTarget;
        }

        public NNExample copyExample()
        {
            IList<double> newInput = new List<double>();
            IList<double> newTarget = new List<double>();
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
            return getTarget().indexHavingMaxValue() == prediction
                    .indexHavingMaxValue();
        }
    } 
}
