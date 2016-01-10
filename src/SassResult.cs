using System;
using System.ComponentModel;
using System.Globalization;
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
        StringBuilder builder = new StringBuilder();

        foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(this))
            builder.AppendFormat(CultureInfo.CurrentCulture, "{0}={1}{2}{2}",
                                 descriptor.Name, descriptor.GetValue(this),
                                 Environment.NewLine);

        return builder.ToString();
    }
}
