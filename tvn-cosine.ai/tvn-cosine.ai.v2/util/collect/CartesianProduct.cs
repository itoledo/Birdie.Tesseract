using aima.core.util.math;
using System.Collections.Generic;
using System.Text;
using tvn_cosine.ai.v2.common.exceptions;

namespace aima.core.util.collect
{
    /**
     * An {@link java.lang.Iterable} over an
     * <a href="http://en.wikipedia.org/wiki/Cartesian_product">n-fold Cartesian
     * product</a>, represented by a {@link java.util.List} of n dimensions, where
     * each element is an n-length {@link java.util.List}.
     *
     * @param <T>
     *            the type of elements in the Cartesian Product.
     *
     * @author Ciaran O'Reilly
     */
    public class CartesianProduct<T>
    {
        private T elementBaseType;
        private int[] radices;
        private MixedRadixInterval mri;
        // For access efficiency we will access the elements from a
        // contiguous 2d array.
        private T[][] dimensions;

        public CartesianProduct(T elementBaseType, IList<IList<T>> dimensions)
        {
            if (null == elementBaseType)
            {
                throw new IllegalArgumentException("elementBaseType");
            }
            if (null == dimensions)
            {
                throw new IllegalArgumentException("dimensions");
            }
            if (0 == dimensions.Count)
            {
                throw new IllegalArgumentException("No dimensions specified");
            }

            this.elementBaseType = elementBaseType;
            // Copy the dimension information into own arrays
            // to ensure they cannot be mutated. This ensures
            // we keep the immutable property.
            this.radices = new int[dimensions.Count];
            for (int i = 0; i < dimensions.Count; i++)
            {
                this.radices[i] = dimensions[i].Count;
            }
            this.mri = new MixedRadixInterval(this.radices);
            // NOTE: the sizes of the 2nd dimension can vary so we will only set the first dimension
            // size first.
            this.dimensions = new T[radices.Length][];
            int currentDimension = -1;
            foreach (IList<T> dimension in dimensions)
            {
                int dimensionIdx = currentDimension++;
                if (0 == dimension.Count)
                {
                    throw new IllegalArgumentException("Dimension " + dimensionIdx + " has no elements in it.");
                }
                // Up front, ensure all the dimension's elements are of the required base type.
                List<T> typeSafeDimension = new List<T>();
                typeSafeDimension.AddRange(dimension);
                // Assign the correctly sized second dimension.
                this.dimensions[dimensionIdx] = new T[typeSafeDimension.Count];
                for (int i = 0; i < typeSafeDimension.Count; i++)
                {
                    this.dimensions[dimensionIdx][i] = typeSafeDimension[i];
                }
            }
        }

        /**
         * Cartesian Power constructor (i.e dimension^n).
         *
         * @param elementBaseType
         *            the base type of the elements in the list (to permit
         *            construction of a type safe array).
         * @param dimension
         *            the dimension to be raised to a given power.
         * @param n
         *            the power to raise the dimension by.
         */
        public CartesianProduct(T elementBaseType, IList<T> dimension, int n)
            : this(elementBaseType, nDimensions(dimension, n))
        { }

        public long size()
        {
            return mri.size();
        }

        private static IList<IList<T>> nDimensions(IList<T> dimension, int n)
        {
            if (n <= 0)
            {
                throw new IllegalArgumentException("n must be > 0");
            }
            List<IList<T>> dimensions = new List<IList<T>>();
            for (int i = 0; i < n; i++)
            {
                dimensions.Add(dimension);
            }
            return dimensions;
        }
    }
}