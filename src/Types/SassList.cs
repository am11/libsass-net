using System;
using System.Collections.Generic;
using System.Linq;

namespace Sass.Types
{
    public class SassList : ISassType, ISassExportableType
    {
        public List<ISassType> Values { get; set; } = new List<ISassType>();
        public SassListSeparator Separator { get; set; } = SassListSeparator.Space;

        private bool _ensured;
        private IntPtr _cachedPtr;

        /// <summary>
        /// Recursively ensures:
        /// * Arbitrary ISassType implementation.
        /// * Circular reference on each "listy" item stored in the values.
        /// </summary>
        /// <param name="lists">List containing instances of SassList</param>
        private void WalkAndEnsureDependencies(List<SassList> lists)
        {
            // Prevent from re-entrance.
            if (_ensured)
                return;

            // FIXME: Should we instead loop through the array and
            //        report the exact index which violates this rule?
            if (!Values.All(v => v is ISassExportableType))
                throw new SassTypeException(SassTypeException.ArbitraryInterfaceImplmentationMessage);

            // Detect the circular-referencing values.
            lists.Add(this);

            var filteredValues = Values.OfType<SassList>().ToList();

            if (filteredValues.Any(v => lists.Contains(v)))
                throw new SassTypeException(SassTypeException.CircularReferenceMessage);

            filteredValues.ForEach(v => v.WalkAndEnsureDependencies(lists));

            _ensured = true;
        }

        public override string ToString()
        {
            return string.Join(Separator.ToString(),
                               Values.Select(v => v.ToString()));
        }

        IntPtr ISassExportableType.GetInternalTypePtr()
        {
            if (_cachedPtr != null)
                return _cachedPtr;

            WalkAndEnsureDependencies(new List<SassList>());

            var list = SassCompiler.sass_make_list(Values.Count, Separator);

            for (int index = 0; index < Values.Count; ++index)
            {
                var exportableValue = (Values[index] as ISassExportableType);

                SassCompiler.sass_list_set_value(
                    list, index, exportableValue.GetInternalTypePtr());
            }

            return _cachedPtr = list;
        }
    }
}
