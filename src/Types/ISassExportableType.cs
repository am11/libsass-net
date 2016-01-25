using System;

namespace Sass.Types
{
    internal interface ISassExportableType
    {
        /// <summary>
        /// Instantiate the type on LibSass heap.
        /// </summary>
        IntPtr GetInternalTypePtr();
    }
}
