using System; 

namespace Leptonica 
{
    /// <summary>
    /// hide tree implementation
    /// </summary>
    public class L_AMap_Node : L_Rbtree_Node
    {
        internal L_AMap_Node(IntPtr pointer) : base(pointer) { }
    }
}
