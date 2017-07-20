using System;
using tvn.cosine.ai.common;

namespace tvn.cosine.ai.util
{
    /**
     * Mock implementation of the Randomizer interface so that the set of Random
     * numbers returned are in fact predefined.
     * 
     * @author Ravi Mohan
     * 
     */
    public class MockRandomizer : IRandom
    {
        private double[] values;
        private int index;

        /**
         * 
         * @param values
         *            the set of predetermined random values to loop over.
         */
        public MockRandomizer(double[] values)
        {
            this.values = new double[values.Length];
            System.Array.Copy(values, 0, this.values, 0, values.Length);
            this.index = 0;
        }

        //
        // START-Randomizer 
        public double NextDouble()
        {
            if (index == values.Length)
            {
                index = 0;
            }

            return values[index++];
        }

        public bool NextBoolean()
        {
            throw new NotImplementedException();
        }

        public int Next()
        {
            throw new NotImplementedException();
        }

        public int Next(int minimumValue, int maximumValue)
        {
            throw new NotImplementedException();
        }

        public int Next(int maximumValue)
        {
            throw new NotImplementedException();
        }
        // END-Randomizer
        //
    }
}
