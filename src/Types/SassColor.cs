using System;

namespace Sass.Types
{
    public class SassColor : ISassType, ISassExportableType
    {
        public double Red { get; set; } = 0;
        public double Green { get; set; } = 0;
        public double Blue { get; set; } = 0;
        public double Alpha { get; set; } = 1;

        private IntPtr CachedPtr { get; set; }

        public override string ToString()
        {
            return $"{Alpha}{Red}{Green}{Blue}";
        }

        IntPtr ISassExportableType.GetInternalTypePtr()
        {
            if (CachedPtr != default(IntPtr))
                return CachedPtr;

            return CachedPtr = SassCompiler.sass_make_color(Red, Green, Blue, Alpha);
        }
    }
}
