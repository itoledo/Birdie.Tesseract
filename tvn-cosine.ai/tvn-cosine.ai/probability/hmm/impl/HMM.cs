using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.exceptions;
using tvn.cosine.ai.probability.domain;
using tvn.cosine.ai.probability.proposition;
using tvn.cosine.ai.probability.util;
using tvn.cosine.ai.util;
using tvn.cosine.ai.util.math;

namespace tvn.cosine.ai.probability.hmm.impl
{
    /**
     * Default implementation of the HiddenMarkovModel interface.
     * 
     * @author Ciaran O'Reilly
     * @author Ravi Mohan
     */
    public class HMM : HiddenMarkovModel
    {
        private RandomVariable stateVariable = null;
        private FiniteDomain stateVariableDomain = null;
        private Matrix transitionModel = null;
        private IMap<object, Matrix> sensorModel = null;
        private Matrix prior = null;

        /**
         * Instantiate a Hidden Markov Model.
         * 
         * @param stateVariable
         *            the single discrete random variable used to describe the
         *            process states 1,...,S.
         * @param transitionModel
         *            the transition model:<br>
         *            <b>P</b>(X<sub>t</sub> | X<sub>t-1</sub>)<br>
         *            is represented by an S * S matrix <b>T</b> where<br>
         *            <b>T</b><sub>ij</sub> = P(X<sub>t</sub> = j | X<sub>t-1</sub>
         *            = i).
         * @param sensorModel
         *            the sensor model in matrix form:<br>
         *            P(e<sub>t</sub> | X<sub>t</sub> = i) for each state i. For
         *            mathematical convenience we place each of these values into an
         *            S * S diagonal matrix.
         * @param prior
         *            the prior distribution represented as a column vector in
         *            Matrix form.
         */
        public HMM(RandomVariable stateVariable, Matrix transitionModel, IMap<object, Matrix> sensorModel, Matrix prior)
        {
            if (!stateVariable.getDomain().isFinite())
            {
                throw new IllegalArgumentException("State Variable for HHM must be finite.");
            }
            this.stateVariable = stateVariable;
            stateVariableDomain = (FiniteDomain)stateVariable.getDomain();
            if (transitionModel.getRowDimension() != transitionModel
                    .getColumnDimension())
            {
                throw new IllegalArgumentException("Transition Model row and column dimensions must match.");
            }
            if (stateVariableDomain.size() != transitionModel.getRowDimension())
            {
                throw new IllegalArgumentException("Transition Model Matrix does not map correctly to the HMM's State Variable.");
            }
            this.transitionModel = transitionModel;
            foreach (Matrix smVal in sensorModel.GetValues())
            {
                if (smVal.getRowDimension() != smVal.getColumnDimension())
                {
                    throw new IllegalArgumentException("Sensor Model row and column dimensions must match.");
                }
                if (stateVariableDomain.size() != smVal.getRowDimension())
                {
                    throw new IllegalArgumentException("Sensor Model Matrix does not map correctly to the HMM's State Variable.");
                }
            }
            this.sensorModel = sensorModel;
            if (transitionModel.getRowDimension() != prior.getRowDimension()
                    && prior.getColumnDimension() != 1)
            {
                throw new IllegalArgumentException("Prior is not of the correct dimensions.");
            }
            this.prior = prior;
        }

        public virtual RandomVariable getStateVariable()
        {
            return stateVariable;
        }

        public virtual Matrix getTransitionModel()
        {
            return transitionModel;
        }

        public virtual IMap<object, Matrix> getSensorModel()
        {
            return sensorModel;
        }

        public virtual Matrix getPrior()
        {
            return prior;
        }

        public virtual Matrix getEvidence(IQueue<AssignmentProposition> evidence)
        {
            if (evidence.Size() != 1)
            {
                throw new IllegalArgumentException("Only a single evidence observation value should be provided.");
            }
            Matrix e = sensorModel.Get(evidence.Get(0).getValue());
            if (null == e)
            {
                throw new IllegalArgumentException("Evidence does not map to sensor model.");
            }
            return e;
        }


        public virtual Matrix createUnitMessage()
        {
            double[] values = new double[stateVariableDomain.size()];
            for (int i = 0; i < values.Length; ++i)
            {
                values[i] = 1D;
            }

            return new Matrix(values, values.Length);
        }


        public virtual Matrix convert(CategoricalDistribution fromCD)
        {
            double[] values = fromCD.getValues();
            return new Matrix(values, values.Length);
        }


        public virtual CategoricalDistribution convert(Matrix fromMessage)
        {
            return new ProbabilityTable(fromMessage.getRowPackedCopy(), stateVariable);
        }

        public virtual IQueue<CategoricalDistribution> convert(IQueue<Matrix> matrixs)
        {
            IQueue<CategoricalDistribution> cds = Factory.CreateQueue<CategoricalDistribution>();
            foreach (Matrix m in matrixs)
            {
                cds.Add(convert(m));
            }
            return cds;
        }
         
        public virtual Matrix normalize(Matrix m)
        {
            double[] values = m.getRowPackedCopy();
            return new Matrix(Util.normalize(values), values.Length);
        }
    }
}
