using System;
using System.Runtime.ConstrainedExecution;

namespace LibSassNet
{
    public partial class Sass
    {
        internal sealed class SafeSassDataContextHandle : SafeSassContextHandle
        {
            internal SafeSassDataContextHandle(string data) :
                base(sass_make_data_context(sass_make_c_string(EncodeAsUtf8(data))))
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

            protected override void SetAdditionalOptions(IntPtr sassOptionsInternal, SassOptions sassOptions)
            {
                if (!string.IsNullOrWhiteSpace(sassOptions.InputPath))
                    sass_option_set_input_path(sassOptionsInternal, EncodeAsUtf8(sassOptions.InputPath));
            }
        }
    }
}
