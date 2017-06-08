using System;
using System.Runtime.InteropServices;

namespace Leptonica
{
    public static class BoxFunc2
    {
        /// <summary>
        /// (1) This is a very simple function that first shifts, then scales.
        /// </summary>
        /// <param name="boxas">boxas</param>
        /// <param name="shiftx"> shiftx, shifty</param>
        /// <param name="shifty"> shiftx, shifty</param>
        /// <param name="scalex">scalex, scaley</param>
        /// <param name="scaley">scalex, scaley</param>
        /// <returns>boxad, or NULL on error</returns>
        public static Boxa boxaTransform(this Boxa boxas, int shiftx, int shifty, float scalex, float scaley)
        {
            if (null == boxas)
            {
                return null;
            }

            var pointer = Native.DllImports.boxaTransform((HandleRef)boxas, shiftx, shifty, scalex, scaley);
            if (IntPtr.Zero != pointer)
            {
                return new Boxa(pointer);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// (1) This is a very simple function that first shifts, then scales.
        /// (2) If the box is invalid, a new invalid box is returned.
        /// </summary>
        /// <param name="box">box</param>
        /// <param name="shiftx"> shiftx, shifty</param>
        /// <param name="shifty"> shiftx, shifty</param>
        /// <param name="scalex">scalex, scaley</param>
        /// <param name="scaley">scalex, scaley</param>
        /// <returns>boxd, or NULL on error</returns>
        public static Box boxTransform(this Box box, int shiftx, int shifty, float scalex, float scaley)
        {
            if (null == box)
            {
                return null;
            }

            var pointer = Native.DllImports.boxTransform((HandleRef)box, shiftx, shifty, scalex, scaley);
            if (IntPtr.Zero != pointer)
            {
                return new Box(pointer);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        ///  <pre>
        ///  Notes:
        ///       (1) This allows a sequence of linear transforms on each box.
        ///           the transforms are from the affine set, composed of
        ///           shift, scaling and rotation, and the order of the
        ///  transforms is specified.
        ///       (2) Although these operations appear to be on an infinite
        ///           2D plane, in practice the region of interest is clipped
        ///  to a finite image.The center of rotation is usually taken
        ///           with respect to the image(either the UL corner or the
        ///           center).  A translation can have two very different effects:
        ///             (a) Moves the boxes across the fixed image region.
        /// (b)Moves the image origin, causing a change in the image
        ///  region and an opposite effective translation of the boxes.
        ///  This function should only be used for (a), where the image
        ///  region is fixed on translation.  If the image region is
        /// 
        ///          changed by the translation, use instead the functions
        ///          in affinecompose.c, where the image region and rotation
        /// center can be computed from the actual clipping due to
        /// translation of the image origin. 
        /// (3) See boxTransformOrdered() for usage and implementation details.
        ///
        /// </ pre >
        /// </summary>
        /// <param name="boxas"></param>
        /// <param name="shiftx"></param>
        /// <param name="shifty"></param>
        /// <param name="scalex"></param>
        /// <param name="scaley"></param>
        /// <param name="xcen"></param>
        /// <param name="ycen"></param>
        /// <param name="angle"></param>
        /// <param name="order"></param>
        /// <returns>boxd, or NULL on error</returns>
        public static Boxa boxaTransformOrdered(this Boxa boxas, int shiftx, int shifty, float scalex, float scaley, int xcen, int ycen, float angle, int order)
        {
            if (null == boxas)
            {
                return null;
            }

            var pointer = Native.DllImports.boxaTransformOrdered((HandleRef)boxas, shiftx, shifty, scalex, scaley, xcen, ycen, angle, order);
            if (IntPtr.Zero != pointer)
            {
                return new Boxa(pointer);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        ///  <pre>
        ///  Notes:
        ///       (1) This allows a sequence of linear transforms, composed of
        ///           shift, scaling and rotation, where the order of the
        ///  transforms is specified.
        /// 
        ///      (2) The rotation is taken about a point specified by(xcen, ycen).
        ///           Let the components of the vector from the center of rotation
        ///  to the box center be(xdif, ydif):
        ///             xdif = (bx + 0.5 * bw) - xcen
        ///  ydif = (by + 0.5 * bh) - ycen
        ///  Then the box center after rotation has new components:
        ///             bxcen = xcen + xdif* cosa + ydif* sina
        ///             bycen = ycen + ydif* cosa - xdif* sina
        ///           where cosa and sina are the cos and sin of the angle,
        ///           and the enclosing box for the rotated box has size:
        ///             rw = |bw* cosa| + |bh* sina|
        ///             rh = |bh* cosa| + |bw* sina|
        ///           where bw and bh are the unrotated width and height.
        ///           Then the box UL corner (rx, ry) is
        ///             rx = bxcen - 0.5 * rw
        ///  ry = bycen - 0.5 * rh
        ///  (3) The center of rotation specified by args %xcen and %ycen
        ///          is the point BEFORE any translation or scaling.If the
        ///           rotation is not the first operation, this function finds
        ///           the actual center at the time of rotation.It does this
        ///  by making the following assumptions:
        ///              (1) Any scaling is with respect to the UL corner, so
        ///                  that the center location scales accordingly.
        ///              (2) A translation does not affect the center of
        ///                  the image; it just moves the boxes.
        ///  We always use assumption(1).  However, assumption(2)
        ///           will be incorrect if the apparent translation is due
        ///  to a clipping operation that, in effect, moves the
        ///           origin of the image.In that case, you should NOT use
        ///           these simple functions.Instead, use the functions
        ///           in affinecompose.c, where the rotation center can be
        ///           computed from the actual clipping due to translation
        ///           of the image origin.
        ///  </pre>
        /// </summary>
        /// <param name="boxs"></param>
        /// <param name="shiftx"></param>
        /// <param name="shifty"></param>
        /// <param name="scalex"></param>
        /// <param name="scaley"></param>
        /// <param name="xcen"></param>
        /// <param name="ycen"></param>
        /// <param name="angle"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static Box boxTransformOrdered(this Box boxs, int shiftx, int shifty, float scalex, float scaley, int xcen, int ycen, float angle, int order)
        {
            if (null == boxs)
            {
                return null;
            }

            var pointer = Native.DllImports.boxTransformOrdered((HandleRef)boxs, shiftx, shifty, scalex, scaley, xcen, ycen, angle, order);
            if (IntPtr.Zero != pointer)
            {
                return new Box(pointer);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// (1) See boxRotateOrth() for details.
        /// </summary>
        /// <param name="boxas"></param>
        /// <param name="w">w, h of image in which the boxa is embedded</param>
        /// <param name="h">w, h of image in which the boxa is embedded</param>
        /// <param name="rotation">rotation 0 = noop, 1 = 90 deg, 2 = 180 deg, 3 = 270 deg; all rotations are clockwise</param>
        /// <returns></returns>
        public static Boxa boxaRotateOrth(this Boxa boxas, int w, int h, int rotation)
        {
            if (null == boxas)
            {
                return null;
            }

            var pointer = Native.DllImports.boxaRotateOrth((HandleRef)boxas, w, h, rotation);
            if (IntPtr.Zero != pointer)
            {
                return new Boxa(pointer);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        ///      (1) Rotate the image with the embedded box by the specified amount.
        ///      (2) After rotation, the rotated box is always measured with
        /// respect to the UL corner of the image.
        /// </summary>
        /// <param name="box"></param>
        /// <param name="w">w, h of image in which the box is embedded</param>
        /// <param name="h">w, h of image in which the box is embedded</param>
        /// <param name="rotation">rotation 0 = noop, 1 = 90 deg, 2 = 180 deg, 3 = 270 deg;all rotations are clockwise</param>
        /// <returns>boxd, or NULL on error</returns>
        public static Box boxRotateOrth(this Box box, int w, int h, int rotation)
        {
            if (null == box)
            {
                return null;
            }

            var pointer = Native.DllImports.boxRotateOrth((HandleRef)box, w, h, rotation);
            if (IntPtr.Zero != pointer)
            {
                return new Box(pointer);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        ///   (1) An empty boxa returns a copy, with a warning.
        /// </summary>
        /// <param name="boxas"> boxas</param>
        /// <param name="sorttype">sorttype L_SORT_BY_X, L_SORT_BY_Y,...</param>
        /// <param name="sortorder">sortorder  L_SORT_INCREASING, L_SORT_DECREASING</param>
        /// <param name="pnaindex">pnaindex [optional] index of sorted order into  original array</param>
        /// <returns>boxad sorted version of boxas, or NULL on error</returns>
        public static Boxa boxaSort(this Boxa boxas, SortTypeFlags sorttype, SortOrderFlags sortorder, out Numa pnaindex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Notes:
        ///      (1) For a large number of boxes(say, greater than 1000), this
        ///          O(n) binsort is much faster than the O(nlogn) shellsort.
        /// For 5000 components, this is over 20x faster than boxaSort().
        ///      (2) Consequently, boxaSort() calls this function if it will
        ///          likely go much faster.
        /// </summary>
        /// <param name="boxas"></param>
        /// <param name="sorttype"></param>
        /// <param name="sortorder"></param>
        /// <param name="pnaindex"></param>
        /// <returns></returns>
        public static Boxa boxaBinSort(this Boxa boxas, int sorttype, int sortorder, IntPtr pnaindex)
        {
            throw new NotImplementedException();
        }

        public static Boxa boxaSortByIndex(this Boxa boxas, IntPtr naindex)
        {
            throw new NotImplementedException();
        }

        //public static Boxa boxaSort2d(this Boxa boxas, IntPtr pnaad, int delta1, int delta2, int minh1);
        //public static Boxa boxaSort2dByIndex(this Boxa boxas, IntPtr naa);
        //public static int boxaGetRankVals(HandleRef boxa, float fract, IntPtr px, IntPtr py, IntPtr pw, IntPtr ph);
        //public static int boxaGetMedianVals(HandleRef boxa, IntPtr px, IntPtr py, IntPtr pw, IntPtr ph);
        //public static int boxaGetAverageSize(HandleRef boxa, IntPtr pw, IntPtr ph);
        //public static int boxaExtractAsNuma(HandleRef boxa, IntPtr pnal, IntPtr pnat, IntPtr pnar, IntPtr pnab, IntPtr pnaw, IntPtr pnah, int keepinvalid);
        //public static int boxaExtractAsPta(HandleRef boxa, IntPtr pptal, IntPtr pptat, IntPtr pptar, IntPtr pptab, IntPtr pptaw, IntPtr pptah, int keepinvalid);
        //public static int boxaaGetExtent(HandleRef baa, IntPtr pw, IntPtr ph, IntPtr pbox, IntPtr pboxa);
        //public static IntPtr boxaaFlattenToBoxa(HandleRef baa, IntPtr pnaindex, int copyflag);
        //public static IntPtr boxaaFlattenAligned(HandleRef baa, int num, IntPtr fillerbox, int copyflag);
        //public static IntPtr boxaEncapsulateAligned(HandleRef boxa, int num, int copyflag);
        //public static IntPtr boxaaTranspose(HandleRef baas);
        //public static int boxaaAlignBox(HandleRef baa, IntPtr box, int delta, IntPtr pindex);
    }
}
