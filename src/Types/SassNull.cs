using System;
using Sass.Compiler;
using Sass.Compiler.Context;
using static Sass.Compiler.SassExterns;

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
            return _cachedPtr = sass_make_null();
        }

        private SassNull()
        { }

        void ISassExportableType.OnInvalidated()
        {
            _cachedPtr = default(IntPtr);
        }
    }
}
