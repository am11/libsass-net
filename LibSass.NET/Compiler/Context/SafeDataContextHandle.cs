﻿//Copyright (C) 2013 by TBAPI-0KA
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
using System.Runtime.ConstrainedExecution;
using Sass.Compiler.Options;
using static Sass.Compiler.SassExterns;

namespace Sass.Compiler.Context
{
    internal sealed class SassSafeDataContextHandle : SassSafeContextHandle
    {
        internal SassSafeDataContextHandle(ISassOptions sassOptions) :
            base(sassOptions, sass_make_data_context(EncodeAsUtf8IntPtr(sassOptions.Data)))
        { }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        protected override bool ReleaseHandle()
        {
            sass_delete_data_context(handle);
            return true;
        }

        protected override SassResult CompileInternalContext()
        {
            sass_compile_data_context(this);
            return GetResult();
        }

        protected override void SetOverriddenOptions(IntPtr sassOptionsInternal, ISassOptions sassOptions)
        {
            if (!string.IsNullOrWhiteSpace(sassOptions.InputPath))
                sass_option_set_input_path(sassOptionsInternal, new SassSafeStringHandle(sassOptions.InputPath));
        }
    }
}
