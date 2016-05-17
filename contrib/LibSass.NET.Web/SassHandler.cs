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
using System.IO;
using System.Web;
using LibSass.Compiler;
using LibSass.Compiler.Options;

namespace LibSass.Web
{
    public class SassHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string path = context.Server.MapPath(context.Request.AppRelativeCurrentExecutionFilePath);
            var file = new FileInfo(path);
            SassCompiler compiler = new SassCompiler(new SassOptions { InputPath = path });
            SassResult result = compiler.Compile();

            if (!file.Name.StartsWith("_") && string.Equals(file.Extension, ".scss", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.ContentType = "text/css";
                context.Response.Write(result.Output);
            }
            else if (file.Name.EndsWith(".css", StringComparison.OrdinalIgnoreCase) && string.Equals(file.Extension, ".map", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.Write(result.Output);
            }
        }

        public bool IsReusable { get { return true; } }
    }
}
