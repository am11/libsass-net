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
using LibSass.Compiler.Options;
using static LibSass.Compiler.SassExterns;

namespace LibSass.Compiler.Context
{
    internal abstract partial class SassSafeContextHandle
    {
        private IntPtr GetCustomImportersHeadPtr(CustomImportDelegate[] customImporters)
        {
            int length = customImporters.Length;
            IntPtr cImporters = sass_make_importer_list(customImporters.Length);

            for (int i = 0; i < length; ++i)
            {
                CustomImportDelegate customImporter = customImporters[i];
                IntPtr pointer = customImporter.Method.MethodHandle.GetFunctionPointer();

                _importersCallbackDictionary.Add(pointer, customImporter);

                var entry = sass_make_importer(SassImporterCallback, length - i - 1, pointer);
                sass_importer_set_list_entry(cImporters, i, entry);
            }

            return cImporters;
        }

        private IntPtr SassImporterCallback(IntPtr url, IntPtr callback, IntPtr compiler)
        {
            IntPtr cookiePtr = sass_importer_get_cookie(callback);
            CustomImportDelegate customImportCallback = _importersCallbackDictionary[cookiePtr];

            IntPtr parentImporterPtr = sass_compiler_get_last_import(compiler);
            IntPtr absolutePathPtr = sass_import_get_abs_path(parentImporterPtr);
            string parentImport = PtrToString(absolutePathPtr);
            string currrentImport = PtrToString(url);
            SassImport[] importsArray = customImportCallback(currrentImport, parentImport, _sassOptions);

            if (importsArray == null)
                return IntPtr.Zero;

            IntPtr cImportsList = sass_make_import_list(importsArray.Length);

            for (int i = 0; i < importsArray.Length; ++i)
            {
                IntPtr entry;
                if (string.IsNullOrEmpty(importsArray[i].Error))
                {
                    entry = sass_make_import_entry(new SassSafeStringHandle(importsArray[i].Path),
                                                   EncodeAsUtf8IntPtr(importsArray[i].Data),
                                                   EncodeAsUtf8IntPtr(importsArray[i].Map));
                }
                else
                {
                    entry = sass_make_import_entry(new SassSafeStringHandle(string.Empty), IntPtr.Zero, IntPtr.Zero);
                    sass_import_set_error(entry, EncodeAsUtf8IntPtr(importsArray[i].Error), -1, -1);
                }

                sass_import_set_list_entry(cImportsList, i, entry);
            }

            return cImportsList;
        }
    }
}
