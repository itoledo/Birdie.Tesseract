using System; 

namespace Leptonica
{
    /// <summary>
    /// Basic rectangle
    /// </summary>
    public class Box : LeptonicaObjectBase
    {
        internal Box(IntPtr pointer) : base(pointer) { }
    }
}
