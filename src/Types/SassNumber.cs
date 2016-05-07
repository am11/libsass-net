using System;

namespace Sass.Types
{
    public class SassNumber : ISassType, ISassExportableType
    {
        public double Value { get; set; }
        public SassUnit Unit { get; set; }

        private IntPtr CachedPtr { get; set; }

        public override string ToString()
        {
            return $"{Value}{Unit}";
        }

        IntPtr ISassExportableType.GetInternalTypePtr()
        {
            if (CachedPtr != default(IntPtr))
                return CachedPtr;

            return CachedPtr = SassCompiler.sass_make_number(Value, Unit.ToString());
        }
    }
}
