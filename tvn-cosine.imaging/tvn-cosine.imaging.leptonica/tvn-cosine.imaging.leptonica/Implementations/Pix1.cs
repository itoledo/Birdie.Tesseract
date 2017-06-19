using System;
using System.Runtime.InteropServices;

namespace Leptonica
{
    public static class Pix1
    {
        // Pix memory management(allows custom allocator and deallocator) 
        public static void setPixMemoryManager(IntPtr allocator, IntPtr deallocator)
        {
            if (null == allocator
             || null == deallocator)
            {
                throw new ArgumentNullException("allocator, deallocator cannot be null");
            }

            Native.DllImports.setPixMemoryManager(allocator, deallocator);
        }

        // Pix creation
        public static Pix pixCreate(int width, int height, int depth)
        {
            var pointer = Native.DllImports.pixCreate(width, height, depth);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        public static Pix pixCreateNoInit(int width, int height, int depth)
        {
            var pointer = Native.DllImports.pixCreateNoInit(width, height, depth);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        public static Pix pixCreateTemplate(this Pix pixs)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            var pointer = Native.DllImports.pixCreateTemplate((HandleRef)pixs);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        public static Pix pixCreateTemplateNoInit(this Pix pixs)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            var pointer = Native.DllImports.pixCreateTemplateNoInit((HandleRef)pixs);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        public static Pix pixCreateHeader(int width, int height, int depth)
        {
            var pointer = Native.DllImports.pixCreateHeader(width, height, depth);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        public static Pix pixClone(this Pix pixs)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            var pointer = Native.DllImports.pixClone((HandleRef)pixs);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        // Pix destruction
        public static void pixDestroy(this Pix ppix)
        {
            if (null == ppix)
            {
                throw new ArgumentNullException("ppix cannot be null");
            }

            var pointer = (IntPtr)ppix;
            Native.DllImports.pixDestroy(ref pointer);
        }

        // Pix copy
        public static Pix pixCopy(this Pix pixd, Pix pixs)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            var pointer = Native.DllImports.pixCopy((HandleRef)pixd, (HandleRef)pixs);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pix(pointer);
            }
        }

        public static int pixResizeImageData(this Pix pixd, Pix pixs)
        {
            if (null == pixd
             || null == pixs)
            {
                throw new ArgumentNullException("pixd, pixs cannot be null");
            }

            return Native.DllImports.pixResizeImageData((HandleRef)pixd, (HandleRef)pixs);
        }

        public static int pixCopyColormap(this Pix pixd, Pix pixs)
        {
            if (null == pixd
             || null == pixs)
            {
                throw new ArgumentNullException("pixd, pixs cannot be null");
            }

            return Native.DllImports.pixCopyColormap((HandleRef)pixd, (HandleRef)pixs);
        }

        public static int pixSizesEqual(this Pix pix1, Pix pix2)
        {
            if (null == pix1
             || null == pix2)
            {
                throw new ArgumentNullException("pix1, pix2 cannot be null");
            }

            return Native.DllImports.pixSizesEqual((HandleRef)pix1, (HandleRef)pix2);
        }

        public static int pixTransferAllData(this Pix pixd, ref Pix ppixs, int copytext, int copyformat)
        {
            if (null == pixd
             || null == ppixs)
            {
                throw new ArgumentNullException("pixd, ppixs cannot be null");
            }

            IntPtr ppixsPtr = (IntPtr)ppixs;
            var result = Native.DllImports.pixTransferAllData((HandleRef)pixd, ref ppixsPtr, copytext, copyformat);
            ppixs = new Pix(ppixsPtr);

            return result;
        }

        public static int pixSwapAndDestroy(out Pix ppixd, ref Pix ppixs)
        {
            if (null == ppixs)
            {
                throw new ArgumentNullException("ppixs cannot be null");
            }

            IntPtr ppixsPtr = (IntPtr)ppixs;
            IntPtr ppixdPtr;
            var result = Native.DllImports.pixSwapAndDestroy(out ppixdPtr, ref ppixsPtr);
            ppixs = new Pix(ppixsPtr);
            ppixd = new Pix(ppixdPtr);

            return result;
        }

        // Pix accessors
        public static int pixGetWidth(this Pix pix)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixGetWidth((HandleRef)pix);
        }

        public static int pixSetWidth(this Pix pix, int width)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixSetWidth((HandleRef)pix, width);
        }

