using System;
using System.Runtime.InteropServices;

namespace Leptonica
{
    public static class Scale
    {
        // Top-level scaling
        public static Pix pixScale(this Pix pixs, float scalex, float scaley)
        {
            throw new NotImplementedException();
        }

        public static Pix pixScaleToSizeRel(this Pix pixs, int delw, int delh)
        {
            throw new NotImplementedException();
        }

        public static Pix pixScaleToSize(this Pix pixs, int wd, int hd)
        {
            throw new NotImplementedException();
        }

        public static Pix pixScaleGeneral(this Pix pixs, float scalex, float scaley, float sharpfract, int sharpwidth)
        {
            throw new NotImplementedException();
        }


        // Linearly interpreted(usually up-) scaling
        public static Pix pixScaleLI(this Pix pixs, float scalex, float scaley)
        {
            throw new NotImplementedException();
        }

        public static Pix pixScaleColorLI(this Pix pixs, float scalex, float scaley)
        {
            throw new NotImplementedException();
        }

        public static Pix pixScaleColor2xLI(this Pix pixs)
        {
            throw new NotImplementedException();
        }

        public static Pix pixScaleColor4xLI(this Pix pixs)
        {
            throw new NotImplementedException();
        }

        public static Pix pixScaleGrayLI(this Pix pixs, float scalex, float scaley)
        {
            throw new NotImplementedException();
        }

        public static Pix pixScaleGray2xLI(this Pix pixs)
        {
            throw new NotImplementedException();
        }

        public static Pix pixScaleGray4xLI(this Pix pixs)
        {
            throw new NotImplementedException();
        }


        // Scaling by closest pixel sampling
        public static Pix pixScaleBySampling(this Pix pixs, float scalex, float scaley)
        {
            throw new NotImplementedException();
        }

        public static Pix pixScaleBySamplingToSize(this Pix pixs, int wd, int hd)
        {
            throw new NotImplementedException();
        }

        public static Pix pixScaleByIntSampling(this Pix pixs, int factor)
        {
            throw new NotImplementedException();
        }


        // Fast integer factor subsampling RGB to gray and to binary
        public static Pix pixScaleRGBToGrayFast(this Pix pixs, int factor, int color)
        {
            throw new NotImplementedException();
        }

        public static Pix pixScaleRGBToBinaryFast(this Pix pixs, int factor, int thresh)
        {
            throw new NotImplementedException();
        }

        public static Pix pixScaleGrayToBinaryFast(this Pix pixs, int factor, int thresh)
        {
            throw new NotImplementedException();
        }


        // Downscaling with(antialias) smoothing
        public static Pix pixScaleSmooth(this Pix pix, float scalex, float scaley)
        {
            throw new NotImplementedException();
        }

        public static Pix pixScaleRGBToGray2(this Pix pixs, float rwt, float gwt, float bwt)
        {
            throw new NotImplementedException();
        }


        // Downscaling with(antialias) area mapping
        public static Pix pixScaleAreaMap(this Pix pix, float scalex, float scaley)
        {
            throw new NotImplementedException();
        }

        public static Pix pixScaleAreaMap2(this Pix pix)
        {
            throw new NotImplementedException();
        }


        // Binary scaling by closest pixel sampling
        public static Pix pixScaleBinary(this Pix pixs, float scalex, float scaley)
        {
            throw new NotImplementedException();
        }


        // Scale-to-gray(1 bpp --> 8 bpp; arbitrary downscaling)
        public static Pix pixScaleToGray(this Pix pixs, float scalefactor)
        {
            throw new NotImplementedException();
        }

        public static Pix pixScaleToGrayFast(this Pix pixs, float scalefactor)
        {
            throw new NotImplementedException();
        }


        // Scale-to-gray(1 bpp --> 8 bpp; integer downscaling)
        public static Pix pixScaleToGray2(this Pix pixs)
        {
            throw new NotImplementedException();
        }

        public static Pix pixScaleToGray3(this Pix pixs)
        {
            throw new NotImplementedException();
        }

        public static Pix pixScaleToGray4(this Pix pixs)
        {
            throw new NotImplementedException();
        }

        public static Pix pixScaleToGray6(this Pix pixs)
        {
            throw new NotImplementedException();
        }

        public static Pix pixScaleToGray8(this Pix pixs)
        {
            throw new NotImplementedException();
        }

        public static Pix pixScaleToGray16(this Pix pixs)
        {
            throw new NotImplementedException();
        }


        // Scale-to-gray by mipmap(1 bpp --> 8 bpp, arbitrary reduction)
        public static Pix pixScaleToGrayMipmap(this Pix pixs, float scalefactor)
        {
            throw new NotImplementedException();
        }


        // Grayscale scaling using mipmap
        public static Pix pixScaleMipmap(this Pix pixs1, Pix pixs2, float scale)
        {
            throw new NotImplementedException();
        }


        // Replicated(integer) expansion(all depths)
        public static Pix pixExpandReplicate(this Pix pixs, int factor)
        {
            throw new NotImplementedException();
        }


        // Upscale 2x followed by binarization
        public static Pix pixScaleGray2xLIThresh(this Pix pixs, int thresh)
        {
            throw new NotImplementedException();
        }

        public static Pix pixScaleGray2xLIDither(this Pix pixs)
        {
            throw new NotImplementedException();
        }


        // Upscale 4x followed by binarization
        public static Pix pixScaleGray4xLIThresh(this Pix pixs, int thresh)
        {
            throw new NotImplementedException();
        }

        public static Pix pixScaleGray4xLIDither(this Pix pixs)
        {
            throw new NotImplementedException();
        }


        // Grayscale downscaling using min and max
        public static Pix pixScaleGrayMinMax(this Pix pixs, int xfact, int yfact, int type)
        {
            throw new NotImplementedException();
        }

        public static Pix pixScaleGrayMinMax2(this Pix pixs, int type)
        {
            throw new NotImplementedException();
        }


        // Grayscale downscaling using rank value
        public static Pix pixScaleGrayRankCascade(this Pix pixs, int level1, int level2, int level3, int level4)
        {
            throw new NotImplementedException();
        }

        public static Pix pixScaleGrayRank2(this Pix pixs, int rank)
        {
            throw new NotImplementedException();
        }


        // Helper function for transferring alpha with scaling
        public static int pixScaleAndTransferAlpha(this Pix pixd, Pix pixs, float scalex, float scaley)
        {
            throw new NotImplementedException();
        }


        // RGB scaling including alpha(blend) component
        public static Pix pixScaleWithAlpha(this Pix pixs, float scalex, float scaley, Pix pixg, float fract)
        {
            throw new NotImplementedException();
        }
    }
}
