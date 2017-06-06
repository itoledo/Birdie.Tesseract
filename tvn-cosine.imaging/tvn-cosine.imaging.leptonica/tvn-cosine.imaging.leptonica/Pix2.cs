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

        /// <summary>
        /// <pre>
        /// Notes:
        ///      (1) Side effect.For a colormapped image, if the requested
        ///          color is not present and there is room to add it in the cmap,
        /// it is added and the new index is returned.If there is no room,
        ///          the index of the closest color in intensity is returned. 
        ///</pre>
        /// </summary>
        /// <param name="pix">pixs all depths; cmap ok</param>
        /// <param name="op">op L_GET_BLACK_VAL, L_GET_WHITE_VAL</param>
        /// <param name="pval">pval pixel value</param>
        /// <returns>0 if OK; 1 on error</returns>
        public static bool pixGetBlackOrWhiteVal(Pix pix, BlackOrWhiteFlags op, out uint pval)
        {
            //ensure pix is not null;
            if (null == pix)
            {
                pval = 0;
                return false;
            }

            return Native.DllImports.pixGetBlackOrWhiteVal((HandleRef)pix, op, out pval) == 0;
        }

        /// <summary>
        ///  <pre>
        ///  Notes:
        ///       (1) Clears all data to 0.  For 1 bpp, this is white; for grayscale
        ///  or color, this is black.
        /// 
        ///      (2) Caution: for colormapped pix, this sets the color to the first
        ///           one in the colormap.Be sure that this is the intended color!
        ///  </pre>
        /// </summary>
        /// <param name="pix">pix all depths; use cmapped with caution</param>
        /// <returns>0 if OK, 1 on error</returns>
        public static bool pixClearAll(Pix pix)
        {
            //ensure pix is not null;
            if (null == pix)
            {
                return false;
            }

            return Native.DllImports.pixClearAll((HandleRef)pix) == 0;
        }

        /// <summary>
        /// <pre>
        /// Notes:
        ///      (1) Sets all data to 1.  For 1 bpp, this is black; for grayscale
        /// or color, this is white.
        ///
        ///     (2) Caution: for colormapped pix, this sets the pixel value to the
        ///          maximum value supported by the colormap: 2^d - 1.  However, this
        ///          color may not be defined, because the colormap may not be full.
        /// </pre>
        /// </summary>
        /// <param name="pix">pix all depths; use cmapped with caution</param>
        /// <returns>0 if OK, 1 on error</returns>
        public static bool pixSetAll(Pix pix)
        {
            //ensure pix is not null;
            if (null == pix)
            {
                return false;
            }

            return Native.DllImports.pixSetAll((HandleRef)pix) == 0;
        }

        /// <summary>
        ///  Notes:
        ///       (1) N.B.For all images, %grayval == 0 represents black and
        ///          %grayval == 255 represents white.
        ///       (2) For depth 8, we do our best to approximate the gray level.
        /// 
        /// For 1 bpp images, any %grayval 128 is black; >= 128 is white.
        /// 
        /// For 32 bpp images, each r,g,b component is set to %grayval,
        ///           and the alpha component is preserved.
        ///       (3) If pix is colormapped, it adds the gray value, replicated in
        ///           all components, to the colormap if it's not there and there
        ///           is room.If the colormap is full, it finds the closest color in
        ///           L2 distance of components.This index is written to all pixels.
        /// </summary>
        /// <param name="pix">pix all depths, cmap ok</param>
        /// <param name="grayval">grayval in range 0 ... 255</param>
        /// <returns>0 if OK; 1 on error</returns>
        public static bool pixSetAllGray(Pix pix, int grayval)
        {
            //ensure pix is not null;
            if (null == pix)
            {
                return false;
            }

            return Native.DllImports.pixSetAllGray((HandleRef)pix, grayval) == 0;
        }

        /// <summary>
        ///  <pre>
        ///  Notes:
        ///       (1) Caution 1!  For colormapped pix, %val is used as an index
        ///           into a colormap.Be sure that index refers to the intended color.
        /// 
        /// If the color is not in the colormap, you should first add it
        ///           and then call this function.
        ///       (2) Caution 2!  For 32 bpp pix, the interpretation of the LSB
        ///           of %val depends on whether spp == 3 (RGB) or spp == 4 (RGBA).
        ///           For RGB, the LSB is ignored in image transformations.
        ///           For RGBA, the LSB is interpreted as the alpha (transparency)
        ///           component; full transparency has alpha == 0x0, whereas
        ///           full opacity has alpha = 0xff.  An RGBA image with full
        ///           opacity behaves like an RGB image. 
        ///       (3) As an example of (2), suppose you want to initialize a 32 bpp
        ///           pix with partial opacity, say 0xee337788.  If the pix is 3 spp,
        ///           the 0x88 alpha component will be ignored and may be changed
        ///           in subsequent processing.  However, if the pix is 4 spp, the
        ///           alpha component will be retained and used. The function
        ///           pixCreate(w, h, 32) makes an RGB image by default, and
        ///           pixSetSpp(pix, 4) can be used to promote an RGB image to RGBA.
        ///  </pre>        
        ///  </summary>
        /// <param name="pix">pix all depths; use cmapped with caution</param>
        /// <param name="val">val  value to set all pixels</param>
        /// <returns>0 if OK; 1 on error</returns>
        public static bool pixSetAllArbitrary(Pix pix, uint val)
        {
            //ensure pix is not null;
            if (null == pix)
            {
                return false;
            }

            return Native.DllImports.pixSetAllArbitrary((HandleRef)pix, val) == 0;
        }

        /// <summary>
        ///       (1) Function for setting all pixels in an image to either black
        ///  or white.
        /// 
        ///      (2) If pixs is colormapped, it adds black or white to the
        /// colormap if it's not there and there is room.  If the colormap
        ///           is full, it finds the closest color in intensity.
        ///  This index is written to all pixels.
        /// </summary>
        /// <param name="pixs">pixs all depths; cmap ok</param>
        /// <param name="op"> L_SET_BLACK, L_SET_WHITE</param>
        /// <returns>0 if OK; 1 on error</returns>
        public static bool pixSetBlackOrWhite(Pix pixs, BlackOrWhiteFlags op)
        {
            //ensure pix is not null;
            if (null == pixs)
            {
                return false;
            }

            return Native.DllImports.pixSetBlackOrWhite((HandleRef)pixs, op) == 0;
        }

        /// <summary>
        /// (1) For example, this can be used to set the alpha component to opaque: pixSetComponentArbitrary(pix, L_ALPHA_CHANNEL, 255)
        /// </summary>
        /// <param name="pix">pix 32 bpp</param>
        /// <param name="comp">comp COLOR_RED, COLOR_GREEN, COLOR_BLUE, L_ALPHA_CHANNEL</param>
        /// <param name="val">val  value to set this component</param>
        /// <returns>0 if OK; 1 on error</returns>
        public static bool pixSetComponentArbitrary(Pix pix, ColorFlags comp, int val)
        {
            //ensure pix is not null;
            if (null == pix)
            {
                return false;
            }

            return Native.DllImports.pixSetComponentArbitrary((HandleRef)pix, comp, val) == 0;
        }

        /// <summary>
        ///     (1) Clears all data in rect to 0.  For 1 bpp, this is white;
        ///         for grayscale or color, this is black.
        ///     (2) Caution: for colormapped pix, this sets the color to the first
        ///         one in the colormap.Be sure that this is the intended color!
        /// </summary>
        /// <param name="pix">pix all depths; can be cmapped</param>
        /// <param name="box">box in which all pixels will be cleared</param>
        /// <returns>0 if OK, 1 on error</returns>
        public static bool pixClearInRect(Pix pix, Box box)
        {
            //ensure pix is not null;
            if (null == pix)
            {
                return false;
            }
            if (null == box)
            {
                return false;
            }

            return Native.DllImports.pixClearInRect((HandleRef)pix, (HandleRef)box) == 0;
        }

        /// <summary>
        ///      (1) Sets all data in rect to 1.  For 1 bpp, this is black;
        ///          for grayscale or color, this is white.
        ///      (2) Caution: for colormapped pix, this sets the pixel value to the
        ///          maximum value supported by the colormap: 2^d - 1.  However, this
        ///          color may not be defined, because the colormap may not be full.
        /// </summary>
        /// <param name="pix">pix all depths, can be cmapped</param>
        /// <param name="box">box in which all pixels will be set</param>
        /// <returns>0 if OK, 1 on error</returns>
        public static bool pixSetInRect(Pix pix, Box box)
        {
            //ensure pix is not null;
            if (null == pix)
            {
                return false;
            }
            if (null == box)
            {
                return false;
            }

            return Native.DllImports.pixSetInRect((HandleRef)pix, (HandleRef)box) == 0;
        }

        /// <summary>
        ///* Notes:
        ///*      (1) For colormapped pix, be sure the value is the intended
        ///*          one in the colormap.
        ///*      (2) Caution: for colormapped pix, this sets each pixel in the
        ///* rect to the color at the index equal to val.Be sure that
        /// this index exists in the colormap and that it is the intended one!
        /// </summary>
        /// <param name="pix">pix all depths; can be cmapped</param>
        /// <param name="box">box in which all pixels will be set to val</param>
        /// <param name="val">val  value to set all pixels</param>
        /// <returns>0 if OK; 1 on error</returns>
        public static bool pixSetInRectArbitrary(Pix pix, Box box, uint val)
        {
            //ensure pix is not null;
            if (null == pix)
            {
                return false;
            }
            if (null == box)
            {
                return false;
            }

            return Native.DllImports.pixSetInRectArbitrary((HandleRef)pix, (HandleRef)box, val) == 0;
        }

        /// <summary>
        ///      (1) This is an in-place function.It blends the input color %val
        /// with the pixels in pixs in the specified rectangle.
        /// If no rectangle is specified, it blends over the entire image.
        /// </summary>
        /// <param name="pixs">pixs 32 bpp rgb</param>
        /// <param name="box">box [optional] in which all pixels will be blended</param>
        /// <param name="val">val  blend value; 0xrrggbb00</param>
        /// <param name="fract">fract fraction of color to be blended with each pixel in pixs</param>
        /// <returns>0 if OK; 1 on error</returns>
        public static bool pixBlendInRect(Pix pixs, Box box, uint val, float fract)
        {
            //ensure pix is not null;
            if (null == pixs)
            {
                return false;
            }
            if (null == box)
            {
                return false;
            }

            return Native.DllImports.pixBlendInRect((HandleRef)pixs, (HandleRef)box, val, fract) == 0;
        }

        /// <summary>
        ///       (1) The pad bits are the bits that expand each scanline to a
        ///           multiple of 32 bits.They are usually not used in
        ///           image processing operations.When boundary conditions
        /// are important, as in seedfill, they must be set properly.
        /// 
        ///      (2) This sets the value of the pad bits(if any) in the last
        ///           32-bit word in each scanline.
        ///       (3) For 32 bpp pix, there are no pad bits, so this is a no-op.
        ///       (4) When writing formatted output, such as tiff, png or jpeg,
        ///           the pad bits have no effect on the raster image that is
        ///           generated by reading back from the file.However, in some
        /// cases, the compressed file itself will depend on the pad
        ///           bits.This is seen, for example, in Windows with 2 and 4 bpp
        /// tiff-compressed images that have pad bits on each scanline.
        /// 
        /// It is sometimes convenient to use a golden file with a
        /// byte-by-byte check to verify invariance.Consequently,
        ///           and because setting the pad bits is cheap, the pad bits are
        ///           set to 0 before writing these compressed files.
        /// </summary>
        /// <param name="pix">pix 1, 2, 4, 8, 16, 32 bpp</param>
        /// <param name="val">val  0 or 1</param>
        /// <returns>0 if OK; 1 on error</returns>
        public static bool pixSetPadBits(Pix pix, int val)
        {
            //ensure pix is not null;
            if (null == pix)
            {
                return false;
            }

            return Native.DllImports.pixSetPadBits((HandleRef)pix, val) == 0;
        }

        /// <summary>
        ///  Notes:
        ///       (1) The pad bits are the bits that expand each scanline to a
        ///           multiple of 32 bits.They are usually not used in
        ///           image processing operations.When boundary conditions
        /// are important, as in seedfill, they must be set properly.
        /// 
        ///      (2) This sets the value of the pad bits(if any) in the last
        ///           32-bit word in each scanline, within the specified
        ///           band of raster lines.
        ///       (3) For 32 bpp pix, there are no pad bits, so this is a no-op.
        /// </summary>
        /// <param name="pix">pix 1, 2, 4, 8, 16, 32 bpp</param>
        /// <param name="by">by  starting y value of band</param>
        /// <param name="bh"> bh  height of band</param>
        /// <param name="val">val  0 or 1</param>
        /// <returns> 0 if OK; 1 on error</returns>
        public static bool pixSetPadBitsBand(Pix pix, int by, int bh, int val)
        {
            //ensure pix is not null;
            if (null == pix)
            {
                return false;
            }

            return Native.DllImports.pixSetPadBitsBand((HandleRef)pix, by, bh, val) == 0;
        }



        //public static bool pixSetOrClearBorder(Pix pixs, int left, int right, int top, int bot, int op);
        //public static bool pixSetBorderVal(Pix pixs, int left, int right, int top, int bot, uint val);
        //public static bool pixSetBorderRingVal(Pix pixs, int dist, uint val);
        //public static bool pixSetMirroredBorder(Pix pixs, int left, int right, int top, int bot);
        //public static Pix pixCopyBorder(Pix pixd, Pix pixs, int left, int right, int top, int bot);

        /*   
 // 
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
