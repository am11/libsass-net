using System;

namespace Sass.Types
{
    public class SassBool : ISassType, ISassExportableType
    {
        private readonly bool _primitiveValue;
        private IntPtr _trueValue;
        private IntPtr _falseValue;
        private static SassBool _trueInstance;
        private static SassBool _falseInstance;

        public static SassBool True => _trueInstance ?? (_trueInstance = new SassBool(true));

        public static SassBool False => _falseInstance ?? (_falseInstance = new SassBool(false));

        private SassBool(bool value)
        {
            _primitiveValue = value;
        }

        IntPtr ISassExportableType.GetInternalTypePtr(InternalPtrValidityEventHandler validityEventHandler)
        {
            IntPtr returnValue;

            if (_primitiveValue)
            {
                if (_trueValue != default(IntPtr))
                    return _trueValue;

                returnValue = _trueValue = SassCompiler.sass_make_boolean(true);
            }
            else
            {
                if (_falseValue != default(IntPtr))
                    return _falseValue;

                returnValue = _falseValue = SassCompiler.sass_make_boolean(false);
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
