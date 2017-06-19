using System;
using System.Runtime.InteropServices;

namespace Leptonica
{
    public static class Pix5
    {
        // Measurement of properties
        public static int pixaFindDimensions(this Pixa pixa, out Numa pnaw, out Numa pnah)
        {
            if (null == pixa)
            {
                throw new ArgumentNullException("pixa cannot be null");
            }

            IntPtr pnawPtr, pnahPtr;
            var result = Native.DllImports.pixaFindDimensions((HandleRef)pixa, out pnawPtr, out pnahPtr);
            pnaw = new Numa(pnawPtr);
            pnah = new Numa(pnahPtr);

            return result;
        }

        public static int pixFindAreaPerimRatio(this Pix pixs, IntPtr tab, out float pfract)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            return Native.DllImports.pixFindAreaPerimRatio((HandleRef)pixs, tab, out pfract);
        }

        public static Numa pixaFindPerimToAreaRatio(this Pixa pixa)
        {
            if (null == pixa)
            {
                throw new ArgumentNullException("pixa cannot be null");
            }

            var pointer = Native.DllImports.pixaFindPerimToAreaRatio((HandleRef)pixa);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Numa(pointer);
            }
        }

        public static int pixFindPerimToAreaRatio(this Pix pixs, IntPtr tab, out float pfract)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            return Native.DllImports.pixFindPerimToAreaRatio((HandleRef)pixs, tab, out pfract);
        }

        public static Numa pixaFindPerimSizeRatio(this Pixa pixa)
        {
            if (null == pixa)
            {
                throw new ArgumentNullException("pixa cannot be null");
            }

            var pointer = Native.DllImports.pixaFindPerimSizeRatio((HandleRef)pixa);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Numa(pointer);
            }
        }

        public static int pixFindPerimSizeRatio(this Pix pixs, IntPtr tab, out float pratio)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            return Native.DllImports.pixFindPerimSizeRatio((HandleRef)pixs, tab, out pratio);
        }

        public static Numa pixaFindAreaFraction(this Pixa pixa)
        {
            if (null == pixa)
            {
                throw new ArgumentNullException("pixa cannot be null");
            }

            var pointer = Native.DllImports.pixaFindAreaFraction((HandleRef)pixa);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Numa(pointer);
            }
        }

        public static int pixFindAreaFraction(this Pix pixs, IntPtr tab, out float pfract)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            return Native.DllImports.pixFindAreaFraction((HandleRef)pixs, tab, out pfract);
        }

        public static Numa pixaFindAreaFractionMasked(this Pixa pixa, Pix pixm, int debug)
        {
            if (null == pixa
             || null == pixm)
            {
                throw new ArgumentNullException("pixa, pixm cannot be null");
            }

            var pointer = Native.DllImports.pixaFindAreaFractionMasked((HandleRef)pixa, (HandleRef)pixm, debug);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Numa(pointer);
            }
        }

        public static int pixFindAreaFractionMasked(this Pix pixs, Box box, Pix pixm, IntPtr tab, out float pfract)
        {
            if (null == pixs
             || null == pixm)
            {
                throw new ArgumentNullException("pixs, pixm cannot be null");
            }

            return Native.DllImports.pixFindAreaFractionMasked((HandleRef)pixs, (HandleRef)box, (HandleRef)pixm, tab, out pfract);
        }

        public static Numa pixaFindWidthHeightRatio(this Pixa pixa)
        {
            if (null == pixa)
            {
                throw new ArgumentNullException("pixa cannot be null");
            }

            var pointer = Native.DllImports.pixaFindWidthHeightRatio((HandleRef)pixa);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Numa(pointer);
            }
        }

        public static Numa pixaFindWidthHeightProduct(this Pixa pixa)
        {
            if (null == pixa)
            {
                throw new ArgumentNullException("pixa cannot be null");
            }

            var pointer = Native.DllImports.pixaFindWidthHeightProduct((HandleRef)pixa);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Numa(pointer);
            }
        }

        public static int pixFindOverlapFraction(this Pix pixs1, Pix pixs2, int x2, int y2, IntPtr tab, out float pratio, out int pnoverlap)
        {
            if (null == pixs1
             || null == pixs2)
            {
                throw new ArgumentNullException("pixs1, pixs2 cannot be null");
            }

            return Native.DllImports.pixFindOverlapFraction((HandleRef)pixs1, (HandleRef)pixs2, x2, y2, tab, out pratio, out pnoverlap);
        }

        public static Boxa pixFindRectangleComps(this Pix pixs, int dist, int minw, int minh)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            var pointer = Native.DllImports.pixFindRectangleComps((HandleRef)pixs, dist, minw, minh);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Boxa(pointer);
            }
        }

        public static int pixConformsToRectangle(this Pix pixs, Box box, int dist, out int pconforms)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            return Native.DllImports.pixConformsToRectangle((HandleRef)pixs, (HandleRef)box, dist, out pconforms);
        }

        // Extract rectangular region
        public static Pixa pixClipRectangles(this Pix pixs, Boxa boxa)
        {
            if (null == pixs
             || null == boxa)
            {
                throw new ArgumentNullException("pixs, boxa cannot be null");
            }

            var pointer = Native.DllImports.pixClipRectangles((HandleRef)pixs, (HandleRef)boxa);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pixa(pointer);
            }
        }

        public static Pix pixClipRectangle(this Pix pixs, Box box, out Box pboxc)
        {
            if (null == pixs
             || null == box)
            {
                throw new ArgumentNullException("pixs, box cannot be null");
            }

            IntPtr pboxcPtr;
            var pointer = Native.DllImports.pixClipRectangle((HandleRef)pixs, (HandleRef)box, out pboxcPtr);
            pboxc = new Box(pboxcPtr);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        public static Pix pixClipMasked(this Pix pixs, Pix pixm, int x, int y, uint outval)
        {
            if (null == pixs
             || null == pixm)
            {
                throw new ArgumentNullException("pixs, pixm cannot be null");
            }

            var pointer = Native.DllImports.pixClipMasked((HandleRef)pixs, (HandleRef)pixm, x, y, outval);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        public static int pixCropToMatch(this Pix pixs1, Pix pixs2, out Pix ppixd1, out Pix ppixd2)
        {
            if (null == pixs1
             || null == pixs2)
            {
                throw new ArgumentNullException("pixs1, pixs2 cannot be null");
            }

            IntPtr ppixd1Ptr, ppixd2Ptr;
            var result = Native.DllImports.pixCropToMatch((HandleRef)pixs1, (HandleRef)pixs2, out ppixd1Ptr, out ppixd2Ptr);

            ppixd1 = new Pix(ppixd1Ptr);
            ppixd2 = new Pix(ppixd2Ptr);

            return result;
        }

        public static Pix pixCropToSize(this Pix pixs, int w, int h)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            var pointer = Native.DllImports.pixCropToSize((HandleRef)pixs, w, h);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        public static Pix pixResizeToMatch(this Pix pixs, Pix pixt, int w, int h)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            var pointer = Native.DllImports.pixResizeToMatch((HandleRef)pixs, (HandleRef)pixt, w, h);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        // Make a frame mask
        public static Pix pixMakeFrameMask(int w, int h, float hf1, float hf2, float vf1, float vf2)
        {
            var pointer = Native.DllImports.pixMakeFrameMask(w, h, hf1, hf2, vf1, vf2);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        // Fraction of Fg pixels under a mask
        public static int pixFractionFgInMask(this Pix pix1, Pix pix2, out float pfract)
        {
            if (null == pix1
             || null == pix2)
            {
                throw new ArgumentNullException("pix1, pix2 cannot be null");
            }

            return Native.DllImports.pixFractionFgInMask((HandleRef)pix1, (HandleRef)pix2, out pfract);
        }

        // Clip to foreground
        public static int pixClipToForeground(this Pix pixs, out Pix ppixd, out Box pbox)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            IntPtr ppixdPtr, pboxPtr;
            var result = Native.DllImports.pixClipToForeground((HandleRef)pixs, out ppixdPtr, out pboxPtr);

            ppixd = new Pix(ppixdPtr);
            pbox = new Box(pboxPtr);

            return result;
        }

        public static int pixTestClipToForeground(this Pix pixs, out int pcanclip)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            var result = Native.DllImports.pixTestClipToForeground((HandleRef)pixs, out pcanclip);

            return result;
        }

        public static int pixClipBoxToForeground(this Pix pixs, Box boxs, out Pix ppixd, out Box pboxd)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            IntPtr ppixdPtr, pboxdPtr;
            var result = Native.DllImports.pixClipBoxToForeground((HandleRef)pixs, (HandleRef)boxs, out ppixdPtr, out pboxdPtr);
            ppixd = new Pix(ppixdPtr);
            pboxd = new Box(pboxdPtr);

            return result;
        }

        public static int pixScanForForeground(this Pix pixs, Box box, int scanflag, out int ploc)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            return Native.DllImports.pixScanForForeground((HandleRef)pixs, (HandleRef)box, scanflag, out ploc);
        }

        public static int pixClipBoxToEdges(this Pix pixs, Box boxs, int lowthresh, int highthresh, int maxwidth, int factor, out Pix ppixd, out Box pboxd)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            IntPtr ppixdPtr, pboxdPtr;
            var result = Native.DllImports.pixClipBoxToEdges((HandleRef)pixs, (HandleRef)boxs, lowthresh, highthresh, maxwidth, factor, out ppixdPtr, out pboxdPtr);
            ppixd = new Pix(ppixdPtr);
            pboxd = new Box(pboxdPtr);

            return result;
        }

        public static int pixScanForEdge(this Pix pixs, Box box, int lowthresh, int highthresh, int maxwidth, int factor, int scanflag, out int ploc)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            return Native.DllImports.pixScanForEdge((HandleRef)pixs, (HandleRef)box, lowthresh, highthresh, maxwidth, factor, scanflag, out ploc);
        }

        // Extract pixel averages and reversals along lines
        public static Numa pixExtractOnLine(this Pix pixs, int x1, int y1, int x2, int y2, int factor)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            var pointer = Native.DllImports.pixExtractOnLine((HandleRef)pixs, x1, y1, x2, y2, factor);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Numa(pointer);
            }
        }

        public static float pixAverageOnLine(this Pix pixs, int x1, int y1, int x2, int y2, int factor)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            return Native.DllImports.pixAverageOnLine((HandleRef)pixs, x1, y1, x2, y2, factor);
        }

        public static Numa pixAverageIntensityProfile(this Pix pixs, float fract, int dir, int first, int last, int factor1, int factor2)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            var pointer = Native.DllImports.pixAverageIntensityProfile((HandleRef)pixs, fract, dir, first, last, factor1, factor2);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Numa(pointer);
            }
        }

        public static Numa pixReversalProfile(this Pix pixs, float fract, int dir, int first, int last, int minreversal, int factor1, int factor2)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            var pointer = Native.DllImports.pixReversalProfile((HandleRef)pixs, fract, dir, first, last, minreversal, factor1, factor2);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Numa(pointer);
            }
        }

        // Extract windowed variance along a line
        public static int pixWindowedVarianceOnLine(this Pix pixs, int dir, int loc, int c1, int c2, int size, out Numa pnad)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            IntPtr pnadPtr;
            var result = Native.DllImports.pixWindowedVarianceOnLine((HandleRef)pixs, dir, loc, c1, c2, size, out pnadPtr);
            pnad = new Numa(pnadPtr);

            return result;
        }

        // Extract min/max of pixel values near lines
        public static int pixMinMaxNearLine(this Pix pixs, int x1, int y1, int x2, int y2, int dist, int direction, out Numa pnamin, out Numa pnamax, out float pminave, out float pmaxave)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            IntPtr pnaminPtr, pnamaxPtr;
            var result = Native.DllImports.pixMinMaxNearLine((HandleRef)pixs, x1, y1, x2, y2, dist, direction, out pnaminPtr, out pnamaxPtr, out pminave, out pmaxave);
            pnamin = new Numa(pnaminPtr);
            pnamax = new Numa(pnamaxPtr);

            return result;
        }

        // Rank row and column transforms
        public static Pix pixRankRowTransform(this Pix pixs)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            var pointer = Native.DllImports.pixRankRowTransform((HandleRef)pixs);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        public static Pix pixRankColumnTransform(this Pix pixs)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            var pointer = Native.DllImports.pixRankColumnTransform((HandleRef)pixs);
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
