using System;

namespace Sass.Types
{
    public class SassNumber : ISassType, ISassExportableType
    {
        public double Value { get; set; }
        public SassUnit Unit { get; set; }

        private IntPtr _cachedPtr { get; set; }

        public override string ToString()
        {
            return $"{Value}{Unit}";
        }

        IntPtr ISassExportableType.GetInternalTypePtr()
        {
            if (_cachedPtr != null)
                return _cachedPtr;

            return _cachedPtr = SassCompiler.sass_make_number(Value, Unit.ToString());
        }
    }
}
