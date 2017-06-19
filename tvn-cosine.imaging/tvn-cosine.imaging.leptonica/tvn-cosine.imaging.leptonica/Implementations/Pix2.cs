using System;
using System.Runtime.InteropServices;

namespace Leptonica
{
    public static class Pix2
    {
        // Pixel poking
        public static int pixGetPixel(this Pix pix, int x, int y, out uint pval)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixGetPixel((HandleRef)pix, x, y, out pval);
        }

        public static int pixSetPixel(this Pix pix, int x, int y, uint val)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixSetPixel((HandleRef)pix, x, y, val);
        }

        public static int pixGetRGBPixel(this Pix pix, int x, int y, out int prval, out int pgval, out int pbval)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixGetRGBPixel((HandleRef)pix, x, y, out prval, out pgval, out pbval);
        }

        public static int pixSetRGBPixel(this Pix pix, int x, int y, int rval, int gval, int bval)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixSetRGBPixel((HandleRef)pix, x, y, rval, gval, bval);
        }

        public static int pixGetRandomPixel(this Pix pix, out uint pval, out int px, out int py)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixGetRandomPixel((HandleRef)pix, out pval, out px, out py);
        }

        public static int pixClearPixel(this Pix pix, int x, int y)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixClearPixel((HandleRef)pix, x, y);
        }

        public static int pixFlipPixel(this Pix pix, int x, int y)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixFlipPixel((HandleRef)pix, x, y);
        }

        public static void setPixelLow(IntPtr line, int x, int depth, uint val)
        {
            if (IntPtr.Zero == line)
            {
                throw new ArgumentNullException("line cannot be null");
            }

            Native.DllImports.setPixelLow(line, x, depth, val);
        }

        // Find black or white value
        public static int pixGetBlackOrWhiteVal(this Pix pixs, int op, out uint pval)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixGetBlackOrWhiteVal((HandleRef)pixs, op, out pval);
        }

        // Full image clear/set/set-to-arbitrary-value
        public static int pixClearAll(this Pix pix)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixClearAll((HandleRef)pix);
        }

        public static int pixSetAll(this Pix pix)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixSetAll((HandleRef)pix);
        }

        public static int pixSetAllGray(this Pix pix, int grayval)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixSetAllGray((HandleRef)pix, grayval);
        }

        public static int pixSetAllArbitrary(this Pix pix, uint val)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixSetAllArbitrary((HandleRef)pix, val);
        }

        public static int pixSetBlackOrWhite(this Pix pixs, int op)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            return Native.DllImports.pixSetBlackOrWhite((HandleRef)pixs, op);
        }

        public static int pixSetComponentArbitrary(this Pix pix, int comp, int val)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixSetComponentArbitrary((HandleRef)pix, comp, val);
        }

        // Rectangular region clear/set/set-to-arbitrary-value/blend
        public static int pixClearInRect(this Pix pix, Box box)
        {
            if (null == pix
             || null == box)
            {
                throw new ArgumentNullException("pix, box cannot be null");
            }

            return Native.DllImports.pixClearInRect((HandleRef)pix, (HandleRef)box);
        }

        public static int pixSetInRect(this Pix pix, Box box)
        {
            if (null == pix
             || null == box)
            {
                throw new ArgumentNullException("pix, box cannot be null");
            }

            return Native.DllImports.pixSetInRect((HandleRef)pix, (HandleRef)box);
        }

        public static int pixSetInRectArbitrary(this Pix pix, Box box, uint val)
        {
            if (null == pix
             || null == box)
            {
                throw new ArgumentNullException("pix, box cannot be null");
            }

            return Native.DllImports.pixSetInRectArbitrary((HandleRef)pix, (HandleRef)box, val);
        }

        public static int pixBlendInRect(this Pix pixs, Box box, uint val, float fract)
        {
            if (null == pixs
             || null == box)
            {
                throw new ArgumentNullException("pixs, box cannot be null");
            }

            return Native.DllImports.pixBlendInRect((HandleRef)pixs, (HandleRef)box, val, fract);
        }

        // Set pad bits
        public static int pixSetPadBits(this Pix pix, int val)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixSetPadBits((HandleRef)pix, val);
        }

        public static int pixSetPadBitsBand(this Pix pix, int by, int bh, int val)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixSetPadBitsBand((HandleRef)pix, by, bh, val);
        }

        // Assign border pixels
        public static int pixSetOrClearBorder(this Pix pixs, int left, int right, int top, int bot, int op)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            return Native.DllImports.pixSetOrClearBorder((HandleRef)pixs, left, right, top, bot, op);
        }

        public static int pixSetBorderVal(this Pix pixs, int left, int right, int top, int bot, uint val)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            return Native.DllImports.pixSetBorderVal((HandleRef)pixs, left, right, top, bot, val);
        }

        public static int pixSetBorderRingVal(this Pix pixs, int dist, uint val)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            return Native.DllImports.pixSetBorderRingVal((HandleRef)pixs, dist, val);
        }

        public static int pixSetMirroredBorder(this Pix pixs, int left, int right, int top, int bot)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            return Native.DllImports.pixSetMirroredBorder((HandleRef)pixs, left, right, top, bot);
        }

        public static Pix pixCopyBorder(this Pix pixd, Pix pixs, int left, int right, int top, int bot)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            var pointer = Native.DllImports.pixCopyBorder((HandleRef)pixd, (HandleRef)pixs, left, right, top, bot);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        // Add and remove border
        public static Pix pixAddBorder(this Pix pixs, int npix, uint val)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            var pointer = Native.DllImports.pixAddBorder((HandleRef)pixs, npix, val);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        public static Pix pixAddBlackOrWhiteBorder(this Pix pixs, int left, int right, int top, int bot, int op)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            var pointer = Native.DllImports.pixAddBlackOrWhiteBorder((HandleRef)pixs, left, right, top, bot, op);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        public static Pix pixAddBorderGeneral(this Pix pixs, int left, int right, int top, int bot, uint val)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            var pointer = Native.DllImports.pixAddBorderGeneral((HandleRef)pixs, left, right, top, bot, val);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        public static Pix pixRemoveBorder(this Pix pixs, int npix)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            var pointer = Native.DllImports.pixRemoveBorder((HandleRef)pixs, npix);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        public static Pix pixRemoveBorderGeneral(this Pix pixs, int left, int right, int top, int bot)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            var pointer = Native.DllImports.pixRemoveBorderGeneral((HandleRef)pixs, left, right, top, bot);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        public static Pix pixRemoveBorderToSize(this Pix pixs, int wd, int hd)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            var pointer = Native.DllImports.pixRemoveBorderToSize((HandleRef)pixs, wd, hd);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        public static Pix pixAddMirroredBorder(this Pix pixs, int left, int right, int top, int bot)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            var pointer = Native.DllImports.pixAddMirroredBorder((HandleRef)pixs, left, right, top, bot);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        public static Pix pixAddRepeatedBorder(this Pix pixs, int left, int right, int top, int bot)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            var pointer = Native.DllImports.pixAddRepeatedBorder((HandleRef)pixs, left, right, top, bot);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        public static Pix pixAddMixedBorder(this Pix pixs, int left, int right, int top, int bot)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            var pointer = Native.DllImports.pixAddMixedBorder((HandleRef)pixs, left, right, top, bot);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        public static Pix pixAddContinuedBorder(this Pix pixs, int left, int right, int top, int bot)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            var pointer = Native.DllImports.pixAddContinuedBorder((HandleRef)pixs, left, right, top, bot);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        // Helper functions using alpha
        public static int pixShiftAndTransferAlpha(this Pix pixd, Pix pixs, float shiftx, float shifty)
        {
            if (null == pixd
             || null == pixs)
            {
                throw new ArgumentNullException("pixd, pixs cannot be null");
            }

            return Native.DllImports.pixShiftAndTransferAlpha((HandleRef)pixd, (HandleRef)pixs, shiftx, shifty);
        }

        public static Pix pixDisplayLayersRGBA(this Pix pixs, uint val, int maxw)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            var pointer = Native.DllImports.pixDisplayLayersRGBA((HandleRef)pixs, val, maxw);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        // Color sample setting and extraction
        public static Pix pixCreateRGBImage(this Pix pixr, Pix pixg, Pix pixb)
        {
            if (null == pixr
             || null == pixg
             || null == pixb)
            {
                throw new ArgumentNullException("pixr, pixg, pixb cannot be null");
            }

            var pointer = Native.DllImports.pixCreateRGBImage((HandleRef)pixr, (HandleRef)pixg, (HandleRef)pixb);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        public static Pix pixGetRGBComponent(this Pix pixs, int comp)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            var pointer = Native.DllImports.pixGetRGBComponent((HandleRef)pixs, comp);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        public static int pixSetRGBComponent(this Pix pixd, Pix pixs, int comp)
        {
            if (null == pixd
             || null == pixs)
            {
                throw new ArgumentNullException("pixd, pixs cannot be null");
            }

            return Native.DllImports.pixSetRGBComponent((HandleRef)pixd, (HandleRef)pixs, comp);
        }

        public static Pix pixGetRGBComponentCmap(this Pix pixs, int comp)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            var pointer = Native.DllImports.pixGetRGBComponentCmap((HandleRef)pixs, comp);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }
        public static int pixCopyRGBComponent(this Pix pixd, Pix pixs, int comp)
        {
            if (null == pixd
             || null == pixs)
            {
                throw new ArgumentNullException("pixd, pixs cannot be null");
            }

            return Native.DllImports.pixCopyRGBComponent((HandleRef)pixd, (HandleRef)pixs, comp);
        }

        public static int composeRGBPixel(int rval, int gval, int bval, out uint ppixel)
        {
            return Native.DllImports.composeRGBPixel(rval, gval, bval, out ppixel);
        }

        public static int composeRGBAPixel(int rval, int gval, int bval, int aval, out uint ppixel)
        {
            return Native.DllImports.composeRGBAPixel(rval, gval, bval, aval, out ppixel);
        }

        public static void extractRGBValues(uint pixel, out int prval, out int pgval, out int pbval)
        {
            Native.DllImports.extractRGBValues(pixel, out prval, out pgval, out pbval);
        }

        public static void extractRGBAValues(uint pixel, out int prval, out int pgval, out int pbval, out int paval)
        {
            Native.DllImports.extractRGBAValues(pixel, out prval, out pgval, out pbval, out paval);
        }

        public static int extractMinMaxComponent(uint pixel, int type)
        {
            return Native.DllImports.extractMinMaxComponent(pixel, type);
        }

        public static int pixGetRGBLine(this Pix pixs, int row, IntPtr bufr, IntPtr bufg, IntPtr bufb)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            return Native.DllImports.pixGetRGBLine((HandleRef)pixs, row, bufr, bufg, bufb);
        }

        // Conversion between big and little endians
        public static Pix pixEndianByteSwapNew(this Pix pixs)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            var pointer = Native.DllImports.pixEndianByteSwapNew((HandleRef)pixs);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        public static int pixEndianByteSwap(this Pix pixs)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            return Native.DllImports.pixEndianByteSwap((HandleRef)pixs);
        }

        public static int lineEndianByteSwap(IntPtr datad, IntPtr datas, int wpl)
        {
            if (IntPtr.Zero == datad)
            {
                throw new ArgumentNullException("datad cannot be null");
            }

            return Native.DllImports.lineEndianByteSwap(datad, datas, wpl);
        }

        public static Pix pixEndianTwoByteSwapNew(this Pix pixs)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            var pointer = Native.DllImports.pixEndianTwoByteSwapNew((HandleRef)pixs);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        public static int pixEndianTwoByteSwap(this Pix pixs)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            return Native.DllImports.pixEndianTwoByteSwap((HandleRef)pixs);
        }


        // Extract raster data as binary string
        public static int pixGetRasterData(this Pix pixs, out IntPtr pdata, out IntPtr pnbytes)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            return Native.DllImports.pixGetRasterData((HandleRef)pixs, out pdata, out pnbytes);
        }

        // Test alpha component opaqueness
        public static int pixAlphaIsOpaque(this Pix pix, out int popaque)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixAlphaIsOpaque((HandleRef)pix, out popaque);
        }

        // Setup helpers for 8 bpp byte processing
        public static IntPtr pixSetupByteProcessing(this Pix pix, out int pw, out int ph)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixSetupByteProcessing((HandleRef)pix, out pw, out ph);
        }

        public static int pixCleanupByteProcessing(this Pix pix, out IntPtr lineptrs)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixCleanupByteProcessing((HandleRef)pix, out lineptrs);
        }

        // Setting parameters for antialias masking with alpha transforms
        public static void l_setAlphaMaskBorder(float val1, float val2)
        {
            Native.DllImports.l_setAlphaMaskBorder(val1, val2);
        }
    }
}
