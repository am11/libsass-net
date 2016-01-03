using System;
using System.Runtime.InteropServices;

namespace LibSassNet
{
    public partial class Sass
    {
        [DllImport("libsass.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr sass_delete_data_context(IntPtr @context);

        [DllImport("libsass.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr sass_delete_file_context(IntPtr @context);

        [DllImport("libsass.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr sass_make_c_string(string @input_string);

        [DllImport("libsass.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int sass_compile_data_context(SafeSassDataContextHandle @data_context);

        [DllImport("libsass.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int sass_compile_file_context(SafeSassFileContextHandle @file_context);

        [DllImport("libsass.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr sass_make_file_context(string @source_string);

        [DllImport("libsass.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr sass_make_data_context(IntPtr @source_string);

        [DllImport("libsass.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr sass_context_get_options(SafeSassContextHandle @context);

        [DllImport("libsass.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr sass_context_get_output_string(SafeSassContextHandle @context);

        [DllImport("libsass.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int sass_context_get_error_status(SafeSassContextHandle @context);

        [DllImport("libsass.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr sass_context_get_error_json(SafeSassContextHandle @context);

        [DllImport("libsass.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr sass_context_get_error_text(SafeSassContextHandle @context);

        [DllImport("libsass.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr sass_context_get_error_message(SafeSassContextHandle @context);

        [DllImport("libsass.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr sass_context_get_error_file(SafeSassContextHandle @context);

        [DllImport("libsass.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr sass_context_get_error_src(SafeSassContextHandle @context);

        [DllImport("libsass.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int sass_context_get_error_line(SafeSassContextHandle @context);

        [DllImport("libsass.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int sass_context_get_error_column(SafeSassContextHandle @context);

        [DllImport("libsass.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr sass_context_get_source_map_string(SafeSassContextHandle @context);

        [DllImport("libsass.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr sass_context_get_included_files(SafeSassContextHandle @context);

        // options
        [DllImport("libsass.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void sass_option_set_input_path(IntPtr sass_options /*options*/, string @input_path);

        [DllImport("libsass.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void sass_option_set_output_path(IntPtr sass_options /*options*/, string @output_path);

        [DllImport("libsass.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void sass_option_set_output_style(IntPtr sass_options /*options*/, SassOutputStyle @output_style);

        [DllImport("libsass.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void sass_option_set_is_indented_syntax_src(IntPtr sass_options /*options*/, bool @indented_syntax);

        [DllImport("libsass.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void sass_option_set_source_comments(IntPtr sass_options /*options*/, bool @source_comments);

        [DllImport("libsass.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void sass_option_set_omit_source_map_url(IntPtr sass_options /*options*/, bool @omit_source_map_url);

        [DllImport("libsass.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void sass_option_set_source_map_embed(IntPtr sass_options /*options*/, bool @source_map_embed);

        [DllImport("libsass.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void sass_option_set_source_map_contents(IntPtr sass_options /*options*/, bool @source_map_contents);

        [DllImport("libsass.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void sass_option_set_source_map_file(IntPtr sass_options /*options*/, string @source_map_file);

        [DllImport("libsass.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void sass_option_set_source_map_root(IntPtr sass_options /*options*/, string @source_map_root);

        [DllImport("libsass.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void sass_option_set_include_path(IntPtr sass_options /*options*/, string @include_path);

        [DllImport("libsass.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void sass_option_set_precision(IntPtr sass_options /*options*/, int @precision);

        [DllImport("libsass.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void sass_option_set_indent(IntPtr sass_options /*options*/, SafeSassStringOptionHandle @indent);

        [DllImport("libsass.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void sass_option_set_linefeed(IntPtr sass_options /*options*/, SafeSassStringOptionHandle @linefeed);
    }
}
