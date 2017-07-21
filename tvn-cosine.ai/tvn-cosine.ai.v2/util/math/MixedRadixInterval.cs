using System.Collections;
using System.Collections.Generic;
using System.Linq;
using tvn_cosine.ai.v2.common;
using tvn_cosine.ai.v2.common.exceptions;

namespace aima.core.util.math
{
    /**
     * A <a href="http://en.wikipedia.org/wiki/Mixed_radix">mixed radix</a>
     * <a href="http://en.wikipedia.org/wiki/Interval_%28mathematics%29">interval
     * </a> where the end points are inclusive.
     *
     * @author Ciaran O'Reilly
     */
    public class MixedRadixInterval 
    {
        private const long MAX_LONG = long.MaxValue;
        private const long TWO = 2L;
        //
        private int[] radices;
        private int[] start;
        private int[] end;
        private long[] cachedRadixValues = null;
        private long leftEndPoint = 0L;
        private long rightEndPoint = 0L;
        private long maxPossibleValue = 0L;
        private long _size = 1L;

        public MixedRadixInterval(int[] radices)
            : this(radices, first(radices), last(radices))
        { }

        public MixedRadixInterval(int[] radices, int[] leftEndPointNumeralValues, int[] rightEndPointNumeralValues)
        {
            this.radices = new int[radices.Length];
            System.Array.Copy(radices, 0, this.radices, 0, radices.Length);

            this.start = new int[leftEndPointNumeralValues.Length];
            System.Array.Copy(leftEndPointNumeralValues, 0, this.start, 0, leftEndPointNumeralValues.Length);

            this.end = new int[rightEndPointNumeralValues.Length];
            System.Array.Copy(rightEndPointNumeralValues, 0, this.end, 0, rightEndPointNumeralValues.Length);

            initialize();
        }

        public static int[] first(int[] radices)
        {
            return new int[radices.Length];
        }

        public static int[] last(int[] radices)
        {
            int[] last = new int[radices.Length];
            for (int i = 0; i < radices.Length; i++)
            {
                last[i] = radices[i] - 1;
            }
            return last;
        }

        public long getLeftEndPointValue()
        {
            return leftEndPoint;
        }

        public long getRightEndPointValue()
        {
            return rightEndPoint;
        }

        public long size()
        {
            return _size;
        }

        public long getMinPossibleValue()
        {
            return 0L;
        }

        public long getMaxPossibleValue()
        {
            return maxPossibleValue;
        }

        /**
         * Returns the value of the mixed radix interval given the specified array
         * of numerals.
         *
         * @return the value of the numerals at within the interval.
         *
         * @throws IllegalArgumentException
         *             if any of the specified numerals is less than zero, or if any
         *             of the specified numerals is greater than or equal to it's
         *             corresponding radix.
         */
        public long getValueFor(int[] numeralValues)
        {
            if (numeralValues.Length != radices.Length)
            {
                throw new IllegalArgumentException("Radix values not same length as Radices.");
            }

            long cvalue = 0L;
            long mvalue = 1L;
            for (int i = numeralValues.Length - 1; i >= 0; i--)
            {
                if (numeralValues[i] < 0 || numeralValues[i] >= radices[i])
                {
                    throw new IllegalArgumentException(
                            "Radix value " + i + " is out of range for radix at this position, which is " + radices[i]);
                }
                if (i != numeralValues.Length - 1)
                {
                    mvalue *= cachedRadixValues[radices[i + 1]];
                }
                cvalue += mvalue * cachedRadixValues[numeralValues[i]];
            }
            return cvalue;
        }

        public int[] getNumeralsFor(long value)
        {
            if (getMinPossibleValue().CompareTo(value) > 0 || getMaxPossibleValue().CompareTo(value) < 0)
            {
                throw new IllegalArgumentException("The value of " + value
                        + " is outside of the possible values that can be represented by this interval's radix values ["
                        + getMinPossibleValue() + ", " + getMaxPossibleValue() + "]");
            }

            int[] numerals = new int[radices.Length];
            long quotient = value;
            for (int i = radices.Length - 1; i >= 0; i--)
            {
                if (!quotient.Equals(0L))
                {
                    numerals[i] = (int)(quotient % cachedRadixValues[radices[i]]);
                    quotient /= cachedRadixValues[radices[i]];
                }
                else
                {
                    // We are done, as the remaining numerals already have there
                    // values defaulted to 0 on construction.
                    break;
                }
            }
            return numerals;
        }
         
        public override string ToString()
        {
            return "[" + getLeftEndPointValue() + ", " + getRightEndPointValue() + "]";
        }
         
        private void initialize()
        {
            if (radices.Length != start.Length || start.Length != end.Length)
            {
                throw new IllegalArgumentException("Lengths of array arguments must all be the same.");
            }
            if (0 == radices.Length)
            {
                throw new IllegalArgumentException("At least 1 radix must be defined.");
            }
            for (int i = 0; i < radices.Length; i++)
            {
                if (radices[i] < 1)
                {
                    throw new IllegalArgumentException("Invalid radix, must be >= 1");
                }
                if (start[i] < 0 || start[i] >= radices[i])
                {
                    throw new IllegalArgumentException("Start numeral value as position " + i + " must be >=0 and < " + radices.Length);
                }
                if (end[i] < 0 || end[i] >= radices[i])
                {
                    throw new IllegalArgumentException("End numeral value as position " + i + " must be >=0 and < " + radices.Length);
                }
            }

            // Cache the possible radix values
            // so we don't have to create BigIntegers
            // for the same set of values multiple times
            int maxRadixValue = radices.Max();
            cachedRadixValues = new long[maxRadixValue + 1];
            for (int i = 0; i <= maxRadixValue; i++)
            {
                cachedRadixValues[i] = (long)i;
            }

            // Now get the min, max, and size information
            this.leftEndPoint = getValueFor(start);
            this.rightEndPoint = getValueFor(end);
            this.maxPossibleValue = getValueFor(last(radices));
            this._size = 1L + rightEndPoint - leftEndPoint;
            if (signum(_size) <= 0)
            {
                throw new IllegalArgumentException("The start numerals have a value greater than the end numeral values");
            }
        }

        public static int signum(long number)
        {
            if (0 == number)
            {
                return 0;
            }
            else if (number < 0)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        } 
    }
}