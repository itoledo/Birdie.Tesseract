using System; 

namespace Leptonica 
{
    /// <summary>
    /// BMP file header
    /// </summary>
    public class BMP_FileHeader : LeptonicaObjectBase
    {
        internal BMP_FileHeader(IntPtr pointer) : base(pointer) { }
    }
}
