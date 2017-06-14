using System; 

namespace Leptonica 
{
    /// <summary>
    /// hide underlying implementation for set
    /// </summary>
    public class L_ASet : L_Rbtree
    {
        internal L_ASet(IntPtr pointer) : base(pointer) { }
    }
}
