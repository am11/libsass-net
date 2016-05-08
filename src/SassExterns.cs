using System;
using System.Runtime.InteropServices;
// ReSharper disable InconsistentNaming

// Suggestion to avoid this from happening in future versions:
// https://resharper-support.jetbrains.com/hc/en-us/community/posts/206675599

namespace Sass
{
    public partial class SassCompiler
    {
        private const string LibName = "libsass";

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sass_make_map(int @length);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sass_map_set_key(IntPtr @value_map, int @index, IntPtr @key);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sass_map_set_value(IntPtr @value_map, int @index, IntPtr @value);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sass_make_list(int @length, Types.SassListSeparator @sep);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void sass_list_set_value(IntPtr @value_list, int @index, IntPtr @value);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sass_make_number(double @value, string @unit);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sass_make_boolean(bool @value);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sass_make_null();

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr sass_delete_data_context(IntPtr @context);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr sass_delete_file_context(IntPtr @context);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr sass_copy_c_string(string @input_string);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int sass_compile_data_context(SafeSassDataContextHandle @data_context);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int sass_compile_file_context(SafeSassFileContextHandle @file_context);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr sass_make_file_context(string @source_string);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr sass_make_data_context(IntPtr @source_string);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr sass_context_get_options(SafeSassContextHandle @context);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr sass_context_get_output_string(SafeSassContextHandle @context);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int sass_context_get_error_status(SafeSassContextHandle @context);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr sass_context_get_error_json(SafeSassContextHandle @context);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr sass_context_get_error_text(SafeSassContextHandle @context);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr sass_context_get_error_message(SafeSassContextHandle @context);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr sass_context_get_error_file(SafeSassContextHandle @context);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr sass_context_get_error_src(SafeSassContextHandle @context);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int sass_context_get_error_line(SafeSassContextHandle @context);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int sass_context_get_error_column(SafeSassContextHandle @context);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr sass_context_get_source_map_string(SafeSassContextHandle @context);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr sass_context_get_included_files(SafeSassContextHandle @context);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr sass_make_importer(SassImporterDelegate @importer_fn, double @priority, IntPtr @cookie);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr sass_make_import_entry(string @path, IntPtr @source, IntPtr @srcmap);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr sass_make_importer_list(int @length);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr sass_make_import_list(int @length);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr sass_make_function_list(int @length);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr sass_make_function(string @signature, IntPtr @cb, IntPtr @cookie);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr sass_importer_get_cookie(IntPtr @cb);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr sass_compiler_get_last_import(IntPtr @compiler);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr sass_import_get_abs_path(IntPtr @entry);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void sass_importer_set_list_entry(IntPtr @list, int @idx, IntPtr @entry);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void sass_function_set_list_entry(IntPtr @list, int @pos, IntPtr @cb);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void sass_import_set_list_entry(IntPtr @list, int @idx, IntPtr @entry);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr sass_import_set_error(IntPtr @import, string @message, int @line, int @col);

        // options
        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void sass_option_set_input_path(IntPtr @sass_options /*options*/, string @input_path);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void sass_option_set_output_path(IntPtr @sass_options /*options*/, string @output_path);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void sass_option_set_output_style(IntPtr @sass_options /*options*/, SassOutputStyle @output_style);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void sass_option_set_is_indented_syntax_src(IntPtr @sass_options /*options*/, bool @indented_syntax);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void sass_option_set_source_comments(IntPtr @sass_options /*options*/, bool @source_comments);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void sass_option_set_omit_source_map_url(IntPtr @sass_options /*options*/, bool @omit_source_map_url);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void sass_option_set_source_map_embed(IntPtr @sass_options /*options*/, bool @source_map_embed);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void sass_option_set_source_map_contents(IntPtr @sass_options /*options*/, bool @source_map_contents);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void sass_option_set_source_map_file(IntPtr @sass_options /*options*/, string @source_map_file);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void sass_option_set_source_map_root(IntPtr @sass_options /*options*/, string @source_map_root);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void sass_option_set_include_path(IntPtr @sass_options /*options*/, string @include_path);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void sass_option_set_precision(IntPtr @sass_options /*options*/, int @precision);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void sass_option_set_indent(IntPtr @sass_options /*options*/, SafeSassStringOptionHandle @indent);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void sass_option_set_linefeed(IntPtr @sass_options /*options*/, SafeSassStringOptionHandle @linefeed);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void sass_option_set_c_importers(IntPtr @sass_options /*options*/, IntPtr @c_importers);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void sass_option_set_c_headers(IntPtr @sass_options /*options*/, IntPtr @c_importers);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void sass_option_push_include_path(IntPtr @sass_options /*options*/, string @path);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr libsass_version();

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr libsass_language_version();
    }
}
