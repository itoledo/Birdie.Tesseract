using System.Collections.Generic;
using tvn.cosine.ai.probability.hmm;
using tvn.cosine.ai.probability.hmm.impl;
using tvn.cosine.ai.util.math;

namespace tvn.cosine.ai.probability.example
{
    /**
     * 
     * @author Ciaran O'Reilly
     * @author Ravi Mohan
     * 
     */
    public class HMMExampleFactory
    {

        public static HiddenMarkovModel<bool> getUmbrellaWorldModel()
        {
            Matrix transitionModel = new Matrix(new double[,] { { 0.7, 0.3 }, { 0.3, 0.7 } });
            IDictionary<bool, Matrix> sensorModel = new Dictionary<bool, Matrix>();
            sensorModel.Add(true, new Matrix(new double[,] { { 0.9, 0.0 }, { 0.0, 0.2 } }));
            sensorModel.Add(false, new Matrix(new double[,] { { 0.1, 0.0 }, { 0.0, 0.8 } }));
            Matrix prior = new Matrix(new double[] { 0.5, 0.5 }, 2);

            return new HMM<bool>(ExampleRV.RAIN_t_RV, transitionModel, sensorModel, prior);
        }
    }
}
