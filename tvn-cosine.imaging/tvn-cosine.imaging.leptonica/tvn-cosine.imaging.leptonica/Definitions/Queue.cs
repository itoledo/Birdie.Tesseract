using System;

namespace Leptonica
{
    /// <summary>
    /// Expandable pointer queue for arbitrary void* data
    /// </summary>
    public class L_Queue : LeptonicaObjectBase
    {
        internal L_Queue(IntPtr pointer) : base(pointer) { }
    }
}
