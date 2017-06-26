using System;
using System.Runtime.InteropServices;

namespace Leptonica
{
    public static class Skew
    {
        // Top-level deskew interfaces
         public static  Pix  pixDeskewBoth(this Pix pixs, int redsearch)
        {
            throw new NotImplementedException();
        }

        public static  Pix  pixDeskew(this Pix pixs, int redsearch)
        {
            throw new NotImplementedException();
        }

        public static  Pix  pixFindSkewAndDeskew(this Pix pixs, int redsearch,  out float pangle,  out float pconf)
        {
            throw new NotImplementedException();
        }

        public static  Pix  pixDeskewGeneral(this Pix pixs, int redsweep, float sweeprange, float sweepdelta, int redsearch, int thresh,  out float pangle,  out float pconf)
        {
            throw new NotImplementedException();
        }


        // Top-level angle-finding interface
        public static  int pixFindSkew(this Pix pixs,  out float pangle,  out float pconf)
        {
            throw new NotImplementedException();
        }


        // Basic angle-finding functions
        public static  int pixFindSkewSweep(this Pix pixs,  out float pangle, int reduction, float sweeprange, float sweepdelta)
        {
            throw new NotImplementedException();
        }

        public static  int pixFindSkewSweepAndSearch(this Pix pixs,  out float pangle,  out float pconf, int redsweep, int redsearch, float sweeprange, float sweepdelta, float minbsdelta)
        {
            throw new NotImplementedException();
        }

        public static  int pixFindSkewSweepAndSearchScore(this Pix pixs,  out float pangle,  out float pconf,  out float pendscore, int redsweep, int redsearch, float sweepcenter, float sweeprange, float sweepdelta, float minbsdelta)
        {
            throw new NotImplementedException();
        }

        public static  int pixFindSkewSweepAndSearchScorePivot(this Pix pixs,  out float pangle,  out float pconf,  out float pendscore, int redsweep, int redsearch, float sweepcenter, float sweeprange, float sweepdelta, float minbsdelta, int pivot)
        {
            throw new NotImplementedException();
        }


        // Search over arbitrary range of angles in orthogonal directions
        public static  int pixFindSkewOrthogonalRange(this Pix pixs,  out float pangle,  out float pconf, int redsweep, int redsearch, float sweeprange, float sweepdelta, float minbsdelta, float confprior)
        {
            throw new NotImplementedException();
        }


        // Differential square sum function for scoring
        public static  int pixFindDifferentialSquareSum(this Pix pixs,  out float psum)
        {
            throw new NotImplementedException();
        }


        // Measures of variance of row sums
        public static  int pixFindNormalizedSquareSum(this Pix pixs,  out float phratio,  out float pvratio,  out float pfract)
        {
            throw new NotImplementedException();
        } 
    }
}
