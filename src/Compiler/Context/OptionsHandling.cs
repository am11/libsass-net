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
using Sass.Compiler.Options;

namespace Sass.Compiler.Context
{
    internal abstract partial class SassSafeContextHandle
    {
        private readonly ISassOptions _sassOptions;
        
        internal void SetOptions(ISassOptions sassOptions)
        {
            IntPtr sassOptionsInternal = SassExterns.sass_context_get_options(this);

            SassExterns.sass_option_set_output_style(sassOptionsInternal, sassOptions.OutputStyle);
            SassExterns.sass_option_set_source_comments(sassOptionsInternal, sassOptions.IncludeSourceComments);
            SassExterns.sass_option_set_source_map_embed(sassOptionsInternal, sassOptions.EmbedSourceMap);
            SassExterns.sass_option_set_omit_source_map_url(sassOptionsInternal, sassOptions.OmitSourceMapUrl);
            SassExterns.sass_option_set_is_indented_syntax_src(sassOptionsInternal, sassOptions.IsIndentedSyntax);
            SassExterns.sass_option_set_source_map_contents(sassOptionsInternal, sassOptions.IncludeSourceMapContents);

            if (sassOptions.Precision.HasValue)
                SassExterns.sass_option_set_precision(sassOptionsInternal, sassOptions.Precision.Value);

            if (!string.IsNullOrWhiteSpace(sassOptions.OutputPath))
                SassExterns.sass_option_set_output_path(sassOptionsInternal, EncodeAsUtf8String(sassOptions.OutputPath));

            if (!string.IsNullOrWhiteSpace(sassOptions.IncludePath))
                SassExterns.sass_option_set_include_path(sassOptionsInternal, EncodeAsUtf8String(sassOptions.IncludePath));

            if (!string.IsNullOrWhiteSpace(sassOptions.SourceMapRoot))
                SassExterns.sass_option_set_source_map_root(sassOptionsInternal, EncodeAsUtf8String(sassOptions.SourceMapRoot));

            if (!string.IsNullOrWhiteSpace(sassOptions.SourceMapFile))
                SassExterns.sass_option_set_source_map_file(sassOptionsInternal, EncodeAsUtf8String(sassOptions.SourceMapFile));

            // Indent can be whitespace.
            if (!string.IsNullOrEmpty(sassOptions.Indent))
            {
                var indent = new SassSafeStringOptionHandle(EncodeAsUtf8String(sassOptions.Indent));
                SassExterns.sass_option_set_indent(sassOptionsInternal, indent);
            }

            // Linefeed can be whitespace (i.e. \r is a whitespace).
            if (!string.IsNullOrEmpty(sassOptions.Linefeed))
            {
                var linefeed = new SassSafeStringOptionHandle(EncodeAsUtf8String(sassOptions.Linefeed));
                SassExterns.sass_option_set_linefeed(sassOptionsInternal, linefeed);
            }

            if (sassOptions.IncludePaths != null)
            {
                foreach (var path in sassOptions.IncludePaths)
                {
                    SassExterns.sass_option_push_include_path(sassOptionsInternal, EncodeAsUtf8String(path));
                }
            }

            if (sassOptions.Importers != null)
            {
                SassExterns.sass_option_set_c_importers(sassOptionsInternal, GetCustomImportersHeadPtr(sassOptions.Importers));
            }

            if (sassOptions.Headers != null)
            {
                SassExterns.sass_option_set_c_headers(sassOptionsInternal, GetCustomImportersHeadPtr(sassOptions.Headers));
            }

            if (sassOptions.Functions != null)
            {
                SassExterns.sass_option_set_c_headers(sassOptionsInternal, GetCustomFunctionsHeadPtr(sassOptions.Functions));
            }

            SetOverriddenOptions(sassOptionsInternal, sassOptions);
        }

        protected virtual void SetOverriddenOptions(IntPtr sassOptionsInternal, ISassOptions sassOptions)
        { /* only `SafeSassDataContextHandle` derived type will implement it. */ }
    }
}
