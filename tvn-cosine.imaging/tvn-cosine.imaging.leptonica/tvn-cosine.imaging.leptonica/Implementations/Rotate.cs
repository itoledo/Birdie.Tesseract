using System;
using System.Runtime.InteropServices;

namespace Leptonica
{
    public static class Rotate
    {
        // General rotation about image center
         public static  Pix  pixRotate(this Pix pixs, float angle, int type, int incolor, int width, int height)
        {
            throw new NotImplementedException();
        }

        public static  Pix  pixEmbedForRotation(this Pix pixs, float angle, int incolor, int width, int height)
        {
            throw new NotImplementedException();
        }


        // General rotation by sampling
        public static  Pix  pixRotateBySampling(this Pix pixs, int xcen, int ycen, float angle, int incolor)
        {
            throw new NotImplementedException();
        }


        // Nice(slow) rotation of 1 bpp image
        public static  Pix  pixRotateBinaryNice(this Pix pixs, float angle, int incolor)
        {
            throw new NotImplementedException();
        }


        // Rotation including alpha(blend) component
        public static  Pix  pixRotateWithAlpha(this Pix pixs, float angle,  Pix  pixg, float fract)
        {
            throw new NotImplementedException();
        } 
    }
}
