using System; 

namespace Leptonica
{
    /// <summary>
    /// Pix with float array
    /// </summary>
    public class FPix : LeptonicaObjectBase
    {
        internal FPix(IntPtr pointer) : base(pointer) { }
    }
}
