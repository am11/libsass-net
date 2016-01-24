﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Sass.Types
{
    public class SassList : ISassType, ISassExportableType
    {
        public List<ISassType> Values { get; set; } = new List<ISassType>();
        public SassListSeparator Separator { get; set; } = SassListSeparator.Space;

        private bool _ensured;

        /// <summary>
        /// Recursively ensures:
        /// * Arbitrary ISassType implementation.
        /// * Circular reference on each "listy" item stored in the values.
        /// </summary>
        /// <param name="lists">List containing instances of SassList</param>
        internal static void WalkAndEnsureDependencies(SassList list, List<SassList> lists)
        {
            // Prevent from re-entrance.
            if (list._ensured)
                return;

            // FIXME: Should we instead loop through the array and
            //        report the exact index which violates this rule?
            if (!list.Values.All(v => v is ISassExportableType))
            {
                throw new SassTypeException(
                    @"The value must not contain an object of type that is an arbitrary implementation 
                      of ISassType. Please use the predefined Sass types or extend the predefined type's
                      functionality using inheritance or extension methods.");
            }

            // Detect the circular-referencing values.
            lists.Add(list);

            var filteredValues = list.Values
                                .Where(v => v is SassList)
                                .Select(v => v as SassList).ToList();

            if (filteredValues.Any(v => lists.Contains(v)))
                throw new SassTypeException(
                    @"Circular reference detected. Values cannot contain
                      self-referencing instance.");

            filteredValues.ForEach(v => WalkAndEnsureDependencies(v, lists));
            list._ensured = true;
        }

        public override string ToString()
        {
            return string.Join(Separator.ToString(), Values.Select(v => v.ToString()));
        }

        IntPtr ISassExportableType.GetInternalTypePtr()
        {
            WalkAndEnsureDependencies(this, new List<SassList>());

            var list = SassCompiler.sass_make_list(Values.Count, Separator);

            for (int index = 0; index < Values.Count; ++index)
            {
                SassCompiler.sass_list_set_value(list, index,
                                                (Values[index] as ISassExportableType).GetInternalTypePtr());
            }

            return list;
        }
    }
}
