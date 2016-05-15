using System;
using Sass.Compiler.Context;
using static Sass.Compiler.SassExterns;
using static Sass.Compiler.Context.SassSafeContextHandle;

namespace Sass.Types
{
    public class SassWarning : ISassType, ISassExportableType
    {
        private IntPtr _cachedPtr;
        public string Message { get; set; }

        public SassWarning(string message)
        {
            Message = message;
        }

        internal SassWarning(IntPtr rawPointer)
        {
            IntPtr rawValue = sass_warning_get_message(rawPointer);
            Message = PtrToString(rawValue);
        }

        IntPtr ISassExportableType.GetInternalTypePtr(InternalPtrValidityEventHandler validityEventHandler)
        {
            if (_cachedPtr != default(IntPtr))
                return _cachedPtr;

            validityEventHandler += (this as ISassExportableType).OnInvalidated;

            return _cachedPtr = sass_make_warning(new SassSafeStringHandle(Message));
        }

        void ISassExportableType.OnInvalidated()
        {
            _cachedPtr = default(IntPtr);
        }
    }
}
