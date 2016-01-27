using System;
using System.Collections.Generic;
using System.Linq;

namespace Sass.Types
{
    public class SassMap : ISassType, ISassExportableType
    {
        public Dictionary<ISassType, ISassType> Values { get; set; } =
            new Dictionary<ISassType, ISassType>();

        private bool _ensured;
        private IntPtr _cachedPtr;

        /// <summary>
        /// Recursively ensures:
        /// * Arbitrary ISassType implementation.
        /// * Circular reference on each "listy" item stored in the values.
        /// * Since this is a map type (with Dictionary types values), we
        ///   need to walk through keys and values for the given input.
        /// </summary>
        /// <param name="list">Dictionary containing instances of SassMap</param>
        private void WalkAndEnsureDependencies(List<SassMap> list)
        {
            // Prevent from re-entrance.
            if (_ensured)
                return;

            // FIXME: Should we instead loop through the array and
            //        report the exact index which violates this rule?
            if (!Values.All(v => v.Key is ISassExportableType &&
                                 v.Value is ISassExportableType))
                throw new SassTypeException(string.Join("",
                     "The value must not contain an object of type that is ",
                     "an arbitrary implementation of ISassType. Please use ",
                     "the predefined Sass types or extend the predefined type's ",
                     "functionality using inheritance or extension methods."));

            // Detect the circular-referencing values.
            list.Add(this);

            var filteredValues = Values.Keys.OfType<SassMap>().ToList();

            if (filteredValues.Any(v => list.Contains(v)))
                throw new SassTypeException(string.Join("",
                     "Circular reference detected in a SassMap.", Environment.NewLine,
                     "Values cannot contain self-referencing instance."));

            filteredValues.ForEach(v => v.WalkAndEnsureDependencies(list));

            filteredValues = Values.Values.OfType<SassMap>().ToList();

            if (filteredValues.Any(v => list.Contains(v)))
                throw new SassTypeException(string.Join("",
                     "Circular reference detected in a SassMap.", Environment.NewLine,
                     "Values cannot contain self-referencing instance."));

            filteredValues.ForEach(v => v.WalkAndEnsureDependencies(list));

            _ensured = true;
        }

        public override string ToString()
        {
            return string.Join(",", Values.Select(v => v.ToString()));
        }

        IntPtr ISassExportableType.GetInternalTypePtr()
        {
            if (_cachedPtr != null)
                return _cachedPtr;

            WalkAndEnsureDependencies(new List<SassMap>());

            var map = SassCompiler.sass_make_map(Values.Count);
            var index = 0;

            foreach (var item in Values)
            {
                var exportableKey = (item.Key as ISassExportableType);
                var exportableValue = (item.Value as ISassExportableType);

                SassCompiler.sass_map_set_key(
                    map, index, exportableKey.GetInternalTypePtr());
                SassCompiler.sass_map_set_value(
                    map, index, exportableValue.GetInternalTypePtr());
                index++;
            }

            return _cachedPtr = map;
        }
    }
}
