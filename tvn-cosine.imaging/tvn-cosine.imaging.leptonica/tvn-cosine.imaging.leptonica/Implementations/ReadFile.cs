using System;
using System.Runtime.InteropServices;

namespace Leptonica
{
    public static class ReadFile
    {
        // Top-level functions for reading images from file
        public static Pixa pixaReadFiles(string dirname, string substr)
        {
            throw new NotImplementedException();
        }

        public static Pixa pixaReadFilesSA(this Sarray sa)
        {
            throw new NotImplementedException();
        }

        public static Pix pixRead(string filename)
        {
            throw new NotImplementedException();
        }

        public static Pix pixReadWithHint(string filename, int hint)
        {
            throw new NotImplementedException();
        }

        public static Pix pixReadIndexed(this Sarray sa, int index)
        {
            throw new NotImplementedException();
        }

        public static Pix pixReadStream(IntPtr fp, int hint)
        {
            throw new NotImplementedException();
        }


        // Read header information from file
        public static int pixReadHeader(string filename, out int pformat, out int pw, out int ph, out int pbps, out int pspp, out int piscmap)
        {
            throw new NotImplementedException();
        }


        // Format finders
        public static int findFileFormat(string filename, out int pformat)
        {
            throw new NotImplementedException();
        }

        public static int findFileFormatStream(IntPtr fp, out int pformat)
        {
            throw new NotImplementedException();
        }

        public static int findFileFormatBuffer(IntPtr buf, out int pformat)
        {
            throw new NotImplementedException();
        }

        public static int fileFormatIsTiff(IntPtr fp)
        {
            throw new NotImplementedException();
        }


        // Read from memory
        public static Pix pixReadMem(IntPtr data, IntPtr size)
        {
            throw new NotImplementedException();
        }

        public static int pixReadHeaderMem(IntPtr data, IntPtr size, out int pformat, out int pw, out int ph, out int pbps, out int pspp, out int piscmap)
        {
            throw new NotImplementedException();
        }


        // Output image file information
        public static int writeImageFileInfo(string filename, IntPtr fpout, int headeronly)
        {
            throw new NotImplementedException();
        }


        // Test function for I/O with different formats
        public static int ioFormatTest(string filename)
        {
            throw new NotImplementedException();
        } 
    }
}
