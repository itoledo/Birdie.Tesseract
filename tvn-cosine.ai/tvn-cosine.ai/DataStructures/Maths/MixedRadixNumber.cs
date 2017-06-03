using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tvn_cosine.ai.DataStructures.Maths
{
    /// <summary>
    /// For details on Mixed Radix Number Representations See <see cref="http://demonstrations.wolfram.com/MixedRadixNumberRepresentations/"/>
    /// </summary>
    public class MixedRadixNumber
    {
        private long maxValue;
        private int[] radices;
        private int[] currentNumeralValue;
        private bool recalculate;

        /// <summary>
        /// Constructs a mixed radix number with a specified value and a specified array of radices.
        /// </summary>
        /// <param name="value">the value of the mixed radix number</param>
        /// <param name="radices">the radices used to represent the value of the mixed radix number</param>
        public MixedRadixNumber(long value, int[] radices)
        {
            this.recalculate = true;
            this.Value = value;
            this.radices = new int[radices.Length];
            Array.Copy(radices, 0, this.radices, 0, radices.Length);
            calculateMaxValue();
        }

        /// <summary>
        /// Constructs a mixed radix number with a specified value and a specified list of radices.
        /// </summary>
        /// <param name="value">the value of the mixed radix number</param>
        /// <param name="radices">the radices used to represent the value of the mixed radix number</param>
        public MixedRadixNumber(long value, ICollection<int> radices)
            : this(value, radices.ToArray())
        { }

        /// <summary>
        /// Constructs a mixed radix number with a specified array of numerals and a specified array of radices.
        /// </summary>
        /// <param name="radixValues">the numerals of the mixed radix number</param>
        /// <param name="radices">the radices of the mixed radix number</param>
        public MixedRadixNumber(int[] radixValues, int[] radices)
            : this(0, radices)
        {
            SetCurrentValueFor(radixValues);
        }

        public long Value { get; private set; }

        /// <summary>
        /// Returns the value of the mixed radix number with the specified array of numerals and the current array of radices.
        /// </summary>
        /// <param name="radixValues"></param>
        /// <returns>the value of the mixed radix number</returns>
        /// <exception cref="ArgumentException">if any of the specified numerals is less than zero, or if any
        /// of the specified numerals is greater than it's corresponding radix.
        /// </exception>
        public long GetCurrentValueFor(int[] radixValues)
        {
            if (radixValues.Length != radices.Length)
            {
                throw new ArgumentException("Radix values not same size as Radices.");
            }

            long cvalue = 0;
            long mvalue = 1;
            for (int i = 0; i < radixValues.Length; ++i)
            {
                if (radixValues[i] < 0 || radixValues[i] >= radices[i])
                {
                    throw new ArgumentException(string.Format("Radix value {0} is out of range for radix at this position", i));
                }
                if (i > 0)
                {
                    mvalue *= radices[i - 1];
                }
                cvalue += mvalue * radixValues[i];
            }
            return cvalue;
        }

        /// <summary>
        /// Sets the value of the mixed radix number with the specified array of numerals and the current array of radices.
        /// </summary>
        /// <param name="radixValues">the numerals of the mixed radix number</param>
        public void SetCurrentValueFor(int[] radixValues)
        {
            this.Value = GetCurrentValueFor(radixValues);
            Array.Copy(radixValues, 0, this.currentNumeralValue, 0, radixValues.Length);
            recalculate = false;
        }

        /// <summary>
        /// Returns the maximum value which can be represented by the current array of radices.
        /// </summary>
        /// <returns>the maximum value which can be represented by the current array of radices.</returns>
        public long GetMaxAllowedValue()
        {
            return maxValue;
        }

        /// <summary>
        /// Increments the value of the mixed radix number, if the value is less than
        /// the maximum value which can be represented by the current array of
        /// radices.
        /// </summary>
        /// <returns><code>true</code> if the increment was successful.</returns>
        public bool Increment()
        {
            if (Value < maxValue)
            {
                ++Value;
                recalculate = true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Decrements the value of the mixed radix number, if the value is greater than zero.
        /// </summary>
        /// <returns><code>true</code> if the decrement was successful.</returns>
        public bool Decrement()
        {
            if (Value > 0)
            {
                --Value;
                recalculate = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns the numeral at the specified position.
        /// </summary>
        /// <param name="atPosition">the position of the numeral to return</param>
        /// <returns>the numeral at the specified position.</returns>
        /// <exception cref="ArgumentException" />
        public int GetCurrentNumeralValue(int atPosition)
        {
            if (0 <= atPosition
             && radices.Length > atPosition)
            {
                if (recalculate)
                {
                    long quotient = Value;
                    for (int i = 0; i < radices.Length; ++i)
                    {
                        if (0 != quotient)
                        {
                            currentNumeralValue[i] = (int)quotient % radices[i];
                            quotient = quotient / radices[i];
                        }
                        else
                        {
                            currentNumeralValue[i] = 0;
                        }

                    }
                    recalculate = false;
                }
                return currentNumeralValue[atPosition];
            }
            throw new ArgumentException(string.Format("Argument atPosition must be >=0 and < {0}", radices.Length));
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            for (int i = 0; i < radices.Length; ++i)
            {
                stringBuilder.Append('[');
                stringBuilder.Append(this.GetCurrentNumeralValue(i));
                stringBuilder.Append(']');
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Sets the maximum value which can be represented by the current array of radices.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// if no radices are defined, if any radix is less than two, or
        /// if the current value is greater than the maximum value which
        /// can be represented by the current array of radices.
        /// </exception>
        private void calculateMaxValue()
        {
            if (0 == radices.Length)
            {
                throw new ArgumentException("At least 1 radix must be defined.");
            }
            for (int i = 0; i < radices.Length; ++i)
            {
                if (2 > radices[i])
                {
                    throw new ArgumentException("Invalid radix, must be >= 2");
                }
            }

            // Calculate the maxValue allowed
            maxValue = radices[0];
            for (int i = 1; i < radices.Length; ++i)
            {
                maxValue *= radices[i];
            }
            maxValue -= 1;

            if (Value > maxValue)
            {
                throw new ArgumentException(string.Format("The value [{0}] cannot be represented with the radices provided, max value is {1}",
                                                          Value, maxValue));
            }

            currentNumeralValue = new int[radices.Length];
        }
    }
}
