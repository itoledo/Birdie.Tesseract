using System;
using System.Runtime.InteropServices;

namespace Leptonica
{
    public static class WaterShed
    {
        // Top-level
        public static L_WShed wshedCreate(this Pix pixs, Pix pixm, int mindepth, int debugflag)
        {
            throw new NotImplementedException();
        }

        public static void wshedDestroy(this L_WShed pwshed)
        {
            throw new NotImplementedException();
        }

        public static int wshedApply(this L_WShed wshed)
        {
            throw new NotImplementedException();
        }


        // Output
        public static int wshedBasins(this L_WShed wshed, out Pixa ppixa, out Numa pnalevels)
        {
            throw new NotImplementedException();
        }

        public static Pix wshedRenderFill(this L_WShed wshed)
        {
            throw new NotImplementedException();
        }

        public static Pix wshedRenderColors(this L_WShed wshed)
        {
            throw new NotImplementedException();
        } 
    }
}
