using System;

namespace Sass.Types
{
    public class SassNumber : ISassType, ISassExportableType
    {
        public double Value { get; set; }
        public SassUnit Unit { get; set; }

        private IntPtr _cachedPtr;

        public override string ToString()
        {
            return $"{Value}{Unit}";
        }

        IntPtr ISassExportableType.GetInternalTypePtr(InternalPtrValidityEventHandler validityEventHandler)
        {
            if (_cachedPtr != default(IntPtr))
                return _cachedPtr;

            validityEventHandler += (this as ISassExportableType).OnInvalidated;
            return _cachedPtr = SassCompiler.sass_make_number(Value, Unit.ToString());
        }

        void ISassExportableType.OnInvalidated()
        {
            _cachedPtr = default(IntPtr);
        }
    }
}
