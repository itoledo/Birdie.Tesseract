using System;
using System.Runtime.InteropServices;

namespace Leptonica
{
    public static class AffineCompose
    {
        // Composable coordinate transforms 
        public static IntPtr createMatrix2dTranslate(float transx, float transy)
        {
            return Native.DllImports.createMatrix2dTranslate(transx, transy);
        }

        public static IntPtr createMatrix2dScale(float scalex, float scaley)
        {
            return Native.DllImports.createMatrix2dScale(scalex, scaley);
        }

        public static IntPtr createMatrix2dRotate(float xc, float yc, float angle)
        {
            return Native.DllImports.createMatrix2dRotate(xc, yc, angle);
        }

        // Special coordinate transforms on pta 
        public static Pta ptaTranslate(this Pta ptas, float transx, float transy)
        {
            if (null == ptas)
            {
                throw new ArgumentNullException("ptas cannot be null.");
            }

            var pointer = Native.DllImports.ptaTranslate((HandleRef)ptas, transx, transy);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pta(pointer);
            }
        }

        public static Pta ptaScale(this Pta ptas, float scalex, float scaley)
        {
            if (null == ptas)
            {
                throw new ArgumentNullException("ptas cannot be null.");
            }

            var pointer = Native.DllImports.ptaScale((HandleRef)ptas, scalex, scaley);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pta(pointer);
            }
        }

        public static Pta ptaRotate(this Pta ptas, float xc, float yc, float angle)
        {
            if (null == ptas)
            {
                throw new ArgumentNullException("ptas cannot be null.");
            }

            var pointer = Native.DllImports.ptaRotate((HandleRef)ptas, xc, yc, angle);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pta(pointer);
            }
        }
         
        // Special coordinate transforms on boxa 
        public static Boxa boxaTranslate(this Boxa boxas, float transx, float transy)
        {
            if (null == boxas)
            {
                throw new ArgumentNullException("boxas cannot be null.");
            }

            var pointer = Native.DllImports.boxaTranslate((HandleRef)boxas, transx, transy);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Boxa(pointer);
            }
        }

        public static Boxa boxaScale(this Boxa boxas, float scalex, float scaley)
        {
            if (null == boxas)
            {
                throw new ArgumentNullException("boxas cannot be null.");
            }

            var pointer = Native.DllImports.boxaScale((HandleRef)boxas, scalex, scaley);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Boxa(pointer);
            }
        }

        public static Boxa boxaRotate(this Boxa boxas, float xc, float yc, float angle)
        {
            if (null == boxas)
            {
                throw new ArgumentNullException("boxas cannot be null.");
            }

            var pointer = Native.DllImports.boxaRotate((HandleRef)boxas, xc, yc, angle);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Boxa(pointer);
            }
        }

        // General coordinate transform on pta and boxa 
        public static Pta ptaAffineTransform(this Pta ptas, IntPtr mat)
        {
            if (null == ptas
             || IntPtr.Zero == mat)
            {
                throw new ArgumentNullException("ptas, mat cannot be null.");
            }

            var pointer = Native.DllImports.ptaAffineTransform((HandleRef)ptas, mat);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Pta(pointer);
            }
        }

        public static Boxa boxaAffineTransform(this Boxa boxas, IntPtr mat)
        {
            if (null == boxas
             || IntPtr.Zero == mat)
            {
                throw new ArgumentNullException("boxas, mat cannot be null.");
            }

            var pointer = Native.DllImports.ptaAffineTransform((HandleRef)boxas, mat);
            if (IntPtr.Zero == pointer)
            {
                return null;
            }
            else
            {
                return new Boxa(pointer);
            }
        }

        // Matrix operations 
        public static int l_productMatVec(IntPtr mat, IntPtr vecs, IntPtr vecd, int size)
        {
            if (IntPtr.Zero == mat
             || IntPtr.Zero == vecs
             || IntPtr.Zero == vecd)
            {
                throw new ArgumentNullException("mat, vecs, vecd cannot be null.");
            }

            return Native.DllImports.l_productMatVec(mat, vecs, vecd, size);
        }

        public static int l_productMat2(IntPtr mat1, IntPtr mat2, IntPtr matd, int size)
        {
            if (IntPtr.Zero == mat1
             || IntPtr.Zero == mat2
             || IntPtr.Zero == matd)
            {
                throw new ArgumentNullException("mat1, mat2, matd cannot be null.");
            }

            return Native.DllImports.l_productMat2(mat1, mat2, matd, size);
        }

        public static int l_productMat3(IntPtr mat1, IntPtr mat2, IntPtr mat3, IntPtr matd, int size)
        {
            if (IntPtr.Zero == mat1
             || IntPtr.Zero == mat2
             || IntPtr.Zero == mat3
             || IntPtr.Zero == matd)
            {
                throw new ArgumentNullException("mat1, mat2, mat3, matd cannot be null.");
            }

            return Native.DllImports.l_productMat3(mat1, mat2, mat3, matd, size);
        }

        public static int l_productMat4(IntPtr mat1, IntPtr mat2, IntPtr mat3, IntPtr mat4, IntPtr matd, int size)
        {
            if (IntPtr.Zero == mat1
             || IntPtr.Zero == mat2
             || IntPtr.Zero == mat3
             || IntPtr.Zero == mat4
             || IntPtr.Zero == matd)
            {
                throw new ArgumentNullException("mat1, mat2, mat3, mat4, matd cannot be null.");
            }

            return Native.DllImports.l_productMat4(mat1, mat2, mat3, mat4, matd, size);
        }
    }
}
