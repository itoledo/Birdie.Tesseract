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
        public static bool pixSetBlackOrWhite(this Pix pixs, BlackOrWhiteFlags op)
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
        public static bool pixBlendInRect(this Pix pixs, Box box, uint val, float fract)
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

        /// <summary>
        /// Notes:
        ///      (1) The border region is defined to be the region in the
        /// image within a specific distance of each edge.Here, we
        ///allow the pixels within a specified distance of each
        ///          edge to be set independently.This either sets or
        ///          clears all pixels in the border region.
        ///      (2) For binary images, use PIX_SET for black and PIX_CLR for white.
        ///      (3) For grayscale or color images, use PIX_SET for white
        /// and PIX_CLR for black.
        /// </summary>
        /// <param name="pixs"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="top"></param>
        /// <param name="bot"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        public static bool pixSetOrClearBorder(this Pix pixs, int left, int right, int top, int bot, GraphicPixelSetting op)
        {
            //ensure pix is not null;
            if (null == pixs)
            {
                return false;
            }

            return Native.DllImports.pixSetOrClearBorder((HandleRef)pixs, left, right, top, bot, op) == 0;
        }

        /// <summary>
        /// <pre>
        /// Notes:
        ///      (1) The border region is defined to be the region in the
        /// image within a specific distance of each edge.Here, we
        /// allow the pixels within a specified distance of each
        ///          edge to be set independently.This sets the pixels
        ///          in the border region to the given input value.
        ///      (2) For efficiency, use pixSetOrClearBorder() if
        ///          you're setting the border to either black or white.
        ///      (3) If d != 32, the input value should be masked off
        ///          to the appropriate number of least significant bits.
        ///      (4) The code is easily generalized for 2 or 4 bpp.
        /// </pre>
        /// </summary>
        /// <param name="pixs">pixs 8, 16 or 32 bpp</param>
        /// <param name="left">left, right, top, bot amount to set</param>
        /// <param name="right">left, right, top, bot amount to set</param>
        /// <param name="top">left, right, top, bot amount to set</param>
        /// <param name="bot">left, right, top, bot amount to set</param>
        /// <param name="val"> val value to set at each border pixel</param>
        /// <returns>0 if OK; 1 on error</returns>
        public static bool pixSetBorderVal(this Pix pixs, int left, int right, int top, int bot, uint val)
        {
            //ensure pix is not null;
            if (null == pixs)
            {
                return false;
            }

            return Native.DllImports.pixSetBorderVal((HandleRef)pixs, left, right, top, bot, val) == 0;
        }

        /// <summary>
        ///      (1) The rings are single-pixel-wide rectangular sets of
        ///          pixels at a given distance from the edge of the pix.
        /// This sets all pixels in a given ring to a value.
        /// </summary>
        /// <param name="pixs">pixs any depth; cmap OK</param>
        /// <param name="dist">dist distance from outside; must be > 0; first ring is 1</param>
        /// <param name="val">val value to set at each border pixel</param>
        /// <returns></returns>
        public static bool pixSetBorderRingVal(this Pix pixs, int dist, uint val)
        {
            //ensure pix is not null;
            if (null == pixs)
            {
                return false;
            }

            return Native.DllImports.pixSetBorderRingVal((HandleRef)pixs, dist, val) == 0;
        }

        /// <summary>
        /// Notes:
        ///      (1) This applies what is effectively mirror boundary conditions
        ///          to a border region in the image.It is in-place.
        ///      (2) This is useful for setting pixels near the border to a
        /// value representative of the near pixels to the interior.
        ///      (3) The general pixRasterop() is used for an in-place operation here
        ///because there is no overlap between the src and dest rectangles.
        /// </summary>
        /// <param name="pixs">pixs all depths; colormap ok</param>
        /// <param name="left">left, right, top, bot number of pixels to set</param>
        /// <param name="right">left, right, top, bot number of pixels to set</param>
        /// <param name="top">left, right, top, bot number of pixels to set</param>
        /// <param name="bot">left, right, top, bot number of pixels to set</param>
        /// <returns></returns>
        public static bool pixSetMirroredBorder(this Pix pixs, int left, int right, int top, int bot)
        {
            //ensure pix is not null;
            if (null == pixs)
            {
                return false;
            }

            return Native.DllImports.pixSetMirroredBorder((HandleRef)pixs, left, right, top, bot) == 0;
        }

        /// <summary>
        ///  <pre>
        ///  Notes:
        ///       (1) pixd can be null, but otherwise it must be the same size
        ///           and depth as pixs.Always returns pixd.
        /// 
        ///      (2) This is useful in situations where by setting a few border
        /// pixels we can avoid having to copy all pixels in pixs into
        ///           pixd as an initialization step for some operation.
        ///           Nevertheless, for safety, if making a new pixd, all the
        ///           non-border pixels are initialized to 0.
        ///  </pre>
        /// </summary>
        /// <param name="pixd">pixd all depths; colormap ok; can be NULL</param>
        /// <param name="pixs">pixs same depth and size as pixd</param>
        /// <param name="left">left, right, top, bot number of pixels to copy</param>
        /// <param name="right">left, right, top, bot number of pixels to copy</param>
        /// <param name="top">left, right, top, bot number of pixels to copy</param>
        /// <param name="bot">left, right, top, bot number of pixels to copy</param>
        /// <returns>pixd, or NULL on error if pixd is not defined</returns>
        public static Pix pixCopyBorder(this Pix pixd, Pix pixs, int left, int right, int top, int bot)
        {
            //ensure pix is not null;
            if (null == pixs)
            {
                return null;
            }

            var pointer = Native.DllImports.pixCopyBorder((HandleRef)pixd, (HandleRef)pixs, left, right, top, bot);
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
        ///  Notes:
        ///       (1) See pixGetBlackOrWhiteVal() for possible side effect(adding
        ///  a color to a colormap).
        ///       (2) The only complication is that pixs may have a colormap.
        ///           There are two ways to add the black or white border:
        ///           (a) As done here(simplest, most efficient)
        ///           (b) l_int32 ws, hs, d;
        ///               pixGetDimensions(pixs, &ws, &hs, &d);
        ///               Pix* pixd = pixCreate(ws + left + right, hs + top + bot, d);
        ///               PixColormap* cmap = pixGetColormap(pixs);
        ///               if (cmap != NULL)
        ///                   pixSetColormap(pixd, pixcmapCopy(cmap));
        ///               pixSetBlackOrWhite(pixd, L_SET_WHITE);  // uses cmap
        ///               pixRasterop(pixd, left, top, ws, hs, PIX_SET, pixs, 0, 0);
        ///  </pre>
        /// </summary>
        /// <param name="pixs">pixs all depths; colormap ok</param>
        /// <param name="left">left, right, top, bot  number of pixels added</param>
        /// <param name="right">left, right, top, bot  number of pixels added</param>
        /// <param name="top">left, right, top, bot  number of pixels added</param>
        /// <param name="bot">left, right, top, bot  number of pixels added</param>
        /// <param name="op">op L_GET_BLACK_VAL, L_GET_WHITE_VAL</param>
        /// <returns>pixd with the added exterior pixels, or NULL on error</returns>
        public static Pix pixAddBlackOrWhiteBorder(this Pix pixs, int left, int right, int top, int bot, int op)
        {
            //ensure pix is not null;
            if (null == pixs)
            {
                return null;
            }

            var pointer = Native.DllImports.pixAddBlackOrWhiteBorder((HandleRef)pixs, left, right, top, bot, op);
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
        /// <pre>
        /// Notes:
        ///      (1) For binary images:
        ///             white:  val = 0
        ///             black:  val = 1
        ///          For grayscale images:
        ///             white:  val = 2 ** d - 1
        ///             black:  val = 0
        ///          For rgb color images:
        ///             white:  val = 0xffffff00
        ///             black:  val = 0
        ///          For colormapped images, set val to the appropriate colormap index.
        ///      (2) If the added border is either black or white, you can use
        ///             pixAddBlackOrWhiteBorder()
        ///          The black and white values for all images can be found with
        ///             pixGetBlackOrWhiteVal()
        ///          which, if pixs is cmapped, may add an entry to the colormap.
        /// Alternatively, if pixs has a colormap, you can find the index
        ///          of the pixel whose intensity is closest to white or black:
        ///             white: pixcmapGetRankIntensity(cmap, 1.0, &index);
        ///             black: pixcmapGetRankIntensity(cmap, 0.0, &index);
        ///          and use that for val.
        /// </pre>
        /// </summary>
        /// <param name="pixs">pixs all depths; colormap ok</param>
        /// <param name="left">left, right, top, bot  number of pixels added</param>
        /// <param name="right">left, right, top, bot  number of pixels added</param>
        /// <param name="top">left, right, top, bot  number of pixels added</param>
        /// <param name="bot">left, righ</param>
        /// <param name="val">val   value of added border pixels</param>
        /// <returns>pixd with the added exterior pixels, or NULL on error</returns>
        public static Pix pixAddBorderGeneral(this Pix pixs, int left, int right, int top, int bot, uint val)
        {
            //ensure pix is not null;
            if (null == pixs)
            {
                return null;
            }

            var pointer = Native.DllImports.pixAddBorderGeneral((HandleRef)pixs, left, right, top, bot, val);
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
        /// pixRemoveBorder
        /// </summary>
        /// <param name="pixs">pixs all depths; colormap ok</param>
        /// <param name="npix">npix number to be removed from each of the 4 sides</param>
        /// <returns>pixd with pixels removed around border, or NULL on error</returns>
        public static Pix pixRemoveBorder(this Pix pixs, int npix)
        {
            //ensure pix is not null;
            if (null == pixs)
            {
                return null;
            }

            var pointer = Native.DllImports.pixRemoveBorder((HandleRef)pixs, npix);
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
        /// pixRemoveBorderGeneral
        /// </summary>
        /// <param name="pixs">pixs all depths; colormap ok</param>
        /// <param name="left">left, right, top, bot  number of pixels removed</param>
        /// <param name="right">left, right, top, bot  number of pixels removed</param>
        /// <param name="top">left, right, top, bot  number of pixels removed</param>
        /// <param name="bot">left, right, top, bot  number of pixels removed</param>
        /// <returns>pixd with pixels removed around border, or NULL on error</returns>
        public static Pix pixRemoveBorderGeneral(this Pix pixs, int left, int right, int top, int bot)
        {
            //ensure pix is not null;
            if (null == pixs)
            {
                return null;
            }

            var pointer = Native.DllImports.pixRemoveBorderGeneral((HandleRef)pixs, left, right, top, bot);
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
        /// <pre>
        /// Notes:
        ///      (1) Removes pixels as evenly as possible from the sides of the
        ///          image, leaving the central part.
        ///      (2) Returns clone if no pixels requested removed, or the target
        ///          sizes are larger than the image.
        /// </pre>
        /// </summary>
        /// <param name="pixs">pixs all depths; colormap ok</param>
        /// <param name="wd">wd target width; use 0 if only removing from height</param>
        /// <param name="hd">hd target height; use 0 if only removing from width</param>
        /// <returns>pixd with pixels removed around border, or NULL on error</returns>
        public static Pix pixRemoveBorderToSize(this Pix pixs, int wd, int hd)
        {
            //ensure pix is not null;
            if (null == pixs)
            {
                return null;
            }

            var pointer = Native.DllImports.pixRemoveBorderToSize((HandleRef)pixs, wd, hd);
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
        /// <pre>
        /// Notes:
        ///      (1) This applies what is effectively mirror boundary conditions.
        ///          For the added border pixels in pixd, the pixels in pixs
        /// near the border are mirror-copied into the border region.
        ///      (2) This is useful for avoiding special operations near
        ///          boundaries when doing image processing operations
        ///          such as rank filters and convolution.In use, one first
        ///          adds mirrored pixels to each side of the image.The number
        ///          of pixels added on each side is half the filter dimension.
        ///          Then the image processing operations proceed over a
        ///          region equal to the size of the original image, and
        /// write directly into a dest pix of the same size as pixs.
        ///      (3) The general pixRasterop() is used for an in-place operation here
        /// because there is no overlap between the src and dest rectangles.
        /// </pre>
        /// </summary>
        /// <param name="pixs">pixs all depths; colormap ok</param>
        /// <param name="left">left, right, top, bot number of pixels added</param>
        /// <param name="right">left, right, top, bot number of pixels added</param>
        /// <param name="top">left, right, top, bot number of pixels added</param>
        /// <param name="bot">left, right, top, bot number of pixels added</param>
        /// <returns>pixd, or NULL on error</returns>
        public static Pix pixAddMirroredBorder(this Pix pixs, int left, int right, int top, int bot)
        {
            //ensure pix is not null;
            if (null == pixs)
            {
                return null;
            }

            var pointer = Native.DllImports.pixAddMirroredBorder((HandleRef)pixs, left, right, top, bot);
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
        /// <pre>
        /// Notes:
        ///      (1) This applies what is effectively mirror boundary conditions.
        ///          For the added border pixels in pixd, the pixels in pixs
        /// near the border are mirror-copied into the border region.
        ///      (2) This is useful for avoiding special operations near
        ///          boundaries when doing image processing operations
        ///          such as rank filters and convolution.In use, one first
        ///          adds mirrored pixels to each side of the image.The number
        ///          of pixels added on each side is half the filter dimension.
        ///          Then the image processing operations proceed over a
        ///          region equal to the size of the original image, and
        /// write directly into a dest pix of the same size as pixs.
        ///      (3) The general pixRasterop() is used for an in-place operation here
        /// because there is no overlap between the src and dest rectangles.
        /// </pre>
        /// </summary>
        /// <param name="pixs"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="top"></param>
        /// <param name="bot"></param>
        /// <returns></returns>
        public static Pix pixAddRepeatedBorder(this Pix pixs, int left, int right, int top, int bot)
        {
            //ensure pix is not null;
            if (null == pixs)
            {
                return null;
            }

            var pointer = Native.DllImports.pixAddRepeatedBorder((HandleRef)pixs, left, right, top, bot);
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
        /// <pre>
        /// Notes:
        ///      (1) This applies mirrored boundary conditions horizontally
        ///          and repeated b.c.vertically.
        ///      (2) It is specifically used for avoiding special operations
        /// near boundaries when convolving a hue-saturation histogram
        ///          with a given window size.The repeated b.c.are used
        ///          vertically for hue, and the mirrored b.c.are used
        /// horizontally for saturation.The number of pixels added
        /// on each side is approximately (but not quite) half the
        ///          filter dimension.The image processing operations can
        ///          then proceed over a region equal to the size of the original
        ///          image, and write directly into a dest pix of the same
        ///          size as pixs.
        ///      (3) The general pixRasterop() can be used for an in-place
        /// operation here because there is no overlap between the
        ///          src and dest rectangles.
        /// </pre>
        /// </summary>
        /// <param name="pixs"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="top"></param>
        /// <param name="bot"></param>
        /// <returns></returns>
        public static Pix pixAddMixedBorder(this Pix pixs, int left, int right, int top, int bot)
        {
            //ensure pix is not null;
            if (null == pixs)
            {
                return null;
            }

            var pointer = Native.DllImports.pixAddMixedBorder((HandleRef)pixs, left, right, top, bot);
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
        /// (1) This adds pixels on each side whose values are equal to the value on the closest boundary pixel.
        /// </summary>
        /// <param name="pixs"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="top"></param>
        /// <param name="bot"></param>
        /// <returns></returns>
        public static Pix pixAddContinuedBorder(this Pix pixs, int left, int right, int top, int bot)
        {
            //ensure pix is not null;
            if (null == pixs)
            {
                return null;
            }

            var pointer = Native.DllImports.pixAddContinuedBorder((HandleRef)pixs, left, right, top, bot);
            if (IntPtr.Zero != pointer)
            {
                return new Pix(pointer);
            }
            else
            {
                return null;
            }
        }

        /*    
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
