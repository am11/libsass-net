using Sass.Compiler;
using Sass.Compiler.Options;
using Xunit;

namespace Sass.Tests
{
    public class CompileTests
    {
        [Fact]
        public void can_compile_simple_string()
        {
            var sass = new SassCompiler(new SassOptions { Data = "body { color: red; }" });
            Assert.NotEmpty(sass.Compile().Output);
        }

        [Fact]
        public void can_compile_file()
        {
            var sass = new SassCompiler(new SassOptions { InputPath = "example.scss", IncludeSourceComments = false });
            var result = sass.Compile();
            Assert.NotEmpty(result.Output);
        }

        [Fact]
        public void when_source_map_file_specfied_should_return_sourcemap_data()
        {
            var sass = new SassCompiler(new SassOptions { InputPath = "example.scss", SourceMapFile = "example.css.map" });
            var result = sass.Compile();
            Assert.NotEmpty(result.SourceMap);
        }
    }
}
