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

namespace Sass.Compiler.Context
{
    internal abstract partial class SassSafeContextHandle
    {
        private IntPtr GetCustomImportersHeadPtr(CustomImportDelegate[] customImporters)
        {
            int length = customImporters.Length;
            IntPtr cImporters = SassExterns.sass_make_importer_list(customImporters.Length);

            for (int i = 0; i < length; ++i)
            {
                CustomImportDelegate customImporter = customImporters[i];
                IntPtr pointer = customImporter.Method.MethodHandle.GetFunctionPointer();

                _importersCallbackDictionary.Add(pointer, customImporter);

                var entry = SassExterns.sass_make_importer(SassImporterCallback, length - i - 1, pointer);
                SassExterns.sass_importer_set_list_entry(cImporters, i, entry);
            }

            return cImporters;
        }

        private IntPtr SassImporterCallback(IntPtr url, IntPtr callback, IntPtr compiler)
        {
            string currrentImport = PtrToString(url);
            IntPtr parentImporterPtr = SassExterns.sass_compiler_get_last_import(compiler);
            string parentImport = PtrToString(SassExterns.sass_import_get_abs_path(parentImporterPtr));
            CustomImportDelegate customImportCallback = _importersCallbackDictionary[SassExterns.sass_importer_get_cookie(callback)];
            SassImport[] importsArray = customImportCallback(currrentImport, parentImport, _sassOptions);

            if (importsArray == null)
                return IntPtr.Zero;

            IntPtr cImportsList = SassExterns.sass_make_import_list(importsArray.Length);

            for (int i = 0; i < importsArray.Length; ++i)
            {
                IntPtr entry;
                if (string.IsNullOrEmpty(importsArray[i].Error))
                {
                    entry = SassExterns.sass_make_import_entry(EncodeAsUtf8String(importsArray[i].Path),
                                                   EncodeAsUtf8IntPtr(importsArray[i].Data),
                                                   EncodeAsUtf8IntPtr(importsArray[i].Map));
                }
                else
                {
                    entry = SassExterns.sass_make_import_entry(string.Empty, IntPtr.Zero, IntPtr.Zero);
                    SassExterns.sass_import_set_error(entry, importsArray[i].Error, -1, -1);
                }

                SassExterns.sass_import_set_list_entry(cImportsList, i, entry);
            }

            return cImportsList;
        }
    }
}
