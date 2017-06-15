using System; 

namespace Leptonica 
{
    /// <summary>
    /// Bmp Info header
    /// </summary>
    public class BMP_InfoHeader : LeptonicaObjectBase
    {
        internal BMP_InfoHeader(IntPtr pointer) : base(pointer) { }
    }
}
