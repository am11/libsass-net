using System.Runtime.ConstrainedExecution;

namespace LibSassNet
{
    public partial class Sass
    {
        internal sealed class SafeSassFileContextHandle : SafeSassContextHandle
        {
            internal SafeSassFileContextHandle(string inputFile) :
                base(sass_make_file_context(EncodeAsUtf8String(inputFile)))
            { }

            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
            protected override bool ReleaseHandle()
            {
                sass_delete_file_context(handle);
                return true;
            }

            public override SassResult CompileContext()
            {
                sass_compile_file_context(this);
                return GetResult();
            }
        }
    }
}
