using System;
using System.Runtime.InteropServices;

namespace Leptonica
{
    public static class BoxFunc1
    {
        /// <summary>
        /// boxContains()
        /// </summary>
        /// <param name="box1">box1, box2</param>
        /// <param name="box2">box1, box2</param>
        /// <param name="presult">presult 1 if box2 is entirely contained within box1, and 0 otherwise</param>
        /// <returns>0 if OK, 1 on error</returns>
        public static bool boxContains(this Box box1, Box box2, out bool presult)
        {
            if (null == box1)
            {
                presult = false;
                return false;
            }
            if (null == box2)
            {
                presult = false;
                return false;
            }

            return Native.DllImports.boxContains((HandleRef)box1, (HandleRef)box2, out presult) == 0;
        }


        /// <summary>
        /// boxContains()
        /// </summary>
        /// <param name="box1">box1, box2</param>
        /// <param name="box2">box1, box2</param>
        /// <param name="presult">presult 1 if box2 is entirely contained within box1, and 0 otherwise</param>
        /// <returns>0 if OK, 1 on error</returns>
        public static bool boxIntersects(this Box box1, Box box2, out bool presult)
        {
            if (null == box1)
            {
                presult = false;
                return false;
            }
            if (null == box2)
            {
                presult = false;
                return false;
            }

            return Native.DllImports.boxIntersects((HandleRef)box1, (HandleRef)box2, out presult) == 0;
        }

