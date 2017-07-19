using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.probability.hmm;
using tvn.cosine.ai.probability.hmm.impl;
using tvn.cosine.ai.util.math;

namespace tvn.cosine.ai.probability.example
{
    public class HMMExampleFactory
    { 
        public static HiddenMarkovModel getUmbrellaWorldModel()
        {
            Matrix transitionModel = new Matrix(new double[,] { { 0.7, 0.3 }, { 0.3, 0.7 } });
            IMap<object, Matrix> sensorModel = Factory.CreateMap<object, Matrix>();
            sensorModel.Put(true, new Matrix(new double[,] { { 0.9, 0.0 }, { 0.0, 0.2 } }));
            sensorModel.Put(false, new Matrix(new double[,] { { 0.1, 0.0 }, { 0.0, 0.8 } }));
            Matrix prior = new Matrix(new double[] { 0.5, 0.5 }, 2);
            return new HMM(ExampleRV.RAIN_t_RV, transitionModel, sensorModel, prior);
        }
    }
}