        public static int pixGetHeight(this Pix pix)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixGetHeight((HandleRef)pix);
        }

        public static int pixSetHeight(this Pix pix, int height)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixSetHeight((HandleRef)pix, height);
        }

        public static int pixGetDepth(this Pix pix)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixGetDepth((HandleRef)pix);
        }

        public static int pixSetDepth(this Pix pix, int depth)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixSetDepth((HandleRef)pix, depth);
        }

        public static int pixGetDimensions(this Pix pix, out int pw, out int ph, out int pd)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixGetDimensions((HandleRef)pix, out pw, out ph, out pd);
        }

        public static int pixSetDimensions(this Pix pix, int w, int h, int d)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixSetDimensions((HandleRef)pix, w, h, d);
        }

        public static int pixCopyDimensions(this Pix pixd, Pix pixs)
        {
            if (null == pixd
             || null == pixs)
            {
                throw new ArgumentNullException("pixd, pixs cannot be null");
            }

            return Native.DllImports.pixCopyDimensions((HandleRef)pixd, (HandleRef)pixs);
        }

        public static int pixGetSpp(this Pix pix)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixGetSpp((HandleRef)pix);
        }

        public static int pixSetSpp(this Pix pix, int spp)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixSetSpp((HandleRef)pix, spp);
        }

        public static int pixCopySpp(this Pix pixd, Pix pixs)
        {
            if (null == pixd
             || null == pixs)
            {
                throw new ArgumentNullException("pixd, pixs cannot be null");
            }

            return Native.DllImports.pixCopySpp((HandleRef)pixd, (HandleRef)pixs);
        }

        public static int pixGetWpl(this Pix pix)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixGetWpl((HandleRef)pix);
        }

        public static int pixSetWpl(this Pix pix, int wpl)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixSetWpl((HandleRef)pix, wpl);
        }

        public static int pixGetRefcount(this Pix pix)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixGetRefcount((HandleRef)pix);
        }

        public static int pixChangeRefcount(this Pix pix, int delta)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixChangeRefcount((HandleRef)pix, delta);
        }

        public static int pixGetXRes(this Pix pix)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixGetXRes((HandleRef)pix);
        }

        public static int pixSetXRes(this Pix pix, int res)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixSetXRes((HandleRef)pix, res);
        }

        public static int pixGetYRes(this Pix pix)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixGetYRes((HandleRef)pix);
        }

        public static int pixSetYRes(this Pix pix, int res)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixSetYRes((HandleRef)pix, res);
        }

        public static int pixGetResolution(this Pix pix, out int pxres, out int pyres)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixGetResolution((HandleRef)pix, out pxres, out pyres);
        }

        public static int pixSetResolution(this Pix pix, int xres, int yres)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixSetResolution((HandleRef)pix, xres, yres);
        }

        public static int pixCopyResolution(this Pix pixd, Pix pixs)
        {
            if (null == pixd
             || null == pixs)
            {
                throw new ArgumentNullException("pixd, pixs cannot be null");
            }

            return Native.DllImports.pixCopyResolution((HandleRef)pixd, (HandleRef)pixs);
        }

        public static int pixScaleResolution(this Pix pix, float xscale, float yscale)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixScaleResolution((HandleRef)pix, xscale, yscale);
        }

        public static int pixGetInputFormat(this Pix pix)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixGetInputFormat((HandleRef)pix);
        }

        public static int pixSetInputFormat(this Pix pix, int informat)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixSetInputFormat((HandleRef)pix, informat);
        }

        public static int pixCopyInputFormat(this Pix pixd, Pix pixs)
        {
            if (null == pixd
             || null == pixs)
            {
                throw new ArgumentNullException("pixd, pixs cannot be null");
            }

            return Native.DllImports.pixCopyInputFormat((HandleRef)pixd, (HandleRef)pixs);
        }

        public static int pixSetSpecial(this Pix pix, int special)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixSetSpecial((HandleRef)pix, special);
        }

        public static string pixGetText(this Pix pix)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            var pointer = Native.DllImports.pixGetText((HandleRef)pix);

            return Marshal.PtrToStringAnsi(pointer);
        }

        public static int pixSetText(this Pix pix, string textstring)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixSetText((HandleRef)pix, textstring);
        }

        public static int pixAddText(this Pix pix, string textstring)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixAddText((HandleRef)pix, textstring);
        }

        public static int pixCopyText(this Pix pixd, Pix pixs)
        {
            if (null == pixd
             || null == pixs)
            {
                throw new ArgumentNullException("pixd, pixs cannot be null");
            }

            return Native.DllImports.pixCopyText((HandleRef)pixd, (HandleRef)pixs);
        }

        public static PixColormap pixGetColormap(this Pix pix)
        {
            if (null == pix)
            {
                throw new ArgumentNullException(") cannot be null");
            }

            var pointer = Native.DllImports.pixGetColormap((HandleRef)pix);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new PixColormap(pointer);

            }
        }

        public static int pixSetColormap(this Pix pix, PixColormap colormap)
        {
            if (null == pix
             || null == colormap)
            {
                throw new ArgumentNullException("pix, colormap cannot be null");
            }

            return Native.DllImports.pixSetColormap((HandleRef)pix, (HandleRef)colormap);
        }

        public static int pixDestroyColormap(this Pix pix)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pbox cannot be null");
            }

            return Native.DllImports.pixDestroyColormap((HandleRef)pix);
        }

        public static IntPtr pixGetData(this Pix pix)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pbox cannot be null");
            }

            return Native.DllImports.pixGetData((HandleRef)pix);
        }

        public static int pixSetData(this Pix pix, IntPtr data)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pbox cannot be null");
            }

            return Native.DllImports.pixSetData((HandleRef)pix, data);
        }

        public static IntPtr pixExtractData(this Pix pixs)
        {
            if (null == pixs)
            {
                throw new ArgumentNullException("pixs cannot be null");
            }

            return Native.DllImports.pixExtractData((HandleRef)pixs);
        }

        public static int pixFreeData(this Pix pix)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixFreeData((HandleRef)pix);
        }

        // Pix line ptrs
        public static IntPtr pixGetLinePtrs(this Pix pix, out int psize)
        {
            if (null == pix)
            {
                throw new ArgumentNullException("pix cannot be null");
            }

            return Native.DllImports.pixGetLinePtrs((HandleRef)pix, out psize);
        }

        // Pix debug
        public static int pixPrintStreamInfo(IntPtr fp, Pix pix, string text)
        {
            if (null == pix
             || IntPtr.Zero == fp)
            {
                throw new ArgumentNullException("pix, fp cannot be null");
            }

            return Native.DllImports.pixPrintStreamInfo(fp, (HandleRef)pix, text);
        }
    }
}
