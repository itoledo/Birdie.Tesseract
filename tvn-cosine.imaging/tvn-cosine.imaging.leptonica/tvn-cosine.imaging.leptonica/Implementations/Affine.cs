using System;
using System.Runtime.InteropServices;

namespace Leptonica
{
    public static class Affine
    {
        // Affine (3 pt) image transformation using a sampled (to nearest integer) transform on each dest point 
        public static Pix pixAffineSampledPta(this Pix pixs, Pta ptad, Pta ptas, int incolor)
        {
            if (null == pixs
             || null == ptad
             || null == ptas)
            {
                throw new ArgumentNullException("pixs, ptad, ptas cannot be null.");
            }

            var pointer = Native.DllImports.pixAffineSampledPta((HandleRef)pixs, (HandleRef)ptad, (HandleRef)ptas, incolor);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        public static Pix pixAffineSampled(this Pix pixs, IntPtr vc, int incolor)
        {
            if (null == pixs
             || null == vc)
            {
                throw new ArgumentNullException("pixs, vc cannot be null.");
            }

            var pointer = Native.DllImports.pixAffineSampled((HandleRef)pixs, vc, incolor);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        // Affine (3 pt) image transformation using interpolation (or area mapping) for anti-aliasing images that are 2, 4, or 8 bpp gray, or colormapped, or 32 bpp RGB
        public static Pix pixAffinePta(this Pix pixs, Pta ptad, Pta ptas, int incolor)
        {
            if (null == pixs
             || null == ptad
             || null == ptas)
            {
                throw new ArgumentNullException("pixs, ptad, ptas cannot be null.");
            }

            var pointer = Native.DllImports.pixAffinePta((HandleRef)pixs, (HandleRef)ptad, (HandleRef)ptas, incolor);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        public static Pix pixAffine(this Pix pixs, IntPtr vc, int incolor)
        {
            if (null == pixs
             || null == vc)
            {
                throw new ArgumentNullException("pixs, vc cannot be null.");
            }

            var pointer = Native.DllImports.pixAffine((HandleRef)pixs, vc, incolor);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        public static Pix pixAffinePtaColor(this Pix pixs, Pta ptad, Pta ptas, uint colorval)
        {
            if (null == pixs
             || null == ptad
             || null == ptas)
            {
                throw new ArgumentNullException("pixs, ptad, ptas cannot be null.");
            }

            var pointer = Native.DllImports.pixAffinePtaColor((HandleRef)pixs, (HandleRef)ptad, (HandleRef)ptas, colorval);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        public static Pix pixAffineColor(this Pix pixs, IntPtr vc, uint colorval)
        {
            if (null == pixs
             || null == vc)
            {
                throw new ArgumentNullException("pixs, vc cannot be null.");
            }

            var pointer = Native.DllImports.pixAffineColor((HandleRef)pixs, vc, colorval);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        public static Pix pixAffinePtaGray(this Pix pixs, Pta ptad, Pta ptas, byte grayval)
        {
            if (null == pixs
             || null == ptad
             || null == ptas)
            {
                throw new ArgumentNullException("pixs, ptad, ptas cannot be null.");
            }

            var pointer = Native.DllImports.pixAffinePtaGray((HandleRef)pixs, (HandleRef)ptad, (HandleRef)ptas, grayval);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        public static Pix pixAffineGray(this Pix pixs, IntPtr vc, byte grayval)
        {
            if (null == pixs
             || null == vc)
            {
                throw new ArgumentNullException("pixs, vc cannot be null.");
            }

            var pointer = Native.DllImports.pixAffineGray((HandleRef)pixs, vc, grayval);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        // Affine transform including alpha (blend) component 
        public static Pix pixAffinePtaWithAlpha(this Pix pixs, Pta ptad, Pta ptas, Pix pixg, float fract, int border)
        {
            if (null == pixs
             || null == ptad
             || null == ptas)
            {
                throw new ArgumentNullException("pixs, ptad, ptas cannot be null.");
            }

            var pointer = Native.DllImports.pixAffinePtaWithAlpha((HandleRef)pixs, (HandleRef)ptad, (HandleRef)ptas, (HandleRef)pixg, fract, border);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        // Affine coordinate transformation 
        public static int getAffineXformCoeffs(this Pta ptas, Pta ptad, out IntPtr pvc)
        {
            if (null == ptad
             || null == ptas)
            {
                throw new ArgumentNullException("ptad, ptas cannot be null.");
            }

            return Native.DllImports.getAffineXformCoeffs((HandleRef)ptad, (HandleRef)ptas, out pvc);
        }

        public static int affineInvertXform(IntPtr vc, out IntPtr pvci)
        {
            if (IntPtr.Zero == vc)
            {
                throw new ArgumentNullException("vc cannot be null.");
            }

            return Native.DllImports.affineInvertXform(vc, out pvci);
        }

        public static int affineXformSampledPt(IntPtr vc, int x, int y, out int pxp, out int pyp)
        {
            if (IntPtr.Zero == vc)
            {
                throw new ArgumentNullException("vc cannot be null.");
            }

            return Native.DllImports.affineXformSampledPt(vc, x, y, out pxp, out pyp);
        }

        public static int affineXformPt(IntPtr vc, int x, int y, out float pxp, out float pyp)
        {
            if (IntPtr.Zero == vc)
            {
                throw new ArgumentNullException("vc cannot be null.");
            }

            return Native.DllImports.affineXformPt(vc, x, y, out pxp, out pyp);
        }

        // Interpolation helper functions  
        public static int linearInterpolatePixelColor(IntPtr datas, int wpls, int w, int h, float x, float y, uint colorval, out uint pval)
        {
            if (IntPtr.Zero == datas)
            {
                throw new ArgumentNullException("vc cannot be null.");
            }

            return Native.DllImports.linearInterpolatePixelColor(datas, wpls, w, h, x, y, colorval, out pval);
        }

        public static int linearInterpolatePixelGray(IntPtr datas, int wpls, int w, int h, float x, float y, int grayval, out int pval)
        {
            if (IntPtr.Zero == datas)
            {
                throw new ArgumentNullException("vc cannot be null.");
            }

            return Native.DllImports.linearInterpolatePixelGray(datas, wpls, w, h, x, y, grayval, out pval);
        }

        // Gauss-jordan linear equation solver

        public static int gaussjordan(IntPtr a, IntPtr b, int n)
        {
            if (IntPtr.Zero == a
             || IntPtr.Zero == b)
            {
                throw new ArgumentNullException("a, b cannot be null.");
            }

            return Native.DllImports.gaussjordan(a, b, n);
        }

        // Affine image transformation using a sequence of  shear/scale/translation operations  
        public static Pix pixAffineSequential(this Pix pixs, Pta ptad, Pta ptas, int bw, int bh)
        {
            if (null == pixs
             || null == ptad
             || null == ptas)
            {
                throw new ArgumentNullException("pixs, ptad, ptas cannot be null.");
            }

            var pointer = Native.DllImports.pixAffineSequential((HandleRef)pixs, (HandleRef)ptad, (HandleRef)ptas, bw, bh);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }
    }
}
