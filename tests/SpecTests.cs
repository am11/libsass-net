using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Sass.Compiler;
using Sass.Compiler.Options;
using Xunit;
using static System.IO.Path;
using static System.IO.File;
using static System.IO.Directory;

namespace Sass.Tests
{
    public class SpecTests
    {
        [Theory, MemberData(nameof(GetSassSpecDataSuites))]
        public void Sass_Specs_Run(string source, string expected, bool error, string[] paths)
        {
            var options = new SassOptions { InputPath = source, IncludePaths = paths };
            var sass = new SassCompiler(options);
            var result = sass.Compile();
            Assert.Equal(Normalize(expected), Normalize(result.Output));
        }

        const string InputFile = "input.scss";
        const string ExpectedFile = "expected_output.css";
        const string ErrorFile = "error";
        const string SubDirectory = "sub";

        private static IEnumerable<object> GetSassSpecDataSuites()
        {
            var assembly = typeof(SpecTests).GetTypeInfo().Assembly;
            var codebase = assembly.CodeBase.Replace("file:///", "");
            var baseDir = GetDirectoryName(GetDirectoryName(GetDirectoryName(codebase)));
            string spec = Combine(baseDir, "sass-spec", "spec");
            string[] ignoreSuites =
            {
                "libsass-todo-issues",
                "libsass-todo-tests"
            };

            var directories = GetDirectories(spec, "*", SearchOption.TopDirectoryOnly)
                             .Select(d => new DirectoryInfo(d))
                             .Where(d => ignoreSuites.All(s => d.Name != s));

            foreach (var directory in directories)
            {
                var testDirectories =
                    GetDirectories(directory.FullName, "*", SearchOption.TopDirectoryOnly)
                   .Select(d => new DirectoryInfo(d));

                foreach (var testDirectory in testDirectories)
                {
                    var testPath = testDirectory.FullName;
                    var hasErrorFile = File.Exists(Combine(testPath, ErrorFile));
                    var hasError = false;
                    if (hasErrorFile)
                    {
                        var errorFileContents = ReadAllText(Combine(testPath, ErrorFile));
                        hasError = !(errorFileContents.StartsWith("DEPRECATION WARNING") ||
                                     errorFileContents.StartsWith("WARNING:") ||
                                     Regex.IsMatch(errorFileContents, @"^.*?\/input.scss:\d+ DEBUG:"));
                    }

                    var inputFile = Combine(testDirectory.FullName, InputFile);

                    if (!File.Exists(inputFile))
                        continue;

                    yield return new object[]
                    {
                        inputFile,
                        ReadAllText(Combine(testDirectory.FullName, ExpectedFile)),
                        hasErrorFile && hasError,
                        new[] {testPath, Combine(testPath, SubDirectory)}
                    };
                }
            }
        }

        private static string Normalize(string input)
        {
            return Regex.Replace(input, @"\s+", string.Empty)
                        .Replace("{\r", "{")
                        .Replace("{", "{\n").Replace(";", ";\n");
        }
    }
}
