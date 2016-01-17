using System;
using System.Globalization;
using System.IO;
using System.Text;

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

    public override string ToString()
    {
        var format = "{1}={2}{0}{0}";
        var culture = CultureInfo.CurrentCulture;
        var linefeed = Environment.NewLine;

        StringBuilder builder = new StringBuilder();
        builder.AppendFormat(culture, format, linefeed, nameof(Output), Output)
               .AppendFormat(culture, format, linefeed, nameof(SourceMap), SourceMap)
               .AppendFormat(culture, format, linefeed, nameof(IncludedFiles), string.Join(Path.PathSeparator.ToString(), IncludedFiles))
               .AppendFormat(culture, format, linefeed, nameof(ErrorColumn), ErrorColumn)
               .AppendFormat(culture, format, linefeed, nameof(ErrorFile), ErrorFile)
               .AppendFormat(culture, format, linefeed, nameof(ErrorJson), ErrorJson)
               .AppendFormat(culture, format, linefeed, nameof(ErrorLine), ErrorLine)
               .AppendFormat(culture, format, linefeed, nameof(ErrorMessage), ErrorMessage)
               .AppendFormat(culture, format, linefeed, nameof(ErrorSource), ErrorSource)
               .AppendFormat(culture, format, linefeed, nameof(ErrorStatus), ErrorStatus)
               .AppendFormat(culture, format, linefeed, nameof(ErrorText), ErrorText);

        return builder.ToString();
    }
}
