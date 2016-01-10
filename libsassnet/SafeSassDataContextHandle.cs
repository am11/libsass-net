using System;
using System.Runtime.ConstrainedExecution;

namespace LibSassNet
{
    public partial class Sass
    {
        internal sealed class SafeSassDataContextHandle : SafeSassContextHandle
        {
            internal SafeSassDataContextHandle(ISassOptions sassOptions) :
                base(sassOptions, sass_make_data_context(EncodeAsUtf8IntPtr(sassOptions.Data)))
            { }

            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
            protected override bool ReleaseHandle()
            {
                sass_delete_data_context(handle);
                return true;
            }

            public override SassResult CompileContext()
            {
                sass_compile_data_context(this);
                return GetResult();
            }

            protected override void SetAdditionalOptions(IntPtr sassOptionsInternal, ISassOptions sassOptions)
            {
                if (!string.IsNullOrWhiteSpace(sassOptions.InputPath))
                    sass_option_set_input_path(sassOptionsInternal, EncodeAsUtf8String(sassOptions.InputPath));
            }
        }
    }
}
