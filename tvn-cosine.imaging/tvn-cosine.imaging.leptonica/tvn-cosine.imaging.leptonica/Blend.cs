using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tvn.Cosine.Imaging;

namespace Leptonica
{
    public static class Blend
    {
        /// <summary>
        /// <pre>
        /// Notes:
        ///      (1) This is a simple top-level interface.  For more flexibility,
        ///          call directly into pixBlendMask(), etc.
        /// </pre>
        /// </summary>
        /// <param name="pixs1">pixs1 blendee</param>
        /// <param name="pixs2">pixs2 blender; typ. smaller</param>
        /// <param name="x">x,y  origin [UL corner] of pixs2 relative to the origin of pixs1; can be &lt; 0</param>
        /// <param name="y">x,y  origin [UL corner] of pixs2 relative to the origin of pixs1; can be &lt; 0</param>
        /// <param name="fract">fract blending fraction</param>
        /// <returns>pixd blended image, or NULL on error</returns>
        public static Pix pixBlend(Pix pixs1, Pix pixs2, int x, int y, float fract)
        {
            //ensure pix is not null;
            if (null == pixs1)
            {
                return null;
            }
            if (null == pixs2)
            {
                return null;
            }

            var pointer = Native.DllImports.pixBlend((HandleRef)pixs1, (HandleRef)pixs2, x, y, fract);

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
        ///      (1) Clipping of pixs2 to pixs1 is done in the inner pixel loop.
        ///      (2) If pixs1 has a colormap, it is removed.
        ///      (3) For inplace operation(pixs1 not cmapped), call it this way:
        ///            pixBlendMask(pixs1, pixs1, pixs2, ...)
        ///      (4) For generating a new pixd:
        ///            pixd = pixBlendMask(NULL, pixs1, pixs2, ...)
        ///      (5) Only call in-place if pixs1 does not have a colormap.
        ///      (6) Invalid %fract defaults to 0.5 with a warning.
        /// Invalid %type defaults to L_BLEND_WITH_INVERSE with a warning.
        /// </pre>
        /// </summary>
        /// <param name="pixd">pixd [optional]; either NULL or equal to pixs1 for in-place</param>
        /// <param name="pixs1">pixs1 blendee, depth > 1</param>
        /// <param name="pixs2">pixs2 blender, 1 bpp; typ. smaller in size than pixs1</param>
        /// <param name="x">x,y  origin [UL corner] of pixs2 relative to the origin of pixs1; can be &lt; 0</param>
        /// <param name="y">x,y  origin [UL corner] of pixs2 relative to the origin of pixs1; can be &lt; 0</param>
        /// <param name="fract">fract blending fraction</param>
        /// <param name="type">type L_BLEND_WITH_INVERSE, L_BLEND_TO_WHITE, L_BLEND_TO_BLACK</param>
        /// <returns></returns>
        public static Pix pixBlendMask(Pix pixd, Pix pixs1, Pix pixs2, int x, int y, float fract, BlendFlags type)
        {
            HandleRef pixdHndlRef = new HandleRef(new object(), IntPtr.Zero);
            //ensure pix is not null;
            if (null == pixs1)
            {
                return null;
            }
            if (null == pixs2)
            {
                return null;
            }
            if (null != pixd)
            {
                pixdHndlRef = (HandleRef)pixd;
            }

            var pointer = Native.DllImports.pixBlendMask(pixdHndlRef, (HandleRef)pixs1, (HandleRef)pixs2, x, y, fract, type);

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
        ///       (1) For inplace operation(pixs1 not cmapped), call it this way:
        ///             pixBlendGray(pixs1, pixs1, pixs2, ...)
        ///       (2) For generating a new pixd:
        ///             pixd = pixBlendGray(NULL, pixs1, pixs2, ...)
        ///       (3) Clipping of pixs2 to pixs1 is done in the inner pixel loop.
        ///       (4) If pixs1 has a colormap, it is removed; otherwise, if pixs1
        ///  has depth  8, it is unpacked to generate a 8 bpp pix.
        /// 
        ///      (5) If transparent = 0, the blending fraction(fract) is
        ///           applied equally to all pixels.
        ///       (6) If transparent = 1, all pixels of value transpix(typically
        ///  either 0 or 0xff) in pixs2 are transparent in the blend.
        ///       (7) After processing pixs1, it is either 8 bpp or 32 bpp:
        ///           ~ if 8 bpp, the fraction of pixs2 is mixed with pixs1.
        ///  ~ if 32 bpp, each component of pixs1 is mixed with
        ///             the same fraction of pixs2.
        ///       (8) For L_BLEND_GRAY_WITH_INVERSE, the white values of the blendee
        ///           (cval == 255 in the code below) result in a delta of 0.
        ///           Thus, these pixels are intrinsically transparent!
        ///           The "pivot" value of the src, at which no blending occurs, is
        ///           128.  Compare with the adaptive pivot in pixBlendGrayAdapt().
        ///       (9) Invalid %fract defaults to 0.5 with a warning.
        ///  Invalid %type defaults to L_BLEND_GRAY with a warning.
        /// 
        /// </pre>
        /// </summary>
        /// <param name="pixd">pixd [optional]; either NULL or equal to pixs1 for in-place</param>
        /// <param name="pixs1">pixs1 blendee, depth > 1</param>
        /// <param name="pixs2">pixs2 blender, 1 bpp; typ. smaller in size than pixs1</param>
        /// <param name="x">x,y  origin [UL corner] of pixs2 relative to the origin of pixs1; can be &lt; 0</param>
        /// <param name="y">x,y  origin [UL corner] of pixs2 relative to the origin of pixs1; can be &lt; 0</param>
        /// <param name="fract">fract blending fraction</param>
        /// <param name="type">type L_BLEND_GRAY, L_BLEND_GRAY_WITH_INVERSE</param>
        /// <param name="transparent">transparent 1 to use transparency; 0 otherwise</param>
        /// <param name="transpix">transpix pixel grayval in pixs2 that is to be transparent</param>
        /// <returns>pixd if OK; pixs1 on error</returns>
        public static Pix pixBlendGray(Pix pixd, Pix pixs1, Pix pixs2, int x, int y, float fract, BlendFlags type, bool transparent, uint transpix)
        {
            HandleRef pixdHndlRef = new HandleRef(new object(), IntPtr.Zero);
            //ensure pix is not null;
            if (null == pixs1)
            {
                return null;
            }
            if (null == pixs2)
            {
                return null;
            }
            if (null != pixd)
            {
                pixdHndlRef = (HandleRef)pixd;
            }

            var pointer = Native.DllImports.pixBlendGray(pixdHndlRef, (HandleRef)pixs1, (HandleRef)pixs2, x, y, fract, type, transparent ? 1 : 0, transpix);

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
        /// Notes:
        ///      (1) For inplace operation(pixs1 not cmapped), call it this way:
        ///            pixBlendGrayInverse(pixs1, pixs1, pixs2, ...)
        ///      (2) For generating a new pixd:
        ///            pixd = pixBlendGrayInverse(NULL, pixs1, pixs2, ...)
        ///      (3) Clipping of pixs2 to pixs1 is done in the inner pixel loop.
        ///      (4) If pixs1 has a colormap, it is removed; otherwise if pixs1
        /// has depth 8, it is unpacked to generate a 8 bpp pix.
        ///
        ///     (5) This is a no-nonsense blender.It changes the src1 pixel except
        ///when the src1 pixel is midlevel gray.  Use fract == 1 for the most
        ///          aggressive blending, where, if the gray pixel in pixs2 is 0,
        ///
        ///we get a complete inversion of the color of the src pixel in pixs1.
        ///
        ///     (6) The basic logic is that each component transforms by:
        ///               d  -->  c* d + (1 - c ) * (f* (1 - d) + d* (1 - f))
        ///          where c is the blender pixel from pixs2,
        ///                f is %fract,
        ///                c and d are normalized to[0...1]
        ///          This has the property that for f == 0 (no blend) or c == 1 (white):
        ///               d  -->  d
        /// For c == 0 (black) we get maximum inversion:
        ///               d  -->  f* (1 - d) + d* (1 - f)   [inversion by fraction f] 
        ///</pre>
        /// </summary>
        /// <param name="pixd">pixd [optional]; either NULL or equal to pixs1 for in-place</param>
        /// <param name="pixs1">pixs1 blendee, depth > 1</param>
        /// <param name="pixs2">pixs2 blender, any depth; typ. smaller in size than pixs1</param>
        /// <param name="x">x,y  origin [UL corner] of pixs2 relative to the origin of pixs1; can be 0</param>
        /// <param name="y">x,y  origin [UL corner] of pixs2 relative to the origin of pixs1; can be 0</param>
        /// <param name="fract">fract blending fraction</param>
        /// <returns>pixd if OK; pixs1 on error</returns>
        public static Pix pixBlendGrayInverse(Pix pixd, Pix pixs1, Pix pixs2, int x, int y, float fract)
        {
            HandleRef pixdHndlRef = new HandleRef(new object(), IntPtr.Zero);
            //ensure pix is not null;
            if (null == pixs1)
            {
                return null;
            }
            if (null == pixs2)
            {
                return null;
            }
            if (null != pixd)
            {
                pixdHndlRef = (HandleRef)pixd;
            }

            var pointer = Native.DllImports.pixBlendGrayInverse(pixdHndlRef, (HandleRef)pixs1, (HandleRef)pixs2, x, y, fract);

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
        /// Notes:
        ///      (1) For inplace operation(pixs1 must be 32 bpp), call it this way:
        ///            pixBlendColor(pixs1, pixs1, pixs2, ...)
        ///      (2) For generating a new pixd:
        ///            pixd = pixBlendColor(NULL, pixs1, pixs2, ...)
        ///      (3) If pixs2 is not 32 bpp rgb, it is converted.
        ///      (4) Clipping of pixs2 to pixs1 is done in the inner pixel loop.
        ///      (5) If pixs1 has a colormap, it is removed to generate a 32 bpp pix.
        ///      (6) If pixs1 has depth 32, it is unpacked to generate a 32 bpp pix.
        ///      (7) If transparent = 0, the blending fraction(fract) is
        ///          applied equally to all pixels.
        ///      (8) If transparent = 1, all pixels of value transpix(typically
        /// either 0 or 0xffffff00) in pixs2 are transparent in the blend.
        /// </pre>
        /// </summary>
        /// <param name="pixd">pixd [optional]; either NULL or equal to pixs1 for in-place</param>
        /// <param name="pixs1">pixs1 blendee; depth > 1</param>
        /// <param name="pixs2">pixs2 blender, any depth;; typ. smaller in size than pixs1</param>
        /// <param name="x">x,y  origin [UL corner] of pixs2 relative to the origin of pixs1</param>
        /// <param name="y">x,y  origin [UL corner] of pixs2 relative to the origin of pixs1</param>
        /// <param name="fract">fract blending fraction</param>
        /// <param name="transparent">transparent 1 to use transparency; 0 otherwise</param>
        /// <param name="transpix">transpix pixel color in pixs2 that is to be transparent</param>
        /// <returns>pixd, or NULL on error</returns>
        public static Pix pixBlendColor(Pix pixd, Pix pixs1, Pix pixs2, int x, int y, float fract, bool transparent, uint transpix)
        {
            HandleRef pixdHndlRef = new HandleRef(new object(), IntPtr.Zero);
            //ensure pix is not null;
            if (null == pixs1)
            {
                return null;
            }
            if (null == pixs2)
            {
                return null;
            }
            if (null != pixd)
            {
                pixdHndlRef = (HandleRef)pixd;
            }

            var pointer = Native.DllImports.pixBlendColor(pixdHndlRef, (HandleRef)pixs1, (HandleRef)pixs2, x, y, fract, transparent ? 1 : 0, transpix);

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
        ///     (1) This generalizes pixBlendColor() in two ways:
        ///         (a) The mixing fraction is specified per channel.
        ///         (b) The mixing fraction may be  0 or> 1, in which case,
        ///             the min or max of two images are taken, respectively.
        ///     (2) Specifically,
        ///         for p = pixs1[i], c = pixs2[i], f = fract[i], i = 1, 2, 3:
        ///             f  0.0:          p --> min(p, c)
        ///             0.0  = f  = 1.0:  p --> (1 - f) * p + f* c
        ///             f > 1.0:          p --> max(a, c)
        ///         Special cases:
        ///             f = 0:   p --> p
        /// f = 1:   p --> c
        ///    (3) See usage notes in pixBlendColor()
        ///     (4) pixBlendColor() would be equivalent to
        ///           pixBlendColorChannel(..., fract, fract, fract, ...);
        ///         at a small cost of efficiency.
        /// </summary>
        /// <param name="pixd">pixd (<optional>; either NULL or equal to pixs1 for in-place)</param>
        /// <param name="pixs1">pixs1 (blendee; depth > 1)</param>
        /// <param name="pixs2">pixs2 (blender, any depth; typ. smaller in size than pixs1)</param>
        /// <param name="x">x,y  (origin [UL corner] of pixs2 relative to the origin of pixs1)</param>
        /// <param name="y">x,y  (origin [UL corner] of pixs2 relative to the origin of pixs1)</param>
        /// <param name="rfract">rfract, gfract, bfract (blending fractions by channel)</param>
        /// <param name="gfract">rfract, gfract, bfract (blending fractions by channel)</param>
        /// <param name="bfract">rfract, gfract, bfract (blending fractions by channel)</param>
        /// <param name="transparent">transparent (1 to use transparency; 0 otherwise)</param>
        /// <param name="transpix">transpix (pixel color in pixs2 that is to be transparent)</param>
        /// <returns>pixd if OK; pixs1 on error</returns>
        public static Pix pixBlendColorByChannel(Pix pixd, Pix pixs1, Pix pixs2, int x, int y, float rfract, float gfract, float bfract, bool transparent, uint transpix)
        {
            HandleRef pixdHndlRef = new HandleRef(new object(), IntPtr.Zero);
            //ensure pix is not null;
            if (null == pixs1)
            {
                return null;
            }
            if (null == pixs2)
            {
                return null;
            }
            if (null != pixd)
            {
                pixdHndlRef = (HandleRef)pixd;
            }

            var pointer = Native.DllImports.pixBlendColorByChannel(pixdHndlRef, (HandleRef)pixs1, (HandleRef)pixs2, x, y, rfract, gfract, bfract, transparent ? 1 : 0, transpix);

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
        ///       (1) For inplace operation(pixs1 not cmapped), call it this way:
        ///             pixBlendGrayAdapt(pixs1, pixs1, pixs2, ...)
        ///           For generating a new pixd:
        ///             pixd = pixBlendGrayAdapt(NULL, pixs1, pixs2, ...)
        ///       (2) Clipping of pixs2 to pixs1 is done in the inner pixel loop.
        ///       (3) If pixs1 has a colormap, it is removed.
        ///       (4) If pixs1 has depth 8, it is unpacked to generate a 8 bpp pix.
        ///       (5) This does a blend with inverse.Whereas in pixGlendGray(), the
        /// zero blend point is where the blendee pixel is 128, here
        ///           the zero blend point is found adaptively, with respect to the
        ///  median of the blendee region.If the median is  128,
        /// 
        /// the zero blend point is found from
        ///               median + shift.
        /// 
        /// Otherwise, if the median >= 128, the zero blend point is
        ///               median - shift.
        /// 
        /// The purpose of shifting the zero blend point away from the
        ///           median is to prevent a situation in pixBlendGray() where
        /// 
        ///          the median is 128 and the blender is not visible.
        /// 
        ///          The default value of shift is 64.
        ///       (6) After processing pixs1, it is either 8 bpp or 32 bpp:
        ///           ~ if 8 bpp, the fraction of pixs2 is mixed with pixs1.
        ///  ~ if 32 bpp, each component of pixs1 is mixed with
        ///             the same fraction of pixs2.
        ///       (7) The darker the blender, the more it mixes with the blendee.
        ///           A blender value of 0 has maximum mixing; a value of 255
        ///           has no mixing and hence is transparent.
        ///  </pre>
        /// </summary>
        /// <param name="pixd">pixd [optional]; either NULL or equal to pixs1 for in-place</param>
        /// <param name="pixs1">pixs1 blendee, depth > 1</param>
        /// <param name="pixs2">pixs2 blender, any depth; typ. smaller in size than pixs1</param>
        /// <param name="x">x,y  origin [UL corner] of pixs2 relative to</param>
        /// <param name="y">the origin of pixs1; can be < 0</param>
        /// <param name="fract">fract blending fraction</param>
        /// <param name="shift">shift >= 0 but = 128: shift of zero blend value from median source; use -1 for default value;</param>
        /// <returns>pixd if OK; pixs1 on error</returns>
        public static Pix pixBlendGrayAdapt(Pix pixd, Pix pixs1, Pix pixs2, int x, int y, float fract, int shift)
        {
            HandleRef pixdHndlRef = new HandleRef(new object(), IntPtr.Zero);
            //ensure pix is not null;
            if (null == pixs1)
            {
                return null;
            }
            if (null == pixs2)
            {
                return null;
            }
            if (null != pixd)
            {
                pixdHndlRef = (HandleRef)pixd;
            }

            var pointer = Native.DllImports.pixBlendGrayAdapt(pixdHndlRef, (HandleRef)pixs1, (HandleRef)pixs2, x, y, fract, shift);

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
        ///       (1) This function combines two pix aligned to the UL corner; they
        ///  need not be the same size.
        /// 
        ///      (2) Each pixel in pixb is multiplied by 'factor' divided by 255, and
        ///           clipped to the range[0... 1].  This gives the fade fraction
        /// to be appied to pixs.Fade either to white (L_BLEND_TO_WHITE)
        ///           or to black(L_BLEND_TO_BLACK).
        ///  </pre>
        /// </summary>
        /// <param name="pixs">pixs colormapped or 8 bpp or 32 bpp</param>
        /// <param name="pixb">pixb 8 bpp blender</param>
        /// <param name="factor">multiplicative factor to apply to blender value</param>
        /// <param name="type">L_BLEND_TO_WHITE, L_BLEND_TO_BLACK</param>
        /// <returns>pixd, or NULL on error</returns>
        public static Pix pixFadeWithGray(Pix pixs, Pix pixb, float factor, BlendFlags type)
        {
            //ensure pix is not null;
            if (null == pixs)
            {
                return null;
            }
            if (null == pixb)
            {
                return null;
            }

            var pointer = Native.DllImports.pixFadeWithGray((HandleRef)pixs, (HandleRef)pixb, factor, type);

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
        ///   Notes:
        ///       (1) pixs2 must be 8 or 32 bpp; either may have a colormap.
        ///       (2) Clipping of pixs2 to pixs1 is done in the inner pixel loop.
        ///       (3) Only call in-place if pixs1 is not colormapped.
        ///       (4) If pixs1 has a colormap, it is removed to generate either an
        ///           8 or 32 bpp pix, depending on the colormap.
        ///       (5) For inplace operation, call it this way:
        ///             pixBlendHardLight(pixs1, pixs1, pixs2, ...)
        ///       (6) For generating a new pixd:
        ///             pixd = pixBlendHardLight(NULL, pixs1, pixs2, ...)
        ///       (7) This is a generalization of the usual hard light blending,
        ///           where fract == 1.0.
        ///       (8) "Overlay" blending is the same as hard light blending, with
        ///  fract == 1.0, except that the components are switched
        ///           in the test.  (Note that the result is symmetric in the
        ///  two components.)
        ///       (9) See, e.g.:
        ///            http://www.pegtop.net/delphi/articles/blendmodes/hardlight.htm
        ///            http://www.digitalartform.com/imageArithmetic.htm
        ///       (10) This function was built by Paco Galanes.
        /// </summary>
        /// <param name="pixd">pixd (<optional>; either NULL or equal to pixs1 for in-place)</param>
        /// <param name="pixs1">pixs1 (blendee; depth > 1, may be cmapped)</param>
        /// <param name="pixs2">pixs2 (blender, 8 or 32 bpp; may be colormapped; typ. smaller in size than pixs1)</param>
        /// <param name="x">x,y (origin [UL corner] of pixs2 relative to the origin of pixs1)</param>
        /// <param name="y">x,y (origin [UL corner] of pixs2 relative to the origin of pixs1)</param>
        /// <param name="fract">fract (blending fraction, or 'opacity factor')</param>
        /// <returns>pixd if OK; pixs1 on error</returns>
        public static Pix pixBlendHardLight(Pix pixd, Pix pixs1, Pix pixs2, int x, int y, float fract)
        {
            HandleRef pixdHndlRef = new HandleRef(new object(), IntPtr.Zero);
            //ensure pix is not null;
            if (null == pixs1)
            {
                return null;
            }
            if (null == pixs2)
            {
                return null;
            }
            if (null != pixd)
            {
                pixdHndlRef = (HandleRef)pixd;
            }

            var pointer = Native.DllImports.pixBlendHardLight(pixdHndlRef, (HandleRef)pixs1, (HandleRef)pixs2, x, y, fract);

            if (IntPtr.Zero != pointer)
            {
                return new Pix(pointer);
            }
            else
            {
                return null;
            }
        }

        ///      /// <summary>
        /// <pre>
        /// Notes:
        ///      (1) This function combines two colormaps, and replaces the pixels
        ///          in pixs that have a specified color value with those in pixb.
        ///      (2) sindex must be in the existing colormap; otherwise an
        ///          error is returned.In use, sindex will typically be the index
        ///         for white(255, 255, 255).
        ///      (3) Blender colors that already exist in the colormap are used;
        ///          others are added.If any blender colors cannot be
        ///          stored in the colormap, an error is returned.
        ///      (4) In the implementation, a mapping is generated from each
        /// original blender colormap index to the corresponding index
        ///         in the expanded colormap for pixs.Then for each pixel in
        ///          pixs with value sindex, and which is covered by a blender pixel,
        ///          the new index corresponding to the blender pixel is substituted
        ///          for sindex.
        /// </pre>
        /// </summary>
        /// <param name="pixs">pixs 2, 4 or 8 bpp, with colormap</param>
        /// <param name="pixb">pixb colormapped blender</param>
        /// <param name="x">x, y UL corner of blender relative to pixs</param>
        /// <param name="y">x, y UL corner of blender relative to pixs</param>
        /// <param name="sindex">sindex colormap index of pixels in pixs to be changed</param>
        /// <returns>0 if OK, 1 on error</returns>
        public static bool pixBlendCmap(Pix pixs, Pix pixb, int x, int y, int sindex)
        {
            //ensure pix is not null;
            if (null == pixs)
            {
                return false;
            }
            if (null == pixb)
            {
                return false;
            }

            return Native.DllImports.pixBlendCmap((HandleRef)pixs, (HandleRef)pixb, x, y, sindex) == 0;
        }

        /// <summary>
        ///  Notes:
        ///       (1) The result is 8 bpp grayscale if both pixs1 and pixs2 are
        ///           8 bpp gray.Otherwise, the result is 32 bpp rgb.
        ///       (2) pixg is an 8 bpp transparency image, where 0 is transparent
        ///  and 255 is opaque.It determines the transparency of pixs2
        ///           when applied over pixs1.It can be null if pixs2 is rgba,
        ///           in which case we use the alpha component of pixs2.
        ///       (3) If pixg exists, it need not be the same size as pixs2.
        ///  However, we assume their UL corners are aligned with each other,
        ///           and placed at the location(x, y) in pixs1.
        /// 
        ///      (4) The pixels in pixd are a combination of those in pixs1
        /// and pixs2, where the amount from pixs2 is proportional to
        ///           the value of the pixel(p) in pixg, and the amount from pixs1
        ///          is proportional to(255 - p).  Thus pixg is a transparency
        ///           image(usually called an alpha blender) where each pixel
        ///           can be associated with a pixel in pixs2, and determines
        ///           the amount of the pixs2 pixel in the final result.
        ///           For example, if pixg is all 0, pixs2 is transparent and
        ///           the result in pixd is simply pixs1.
        ///       (5) A typical use is for the pixs2/pixg combination to be
        ///           a small watermark that is applied to pixs1.
        ///  </pre>
        /// </summary>
        /// <param name="pixs1">pixs1 8 bpp gray, rgb, rgba or colormapped</param>
        /// <param name="pixs2">pixs2 8 bpp gray, rgb, rgba or colormapped</param>
        /// <param name="pixg">pixg [optional] 8 bpp gray, for transparency of pixs2;can be null</param>
        /// <param name="x">x, y UL corner of pixs2 and pixg with respect to pixs1</param>
        /// <param name="y">x, y UL corner of pixs2 and pixg with respect to pixs1</param>
        /// <returns>pixd blended image, or NULL on error</returns>
        public static Pix pixBlendWithGrayMask(Pix pixs1, Pix pixs2, Pix pixg, int x, int y)
        {
            HandleRef pixdHndlRef = new HandleRef(new object(), IntPtr.Zero);
            //ensure pix is not null;
            if (null == pixs1)
            {
                return null;
            }
            if (null == pixs2)
            {
                return null;
            }

            if (null != pixg)
            {
                pixdHndlRef = (HandleRef)pixg;
            }

            var pointer = Native.DllImports.pixBlendWithGrayMask((HandleRef)pixs1, (HandleRef)pixs2, pixdHndlRef, x, y);

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
        ///       (1) This in effect replaces light background pixels in pixs
        ///  by the input color.It does it by alpha blending so that
        ///           there are no visible artifacts from hard cutoffs.
        ///       (2) If pixd == pixs, this is done in-place.
        ///       (3) If box == NULL, this is performed on all of pixs.
        ///       (4) The alpha component for blending is derived from pixs,
        ///           by converting to grayscale and enhancing with a TRC.
        ///       (5) The last three arguments specify the TRC operation.
        ///           Suggested values are: %gamma = 0.3, %minval = 50, %maxval = 200.
        ///  To skip the TRC, use %gamma == 1, %minval = 0, %maxval = 255.
        ///  See pixGammaTRC() for details.
        /// 
        /// </pre>
        /// </summary>
        /// <param name="pixd">pixd can be NULL or pixs</param>
        /// <param name="pixs">pixs 32 bpp rgb</param>
        /// <param name="box">box region for blending; can be NULL)</param>
        /// <param name="color">color 32 bit color in 0xrrggbb00 format</param>
        /// <param name="gamma">gamma, minval, maxval args for grayscale TRC mapping</param>
        /// <param name="minval">gamma, minval, maxval args for grayscale TRC mapping</param>
        /// <param name="maxval">gamma, minval, maxval args for grayscale TRC mapping</param>
        /// <returns>pixd always</returns>
        public static Pix pixBlendBackgroundToColor(Pix pixd, Pix pixs, Box box, Color color, float gamma, int minval, int maxval)
        {
            HandleRef boxdHndlRef = new HandleRef(new object(), IntPtr.Zero);
            HandleRef pixdHndlRef = new HandleRef(new object(), IntPtr.Zero);
            //ensure pix is not null;
            if (null == pixs)
            {
                return null;
            }
            if (null == pixd)
            {
                pixdHndlRef = (HandleRef)pixd;
            }

            if (null != box)
            {
                boxdHndlRef = (HandleRef)box;
            }

            var pointer = Native.DllImports.pixBlendBackgroundToColor(pixdHndlRef, (HandleRef)pixs, boxdHndlRef, color.ToAbgrUint(), gamma, minval, maxval);

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
        ///      (1) This filters all pixels in the specified region by
        ///          multiplying each component by the input color.
        /// This leaves black invariant and transforms white to the
        ///          input color.
        ///      (2) If pixd == pixs, this is done in-place.
        ///      (3) If box == NULL, this is performed on all of pixs.
        /// </pre>
        /// </summary>
        /// <param name="pixd">pixd can be NULL or pixs</param>
        /// <param name="pixs">pixs 32 bpp rgb</param>
        /// <param name="box">box region for filtering; can be NULL)</param>
        /// <param name="color">color 32 bit color in 0xrrggbb00 format</param>
        /// <returns>pixd always</returns>
        public static Pix pixMultiplyByColor(Pix pixd, Pix pixs, Box box, Color color)
        {
            HandleRef boxdHndlRef = new HandleRef(new object(), IntPtr.Zero);
            HandleRef pixdHndlRef = new HandleRef(new object(), IntPtr.Zero);
            //ensure pix is not null;
            if (null == pixs)
            {
                return null;
            }
            if (null == pixd)
            {
                pixdHndlRef = (HandleRef)pixd;
            }

            if (null != box)
            {
                boxdHndlRef = (HandleRef)box;
            }

            var pointer = Native.DllImports.pixMultiplyByColor(pixdHndlRef, (HandleRef)pixs, boxdHndlRef, color.ToAbgrUint());

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
        ///       (1) This is a convenience function that renders 32 bpp RGBA images
        ///           (with an alpha channel) over a uniform background of
        ///  value %color.To render over a white background,
        ///           use %color = 0xffffff00.  The result is an RGB image.
        /// 
        ///      (2) If pixs does not have an alpha channel, it returns a clone
        /// of pixs.
        /// </pre>
        /// </summary>
        /// <param name="pixs">pixs 32 bpp rgba, with alpha</param>
        /// <param name="color">color 32 bit color in 0xrrggbb00 format</param>
        /// <returns>pixd 32 bpp rgb: pixs blended over uniform color %color, a clone of pixs if no alpha, and NULL on error</returns>
        public static Pix pixAlphaBlendUniform(Pix pixs, Color color)
        {
            //ensure pix is not null;
            if (null == pixs)
            {
                return null;
            }

            var pointer = Native.DllImports.pixAlphaBlendUniform((HandleRef)pixs, color.ToAbgrUint());

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
        ///       (1) This is a simple alpha layer generator, where typically white has
        ///           maximum transparency and black has minimum.
        ///       (2) If %invert == 1, generate the same alpha layer but invert
        ///  the input image photometrically.This is useful for blending
        ///  over dark images, where you want dark regions in pixs, such
        ///           as text, to be lighter in the blended image.
        /// 
        ///       (3) The fade %fract gives the minimum transparency(i.e.,
        /// 
        ///  maximum opacity).  A small fraction is useful for adding
        ///  a watermark to an image.
        ///       (4) If pixs has a colormap, it is removed to rgb.
        /// 
        ///       (5) If pixs already has an alpha layer, it is overwritten.
        /// 
        ///  </pre>
        /// </summary>
        /// <param name="pixs">pixs any depth</param>
        /// <param name="fract">fract fade fraction in the alpha component</param>
        /// <param name="invert">invert 1 to photometrically invert pixs</param>
        /// <returns>pixd 32 bpp with alpha, or NULL on error</returns>
        public static Pix pixAddAlphaToBlend(Pix pixs, float fract, int invert)
        {
            //ensure pix is not null;
            if (null == pixs)
            {
                return null;
            }

            var pointer = Native.DllImports.pixAddAlphaToBlend((HandleRef)pixs, fract, invert);

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
        ///       (1) The generated alpha component is transparent over white
        ///           (background) pixels in pixs, and quickly grades to opaque
        ///  away from the transparent parts.This is a cheap and
        /// dirty alpha generator.  The 2 pixel gradation is useful
        /// to blur the boundary between the transparent region
        ///          (that will render entirely from a backing image) and
        /// the remainder which renders from pixs.
        /// 
        ///      (2) All alpha component bits in pixs are overwritten. 
        /// </pre>
        /// </summary>
        /// <param name="pixs">pixs colormapped or 32 bpp rgb; no alpha</param>
        /// <returns>pixd new pix with meaningful alpha component, or NULL on error</returns>
        public static Pix pixSetAlphaOverWhite(Pix pixs)
        {
            //ensure pix is not null;
            if (null == pixs)
            {
                return null;
            }

            var pointer = Native.DllImports.pixSetAlphaOverWhite((HandleRef)pixs);

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
