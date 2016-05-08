using System;

namespace Sass.Types
{
    internal class SassNull : ISassType, ISassExportableType
    {
        private static IntPtr _cachedPtr;

        IntPtr ISassExportableType.GetInternalTypePtr(InternalPtrValidityEventHandler validityEventHandler)
        {
            if (_cachedPtr != default(IntPtr))
                return _cachedPtr;

            validityEventHandler += (this as ISassExportableType).OnInvalidated;
            return _cachedPtr = SassCompiler.sass_make_null();
        }

        private SassNull()
        { }

        void ISassExportableType.OnInvalidated()
        {
            _cachedPtr = default(IntPtr);
        }
    }
}
