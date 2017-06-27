using System; 

namespace Leptonica
{
    /// <summary>
    /// Basic Pix
    /// </summary>
    public class Pix : LeptonicaObjectBase
    {
        internal Pix(IntPtr pointer) : base(pointer) { }

        public static explicit operator Pix(IntPtr pointer)
        {
            return new Pix(pointer);
        }

        public int Width
        {
            get
            {
                return this.pixGetWidth();
            }
            set
            {
                this.pixSetWidth(value);
            }
        }
    }
}
