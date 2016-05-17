using LibSass.Compiler;
using LibSass.Compiler.Options;
using Xunit;

namespace LibSass.Tests
{
    public class CompileTests
    {
        [Fact]
        public void can_compile_simple_string()
        {
            var options = new SassOptions {Data = "body { color: red; }"};
            var sass = new SassCompiler(options);
            var result = sass.Compile();
            Assert.NotEmpty(result.Output);
        }

        [Fact]
        public void can_compile_file()
        {
            var options = new SassOptions {InputPath = "Fixtures/example.scss", IncludeSourceComments = false};
            var sass = new SassCompiler(options);
            var result = sass.Compile();
            Assert.NotEmpty(result.Output);
        }

        [Fact]
        public void when_source_map_file_specfied_should_return_sourcemap_data()
        {
            var options = new SassOptions {InputPath = "Fixtures/example.scss", SourceMapFile = "Fixtures/example.css.map"};
            var sass = new SassCompiler(options);
            var result = sass.Compile();
            Assert.NotEmpty(result.SourceMap);
        }
    }
}
