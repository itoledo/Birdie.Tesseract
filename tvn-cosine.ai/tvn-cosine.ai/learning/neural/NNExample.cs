﻿using tvn.cosine.ai.common.collections;

namespace tvn.cosine.ai.learning.neural
{
    public class NNExample
    {
        private readonly IQueue<double> normalizedInput, normalizedTarget;

        public NNExample(IQueue<double> normalizedInput, IQueue<double> normalizedTarget)
        {
            this.normalizedInput = normalizedInput;
            this.normalizedTarget = normalizedTarget;
        }

        public NNExample copyExample()
        {
            IQueue<double> newInput = Factory.CreateQueue<double>();
            IQueue<double> newTarget = Factory.CreateQueue<double>();
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
