using System;
using System.Runtime.InteropServices;

namespace Leptonica
{
    /// <summary>
    /// Pix2.c
    /// </summary>
    public static class Pix2
    {
        /// <summary>
        /// (1) See pixGetBlackOrWhiteVal() for values of black and white pixels.
        /// </summary>
        /// <param name="source">pixs all depths; colormap ok</param>
        /// <param name="width">npix number of pixels to be added to each side</param>
        /// <param name="borderColor">val  value of added border pixels</param>
        /// <returns>pixd with the added exterior pixels, or NULL on error</returns>
        public static Pix pixAddBorder(Pix source, int width, Tvn.Cosine.Imaging.Color borderColor)
        {
            //ensure pix is not null;
            if (source == null || borderColor == null || width < 1)
            {
                return null;
            }

            var pointer = Native.DllImports.pixAddBorder((HandleRef)source, width, borderColor.ToAbgrUint());
            if (pointer != IntPtr.Zero)
            {
                return new Pix(pointer);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        ///  <pre>
        ///  Notes:
        ///       (1) This returns the value in the data array.If the pix is
        ///           colormapped, it returns the colormap index, not the rgb value.
        ///       (2) Because of the function overhead and the parameter checking,
        ///           this is much slower than using the GET_DATA_*() macros directly.
        ///           Speed on a 1 Mpixel RGB image, using a 3 GHz machine:
        /// * pixGet/pixSet: ~25 Mpix/sec
        ///  GET_DATA/SET_DATA: ~350 MPix/sec
        /// If speed is important and you're doing random access into
        ///  the pix, use pixGetLinePtrs() and the array access macros.
        /// 
        ///      (3) If the point is outside the image, this returns an error (1),
        ///           with 0 in %pval.  To avoid spamming output, it fails silently.
        ///  </pre>
        /// </summary>
        /// <param name="pix">pix</param>
        /// <param name="x">x,y pixel coords</param>
        /// <param name="y">x,y pixel coords</param>
        /// <param name="pval">pval pixel value</param>
        /// <returns>0 if OK; 1 on error</returns>
        public static bool pixGetPixel(Pix pix, int x, int y, out int pval)
        {
            //ensure pix is not null;
            if (null == pix)
            {
                pval = 0;
                return false;
            }

            return Native.DllImports.pixGetPixel((HandleRef)pix, x, y, out pval) == 0;
        }

        /// <summary>
        ///  <pre>
        ///  Notes:
        ///       (1) Warning: the input value is not checked for overflow with respect
        ///  the the depth of %pix, and the sign bit(if any) is ignored.
        /// 
        ///  For d == 1, %val > 0 sets the bit on.
        ///           * For d == 2, 4, 8 and 16, %val is masked to the maximum allowable
        /// pixel value, and any (invalid) higher order bits are discarded.
        /// 
        ///      (2) See pixGetPixel() for information on performance.
        /// 
        /// </pre>
        /// </summary>
        /// <param name="pix">pix</param>
        /// <param name="x">x,y pixel coords</param>
        /// <param name="y">x,y pixel coords</param>
        /// <param name="pval">val value to be inserted</param>
        /// <returns>0 if OK; 1 on error</returns>
        public static bool pixSetPixel(Pix pix, int x, int y, uint pval)
        {
            //ensure pix is not null;
            if (null == pix)
            {
                return false;
            }

            return Native.DllImports.pixSetPixel((HandleRef)pix, x, y, pval) == 0;
        }

        /// <summary>
        ///  pixGetRGBPixel()
        /// </summary>
        /// <param name="pix">pix 32 bpp rgb, not colormapped</param>
        /// <param name="x">x,y pixel coords</param>
        /// <param name="y">x,y pixel coords</param>
        /// <param name="prval">prval [optional] red component</param>
        /// <param name="pgval">prval [optional] green component</param>
        /// <param name="pbval">prval [optional] blue component</param>
        /// <returns>0 if OK; 1 on error</returns>
        public static bool pixGetRGBPixel(Pix pix, int x, int y, out int prval, out int pgval, out int pbval)
        {
            //ensure pix is not null;
            if (null == pix)
            {
                prval = 0;
                pgval = 0;
                pbval = 0;
                return false;
            }

            return Native.DllImports.pixGetRGBPixel((HandleRef)pix, x, y, out prval, out pgval, out pbval) == 0;
        }

        /// <summary>
        ///  pixSetRGBPixel()
        /// </summary>
        /// <param name="pix">pix 32 bpp rgb, not colormapped</param>
        /// <param name="x">x,y pixel coords</param>
        /// <param name="y">x,y pixel coords</param>
        /// <param name="prval">prval [optional] red component</param>
        /// <param name="pgval">prval [optional] green component</param>
        /// <param name="pbval">prval [optional] blue component</param>
        /// <returns>0 if OK; 1 on error</returns>
        public static bool pixSetRGBPixel(Pix pix, int x, int y, int prval, int pgval, int pbval)
        {
            //ensure pix is not null;
            if (null == pix)
            {
                return false;
            }

            return Native.DllImports.pixSetRGBPixel((HandleRef)pix, x, y, prval, pgval, pbval) == 0;
        }

        /// <summary>
        /// (1) If the pix is colormapped, it returns the rgb value.
        /// </summary>
        /// <param name="pix">pix any depth; can be colormapped</param>
        /// <param name="pval">pval [optional] pixel value</param>
        /// <param name="px">px [optional] x coordinate chosen; can be null</param>
        /// <param name="py">py [optional] x coordinate chosen; can be null</param>
        /// <returns>0 if OK; 1 on error</returns>
        public static bool pixGetRandomPixel(Pix pix, out int pval, out int px, out int py)
        {
            //ensure pix is not null;
            if (null == pix)
            {
                pval = 0;
                px = 0;
                py = 0;
                return false;
            }

            return Native.DllImports.pixGetRandomPixel((HandleRef)pix, out pval, out px, out py) == 0;
        }

        /// <summary>
        /// pixClearPixel()
        /// </summary>
        /// <param name="pix">pix</param>
        /// <param name="x">x,y pixel coords</param>
        /// <param name="y">x,y pixel coords</param>
        /// <returns></returns>
        public static bool pixClearPixel(Pix pix, int x, int y)
        {
            //ensure pix is not null;
            if (null == pix)
            {
                return false;
            }

            return Native.DllImports.pixClearPixel((HandleRef)pix, x, y) == 0;
        }

        /// <summary>
        /// pixFlipPixel()
        /// </summary>
        /// <param name="pix">pix</param>
        /// <param name="x">x,y pixel coords</param>
        /// <param name="y">x,y pixel coords</param>
        /// <returns></returns>
        public static bool pixFlipPixel(Pix pix, int x, int y)
        {
            //ensure pix is not null;
            if (null == pix)
            {
                return false;
            }

            return Native.DllImports.pixFlipPixel((HandleRef)pix, x, y) == 0;
        }


        /*
 //Pixel poking       
 //           void        setPixelLow()
 //
 //      Find black or white value
 //           l_int32     pixGetBlackOrWhiteVal()
 //
 //      Full image clear/set/set-to-arbitrary-value
 //           l_int32     pixClearAll()
 //           l_int32     pixSetAll()
 //           l_int32     pixSetAllGray()
 //           l_int32     pixSetAllArbitrary()
 //           l_int32     pixSetBlackOrWhite()
 //           l_int32     pixSetComponentArbitrary()
 //
 //      Rectangular region clear/set/set-to-arbitrary-value/blend
 //           l_int32     pixClearInRect()
 //           l_int32     pixSetInRect()
 //           l_int32     pixSetInRectArbitrary()
 //           l_int32     pixBlendInRect()
 //
 //      Set pad bits
 //           l_int32     pixSetPadBits()
 //           l_int32     pixSetPadBitsBand()
 //
 //      Assign border pixels
 //           l_int32     pixSetOrClearBorder()
 //           l_int32     pixSetBorderVal()
 //           l_int32     pixSetBorderRingVal()
 //           l_int32     pixSetMirroredBorder()
 //           PIX        *pixCopyBorder()
 //
 //      Add and remove border 
 //           PIX        *pixAddBlackOrWhiteBorder()
 //           PIX        *pixAddBorderGeneral()
 //           PIX        *pixRemoveBorder()
 //           PIX        *pixRemoveBorderGeneral()
 //           PIX        *pixRemoveBorderToSize()
 //           PIX        *pixAddMirroredBorder()
 //           PIX        *pixAddRepeatedBorder()
 //           PIX        *pixAddMixedBorder()
 //           PIX        *pixAddContinuedBorder()
 //
 //      Helper functions using alpha
 //           l_int32     pixShiftAndTransferAlpha()
 //           PIX        *pixDisplayLayersRGBA()
 //
 //      Color sample setting and extraction
 //           PIX        *pixCreateRGBImage()
 //           PIX        *pixGetRGBComponent()
 //           l_int32     pixSetRGBComponent()
 //           PIX        *pixGetRGBComponentCmap()
 //           l_int32     pixCopyRGBComponent()
 //           l_int32     composeRGBPixel()
 //           l_int32     composeRGBAPixel()
 //           void        extractRGBValues()
 //           void        extractRGBAValues()
 //           l_int32     extractMinMaxComponent()
 //           l_int32     pixGetRGBLine()
 //
 //      Conversion between big and little endians
 //           PIX        *pixEndianByteSwapNew()
 //           l_int32     pixEndianByteSwap()
 //           l_int32     lineEndianByteSwap()
 //           PIX        *pixEndianTwoByteSwapNew()
 //           l_int32     pixEndianTwoByteSwap()
 //
 //      Extract raster data as binary string
 //           l_int32     pixGetRasterData()
 //
 //      Test alpha component opaqueness
 //           l_int32     pixAlphaIsOpaque
 //
 //      Setup helpers for 8 bpp byte processing
 //           l_uint8   **pixSetupByteProcessing()
 //           l_int32     pixCleanupByteProcessing()
 //
 //      Setting parameters for antialias masking with alpha transforms
 //           void        l_setAlphaMaskBorder()
         * */
    }
}
