//Copyright (C) 2013 by TBAPI-0KA
//
//Permission is hereby granted, free of charge, to any person obtaining a copy of
//this software and associated documentation files (the "Software"), to deal in
//the Software without restriction, including without limitation the rights to
//use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
//of the Software, and to permit persons to whom the Software is furnished to do
//so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System;
using System.Runtime.InteropServices;
using Sass.Compiler.Context;
using Sass.Compiler.Options;

namespace Sass.Compiler
{
    public struct SassInfo
    {
        public string LibsassVersion { get; internal set; }
        public string SassLanguageVersion { get; internal set; }
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate IntPtr SassImporterDelegate(IntPtr currrentPath, IntPtr callback, IntPtr compiler);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate IntPtr SassFunctionDelegate(IntPtr currrentPath, IntPtr callback, IntPtr compiler);

    public class SassCompiler
    {
        public static readonly SassInfo SassInfo;

        static SassCompiler()
        {
            SassInfo = new SassInfo
            {
                LibsassVersion = SassSafeContextHandle.LibsassVersion(),
                SassLanguageVersion = SassSafeContextHandle.SassLanguageVersion()
            };
        }

        private readonly SassSafeContextHandle _internalContext;

        /// <summary>
        /// Provides an instance of LibSass wrapper class.
        /// </summary>
        /// <param name="sassOptions">Sass options object for compilation.</param>
        /// <remarks>
        /// Replicates LibSass behaviour for context resolution, that is;
        /// if data is provided, make data context and set input file as
        /// supplementary option. Otherwise make a file context.
        /// </remarks>
        public SassCompiler(ISassOptions sassOptions)
        {
            if (string.IsNullOrEmpty(sassOptions.Data))
            {
                _internalContext = new SassSafeFileContextHandle(sassOptions);
            }
            else
            {
                _internalContext = new SassSafeDataContextHandle(sassOptions);
            }

            _internalContext.SetOptions(sassOptions);
        }

        public SassResult Compile()
        {
            return _internalContext.CompileContext();
        }
    }
}
