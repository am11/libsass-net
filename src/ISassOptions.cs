namespace Sass
{
    public interface ISassOptions
    {
        /// <summary>
        /// Data to be compiled. This can be null, if InputPath is set.
        /// </summary>
        string Data { get; set; }

        /// <summary>
        /// Precision for fractional numbers.
        /// </summary>
        int? Precision { get; set; }

        /// <summary>
        /// Output style for the generated CSS code
        /// A value from above SASS_STYLE_* constants.
        /// </summary>
        SassOutputStyle OutputStyle { get; set; }

        /// <summary>
        /// Emit comments in the generated CSS indicating
        /// the corresponding source line.
        /// </summary>
        bool IncludeSourceComments { get; set; }

        /// <summary>
        /// Embed sourceMappingUrl as data URI.
        /// </summary>
        bool EmbedSourceMap { get; set; }

        /// <summary>
        /// Embed include contents in maps.
        /// </summary>
        bool IncludeSourceMapContents { get; set; }

        /// <summary>
        /// Disable sourceMappingUrl in CSS output.
        /// </summary>
        bool OmitSourceMapUrl { get; set; }

        /// <summary>
        /// Treat source_string as sass (as opposed to scss).
        /// </summary>
        bool IsIndentedSyntax { get; set; }

        /// <summary>
        /// The input path is used for source map
        /// generation. It can be used to define
        /// something with string compilation or to
        /// overload the input file path. It is
        /// set to "stdin" for data contexts and
        /// to the input file on file contexts.
        /// </summary>
        string InputPath { get; set; }

        /// <summary>
        /// The output path is used for source map
        /// generation. Libsass will not write to
        /// this file, it is just used to create
        /// information in source-maps etc.
        /// </summary>
        string OutputPath { get; set; }

        /// <summary>
        /// String to be used for indentation.
        /// </summary>
        string Indent { get; set; }

        /// <summary>
        /// String to be used to for line feeds.
        /// </summary>
        string Linefeed { get; set; }

        /// <summary>
        /// List of paths.
        /// Colon-separated on Unix.
        /// Semicolon-separated on Windows.
        /// </summary>
        string IncludePath { get; set; }

        /// <summary>
        /// Colon-separated list of paths
        /// Semicolon-separated on Windows
        /// Maybe use array interface instead?
        /// </summary>
        string PluginPath { get; set; }

        /// <summary>
        /// Include paths (linked string list)
        /// </summary>
        string[] IncludePaths { get; set; }

        /// <summary>
        /// Plugin paths (linked string list).
        /// </summary>
        string[] PluginPaths { get; set; }

        /// <summary>
        /// Path to source map file
        /// Enables source map generation
        /// Used to create sourceMappingUrl.
        /// </summary>
        string SourceMapFile { get; set; }

        /// <summary>
        /// Directly inserted in source maps.
        /// </summary>
        string SourceMapRoot { get; set; }

        /// <summary>
        /// Custom functions that can be called from sccs code.
        /// </summary>
        // IntPtr Functions { get; set; }

        /// <summary>
        /// List of custom importers.
        /// </summary>
        CustomImportDelegate[] Importers { get; set; }

        /// <summary>
        /// List of custom headers (Experimental).
        /// Opposite to custom importers, all custom headers will be
        /// executed in priority order and all imports will be accumulated
        /// (so many custom headers can add various custom mixins or css-code).
        /// </summary>
        CustomImportDelegate[] Headers { get; set; }
    }
}
