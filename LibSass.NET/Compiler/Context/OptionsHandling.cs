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
using LibSass.Compiler.Options;
using static LibSass.Compiler.SassExterns;

namespace LibSass.Compiler.Context
{
    internal abstract partial class SassSafeContextHandle
    {   
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
                sass_option_set_output_path(sassOptionsInternal, new SassSafeStringHandle(sassOptions.OutputPath));

            if (!string.IsNullOrWhiteSpace(sassOptions.IncludePath))
                sass_option_set_include_path(sassOptionsInternal, new SassSafeStringHandle(sassOptions.IncludePath));

            if (!string.IsNullOrWhiteSpace(sassOptions.SourceMapRoot))
                sass_option_set_source_map_root(sassOptionsInternal, new SassSafeStringHandle(sassOptions.SourceMapRoot));

            if (!string.IsNullOrWhiteSpace(sassOptions.SourceMapFile))
                sass_option_set_source_map_file(sassOptionsInternal, new SassSafeStringHandle(sassOptions.SourceMapFile));

            // Indent can be whitespace.
            if (!string.IsNullOrEmpty(sassOptions.Indent))
            {
                var indent = new SassSafeStringHandle(sassOptions.Indent);
                sass_option_set_indent(sassOptionsInternal, indent);
            }

            // Linefeed can be whitespace (i.e. \r is a whitespace).
            if (!string.IsNullOrEmpty(sassOptions.Linefeed))
            {
                var linefeed = new SassSafeStringHandle(sassOptions.Linefeed);
                sass_option_set_linefeed(sassOptionsInternal, linefeed);
            }

            if (sassOptions.IncludePaths != null)
            {
                foreach (var path in sassOptions.IncludePaths)
                {
                    sass_option_push_include_path(sassOptionsInternal, new SassSafeStringHandle(path));
                }
            }

            if (sassOptions.Importers != null)
            {
                sass_option_set_c_importers(sassOptionsInternal, GetCustomImportersHeadPtr(sassOptions.Importers));
            }

            if (sassOptions.Headers != null)
            {
                sass_option_set_c_headers(sassOptionsInternal, GetCustomImportersHeadPtr(sassOptions.Headers));
            }

            if (sassOptions.Functions != null)
            {
                sass_option_set_c_functions(sassOptionsInternal, GetCustomFunctionsHeadPtr(sassOptions.Functions));
            }

            SetOverriddenOptions(sassOptionsInternal, sassOptions);
        }

        protected virtual void SetOverriddenOptions(IntPtr sassOptionsInternal, ISassOptions sassOptions)
        { /* only `SafeSassDataContextHandle` derived type will implement it. */ }
    }
}
