using System;
using System.Runtime.InteropServices;

namespace Leptonica
{
    public static class AdaptMap
    {
        #region constants 
        /// <summary>
        /// default tile width
        /// </summary>
        public const int DEFAULT_TILE_WIDTH = 10;
        /// <summary>
        /// default tile height
        /// </summary>
        public const int DEFAULT_TILE_HEIGHT = 15;
        /// <summary>
        /// default fg threshold
        /// </summary>
        public const int DEFAULT_FG_THRESHOLD = 60;
        /// <summary>
        /// default minimum count
        /// </summary>
        public const int DEFAULT_MIN_COUNT = 40;
        public const int DEFAULT_BG_VAL = 200;
        /// <summary>
        /// default bg value 
        /// </summary>
        public const int DEFAULT_X_SMOOTH_SIZE = 2;
        /// <summary>
        /// default x smooth size 
        /// </summary>
        public const int DEFAULT_Y_SMOOTH_SIZE = 1;
        #endregion

        // Clean background to white using background normalization 
        public static Pix pixCleanBackgroundToWhite(this Pix pixs, Pix pixim, Pix pixg, float gamma, int blackval, int whiteval)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null.");
            }

            var pointer = Native.DllImports.pixCleanBackgroundToWhite((HandleRef)pixs, (HandleRef)pixim, (HandleRef)pixg, gamma, blackval, whiteval);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        // Adaptive background normalization (top-level functions) 
        public static Pix pixBackgroundNormSimple(this Pix pixs, Pix pixim, Pix pixg)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null.");
            }

            var pointer = Native.DllImports.pixBackgroundNormSimple((HandleRef)pixs, (HandleRef)pixim, (HandleRef)pixg);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        public static Pix pixBackgroundNorm(this Pix pixs, Pix pixim, Pix pixg, int sx, int sy, int thresh, int mincount, int bgval, int smoothx, int smoothy)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null.");
            }

            var pointer = Native.DllImports.pixBackgroundNorm((HandleRef)pixs, (HandleRef)pixim, (HandleRef)pixg, sx, sy, thresh, mincount, bgval, smoothx, smoothy);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        public static Pix pixBackgroundNormMorph(this Pix pixs, Pix pixim, int reduction, int size, int bgval)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null.");
            }

            var pointer = Native.DllImports.pixBackgroundNormMorph((HandleRef)pixs, (HandleRef)pixim, reduction, size, bgval);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        // Arrays of inverted background values for normalization (16 bpp)
        public static int pixBackgroundNormGrayArray(this Pix pixs, Pix pixim, int sx, int sy, int thresh, int mincount, int bgval, int smoothx, int smoothy, out Pix ppixd)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null.");
            }

            IntPtr pointer;
            var success = Native.DllImports.pixBackgroundNormGrayArray((HandleRef)pixs, (HandleRef)pixim, sx, sy, thresh, mincount, bgval, smoothx, smoothy, out pointer);
            if (IntPtr.Zero == pointer)
            {
                ppixd = null;
            }
            else
            {
                ppixd = new Pix(pointer);
            }
            return success;
        }

        public static int pixBackgroundNormRGBArrays(this Pix pixs, Pix pixim, Pix pixg, int sx, int sy, int thresh, int mincount, int bgval, int smoothx, int smoothy, out Pix ppixr, out Pix ppixg, out Pix ppixb)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null.");
            }

            IntPtr ppixrPtr, ppixgPtr, ppixbPtr;
            var success = Native.DllImports.pixBackgroundNormRGBArrays((HandleRef)pixs, (HandleRef)pixim, (HandleRef)pixg, sx, sy, thresh, mincount, bgval, smoothx, smoothy, out ppixrPtr, out ppixgPtr, out ppixbPtr);
            if (IntPtr.Zero == ppixrPtr
             || IntPtr.Zero == ppixgPtr
             || IntPtr.Zero == ppixbPtr)
            {
                ppixr = null;
                ppixg = null;
                ppixb = null;
            }
            else
            {
                ppixr = new Pix(ppixrPtr);
                ppixg = new Pix(ppixgPtr);
                ppixb = new Pix(ppixbPtr);
            }
            return success;
        }

        public static int pixBackgroundNormGrayArrayMorph(this Pix pixs, Pix pixim, int reduction, int size, int bgval, out Pix ppixd)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null.");
            }

            IntPtr pointer;
            var success = Native.DllImports.pixBackgroundNormGrayArrayMorph((HandleRef)pixs, (HandleRef)pixim, reduction, size, bgval, out pointer);
            if (IntPtr.Zero == pointer)
            {
                ppixd = null;
            }
            else
            {
                ppixd = new Pix(pointer);
            }
            return success;
        }

        public static int pixBackgroundNormRGBArraysMorph(this Pix pixs, Pix pixim, int reduction, int size, int bgval, out Pix ppixr, out Pix ppixg, out Pix ppixb)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null.");
            }

            IntPtr ppixrPtr, ppixgPtr, ppixbPtr;
            var success = Native.DllImports.pixBackgroundNormRGBArraysMorph((HandleRef)pixs, (HandleRef)pixim, reduction, size, bgval, out ppixrPtr, out ppixgPtr, out ppixbPtr);
            if (IntPtr.Zero == ppixrPtr
             || IntPtr.Zero == ppixgPtr
             || IntPtr.Zero == ppixbPtr)
            {
                ppixr = null;
                ppixg = null;
                ppixb = null;
            }
            else
            {
                ppixr = new Pix(ppixrPtr);
                ppixg = new Pix(ppixgPtr);
                ppixb = new Pix(ppixbPtr);
            }
            return success;
        }

        // Measurement of local background
        public static int pixGetBackgroundGrayMap(this Pix pixs, Pix pixim, int sx, int sy, int thresh, int mincount, out Pix ppixd)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null.");
            }

            IntPtr pointer;
            var success = Native.DllImports.pixGetBackgroundGrayMap((HandleRef)pixs, (HandleRef)pixim, sx, sy, thresh, mincount, out pointer);
            if (IntPtr.Zero == pointer)
            {
                ppixd = null;
            }
            else
            {
                ppixd = new Pix(pointer);
            }
            return success;
        }

        public static int pixGetBackgroundRGBMap(this Pix pixs, Pix pixim, Pix pixg, int sx, int sy, int thresh, int mincount, out Pix ppixmr, out Pix ppixmg, out Pix ppixmb)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null.");
            }

            IntPtr ppixmrPtr, ppixmgPtr, ppixmbPtr;
            var success = Native.DllImports.pixGetBackgroundRGBMap((HandleRef)pixs, (HandleRef)pixim, (HandleRef)pixg, sx, sy, thresh, mincount, out ppixmrPtr, out ppixmgPtr, out ppixmbPtr);
            if (IntPtr.Zero == ppixmrPtr
             || IntPtr.Zero == ppixmgPtr
             || IntPtr.Zero == ppixmbPtr)
            {
                ppixmr = null;
                ppixmg = null;
                ppixmb = null;
            }
            else
            {
                ppixmr = new Pix(ppixmrPtr);
                ppixmg = new Pix(ppixmgPtr);
                ppixmb = new Pix(ppixmbPtr);
            }
            return success;
        }

        public static int pixGetBackgroundGrayMapMorph(this Pix pixs, Pix pixim, int reduction, int size, out Pix ppixm)
        {
            if (null == pixs)
            {
                ppixm = null;
                throw new ArgumentNullException("pixs cannot be null.");
            }

            IntPtr pointer;
            var success = Native.DllImports.pixGetBackgroundGrayMapMorph((HandleRef)pixs, (HandleRef)pixim, reduction, size, out pointer) ;
            if (IntPtr.Zero == pointer)
            {
                ppixm = null; 
            }
            else
            {
                ppixm = new Pix(pointer);
            }
            return success;
        }

        public static int pixGetBackgroundRGBMapMorph(this Pix pixs, Pix pixim, int reduction, int size, out Pix ppixmr, out Pix ppixmg, out Pix ppixmb)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null.");
            }

            IntPtr ppixmrPtr, ppixmgPtr, ppixmbPtr;
            var success = Native.DllImports.pixGetBackgroundRGBMapMorph((HandleRef)pixs, (HandleRef)pixim, reduction, size, out ppixmrPtr, out ppixmgPtr, out ppixmbPtr);
            if (IntPtr.Zero == ppixmrPtr
             || IntPtr.Zero == ppixmgPtr
             || IntPtr.Zero == ppixmbPtr)
            {
                ppixmr = null;
                ppixmg = null;
                ppixmb = null; 
            }
            else
            {
                ppixmr = new Pix(ppixmrPtr);
                ppixmg = new Pix(ppixmgPtr);
                ppixmb = new Pix(ppixmbPtr);
            }
            return success;
        }

        public static int pixFillMapHoles(this Pix pixs, int nx, int ny, int filltype)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null.");
            }

            return Native.DllImports.pixFillMapHoles((HandleRef)pixs, nx, ny, filltype);
        }

        public static Pix pixExtendByReplication(this Pix pixs, int addw, int addh)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null.");
            }

            var pointer = Native.DllImports.pixExtendByReplication((HandleRef)pixs, addw, addh);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        public static int pixSmoothConnectedRegions(this Pix pixs, Pix pixm, int factor)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null.");
            }

            return Native.DllImports.pixSmoothConnectedRegions((HandleRef)pixs, (HandleRef)pixm, factor);
        }

        // Generate inverted background map for each component 
        public static Pix pixGetInvBackgroundMap(this Pix pixs, int bgval, int smoothx, int smoothy)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null.");
            }

            var pointer = Native.DllImports.pixGetInvBackgroundMap((HandleRef)pixs, bgval, smoothx, smoothy);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        // Apply inverse background map to image
        public static Pix pixApplyInvBackgroundGrayMap(this Pix pixs, Pix pixm, int sx, int sy)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null.");
            }

            var pointer = Native.DllImports.pixApplyInvBackgroundGrayMap((HandleRef)pixs, (HandleRef)pixm, sx, sy);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        public static Pix pixApplyInvBackgroundRGBMap(this Pix pixs, Pix pixmr, Pix pixmg, Pix pixmb, int sx, int sy)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null.");
            }

            var pointer = Native.DllImports.pixApplyInvBackgroundRGBMap((HandleRef)pixs, (HandleRef)pixmr, (HandleRef)pixmg, (HandleRef)pixmb, sx, sy);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        // Apply variable map
        public static Pix pixApplyVariableGrayMap(this Pix pixs, Pix pixg, int target)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null.");
            }

            var pointer = Native.DllImports.pixApplyVariableGrayMap((HandleRef)pixs, (HandleRef)pixg, target);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        // Non-adaptive (global) mapping
        public static Pix pixGlobalNormRGB(this Pix pixd, Pix pixs, int rval, int gval, int bval, int mapval)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null.");
            }

            var pointer = Native.DllImports.pixGlobalNormRGB((HandleRef)pixs, (HandleRef)pixs, rval, gval, bval, mapval);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        public static Pix pixGlobalNormNoSatRGB(this Pix pixd, Pix pixs, int rval, int gval, int bval, int factor, float rank)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null.");
            }

            var pointer = Native.DllImports.pixGlobalNormNoSatRGB((HandleRef)pixs, (HandleRef)pixs, rval, gval, bval, factor, rank);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        // Adaptive threshold spread normalization
        public static int pixThresholdSpreadNorm(this Pix pixs, int filtertype, int edgethresh, int smoothx, int smoothy, float gamma, int minval, int maxval, int targetthresh, out Pix ppixth, out Pix ppixb, out Pix ppixd)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null.");
            }

            IntPtr ppixthPtr, ppixbPtr, ppixdPtr;
            var success = Native.DllImports.pixThresholdSpreadNorm((HandleRef)pixs, filtertype, edgethresh, smoothx, smoothy, gamma, minval, maxval, targetthresh, out ppixthPtr, out ppixbPtr, out ppixdPtr);
            if (IntPtr.Zero == ppixthPtr
             || IntPtr.Zero == ppixbPtr
             || IntPtr.Zero == ppixdPtr)
            {
                ppixth = null;
                ppixb = null;
                ppixd = null; 
            }
            else
            {
                ppixth = new Pix(ppixthPtr);
                ppixb = new Pix(ppixbPtr);
                ppixd = new Pix(ppixdPtr);
            }
            return success;
        }

        // Adaptive background normalization (flexible adaptaption)
        public static Pix pixBackgroundNormFlex(this Pix pixs, int sx, int sy, int smoothx, int smoothy, int delta)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null.");
            }

            var pointer = Native.DllImports.pixBackgroundNormFlex((HandleRef)pixs, sx, sy, smoothx, smoothy, delta);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        // Adaptive contrast normalization
        public static Pix pixContrastNorm(this Pix pixd, Pix pixs, int sx, int sy, int mindiff, int smoothx, int smoothy)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null.");
            }

            var pointer = Native.DllImports.pixContrastNorm((HandleRef)pixs, (HandleRef)pixs, sx, sy, mindiff, smoothx, smoothy);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        public static int pixMinMaxTiles(this Pix pixs, int sx, int sy, int mindiff, int smoothx, int smoothy, out Pix ppixmin, out Pix ppixmax)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null.");
            }

            IntPtr ppixminPtr, ppixmaxPtr;
            var success = Native.DllImports.pixMinMaxTiles((HandleRef)pixs, sx, sy, mindiff, smoothx, smoothy, out ppixminPtr, out ppixmaxPtr);
            if (IntPtr.Zero == ppixminPtr
             || IntPtr.Zero == ppixmaxPtr)
            {
                ppixmin = null;
                ppixmax = null; 
            }
            else
            {
                ppixmin = new Pix(ppixminPtr);
                ppixmax = new Pix(ppixmaxPtr);
            }
            return success;
        }

        public static int pixSetLowContrast(this Pix pixs1, Pix pixs2, int mindiff)
        {
            if (null == pixs1
             || null == pixs2)
            {
                throw new ArgumentNullException("pixs cannot be null.");
            }

            return Native.DllImports.pixSetLowContrast((HandleRef)pixs1, (HandleRef)pixs2, mindiff);
        }

        public static Pix pixLinearTRCTiled(this Pix pixd, Pix pixs, int sx, int sy, Pix pixmin, Pix pixmax)
        {
            if (null == pixs
             || null == pixmin
             || null == pixmax)
            {
                throw new ArgumentNullException("pixs, pixmin, pixmax cannot be null.");
            }

            var pointer = Native.DllImports.pixLinearTRCTiled((HandleRef)pixs, (HandleRef)pixs, sx, sy, (HandleRef)pixmin, (HandleRef)pixmax);
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
