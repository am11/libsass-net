using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace LibSassNet
{
    public partial class Sass
    {
        [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
        [SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
        internal sealed class SafeSassStringOptionHandle : SafeHandle
        {
            internal SafeSassStringOptionHandle(string optionValue) :
                  base(IntPtr.Zero, true)
            {
                handle = Marshal.StringToCoTaskMemAnsi(optionValue);
            }

            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
            protected override bool ReleaseHandle()
            {
                Marshal.FreeCoTaskMem(handle);
                return true;
            }

            public override bool IsInvalid
            {
                get { return handle == IntPtr.Zero; }
            }
        }
    }
}
