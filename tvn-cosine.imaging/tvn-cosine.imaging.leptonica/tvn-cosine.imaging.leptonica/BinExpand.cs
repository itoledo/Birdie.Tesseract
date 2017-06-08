using System;
using System.Runtime.InteropServices;

namespace Leptonica
{
    /// <summary>
    /// Replicated expansion(integer scaling)
    /// PIX     * pixExpandBinaryReplicate()
    ///
    /// Special case: power of 2 replicated expansion
    /// PIX     * pixExpandBinaryPower2()
    /// </summary>
    public static class BinExpand
    {
        /// <summary>
        /// pixExpandBinaryReplicate()
        /// </summary>
        /// <param name="pixs">pixs 1 bpp</param>
        /// <param name="xfact">integer scale factor for horiz. replicative expansion</param>
        /// <param name="yfact">integer scale factor for vertical replicative expansion</param>
        /// <returns>pixd scaled up, or NULL on error</returns>
        public static Pix pixExpandBinaryReplicate(this Pix pixs, int xfact, int yfact)
        {
            if (null == pixs)
            {
                return null;
            }

            var pointer = Native.DllImports.pixExpandBinaryReplicate((HandleRef)pixs, xfact, yfact);

            if (IntPtr.Zero != pointer)
            {
                return new Pix(pointer);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// pixExpandBinaryPower2()
        /// </summary>
        /// <param name="pixs">pixs 1 bpp</param>
        /// <param name="factor">factor expansion factor: 1, 2, 4, 8, 16</param>
        /// <returns>pixd expanded 1 bpp by replication, or NULL on error</returns>
        public static Pix pixExpandBinaryPower2(this Pix pixs, int factor)
        {
            if (null == pixs)
            {
                return null;
            }

            var pointer = Native.DllImports.pixExpandBinaryPower2((HandleRef)pixs, factor);

            if (IntPtr.Zero != pointer)
            {
                return new Pix(pointer);
            }
            else
            {
                return null;
            }
        }
    }
}
