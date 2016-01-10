namespace LibSassNet
{
    public class SassImport
    {
        public string Path { get; set; }
        public string Data { get; set; }
        public string Map { get; set; }
        public string Error { get; set; }
    }

    /// <summary>
    /// Prototype for the importer method.
    /// The parameters can be used to resolve
    /// paths or resource to download data from.
    /// </summary>
    /// <param name="url">URL parsed by the compiler.</param>
    /// <param name="previousImport">Previous URL parsed by the compiler.</param>
    /// <param name="sassOptions">Options object which used for compilation.</param>
    /// <returns>List of SassImport objects.</returns>
    public delegate SassImport[] CustomImportDelegate(string url, string previousImport, ISassOptions sassOptions);
}
