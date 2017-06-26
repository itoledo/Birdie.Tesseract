using System;
using System.Runtime.InteropServices;

namespace Leptonica
{
    public static class WriteFile
    {
        // High-level procedures for writing images to file:
        public static int pixaWriteFiles(string rootname, Pixa pixa, int format)
        {
            throw new NotImplementedException();
        }

        public static int pixWrite(string fname, Pix pix, int format)
        {
            throw new NotImplementedException();
        }

        public static int pixWriteAutoFormat(string filename, Pix pix)
        {
            throw new NotImplementedException();
        }

        public static int pixWriteStream(IntPtr fp, Pix pix, int format)
        {
            throw new NotImplementedException();
        }

        public static int pixWriteImpliedFormat(string filename, Pix pix, int quality, int progressive)
        {
            throw new NotImplementedException();
        }


        // Selection of output format if default is requested
        public static int pixChooseOutputFormat(this Pix pix)
        {
            throw new NotImplementedException();
        }

        public static int getImpliedFileFormat(string filename)
        {
            throw new NotImplementedException();
        }

        public static int pixGetAutoFormat(this Pix pix, out int pformat)
        {
            throw new NotImplementedException();
        }

        public static IntPtr getFormatExtension(int format)
        {
            throw new NotImplementedException();
        }


        // Write to memory
        public static int pixWriteMem(out IntPtr pdata, IntPtr psize, Pix pix, int format)
        {
            throw new NotImplementedException();
        }


        // Image display for debugging
        public static int l_fileDisplay(string fname, int x, int y, float scale)
        {
            throw new NotImplementedException();
        }

        public static int pixDisplay(this Pix pixs, int x, int y)
        {
            throw new NotImplementedException();
        }

        public static int pixDisplayWithTitle(this Pix pixs, int x, int y, string title, int dispflag)
        {
            throw new NotImplementedException();
        }

        public static int pixSaveTiled(this Pix pixs, HandleRef pixa, float scalefactor, int newrow, int space, int dp)
        {
            throw new NotImplementedException();
        }

        public static int pixSaveTiledOutline(this Pix pixs, HandleRef pixa, float scalefactor, int newrow, int space, int linewidth, int dp)
        {
            throw new NotImplementedException();
        }

        public static int pixSaveTiledWithText(this Pix pixs, HandleRef pixa, int outwidth, int newrow, int space, int linewidth, L_Bmf bmf, string textstr, uint val, int location)
        {
            throw new NotImplementedException();
        }

        public static void l_chooseDisplayProg(int selection)
        {
            throw new NotImplementedException();
        }


        // Deprecated pix output for debugging
        public static int pixDisplayWrite(this Pix pixs, int reduction)
        {
            throw new NotImplementedException();
        }

        public static int pixDisplayWriteFormat(this Pix pixs, int reduction, int format)
        {
            throw new NotImplementedException();
        }

        public static int pixDisplayMultiple(int res, float scalefactor, string fileout)
        {
            throw new NotImplementedException();
        } 
    }
}
