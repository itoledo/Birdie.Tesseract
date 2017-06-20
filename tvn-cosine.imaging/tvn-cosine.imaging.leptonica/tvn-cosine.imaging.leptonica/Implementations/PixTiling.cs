using System;
using System.Runtime.InteropServices;

namespace Leptonica
{
    public static class Pixtiling
    {
        internal static extern PixTiling pixTilingCreate(this Pix pixs, int nx, int ny, int w, int h, int xoverlap, int yoverlap)
        {
            throw new NotImplementedException();
        }

        internal static extern void pixTilingDestroy(this PixTiling ppt)
        {
            throw new NotImplementedException();
        }

        internal static extern int pixTilingGetCount(this PixTiling pt, out int pnx, out int pny)
        {
            throw new NotImplementedException();
        }

        internal static extern int pixTilingGetSize(this PixTiling pt, out int pw, out int ph)
        {
            throw new NotImplementedException();
        }

        internal static extern Pix pixTilingGetTile(this PixTiling pt, int i, int j)
        {
            throw new NotImplementedException();
        }

        internal static extern int pixTilingNoStripOnPaint(this PixTiling pt)
        {
            throw new NotImplementedException();
        }

        internal static extern int pixTilingPaintTile(this Pix pixd, int i, int j, Pix pixs, PixTiling pt)
        {
            throw new NotImplementedException();
        }

    }
}
