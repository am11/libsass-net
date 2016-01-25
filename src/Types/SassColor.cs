using System;

namespace Sass.Types
{
    public class SassColor: ISassType, ISassExportableType
    {
        IntPtr ISassExportableType.GetInternalTypePtr()
        {
            throw new NotImplementedException();
        }
    }
}
