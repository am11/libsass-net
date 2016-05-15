﻿using System;
using Sass.Compiler.Context;
using static Sass.Compiler.SassExterns;
using static Sass.Compiler.Context.SassSafeContextHandle;

namespace Sass.Types
{
    public class SassError : ISassType, ISassExportableType
    {
        private IntPtr _cachedPtr;
        public string Message { get; set; }

        public SassError(string message)
        {
            Message = message;
        }

        internal SassError(IntPtr rawPointer)
        {
            IntPtr rawValue = sass_error_get_message(rawPointer);
            Message = PtrToString(rawValue);
        }

        IntPtr ISassExportableType.GetInternalTypePtr(InternalPtrValidityEventHandler validityEventHandler)
        {
            if (_cachedPtr != default(IntPtr))
                return _cachedPtr;

            validityEventHandler += (this as ISassExportableType).OnInvalidated;

            return _cachedPtr = sass_make_error(new SassSafeStringHandle(Message));
        }

        void ISassExportableType.OnInvalidated()
        {
            _cachedPtr = default(IntPtr);
        }
    }
}
