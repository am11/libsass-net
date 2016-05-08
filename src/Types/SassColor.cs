using System;

namespace Sass.Types
{
    public class SassColor : ISassType, ISassExportableType
    {
        public int Red { get; set; } = 0;
        public int Green { get; set; } = 0;
        public int Blue { get; set; } = 0;
        public double Alpha { get; set; } = 1;

        private IntPtr _cachedPtr;

        public override string ToString()
        {
            var red = Math.Min(Math.Max(Red, 0), 255);
            var green = Math.Min(Math.Max(Green, 0), 255);
            var blue = Math.Min(Math.Max(Blue, 0), 255);
            var alpha = Math.Min(Math.Max(Alpha, 0), 1.0);

            return $"rgba({red},{green},{blue},{alpha})";
        }

        IntPtr ISassExportableType.GetInternalTypePtr(InternalPtrValidityEventHandler validityEventHandler)
        {
            if (_cachedPtr != default(IntPtr))
                return _cachedPtr;

            validityEventHandler += (this as ISassExportableType).OnInvalidated;
            return _cachedPtr = SassCompiler.sass_make_color(Red, Green, Blue, Alpha);
        }

        void ISassExportableType.OnInvalidated()
        {
            _cachedPtr = default(IntPtr);
        }
    }
}
