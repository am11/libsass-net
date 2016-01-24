using System;

namespace Sass.Types
{
    public class SassColor: ISassType, ISassExportableType
    {
        public double Value { get; set; }
        public SassUnit Unit { get; set; }

        public override string ToString()
        {
            return $"{Value}{Unit}";
        }

        IntPtr ISassExportableType.GetInternalTypePtr()
        {
            return SassCompiler.sass_make_number(Value, Unit.ToString());
        }
    }
}
