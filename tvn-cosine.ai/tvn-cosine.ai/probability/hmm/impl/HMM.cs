using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class HMM<T> : HiddenMarkovModel<T>
    {
        private RandomVariable stateVariable = null;
        private FiniteDomain<T> stateVariableDomain = null;
        private Matrix transitionModel = null;
        private IDictionary<T, Matrix> sensorModel = null;
        private Matrix prior = null;

        /**
         * Instantiate a Hidden Markov Model.
         * 
         * @param stateVariable
         *            the single discrete random variable used to describe the
         *            process states 1,...,S.
         * @param transitionModel
         *            the transition model: 
         *            <b>P</b>(X<sub>t</sub> | X<sub>t-1</sub>) 
         *            is represented by an S * S matrix <b>T</b> where 
         *            <b>T</b><sub>ij</sub> = P(X<sub>t</sub> = j | X<sub>t-1</sub>
         *            = i).
         * @param sensorModel
         *            the sensor model in matrix form: 
         *            P(e<sub>t</sub> | X<sub>t</sub> = i) for each state i. For
         *            mathematical convenience we place each of these values into an
         *            S * S diagonal matrix.
         * @param prior
         *            the prior distribution represented as a column vector in
         *            Matrix form.
         */
        public HMM(RandomVariable stateVariable, Matrix transitionModel, IDictionary<T, Matrix> sensorModel, Matrix prior)
        {
            if (!stateVariable.getDomain().isFinite())
            {
                throw new ArgumentException("State Variable for HHM must be finite.");
            }
            this.stateVariable = stateVariable;
            stateVariableDomain = (FiniteDomain<T>)stateVariable.getDomain();
            if (transitionModel.getRowDimension() != transitionModel.getColumnDimension())
            {
                throw new ArgumentException("Transition Model row and column dimensions must match.");
            }
            if (stateVariableDomain.size() != transitionModel.getRowDimension())
            {
                throw new ArgumentException("Transition Model Matrix does not map correctly to the HMM's State Variable.");
            }
            this.transitionModel = transitionModel;
            foreach (Matrix smVal in sensorModel.Values)
            {
                if (smVal.getRowDimension() != smVal.getColumnDimension())
                {
                    throw new ArgumentException("Sensor Model row and column dimensions must match.");
                }
                if (stateVariableDomain.size() != smVal.getRowDimension())
                {
                    throw new ArgumentException("Sensor Model Matrix does not map correctly to the HMM's State Variable.");
                }
            }
            this.sensorModel = sensorModel;
            if (transitionModel.getRowDimension() != prior.getRowDimension()
                    && prior.getColumnDimension() != 1)
            {
                throw new ArgumentException("Prior is not of the correct dimensions.");
            }
            this.prior = prior;
        }

        //
        // START-HiddenMarkovModel 
        public RandomVariable getStateVariable()
        {
            return stateVariable;
        }

        public Matrix getTransitionModel()
        {
            return transitionModel;
        }

        public IDictionary<T, Matrix> getSensorModel()
        {
            return sensorModel;
        }

        public Matrix getPrior()
        {
            return prior;
        }

        public Matrix getEvidence(IList<AssignmentProposition<T>> evidence)
        {
            if (evidence.Count != 1)
            {
                throw new ArgumentException("Only a single evidence observation value should be provided.");
            }
            Matrix e = sensorModel[evidence[0].getValue()];
            if (null == e)
            {
                throw new ArgumentException("Evidence does not map to sensor model.");
            }
            return e;
        }

        public Matrix createUnitMessage()
        {
            double[] values = new double[stateVariableDomain.size()];
            for (int i = 0; i < values.Length; ++i)
                values[i] = 1D;
            return new Matrix(values, values.Length);
        }

        public Matrix convert(CategoricalDistribution<T> fromCD)
        {
            double[] values = fromCD.getValues();
            return new Matrix(values, values.Length);
        }

        public CategoricalDistribution<T> convert(Matrix fromMessage)
        {
            return new ProbabilityTable<T>(fromMessage.getRowPackedCopy(), stateVariable);
        }

        public IList<CategoricalDistribution<T>> convert(IList<Matrix> matrixs)
        {
            IList<CategoricalDistribution<T>> cds = new List<CategoricalDistribution<T>>();
            foreach (Matrix m in matrixs)
            {
                cds.Add(convert(m));
            }
            return cds;
        }

        public Matrix normalize(Matrix m)
        {
            double[] values = m.getRowPackedCopy();
            return new Matrix(Util.normalize(values), values.Length);
        }

        // END-HiddenMarkovModel
        //
    }
}
