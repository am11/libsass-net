using System;
using Sass.Compiler.Context;
using static Sass.Compiler.SassExterns;

namespace Sass.Types
{
    public class SassColor : ISassType, ISassExportableType
    {
        private IntPtr _cachedPtr;

        public double Red { get; set; } = 0;
        public double Green { get; set; } = 0;
        public double Blue { get; set; } = 0;
        public double Alpha { get; set; } = 1;

        internal SassColor(IntPtr rawPointer)
        {
            Red = sass_color_get_r(rawPointer);
            Green = sass_color_get_g(rawPointer);
            Blue = sass_color_get_b(rawPointer);
            Alpha = sass_color_get_a(rawPointer);
        }

        public override string ToString()
        {
            double red = Math.Min(Math.Max(Red, 0), 255);
            double green = Math.Min(Math.Max(Green, 0), 255);
            double blue = Math.Min(Math.Max(Blue, 0), 255);
            double alpha = Math.Min(Math.Max(Alpha, 0), 1.0);

            return $"rgba({red},{green},{blue},{alpha})";
        }

        IntPtr ISassExportableType.GetInternalTypePtr(InternalPtrValidityEventHandler validityEventHandler)
        {
            if (_cachedPtr != default(IntPtr))
                return _cachedPtr;

            validityEventHandler += (this as ISassExportableType).OnInvalidated;
            return _cachedPtr = sass_make_color(Red, Green, Blue, Alpha);
        }

        void ISassExportableType.OnInvalidated()
        {
            _cachedPtr = default(IntPtr);
        }
    }
}
