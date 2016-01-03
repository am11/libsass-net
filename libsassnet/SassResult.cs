public class SassResult
{
    public string Output { get; internal set; }
    public int ErrorColumn { get; internal set; }
    public string ErrorFile { get; internal set; }
    public string ErrorJson { get; internal set; }
    public int ErrorLine { get; internal set; }
    public string ErrorMessage { get; internal set; }
    public string ErrorSource { get; internal set; }
    public int ErrorStatus { get; internal set; }
    public string ErrorText { get; internal set; }
    public string SourceMap { get; internal set; }
    public string[] IncludedFiles { get; internal set; }
    // TODO: more stuff; stats, environment (as in actual Ruby Sass like mutable environment)?
}
