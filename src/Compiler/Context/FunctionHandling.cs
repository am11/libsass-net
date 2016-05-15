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
using Sass.Compiler.Options;
using Sass.Types;
using static Sass.Compiler.SassExterns;

namespace Sass.Compiler.Context
{
    internal abstract partial class SassSafeContextHandle
    {
        private IntPtr GetCustomFunctionsHeadPtr(SassFunctionCollection customFunctions)
        {
            int length = customFunctions.Count;
            IntPtr cFunctions = sass_make_function_list(customFunctions.Count);

            for (int i = 0; i < length; ++i)
            {
                SassFunction customFunction = customFunctions[i];
                IntPtr pointer = customFunction.CustomFunctionDelegate.Method.MethodHandle.GetFunctionPointer();

                _functionsCallbackDictionary.Add(pointer, customFunction.CustomFunctionDelegate);

                var cb = sass_make_function(customFunction.Signature, SassFunctionCallback, pointer);
                sass_function_set_list_entry(cFunctions, i, cb);
            }

            return cFunctions;
        }

        private IntPtr SassFunctionCallback(IntPtr sassValues, IntPtr callback, IntPtr compiler)
        {
            ISassType[] convertedValues = TypeFactory.GetSassArguments(sassValues);

            IntPtr signaturePtr = sass_function_get_signature(callback);
            string signature = PtrToString(signaturePtr);

            IntPtr cookiePtr = sass_function_get_cookie(callback);
            CustomFunctionDelegate customFunctionCallback = _functionsCallbackDictionary[cookiePtr];

            ISassType returnedValue = customFunctionCallback(_sassOptions, signature, convertedValues);

            return TypeFactory.GetRawPointer(returnedValue, ValidityEvent);
        }
    }
}
