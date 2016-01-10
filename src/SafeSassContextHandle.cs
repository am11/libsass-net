//Copyright (C) 2013 by TBAPI-0KA
//
//Permission is hereby granted, free of charge, to any person obtaining a copy of
//this software and associated documentation files (the "Software"), to deal in
//the Software without restriction, including without limitation the rights to
//use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
//of the Software, and to permit persons to whom the Software is furnished to do
//so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace Sass
{
    public partial class SassCompiler
    {
        [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
        [SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
        internal abstract class SafeSassContextHandle : SafeHandle
        {
            private SassImporterDelegate _importerCallback;
            private ISassOptions _sassOptions;
            private readonly Dictionary<IntPtr, CustomImportDelegate> CallbackDictionary;

            internal SafeSassContextHandle(ISassOptions sassOptions, IntPtr method) :
                base(IntPtr.Zero, true)
            {
                handle = method;
                _sassOptions = sassOptions;
                CallbackDictionary = new Dictionary<IntPtr, CustomImportDelegate>();
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

            protected static string EncodeAsUtf8String(string utf16String)
            {
                if (string.IsNullOrEmpty(utf16String))
                {
                    return string.Empty;
                }

                // Get UTF-8 bytes from UTF-16 string
                byte[] utf8Bytes = Encoding.UTF8.GetBytes(utf16String);

                // Return UTF-8 bytes as ANSI string
                return Encoding.Default.GetString(utf8Bytes);
            }

            protected static IntPtr EncodeAsUtf8IntPtr(string utf16String)
            {
                if (string.IsNullOrEmpty(utf16String))
                {
                    return IntPtr.Zero;
                }

                return sass_copy_c_string(EncodeAsUtf8String(utf16String));
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

            internal void SetOptions(ISassOptions sassOptions)
            {
                IntPtr sassOptionsInternal = sass_context_get_options(this);

                sass_option_set_output_style(sassOptionsInternal, sassOptions.OutputStyle);
                sass_option_set_source_comments(sassOptionsInternal, sassOptions.IncludeSourceComments);
                sass_option_set_source_map_embed(sassOptionsInternal, sassOptions.EmbedSourceMap);
                sass_option_set_omit_source_map_url(sassOptionsInternal, sassOptions.OmitSourceMapUrl);
                sass_option_set_is_indented_syntax_src(sassOptionsInternal, sassOptions.IsIndentedSyntax);
                sass_option_set_source_map_contents(sassOptionsInternal, sassOptions.IncludeSourceMapContents);

                if (sassOptions.Precision.HasValue)
                    sass_option_set_precision(sassOptionsInternal, sassOptions.Precision.Value);

                if (!string.IsNullOrWhiteSpace(sassOptions.OutputPath))
                    sass_option_set_output_path(sassOptionsInternal, EncodeAsUtf8String(sassOptions.OutputPath));

                if (!string.IsNullOrWhiteSpace(sassOptions.IncludePath))
                    sass_option_set_output_path(sassOptionsInternal, EncodeAsUtf8String(sassOptions.IncludePath));

                if (!string.IsNullOrWhiteSpace(sassOptions.SourceMapRoot))
                    sass_option_set_source_map_root(sassOptionsInternal, EncodeAsUtf8String(sassOptions.SourceMapRoot));

                if (!string.IsNullOrWhiteSpace(sassOptions.SourceMapFile))
                    sass_option_set_source_map_file(sassOptionsInternal, EncodeAsUtf8String(sassOptions.SourceMapFile));

                // Indent can be whitespace.
                if (!string.IsNullOrEmpty(sassOptions.Indent))
                {
                    SafeSassStringOptionHandle indent = new SafeSassStringOptionHandle(EncodeAsUtf8String(sassOptions.Indent));
                    sass_option_set_indent(sassOptionsInternal, indent);
                }

                // Linefeed can be whitespace (i.e. \r is a whitespace).
                if (!string.IsNullOrEmpty(sassOptions.Linefeed))
                {
                    SafeSassStringOptionHandle linefeed = new SafeSassStringOptionHandle(EncodeAsUtf8String(sassOptions.Linefeed));
                    sass_option_set_linefeed(sassOptionsInternal, linefeed);
                }

                SetAdditionalOptions(sassOptionsInternal, sassOptions);

                if (sassOptions.CustomImporters != null)
                {
                    sass_option_set_c_importers(sassOptionsInternal, PrepareCustomImporters(sassOptions.CustomImporters));
                }
            }

            private IntPtr PrepareCustomImporters(CustomImportDelegate[] customImporters)
            {
                int length = customImporters.Length;
                IntPtr cImporters = sass_make_importer_list(customImporters.Length);
                IntPtr entry;
                _importerCallback = SassImporterCallback;

                for (int i = 0; i < length; ++i)
                {
                    CustomImportDelegate customImporter = customImporters[i];
                    IntPtr pointer = customImporter.Method.MethodHandle.GetFunctionPointer();

                    CallbackDictionary.Add(pointer, customImporter);

                    entry = sass_make_importer(_importerCallback, length - i - 1, pointer);
                    sass_importer_set_list_entry(cImporters, i, entry);
                }

                return cImporters;
            }

            private IntPtr SassImporterCallback(IntPtr url, IntPtr callback, IntPtr compiler)
            {
                string currrentImport = PtrToString(url);
                IntPtr parentImporterPtr = sass_compiler_get_last_import(compiler);
                string parentImport = PtrToString(sass_import_get_abs_path(parentImporterPtr));
                CustomImportDelegate customImportCallback = CallbackDictionary[sass_importer_get_cookie(callback)];
                SassImport[] importsArray = customImportCallback(currrentImport, parentImport, _sassOptions);

                if (importsArray == null)
                    return IntPtr.Zero;

                IntPtr cImportsList = sass_make_import_list(importsArray.Length);
                IntPtr entry;

                for (int i = 0; i < importsArray.Length; ++i)
                {
                    if (string.IsNullOrEmpty(importsArray[i].Error))
                    {
                        entry = sass_make_import_entry(EncodeAsUtf8String(importsArray[i].Path),
                                                       EncodeAsUtf8IntPtr(importsArray[i].Data),
                                                       EncodeAsUtf8IntPtr(importsArray[i].Map));
                    }
                    else
                    {
                        entry = sass_make_import_entry(string.Empty, IntPtr.Zero, IntPtr.Zero);
                        sass_import_set_error(entry, importsArray[i].Error, -1, -1);
                    }

                    sass_import_set_list_entry(cImportsList, i, entry);
                }

                return cImportsList;
            }

            protected virtual void SetAdditionalOptions(IntPtr sassOptionsInternal, ISassOptions sassOptions)
            { /* only `SafeSassDataContextHandle` derived type will implement it. */ }
        }
    }
}