        /// <summary>
        /// (1) All boxes in boxa that are entirely outside box are removed
        /// </summary>
        /// <param name="boxas">boxas</param>
        /// <param name="box">box for containment</param>
        /// <returns>boxad boxa with all boxes in boxas that are entirely contained in box, or NULL on error</returns>
        public static Boxa boxaContainedInBox(this Boxa boxas, Box box)
        {
            if (null == boxas)
            {
                return null;
            }
            if (null == box)
            {
                return null;
            }

            var pointer = Native.DllImports.boxaContainedInBox((HandleRef)boxas, (HandleRef)box);
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
        /// (1) All boxes in boxa that intersect with box (i.e., are completely or partially contained in box) are retained.
        /// </summary>
        /// <param name="boxas">boxas</param>
        /// <param name="box">box for intersecting</param>
        /// <returns>boxad boxa with all boxes in boxas that intersect box, or NULL on error</returns>
        public static Boxa boxaIntersectsBox(this Boxa boxas, Box box)
        {
            if (null == boxas)
            {
                return null;
            }
            if (null == box)
            {
                return null;
            }

            var pointer = Native.DllImports.boxaIntersectsBox((HandleRef)boxas, (HandleRef)box);
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
        /// (1) All boxes in boxa not intersecting with box are removed, and the remaining boxes are clipped to box.
        /// </summary>
        /// <param name="boxas">boxas</param>
        /// <param name="box">box for clipping</param>
        /// <returns></returns>
        public static Boxa boxaClipToBox(this Boxa boxas, Box box)
        {
            if (null == boxas)
            {
                return null;
            }
            if (null == box)
            {
                return null;
            }

            var pointer = Native.DllImports.boxaClipToBox((HandleRef)boxas, (HandleRef)box);
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
        /// (1) This is the geometric intersection of the two rectangles.
        /// </summary>
        /// <param name="box1">box1, box2 two boxes</param>
        /// <param name="box2">box1, box2 two boxes</param>
        /// <returns>box of overlap region between input boxes,or NULL if no overlap or on error</returns>
        public static Box boxOverlapRegion(this Box box1, Box box2)
        {
            if (null == box1)
            {
                return null;
            }
            if (null == box2)
            {
                return null;
            }

            var pointer = Native.DllImports.boxOverlapRegion((HandleRef)box1, (HandleRef)box2);
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
        ///  (1) This is the geometric union of the two rectangles.
        /// </summary>
        /// <param name="box1">box1, box2 two boxes</param>
        /// <param name="box2">box1, box2 two boxes</param>
        /// <returns>box of bounding region containing the input boxes,or NULL on error</returns>
        public static Box boxBoundingRegion(this Box box1, Box box2)
        {
            if (null == box1)
            {
                return null;
            }
            if (null == box2)
            {
                return null;
            }

            var pointer = Native.DllImports.boxBoundingRegion((HandleRef)box1, (HandleRef)box2);
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
        /// (1) The result depends on the order of the input boxes, because the overlap is taken as a fraction of box2.
        /// </summary>
        /// <param name="box1">box1, box2 two boxes</param>
        /// <param name="box2">box1, box2 two boxes</param>
        /// <param name="pfract"></param>
        /// <returns>0 if OK, 1 on error.</returns>
        public static bool boxOverlapFraction(this Box box1, Box box2, out float pfract)
        {
            if (null == box1)
            {
                pfract = 0;
                return false;
            }
            if (null == box2)
            {
                pfract = 0;
                return false;
            }

            return Native.DllImports.boxOverlapFraction((HandleRef)box1, (HandleRef)box2, out pfract) == 0;
        }

        /// <summary>
        /// boxOverlapArea()
        /// </summary>
        /// <param name="box1">box1, box2 two boxes</param>
        /// <param name="box2">box1, box2 two boxes</param>
        /// <param name="parea"> parea the number of pixels in the overlap</param>
        /// <returns>0 if OK, 1 on error.</returns>
        public static bool boxOverlapArea(this Box box1, Box box2, out int parea)
        {
            if (null == box1)
            {
                parea = 0;
                return false;
            }
            if (null == box2)
            {
                parea = 0;
                return false;
            }

            return Native.DllImports.boxOverlapArea((HandleRef)box1, (HandleRef)box2, out parea) == 0;
        }

        /// <summary>
        ///  <pre>
        ///  Notes:
        ///       (1) For all n(n-1)/2 box pairings, if two boxes overlap, either:
        ///           (a) op == L_COMBINE: get the bounding region for the two,
        ///               replace the larger with the bounding region, and remove
        ///               the smaller of the two, or
        ///           (b) op == L_REMOVE_SMALL: just remove the smaller.
        ///       (2) If boxas is 2D sorted, range can be small, but if it is
        ///           not spatially sorted, range should be large to allow all
        ///  pairwise comparisons to be made.
        ///       (3) The %min_overlap parameter allows ignoring small overlaps.
        ///           If %min_overlap == 1.0, only boxes fully contained in larger
        /// boxes can be considered for removal; if %min_overlap == 0.0,
        ///           this constraint is ignored.
        ///       (4) The %max_ratio parameter allows ignoring overlaps between
        ///           boxes that are not too different in size.If %max_ratio == 0.0,
        ///           no boxes can be removed; if %max_ratio == 1.0, this constraint
        ///           is ignored.
        ///  </pre>
        /// </summary>
        /// <param name="boxas">boxas</param>
        /// <param name="op">op L_COMBINE, L_REMOVE_SMALL</param>
        /// <param name="range">range > 0, forward distance over which overlaps are checked</param>
        /// <param name="min_overlap">min_overlap minimum fraction of smaller box required for overlap to count; 0.0 to ignore</param>
        /// <param name="max_ratio">max_ratio maximum fraction of small/large areas for overlap to count; 1.0 to ignore</param> 
        /// <returns>boxad, or NULL on error.</returns>
        public static Boxa boxaHandleOverlaps(this Boxa boxas, OverlapBoundingBoxFlags op, int range, float min_overlap, float max_ratio)
        {
            IntPtr pnamapIntPtr = IntPtr.Zero;

            if (null == boxas)
            {
                return null;
            }

            var pointer = Native.DllImports.boxaHandleOverlaps((HandleRef)boxas, op, range, min_overlap, max_ratio, out pnamapIntPtr);
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
        ///      (1) This measures horizontal and vertical separation of the
        ///          two boxes.If the boxes are touching but have no pixels
        ///          in common, the separation is 0.  If the boxes overlap by
        /// a distance d, the returned separation is -d.
        /// </summary>
        /// <param name="box1">box1, box2 two boxes, in any order</param>
        /// <param name="box2">box1, box2 two boxes, in any order</param>
        /// <param name="ph_sep">ph_sep [optional] horizontal separation</param>
        /// <param name="pv_sep">pv_sep [optional] vertical separation</param>
        /// <returns>0 if OK, 1 on error</returns>
        public static bool boxSeparationDistance(this Box box1, Box box2, out int ph_sep, out int pv_sep)
        {
            if (null == box1)
            {
                ph_sep = 0;
                pv_sep = 0;
                return false;
            }

            if (null == box2)
            {
                ph_sep = 0;
                pv_sep = 0;
                return false;
            }

            return Native.DllImports.boxSeparationDistance((HandleRef)box1, (HandleRef)box2, out ph_sep, out pv_sep) == 0;
        }

        /// <summary>
        /// boxContainsPt()
        /// </summary>
        /// <param name="box">box</param>
        /// <param name="x">x, y a point</param>
        /// <param name="y">x, y a point</param>
        /// <param name="pcontains">pcontains 1 if box contains point; 0 otherwise</param>
        /// <returns>0 if OK, 1 on error.</returns>
        public static bool boxContainsPt(this Box box, float x, float y, out bool pcontains)
        {
            if (null == box)
            {
                pcontains = false;
                return false;
            }

            return Native.DllImports.boxContainsPt((HandleRef)box, x, y, out pcontains) == 0;
        }

        /// <summary>
        /// (1) Uses euclidean distance between centroid and point.
        /// </summary>
        /// <param name="boxa">boxa</param>
        /// <param name="x">x, y  point</param>
        /// <param name="y">x, y  point</param>
        /// <returns>box with centroid closest to the given point [x,y],or NULL if no boxes in boxa</returns>
        public static Box boxaGetNearestToPt(this Boxa boxa, int x, int y)
        {
            if (null == boxa)
            {
                return null;
            }

            var pointer = Native.DllImports.boxaGetNearestToPt((HandleRef)boxa, x, y);
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
        ///       (1) If the intersection is at only one point(a corner), the
        /// coordinates are returned in (x1, y1).
        ///       (2) Represent a vertical line by one with a large but finite slope.
        ///  </pre>
        /// </summary>
        /// <param name="box">box</param>
        /// <param name="x">x, y point that line goes through</param>
        /// <param name="y">x, y point that line goes through</param>
        /// <param name="slope">slope of line</param>
        /// <param name="px1">px1, py1 1st point of intersection with box</param>
        /// <param name="py1">px1, py1 1st point of intersection with box</param>
        /// <param name="px2">px2, py2 2nd point of intersection with box</param>
        /// <param name="py2">px2, py2 2nd point of intersection with box</param>
        /// <param name="pn">pn number of points of intersection</param>
        /// <returns>0 if OK, 1 on error</returns>
        public static bool boxIntersectByLine(this Box box, int x, int y, float slope, out int px1, out int py1, out int px2, out int py2, out int pn)
        {
            if (null == box)
            {
                px1 = 0;
                py1 = 0;
                px2 = 0;
                py2 = 0;
                pn = 0;
                return false;
            }

            return Native.DllImports.boxIntersectByLine((HandleRef)box, x, y, slope, out px1, out py1, out px2, out py2, out pn) == 0;
        }

        /// <summary>
        /// boxGetCenter()
        /// </summary>
        /// <param name="box">box</param>
        /// <param name="pcx">pcx, pcy location of center of box</param>
        /// <param name="pcy">pcx, pcy location of center of box</param>
        /// <returns>0 if OK, 1 on error</returns>
        public static bool boxGetCenter(this Box box, out float pcx, out float pcy)
        {
            if (null == box)
            {
                pcx = 0;
                pcy = 0;
                return false;
            }

            return Native.DllImports.boxGetCenter((HandleRef)box, out pcx, out pcy) == 0;
        }

        /// <summary>
        /// Notes:
        ///      (1) This can be used to clip a rectangle to an image.
        /// The clipping rectangle is assumed to have a UL corner at(0, 0),
        ///          and a LR corner at(wi - 1, hi - 1).
        /// </summary>
        /// <param name="box">box</param>
        /// <param name="wi">wi, hi rectangle representing image</param>
        /// <param name="hi">wi, hi rectangle representing image</param>
        /// <returns>part of box within given rectangle, or NULL on error or if box is entirely outside the rectangle</returns>
        public static Box boxClipToRectangle(this Box box, int wi, int hi)
        {
            if (null == box)
            {
                return null;
            }

            var pointer = Native.DllImports.boxClipToRectangle((HandleRef)box, wi, hi);
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
        /// <pre>
        /// Notes:
        ///      (1) The return value should be checked.  If it is 1, the
        ///          returned parameter values are bogus.
        ///      (2) This simplifies the selection of pixel locations within
        ///          a given rectangle:
        ///             for (i = ystart; i yend; i++ {
        ///                 ...
        ///                 for (j = xstart; j xend; j++ {
        ///                     ....
        /// </pre>
        /// </summary>
        /// <param name="box">box [optional] requested box; can be null</param>
        /// <param name="w">w, h clipping box size; typ. the size of an image</param>
        /// <param name="h"></param>
        /// <param name="pxstart">pxstart start x coordinate</param>
        /// <param name="pystart">pxstart start y coordinate</param>
        /// <param name="pxend">pxend one pixel beyond clipping box</param>
        /// <param name="pyend">pyend one pixel beyond clipping box</param>
        /// <param name="pbw">pbw [optional] clipped width</param>
        /// <param name="pbh">pbh [optional] clipped height</param>
        /// <returns></returns>
        public static bool boxClipToRectangleParams(this Box box, int w, int h, out int pxstart, out int pystart, out int pxend, out int pyend, out int pbw, out int pbh)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <pre>
        /// Notes:
        ///      (1) Set boxd == NULL to get new box; boxd == boxs for in-place;
        ///          or otherwise to resize existing boxd.
        ///      (2) For usage, suggest one of these:
        ///               boxd = boxRelocateOneSide(NULL, boxs, ...);   // new
        ///               boxRelocateOneSide(boxs, boxs, ...);          // in-place
        ///               boxRelocateOneSide(boxd, boxs, ...);          // other
        /// </pre>
        /// </summary>
        /// <param name="boxd">boxd [optional]; this can be null, equal to boxs, or different from boxs;</param>
        /// <param name="boxs">boxs starting box; to have one side relocated</param>
        /// <param name="loc">loc new location of the side that is changing</param>
        /// <param name="sideflag">sideflag L_FROM_LEFT, etc., indicating the side that moves</param>
        /// <returns>boxd, or NULL on error or if the computed boxd has width or height <= 0.</returns>
        public static Box boxRelocateOneSide(this Box boxd, Box boxs, int loc, ScanDirectionFlags sideflag)
        {
            if (null == boxs)
            {
                return null;
            }

            var pointer = Native.DllImports.boxRelocateOneSide((HandleRef)boxd, (HandleRef)boxs, loc, sideflag);
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
        /// Notes:
        ///      (1) Set boxd == NULL to get new box; boxd == boxs for in-place;
        ///          or otherwise to resize existing boxd.
        ///      (2) For usage, suggest one of these:
        ///               boxd = boxAdjustSides(NULL, boxs, ...);   // new
        ///               boxAdjustSides(boxs, boxs, ...);          // in-place
        ///               boxAdjustSides(boxd, boxs, ...);          // other
        ///      (3) New box dimensions are cropped at left and top to x >= 0 and y >= 0.
        ///      (4) For example, to expand in-place by 20 pixels on each side, use
        ///             boxAdjustSides(box, box, -20, 20, -20, 20);
        /// </summary>
        /// <param name="boxd">boxd  [optional]; this can be null, equal to boxs, or different from boxs</param>
        /// <param name="boxs"> boxs  starting box; to have sides adjusted</param>
        /// <param name="delleft">delleft, delright, deltop, delbot changes in location of each side</param>
        /// <param name="delright">delleft, delright, deltop, delbot changes in location of each side</param>
        /// <param name="deltop">delleft, delright, deltop, delbot changes in location of each side</param>
        /// <param name="delbot">delleft, delright, deltop, delbot changes in location of each side</param>
        /// <returns>boxd, or NULL on error or if the computed boxd has width or height <= 0.</returns>
        public static Box boxAdjustSides(this Box boxd, Box boxs, int delleft, int delright, int deltop, int delbot)
        {
            if (null == boxs)
            {
                return null;
            }

            var pointer = Native.DllImports.boxAdjustSides((HandleRef)boxd, (HandleRef)boxs, delleft, delright, deltop, delbot);
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
        ///       (1) Sets the given side of each box.Use boxad == NULL for a new
        /// 
        /// boxa, and boxad == boxas for in-place.
        /// 
        ///      (2) Use one of these:
        ///                boxad = boxaSetSide(NULL, boxas, ...);   // new
        ///                boxaSetSide(boxas, boxas, ...);  // in-place
        ///  </pre>
        /// </summary>
        /// <param name="boxad">boxad use NULL to get a new one; same as boxas for in-place</param>
        /// <param name="boxas">boxas</param>
        /// <param name="side">side L_SET_LEFT, L_SET_RIGHT, L_SET_TOP, L_SET_BOT</param>
        /// <param name="val">val location to set for given side, for each box</param>
        /// <param name="thresh">thresh min abs difference to cause resetting to %val</param>
        /// <returns>boxad, or NULL on error</returns>
        public static Boxa boxaSetSide(this Boxa boxad, Boxa boxas, BoxSizeAdjustmentLocationFlags side, int val, int thresh)
        {
            if (null == boxas)
            {
                return null;
            }

            var pointer = Native.DllImports.boxaSetSide((HandleRef)boxad, (HandleRef)boxas, side, val, thresh);
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
        ///  Notes:
        ///       (1) Conditionally adjusts the width of each box, by moving
        ///           the indicated edges(left and/or right) if the width differs
        /// by %thresh or more from %target.
        /// 
        ///      (2) Use boxad == NULL for a new boxa, and boxad == boxas for in-place.
        /// 
        /// Use one of these:
        ///                boxad = boxaAdjustWidthToTarget(NULL, boxas, ...);   // new
        ///                boxaAdjustWidthToTarget(boxas, boxas, ...);  // in-place
        ///  </pre> 
        /// </summary>
        /// <param name="boxad">boxad use NULL to get a new one; same as boxas for in-place</param>
        /// <param name="boxas">boxas</param>
        /// <param name="sides">sides L_ADJUST_LEFT, L_ADJUST_RIGHT, L_ADJUST_LEFT_AND_RIGHT</param>
        /// <param name="target">target target width if differs by more than thresh</param>
        /// <param name="thresh">thresh min abs difference in width to cause adjustment</param>
        /// <returns>boxad, or NULL on error</returns>
        public static Boxa boxaAdjustWidthToTarget(this Boxa boxad, Boxa boxas, BoxSizeAdjustmentLocationFlags sides, int target, int thresh)
        {
            if (null == boxas)
            {
                return null;
            }

            var pointer = Native.DllImports.boxaAdjustWidthToTarget((HandleRef)boxad, (HandleRef)boxas, sides, target, thresh);
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
        ///  Notes:
        ///       (1) Conditionally adjusts the height of each box, by moving
        ///           the indicated edges(top and/or bot) if the height differs
        /// by %thresh or more from %target.
        /// 
        ///      (2) Use boxad == NULL for a new boxa, and boxad == boxas for in-place.
        /// 
        /// Use one of these:
        ///                boxad = boxaAdjustHeightToTarget(NULL, boxas, ...);   // new
        ///                boxaAdjustHeightToTarget(boxas, boxas, ...);  // in-place
        ///  </pre> 
        /// </summary>
        /// <param name="boxad">boxad use NULL to get a new one</param>
        /// <param name="boxas">boxas</param>
        /// <param name="sides">sides L_ADJUST_TOP, L_ADJUST_BOT, L_ADJUST_TOP_AND_BOT</param>
        /// <param name="target">target target height if differs by more than thresh</param>
        /// <param name="thresh">thresh min abs difference in height to cause adjustment</param>
        /// <returns>boxad, or NULL on error</returns>
        public static Boxa boxaAdjustHeightToTarget(this Boxa boxad, Boxa boxas, BoxSizeAdjustmentLocationFlags sides, int target, int thresh)
        {
            if (null == boxas)
            {
                return null;
            }

            var pointer = Native.DllImports.boxaAdjustHeightToTarget((HandleRef)boxad, (HandleRef)boxas, sides, target, thresh);
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
        /// boxEqual()
        /// </summary>
        /// <param name="box1">box1</param>
        /// <param name="box2">box2</param>
        /// <param name="psame">psame 1 if equal; 0 otherwise</param>
        /// <returns>0 if OK, 1 on error</returns>
        public static bool boxEqual(this Box box1, Box box2, out bool psame)
        {
            if (null == box1)
            {
                psame = false;
                return false;
            }
            if (null == box2)
            {
                psame = false;
                return false;
            }

            return Native.DllImports.boxEqual((HandleRef)box1, (HandleRef)box1, out psame) == 0;
        }

        /// <summary>
        ///  <pre>
        ///  Notes:
        ///       (1) The two boxa are the "same" if they contain the same
        ///           boxes and each box is within %maxdist of its counterpart
        ///           in their positions within the boxa.This allows for
        ///           small rearrangements.Use 0 for maxdist if the boxa
        ///           must be identical.
        ///       (2) This applies only to geometry and ordering; refcounts
        ///  are not considered.
        ///       (3) %maxdist allows some latitude in the ordering of the boxes.
        /// 
        /// For the boxa to be the "same", corresponding boxes must
        /// be within %maxdist of each other.Note that for large
        ///          %maxdist, we should use a hash function for efficiency.
        /// 
        ///      (4) naindex[i] gives the position of the box in boxa2 that
        ///           corresponds to box i in boxa1.It is only returned if the
        /// boxa are equal.
        ///  </pre>
        /// </summary>
        /// <param name="boxa1">boxa1</param>
        /// <param name="boxa2">boxa2</param>
        /// <param name="maxdist">maxdist</param>
        /// <param name="pnaindex">pnaindex [optional] index array of correspondences</param>
        /// <param name="psame">psame (1 if equal; 0 otherwise</param>
        /// <returns>0 if OK, 1 on error</returns>
        public static bool boxaEqual(this Boxa boxa1, Boxa boxa2, int maxdist, out Numa pnaindex, out bool psame)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <pre>
        /// Notes:
        /// (1) The values of leftdiff(etc) are the maximum allowed deviations
        /// between the locations of the left(etc) sides.If any side
        /// pairs differ by more than this amount, the boxes are not similar.
        ///
        /// </pre>
        /// </summary>
        /// <param name="box1">box1</param>
        /// <param name="box2">box2</param>
        /// <param name="leftdiff">leftdiff, rightdiff, topdiff, botdif</param>
        /// <param name="rightdiff">leftdiff, rightdiff, topdiff, botdif</param>
        /// <param name="topdiff">leftdiff, rightdiff, topdiff, botdif</param>
        /// <param name="botdiff">leftdiff, rightdiff, topdiff, botdif</param>
        /// <param name="psimilar"><psimilar 1 if similar; 0 otherwise/param>
        /// <returns> 0 if OK, 1 on error</returns>
        public static bool boxSimilar(this Box box1, Box box2, int leftdiff, int rightdiff, int topdiff, int botdiff, out bool psimilar)
        {
            if (null == box1)
            {
                psimilar = false;
                return false;
            }
            if (null == box2)
            {
                psimilar = false;
                return false;
            }

            return Native.DllImports.boxSimilar((HandleRef)box1, (HandleRef)box1, leftdiff, rightdiff, topdiff, botdiff, out psimilar) == 0;
        }

        /// <summary>
        ///       (1) See boxSimilar() for parameter usage.
        ///       (2) Corresponding boxes are taken in order in the two boxa.
        ///       (3) %nasim is an indicator array with a(0/1) for each box pair.
        /// 
        ///      (4) With %nasim or debug == 1, boxes continue to be tested
        /// after failure.
        /// </summary>
        /// <param name="boxa1">boxa1</param>
        /// <param name="boxa2">boxa2</param>
        /// <param name="leftdiff">leftdiff, rightdiff, topdiff, botdiff</param>
        /// <param name="rightdiff">leftdiff, rightdiff, topdiff, botdiff</param>
        /// <param name="topdiff">leftdiff, rightdiff, topdiff, botdiff</param>
        /// <param name="botdiff">leftdiff, rightdiff, topdiff, botdiff</param>
        /// <param name="debug">debug output details of non-similar boxes</param>
        /// <param name="psimilar">psimilar 1 if similar; 0 otherwise</param>
        /// <param name="pnasim">pnasim [optional] na containing 1 if similar; else 0</param>
        /// <returns>0 if OK, 1 on error</returns>
        public static bool boxaSimilar(this Boxa boxa1, Boxa boxa2, int leftdiff, int rightdiff, int topdiff, int botdiff, int debug, out bool psimilar, out Numa pnasim)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Notes:
        ///      (1) This appends a clone of each indicated box in boxas to boxad
        ///      (2) istart< 0 is taken to mean 'read from the start' (istart = 0)
        ///      (3) iend< 0 means 'read to the end'
        ///      (4) if boxas == NULL or has no boxes, this is a no-op.
        /// </summary>
        /// <param name="boxad">boxad  dest boxa; add to this one</param>
        /// <param name="boxas">boxas  source boxa; add from this one</param>
        /// <param name="istart">istart  starting index in boxas</param>
        /// <param name="iend">iend  ending index in boxas; use -1 to cat all</param>
        /// <returns>0 if OK, 1 on error</returns>
        public static bool boxaJoin(this Boxa boxad, Boxa boxas, int istart, int iend)
        {
            if (null == boxad)
            { 
                return false;
            }
            if (null == boxas)
            { 
                return false;
            }

            return Native.DllImports.boxaJoin((HandleRef)boxad, (HandleRef)boxas, istart, iend) == 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baad"></param>
        /// <param name="baas"></param>
        /// <param name="istart"></param>
        /// <param name="iend"></param>
        /// <returns></returns>
        public static bool boxaaJoin(this Boxaa baad, Boxaa baas, int istart, int iend)
        {
            if (null == baad)
            {
                return false;
            }
            if (null == baas)
            {
                return false;
            }

            return Native.DllImports.boxaaJoin((HandleRef)baad, (HandleRef)baas, istart, iend) == 0;
        }

        /// <summary>
        /// <pre>
        /// Notes:
        ///      (1) If %fillflag == 1, boxae has copies of the even boxes
        ///          in their original location, and nvalid boxes are placed
        ///          in the odd array locations.And v.v.
        ///      (2) If %fillflag == 0, boxae has only copies of the even boxes.
        /// </pre>
        /// </summary>
        /// <param name="boxa">boxa</param>
        /// <param name="fillflag">fillflag 1 to put invalid boxes in place; 0 to omit</param>
        /// <param name="pboxae">pboxae, pboxao save even and odd boxes in their separate boxa, setting the other type to invalid boxes.</param>
        /// <param name="pboxao">pboxae, pboxao save even and odd boxes in their separate boxa, setting the other type to invalid boxes.</param>
        /// <returns>0 if OK, 1 on error</returns>
        public static bool boxaSplitEvenOdd(this Boxa boxa, int fillflag, out Boxa pboxae, out Boxa pboxao)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <pre>
        /// Notes:
        ///      (1) This is essentially the inverse of boxaSplitEvenOdd().
        ///          Typically, boxae and boxao were generated by boxaSplitEvenOdd(),
        ///          and the value of %fillflag needs to be the same in both calls.
        ///      (2) If %fillflag == 1, both boxae and boxao are of the same size;
        ///          otherwise boxae may have one more box than boxao.
        /// </pre>
        /// </summary>
        /// <param name="boxae">boxae boxes to go in even positions in merged boxa</param>
        /// <param name="boxao">boxao boxes to go in odd positions in merged boxa</param>
        /// <param name="fillflag">fillflag 1 if there are invalid boxes in placeholders</param>
        /// <returns> boxad merged, or NULL on error</returns>
        public static Boxa boxaMergeEvenOdd(this Boxa boxae, Boxa boxao, int fillflag)
        {
            if (null == boxae)
            {
                return null;
            }
            if (null == boxao)
            {
                return null;
            }

            var pointer = Native.DllImports.boxaMergeEvenOdd((HandleRef)boxae, (HandleRef)boxao, fillflag);
            if (IntPtr.Zero != pointer)
            {
                return new Boxa(pointer);
            }
            else
            {
                return null;
            }
        }
    }
}
