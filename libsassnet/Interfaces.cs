using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace LibSassNet_2
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct Sass_Options
    {
        /// <summary>
        /// Precision for fractional numbers.
        /// </summary>
        int @precision;

        /// <summary>
        /// Output style for the generated CSS code
        /// A value from above SASS_STYLE_* constants.
        /// </summary>
        public Sass_Output_Style @output_style;

        /// <summary>
        /// Emit comments in the generated CSS indicating
        /// the corresponding source line.
        /// </summary>
        bool @source_comments;

        /// <summary>
        /// Embed sourceMappingUrl as data URI.
        /// </summary>
        bool @source_map_embed;

        /// <summary>
        /// Embed include contents in maps.
        /// </summary>
        bool @source_map_contents;

        /// <summary>
        /// Disable sourceMappingUrl in CSS output.
        /// </summary>
        bool @omit_source_map_url;

        /// <summary>
        /// Treat source_string as sass (as opposed to scss).
        /// </summary>
        bool @is_indented_syntax_src;

        /// <summary>
        /// The input path is used for source map
        /// generation. It can be used to define
        /// something with string compilation or to
        /// overload the input file path. It is
        /// set to "stdin" for data contexts and
        /// to the input file on file contexts.
        /// </summary>
        string @input_path;

        /// <summary>
        /// The output path is used for source map
        /// generation. Libsass will not write to
        /// this file, it is just used to create
        /// information in source-maps etc.
        /// </summary>
        string @output_path;

        /// <summary>
        /// String to be used for indentation.
        /// </summary>
        string @indent;

        /// <summary>
        /// String to be used to for line feeds.
        /// </summary>
        string @linefeed;

        /// <summary>
        /// List of paths.
        /// Colon-separated on Unix.
        /// Semicolon-separated on Windows.
        /// </summary>
        string @include_path;

        /// <summary>
        /// Colon-separated list of paths
        /// Semicolon-separated on Windows
        /// Maybe use array interface instead?
        /// </summary>
        private string @plugin_path;

        /// <summary>
        /// Include paths (linked string list)
        /// </summary>
        List<string> @include_paths;

        /// <summary>
        /// Plugin paths (linked string list).
        /// </summary>
        List<string> @plugin_paths;

        /// <summary>
        /// Path to source map file
        /// Enables source map generation
        /// Used to create sourceMappingUrl.
        /// </summary>
        string @source_map_file;

        /// <summary>
        /// Directly inserted in source maps.
        /// </summary>
        string @source_map_root;

        /// <summary>
        /// Custom functions that can be called from sccs code.
        /// </summary>
        Sass_Function_List @c_functions;

        /// <summary>
        /// List of custom importers.
        /// </summary>
        Sass_Importer_List @c_importers;

        /// <summary>
        /// List of custom headers.
        /// </summary>
        Sass_Importer_List @c_headers;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct Sass_Importer
    {
        string @path;
        string @base;
        string @source;
        string @srcmap;
        string @error;
        int @line;
        int @column;
    }

    //  typedef union Sass_Value* (*Sass_Function_Fn)
    //(const union Sass_Value*, Sass_Function_Entry cb, struct Sass_Options* options);


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct Sass_Function
    {
        string @signature;
        Func<Sass_Value[], Sass_Function_Entry, SassOptions[], Sass_Value[]> @function;
        IntPtr @cookie;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct Sass_Function_Entry
    {
        public Sass_Function[] list;
    }

    public partial struct Sass_Unknown
    {
        public Sass_Tag @tag;
    }

    public partial struct Sass_Boolean
    {
        public Sass_Tag @tag;
        public bool @value;
    }

    public partial struct Sass_Number
    {
        public Sass_Tag @tag;
        public double @value;
        [MarshalAs(UnmanagedType.LPStr)]
        public string @unit;
    }

    public partial struct Sass_Color
    {
        public Sass_Tag @tag;
        public double @r;
        public double @g;
        public double @b;
        public double @a;
    }

    public partial struct Sass_String
    {
        public Sass_Tag @tag;
        public bool @quoted;
        [MarshalAs(UnmanagedType.LPStr)]
        public string @value;
    }

    public partial struct Sass_List
    {
        public Sass_Tag @tag;
        public Sass_Separator @separator;
        public ulong @length;
        public IntPtr @values;
    }

    public enum Sass_Separator
    {
        @SASS_COMMA = 0,
        @SASS_SPACE = 1,
    }

    public partial struct Sass_Map
    {
        public Sass_Tag @tag;
        public ulong @length;
        public partial struct Sass_MapPair
        {
        }

        public IntPtr @pairs;
    }

    public partial struct Sass_Null
    {
        public Sass_Tag @tag;
    }

    public partial struct Sass_Error
    {
        public Sass_Tag @tag;
        [MarshalAs(UnmanagedType.LPStr)]
        public string @message;
    }

    public partial struct Sass_Warning
    {
        public Sass_Tag @tag;
        [MarshalAs(UnmanagedType.LPStr)]
        public string @message;
    }

    public enum Sass_Tag
    {
        @SASS_BOOLEAN = 0,
        @SASS_NUMBER = 1,
        @SASS_COLOR = 2,
        @SASS_STRING = 3,
        @SASS_LIST = 4,
        @SASS_MAP = 5,
        @SASS_NULL = 6,
        @SASS_ERROR = 7,
        @SASS_WARNING = 8,
    }

    struct Sass_Value
    {
        Sass_Unknown @unknown;
        Sass_Boolean @boolean;
        Sass_Number @number;
        Sass_Color @color;
        Sass_String @string;
        Sass_List @list;
        Sass_Map @map;
        Sass_Null @null;
        Sass_Error @error;
        Sass_Warning @warning;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct Sass_Function_List
    {
        public Sass_Function[] list;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct Sass_Importer_List
    {
        Sass_Importer[] list;
    }

    public class SassOptions
    {
        public string OutFile { get; set; }
        public Sass_Output_Style OutputStyle { get; set; }
        public bool IndentedSyntax { get; set; }
        public bool SourceComments { get; set; }
        public bool OmitSourceMapUrl { get; set; }
        public bool SourceMapEmbed { get; set; }
        public bool SourceMapContents { get; set; }
        public string SourceMapFile { get; set; }
        public string SourceMapRoot { get; set; }
        public string IncludePath { get; set; }
        public int Precision { get; set; }
        public string Indent { get; set; }
        public string Linefeed { get; set; }
    }

    public class SassContext
    {
        public string SourceString;
        public string OutputString;
        public SassOptions Options;
        public bool ErrorStatus;
        public string ErrorMessage;
    }

    public class SassFileContext
    {
        public string InputPath;
        public string OutputString;
        public string OutputSourceMapFile;
        public string OutputSourceMap;
        public SassOptions Options;
        public bool ErrorStatus;
        public string ErrorMessage;
    }

    public class SassFolderContext
    {
        public string SearchPath;
        public string OutputPath;
        public SassOptions Options;
        public bool ErrorStatus;
        public string ErrorMessage;
    }

    public class SassToScssConversionContext
    {
        public string SourceText;
        public string OutputText;
    }

    public interface ISassInterface
    {
        int Compile(SassContext sassContext);
        int Compile(SassFileContext sassFileContext);
        void Convert(SassToScssConversionContext context);
        // Folder context isn't implemented in core libsass library now
        /*int Compile(SassFolderContext^ sassFolderContext);*/
    }

    public enum Sass_Output_Style
    {
        SASS_STYLE_NESTED,
        SASS_STYLE_EXPANDED,
        SASS_STYLE_COMPACT,
        SASS_STYLE_COMPRESSED
    }

    public class SassInterface : ISassInterface
    {
        public int Compile(SassContext sassContext) { return 0; }
        public int Compile(SassFileContext sassFileContext) { return 0; }
        public void Convert(SassToScssConversionContext context) { }
    }

    public class LibSass
    {
        public enum blah
        {
            a, b, c
        }


        public string SetOptions(SassOptions sassOptions)
        {
            Sass_Options sassOptionsInternal = new Sass_Options();
            sass_option_set_output_path(sassOptionsInternal, sassOptions.OutFile);
            sass_option_set_output_style(sassOptionsInternal, sassOptions.OutputStyle);
            sass_option_set_is_indented_syntax_src(sassOptionsInternal, sassOptions.IndentedSyntax);
            sass_option_set_source_comments(sassOptionsInternal, sassOptions.SourceComments);
            sass_option_set_omit_source_map_url(sassOptionsInternal, sassOptions.OmitSourceMapUrl);
            sass_option_set_source_map_embed(sassOptionsInternal, sassOptions.SourceMapEmbed);
            sass_option_set_source_map_contents(sassOptionsInternal, sassOptions.SourceMapContents);
            sass_option_set_source_map_file(sassOptionsInternal, sassOptions.SourceMapFile);
            sass_option_set_source_map_root(sassOptionsInternal, sassOptions.SourceMapRoot);
            sass_option_set_include_path(sassOptionsInternal, sassOptions.IncludePath);
            sass_option_set_precision(sassOptionsInternal, sassOptions.Precision);
            sass_option_set_indent(sassOptionsInternal, sassOptions.Indent);
            sass_option_set_linefeed(sassOptionsInternal, sassOptions.Linefeed);

            //string s = Marshal.PtrToStringAnsi(callnativefunc());
            return string.Empty;
        }

        #region Externs
        [DllImport("libsass.dll", CharSet = CharSet.Unicode)]
        private static extern void sass_option_set_output_path(Sass_Options sass_options, string out_file);

        [DllImport("libsass.dll", CharSet = CharSet.Unicode)]
        private static extern void sass_option_set_output_style(Sass_Options sass_options, Sass_Output_Style output_style);

        [DllImport("libsass.dll", CharSet = CharSet.Unicode)]
        private static extern void sass_option_set_is_indented_syntax_src(Sass_Options sass_options, bool indented_syntax);

        [DllImport("libsass.dll", CharSet = CharSet.Unicode)]
        private static extern void sass_option_set_source_comments(Sass_Options sass_options, bool source_comments);

        [DllImport("libsass.dll", CharSet = CharSet.Unicode)]
        private static extern void sass_option_set_omit_source_map_url(Sass_Options sass_options, bool omit_source_map_url);

        [DllImport("libsass.dll", CharSet = CharSet.Unicode)]
        private static extern void sass_option_set_source_map_embed(Sass_Options sass_options, bool source_map_embed);

        [DllImport("libsass.dll", CharSet = CharSet.Unicode)]
        private static extern void sass_option_set_source_map_contents(Sass_Options sass_options, bool source_map_contents);

        [DllImport("libsass.dll", CharSet = CharSet.Unicode)]
        private static extern void sass_option_set_source_map_file(Sass_Options sass_options, string source_map_file);

        [DllImport("libsass.dll", CharSet = CharSet.Unicode)]
        private static extern void sass_option_set_source_map_root(Sass_Options sass_options, string source_map_root);

        [DllImport("libsass.dll", CharSet = CharSet.Unicode)]
        private static extern void sass_option_set_include_path(Sass_Options sass_options, string include_path);

        [DllImport("libsass.dll", CharSet = CharSet.Unicode)]
        private static extern void sass_option_set_precision(Sass_Options sass_options, int precision);

        [DllImport("libsass.dll", CharSet = CharSet.Unicode)]
        private static extern void sass_option_set_indent(Sass_Options sass_options, string indent);

        [DllImport("libsass.dll", CharSet = CharSet.Unicode)]
        private static extern void sass_option_set_linefeed(Sass_Options sass_options, string linefeed);
        #endregion
    }
}
