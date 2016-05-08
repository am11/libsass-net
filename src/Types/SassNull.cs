using System;

namespace Sass.Types
{
    internal class SassNull : ISassType
    {
        private static IntPtr _value;

        internal static IntPtr Value
        {
            get
            {
                if (_value != default(IntPtr))
                    return _value;

                return _value = SassCompiler.sass_make_null();
            }
        }

        private SassNull()
        { }
    }
}
