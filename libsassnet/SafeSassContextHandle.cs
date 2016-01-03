using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace LibSassNet
{
    public partial class Sass
    {
        [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
        [SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
        internal abstract class SafeSassContextHandle : SafeHandle
        {
            internal SafeSassContextHandle(IntPtr method) :
                base(IntPtr.Zero, true)
            {
                handle = method;
            }

            public override bool IsInvalid
            {
                get { return handle == IntPtr.Zero; }
            }

            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
            public abstract SassResult CompileContext();

            private static string[] PtrToStringArray(IntPtr stringArray)
            {
                if (stringArray == IntPtr.Zero)
                    return new string[0];

                List<string> members = new List<string>();

                for (int count = 0; Marshal.ReadIntPtr(stringArray, count * IntPtr.Size) != IntPtr.Zero; ++count)
                {
                    members.Add(PtrToString(Marshal.ReadIntPtr(stringArray, count * IntPtr.Size)));
                }

                return members.ToArray();
            }

            private static string PtrToString(IntPtr handle)
            {
                if (handle == IntPtr.Zero)
                    return null;

                var data = new List<byte>();
                var offset = 0;
                byte ch;

                while (true)
                {
                    ch = Marshal.ReadByte(handle, offset++);

                    if (ch == 0)
                        break;

                    data.Add(ch);
                }

                return Encoding.UTF8.GetString(data.ToArray());
            }

            protected static string EncodeAsUtf8(string utf16String)
            {
                // Get UTF-8 bytes from UTF-16 string
                byte[] utf8Bytes = Encoding.UTF8.GetBytes(utf16String);

                // Return UTF-8 bytes as ANSI string
                return Encoding.Default.GetString(utf8Bytes);
            }

            protected SassResult GetResult()
            {
                return new SassResult
                {
                    Output = PtrToString(sass_context_get_output_string(this)),
                    ErrorColumn = sass_context_get_error_column(this),
                    ErrorFile = PtrToString(sass_context_get_error_file(this)),
                    ErrorJson = PtrToString(sass_context_get_error_json(this)),
                    ErrorLine = sass_context_get_error_line(this),
                    ErrorMessage = PtrToString(sass_context_get_error_message(this)),
                    ErrorSource = PtrToString(sass_context_get_error_src(this)),
                    ErrorStatus = sass_context_get_error_status(this),
                    ErrorText = PtrToString(sass_context_get_error_text(this)),
                    SourceMap = PtrToString(sass_context_get_source_map_string(this)),
                    IncludedFiles = PtrToStringArray(sass_context_get_included_files(this))
                };
            }

            internal void SetOptions(SassOptions sassOptions)
            {
                IntPtr sassOptionsInternal = sass_context_get_options(this);

                sass_option_set_precision(sassOptionsInternal, sassOptions.Precision);
                sass_option_set_output_style(sassOptionsInternal, sassOptions.OutputStyle);
                sass_option_set_source_comments(sassOptionsInternal, sassOptions.SourceComments);
                sass_option_set_source_map_embed(sassOptionsInternal, sassOptions.SourceMapEmbed);
                sass_option_set_omit_source_map_url(sassOptionsInternal, sassOptions.OmitSourceMapUrl);
                sass_option_set_is_indented_syntax_src(sassOptionsInternal, sassOptions.IsIndentedSyntax);
                sass_option_set_source_map_contents(sassOptionsInternal, sassOptions.SourceMapContents);

                if (!string.IsNullOrWhiteSpace(sassOptions.OutputPath))
                    sass_option_set_output_path(sassOptionsInternal, EncodeAsUtf8(sassOptions.OutputPath));

                if (!string.IsNullOrWhiteSpace(sassOptions.IncludePath))
                    sass_option_set_output_path(sassOptionsInternal, EncodeAsUtf8(sassOptions.IncludePath));

                if (!string.IsNullOrWhiteSpace(sassOptions.SourceMapRoot))
                    sass_option_set_source_map_root(sassOptionsInternal, EncodeAsUtf8(sassOptions.SourceMapRoot));

                if (!string.IsNullOrWhiteSpace(sassOptions.SourceMapFile))
                    sass_option_set_source_map_file(sassOptionsInternal, EncodeAsUtf8(sassOptions.SourceMapFile));

                // Indent can be whitespace.
                if (!string.IsNullOrEmpty(sassOptions.Indent))
                {
                    SafeSassStringOptionHandle indent = new SafeSassStringOptionHandle(EncodeAsUtf8(sassOptions.Indent));
                    sass_option_set_indent(sassOptionsInternal, indent);
                }

                // Linefeed can be whitespace (i.e. \r is a whitespace).
                if (!string.IsNullOrEmpty(sassOptions.Linefeed))
                {
                    SafeSassStringOptionHandle linefeed = new SafeSassStringOptionHandle(EncodeAsUtf8(sassOptions.Linefeed));
                    sass_option_set_linefeed(sassOptionsInternal, linefeed);
                }

                SetAdditionalOptions(sassOptionsInternal, sassOptions);
            }

            protected virtual void SetAdditionalOptions(IntPtr sassOptionsInternal, SassOptions sassOptions)
            { /* only `SafeSassDataContextHandle` derived type will implement it. */ }
        }
    }
}
