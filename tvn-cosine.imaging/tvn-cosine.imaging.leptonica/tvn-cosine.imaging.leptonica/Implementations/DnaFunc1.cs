using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leptonica.Implementations
{
    public static class DnaFunc1
    {
        // Rearrangements
       public static  int l_dnaJoin(this L_Dna dad, L_Dna das, int istart, int iend);
       public static  L_Dna l_dnaaFlattenToDna(L_DNAA* daa);

        // Conversion between numa and dna
       public static  Numa  l_dnaConvertToNuma(this L_Dna da);
       public static  L_Dna numaConvertToDna(NUMA* na);

        // Set operations using aset (rbtree)
       public static  L_Dna l_dnaUnionByAset(this L_Dna da1, L_Dna da2);
       public static  L_Dna l_dnaRemoveDupsByAset(this L_Dna das);
       public static  L_Dna l_dnaIntersectionByAset(this L_Dna da1, L_Dna da2);
       public static  L_ASet  l_asetCreateFromDna(this L_Dna da);

        // Miscellaneous operations
       public static  L_Dna l_dnaDiffAdjValues(this L_Dna das);
    }
}
