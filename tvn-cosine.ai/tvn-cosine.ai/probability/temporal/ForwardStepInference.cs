using System.Collections.Generic;
using tvn.cosine.ai.probability.proposition;

namespace tvn.cosine.ai.probability.temporal
{
    /**
     * * The FORWARD operator is defined by Equation (15.5). 
     * 
     * <pre>
     * <b>P</b>(<b>X</b><sub>t+1</sub> | <b>e</b><sub>1:t+1</sub>) 
     * = &alpha;<b>P</b>(<b>e</b><sub>t+1</sub> | <b>X</b><sub>t+1</sub>)&sum;<sub><b>x</b><sub>t</sub></sub><b>P</b>(<b>X</b><sub>t+1</sub> | <b>x</b><sub>t</sub>, <b>e</b><sub>1:t</sub>)P(<b>x</b><sub>t</sub> | <b>e</b><sub>1:t</sub>)
     * = &alpha;<b>P</b>(<b>e</b><sub>t+1</sub> | <b>X</b><sub>t+1</sub>)&sum;<sub><b>x</b><sub>t</sub></sub><b>P</b>(<b>X</b><sub>t+1</sub> | <b>x</b><sub>t</sub>)P(<b>x</b><sub>t</sub> | <b>e</b><sub>1:t</sub>) (Markov Assumption)
     * </pre>
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public interface ForwardStepInference<T>
    {

        /**
         * The FORWARD operator is defined by Equation (15.5). 
         * 
         * <pre>
         * <b>P</b>(<b>X</b><sub>t+1</sub> | <b>e</b><sub>1:t+1</sub>) 
         * = &alpha;<b>P</b>(<b>e</b><sub>t+1</sub> | <b>X</b><sub>t+1</sub>)&sum;<sub><b>x</b><sub>t</sub></sub><b>P</b>(<b>X</b><sub>t+1</sub> | <b>x</b><sub>t</sub>, <b>e</b><sub>1:t</sub>)P(<b>x</b><sub>t</sub> | <b>e</b><sub>1:t</sub>)
         * = &alpha;<b>P</b>(<b>e</b><sub>t+1</sub> | <b>X</b><sub>t+1</sub>)&sum;<sub><b>x</b><sub>t</sub></sub><b>P</b>(<b>X</b><sub>t+1</sub> | <b>x</b><sub>t</sub>)P(<b>x</b><sub>t</sub> | <b>e</b><sub>1:t</sub>) (Markov Assumption)
         * </pre>
         * 
         * @param f1_t
         *            f<sub>1:t</sub>
         * @param e_tp1
         *            <b>e</b><sub>t+1</sub>
         * @return f<sub>1:t+1</sub>
         */
        CategoricalDistribution<T> forward(CategoricalDistribution<T> f1_t, IList<AssignmentProposition<T>> e_tp1);
    } 
}
