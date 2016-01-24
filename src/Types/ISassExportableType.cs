using System;

namespace Sass.Types
{
    internal interface ISassExportableType
    {
        /// <summary>
        /// Intantiate the type on LibSass heap.
        /// </summary>
        IntPtr GetInternalTypePtr();
    }
}
