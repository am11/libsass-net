using System;
using Sass.Compiler.Context;
using static Sass.Compiler.Context.SassSafeContextHandle;

namespace Sass.Types
{
    public class SassString : ISassType, ISassExportableType
    {
        private IntPtr _cachedPtr;
        public string Value { get; set; }

        public SassString(string value)
        {
            Value = value;
        }

        internal SassString(IntPtr rawPointer)
        {
            Value = PtrToString(rawPointer);
        }

        IntPtr ISassExportableType.GetInternalTypePtr(InternalPtrValidityEventHandler validityEventHandler)
        {
            if (_cachedPtr != default(IntPtr))
                return _cachedPtr;

            validityEventHandler += (this as ISassExportableType).OnInvalidated;
            return _cachedPtr = EncodeAsUtf8IntPtr(Value);
        }

        void ISassExportableType.OnInvalidated()
        {
            _cachedPtr = default(IntPtr);
        }
    }
}
