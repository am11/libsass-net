﻿using System;
using Sass.Compiler.Context;
using static Sass.Compiler.SassExterns;

namespace Sass.Types
{
    public class SassBool : ISassType, ISassExportableType
    {
        public bool Value { get; set; }
        private IntPtr _trueValue;
        private IntPtr _falseValue;
        private static SassBool _trueInstance;
        private static SassBool _falseInstance;

        public static SassBool True => _trueInstance ?? (_trueInstance = new SassBool(true));

        public static SassBool False => _falseInstance ?? (_falseInstance = new SassBool(false));

        internal static SassBool GetBoolValue(IntPtr rawPointer)
        {
            return sass_boolean_get_value(rawPointer) ?
                   _trueInstance :
                   _falseInstance;
        }

        private SassBool(bool value)
        {
            Value = value;
        }

        IntPtr ISassExportableType.GetInternalTypePtr(InternalPtrValidityEventHandler validityEventHandler)
        {
            IntPtr returnValue;

            if (Value)
            {
                if (_trueValue != default(IntPtr))
                    return _trueValue;

                returnValue = _trueValue = sass_make_boolean(true);
            }
            else
            {
                if (_falseValue != default(IntPtr))
                    return _falseValue;

                returnValue = _falseValue = sass_make_boolean(false);
            }

            validityEventHandler += (this as ISassExportableType).OnInvalidated;
            return returnValue;
        }

        void ISassExportableType.OnInvalidated()
        {
            _trueValue = default(IntPtr);
            _falseValue = default(IntPtr);
        }
    }
}
