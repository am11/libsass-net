namespace LibSassNet
{
    public struct SassOptions
    {
        /// <summary>
        /// Data to be compiled. This can be null, if InputPath is set.
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Precision for fractional numbers.
        /// </summary>
        public int? Precision { get; set; }

        /// <summary>
        /// Output style for the generated CSS code
        /// A value from above SASS_STYLE_* constants.
        /// </summary>
        public SassOutputStyle OutputStyle { get; set; }

        /// <summary>
        /// Emit comments in the generated CSS indicating
        /// the corresponding source line.
        /// </summary>
        public bool SourceComments { get; set; }

        /// <summary>
        /// Embed sourceMappingUrl as data URI.
        /// </summary>
        public bool SourceMapEmbed { get; set; }

        /// <summary>
        /// Embed include contents in maps.
        /// </summary>
        public bool SourceMapContents { get; set; }

        /// <summary>
        /// Disable sourceMappingUrl in CSS output.
        /// </summary>
        public bool OmitSourceMapUrl { get; set; }

        /// <summary>
        /// Treat source_string as sass (as opposed to scss).
        /// </summary>
        public bool IsIndentedSyntax { get; set; }

        /// <summary>
        /// The input path is used for source map
        /// generation. It can be used to define
        /// something with string compilation or to
        /// overload the input file path. It is
        /// set to "stdin" for data contexts and
        /// to the input file on file contexts.
        /// </summary>
        public string InputPath { get; set; }

        /// <summary>
        /// The output path is used for source map
        /// generation. Libsass will not write to
        /// this file, it is just used to create
        /// information in source-maps etc.
        /// </summary>
        public string OutputPath { get; set; }

        /// <summary>
        /// String to be used for indentation.
        /// </summary>
        public string Indent { get; set; }

        /// <summary>
        /// String to be used to for line feeds.
        /// </summary>
        public string Linefeed { get; set; }

        /// <summary>
        /// List of paths.
        /// Colon-separated on Unix.
        /// Semicolon-separated on Windows.
        /// </summary>
        public string IncludePath { get; set; }

        /// <summary>
        /// Colon-separated list of paths
        /// Semicolon-separated on Windows
        /// Maybe use array interface instead?
        /// </summary>
        public string PluginPath { get; set; }

        /// <summary>
        /// Include paths (linked string list)
        /// </summary>
        public string[] IncludePaths { get; set; }

        /// <summary>
        /// Plugin paths (linked string list).
        /// </summary>
        public string[] PluginPaths { get; set; }

        /// <summary>
        /// Path to source map file
        /// Enables source map generation
        /// Used to create sourceMappingUrl.
        /// </summary>
        public string SourceMapFile { get; set; }

        /// <summary>
        /// Directly inserted in source maps.
        /// </summary>
        public string SourceMapRoot { get; set; }

        ///// <summary>
        ///// Custom functions that can be called from sccs code.
        ///// </summary>
        //public Sass_Function_List CustomFunction { get; set; }

        ///// <summary>
        ///// List of custom importers.
        ///// </summary>
        //public Sass_Importer_List CustomImporters { get; set; }

        ///// <summary>
        ///// List of custom headers.
        ///// </summary>
        //public Sass_Importer_List CustomHeaders { get; set; }
    }
}
