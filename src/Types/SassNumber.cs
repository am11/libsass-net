using System;
using Sass.Compiler.Context;
using static Sass.Compiler.SassExterns;
using static Sass.Compiler.Context.SassSafeContextHandle;

namespace Sass.Types
{
    public class SassNumber : ISassType, ISassExportableType
    {
        private IntPtr _cachedPtr;
        public double Value { get; set; }

        /// <summary>
        /// size units:
        /// In, Cm, Pc, Mm, Pt, Px,
        ///
        /// angle units
        /// Deg, Grad, Rad, Turn,
        ///
        /// time units 
        /// Sec, Msec,
        ///
        /// frequency units 
        /// Hertz, Khertz,
        ///
        /// resolutions units 
        /// Dpi, Dpcm, Dppx
        /// </summary>
        public string Unit { get; set; }

        internal SassNumber(IntPtr rawPointer)
        {
            Value = sass_number_get_value(rawPointer);
            Unit = PtrToString(sass_number_get_unit(rawPointer));
        }

        public override string ToString()
        {
            return $"{Value}{Unit}";
        }

        IntPtr ISassExportableType.GetInternalTypePtr(InternalPtrValidityEventHandler validityEventHandler)
        {
            if (_cachedPtr != default(IntPtr))
                return _cachedPtr;

            validityEventHandler += (this as ISassExportableType).OnInvalidated;
            return _cachedPtr = sass_make_number(Value, Unit.ToString());
        }

        void ISassExportableType.OnInvalidated()
        {
            _cachedPtr = default(IntPtr);
        }
    }
}
