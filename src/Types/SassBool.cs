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

        IntPtr ISassExportableType.GetInternalTypePtr()
        {
            if (_primitiveValue)
            {
                if (_trueValue != default(IntPtr))
                    return _trueValue;

                return _trueValue = SassCompiler.sass_make_boolean(true);
            }

            if (_falseValue != default(IntPtr))
                return _falseValue;

            return _falseValue = SassCompiler.sass_make_boolean(false);

        }
    }
}
