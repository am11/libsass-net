using System;
using Sass.Compiler.Context;
using static Sass.Compiler.SassExterns;

namespace Sass.Types
{
    internal class SassNull : ISassType, ISassExportableType
    {
        private static IntPtr _cachedPtr;
        private static SassNull _instance;

        IntPtr ISassExportableType.GetInternalTypePtr(InternalPtrValidityEventHandler validityEventHandler, bool dontCache)
        {
            if (_cachedPtr != default(IntPtr))
                return _cachedPtr;

            if (!dontCache)
                validityEventHandler += (this as ISassExportableType).OnInvalidated;

            return _cachedPtr = sass_make_null();
        }

        internal static SassNull Instance => _instance ?? (_instance = new SassNull());

        private SassNull()
        { }

        void ISassExportableType.OnInvalidated()
        {
            _cachedPtr = default(IntPtr);
        }
    }
}
