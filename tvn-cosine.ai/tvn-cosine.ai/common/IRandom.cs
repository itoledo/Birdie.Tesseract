namespace tvn.cosine.ai.common
{
    /// <summary>
    /// Represents a pseudo-random number generator, which is a device that produces
    /// a sequence of numbers that meet certain statistical requirements for randomness.
    /// </summary>
    public interface IRandom
    {
        /// <summary>
        /// Returns a random boolean.
        /// </summary>
        /// <returns>A random boolean.</returns>
        bool NextBoolean();

        /// <summary>
        /// Returns a non-negative random integer.
        /// </summary>
        /// <returns>A 32-bit signed integer that is greater than or equal to 0 and less than int.MAX.</returns>
        int Next();

        /// <summary>
        /// Returns a random integer that is within a specified range.
        /// </summary>
        /// <param name="minimumValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maximumValue">
        /// The exclusive upper bound of the random number to be generated.
        /// maximumValue must be greater than or equal to 0.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that is greater than or equal to minimumValue, and less than maxValue;
        /// that is, the range of return values ordinarily includes minimumValue but not maxValue. However,
        /// if maxValue equals 0, maxValue is returned.
        /// </returns>
        int Next(int minimumValue, int maximumValue);

        /// <summary>
        /// Returns a non-negative random integer that is less than the specified maximum.
        /// </summary>
        /// <param name="maximumValue">
        /// The exclusive upper bound of the random number to be generated.
        /// maximumValue must be greater than or equal to 0.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that is greater than or equal to 0, and less than maxValue;
        /// that is, the range of return values ordinarily includes 0 but not maxValue. However,
        /// if maxValue equals 0, maxValue is returned.
        /// </returns>
        int Next(int maximumValue);

        /// <summary>
        /// Returns a random floating-point number that is greater than or equal to 0.0, and less than 1.0.
        /// </summary>
        /// <returns>A double-precision floating point number that is greater than or equal to 0.0, and less than 1.0.</returns>
        double NextDouble();
    }
}
