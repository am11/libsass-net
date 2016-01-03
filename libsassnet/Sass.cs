using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace LibSassNet
{
    public partial class Sass
    {
        private readonly SafeSassContextHandle _currentContext;

        /// <summary>
        /// Provides an instance of LibSass wrapper class.
        /// </summary>
        /// <param name="sassOptions">Sass options object for compilation.</param>
        /// <remarks>
        /// Replicates LibSass behaviour for context resolution, that is;
        /// if data is provided, make data context and set input file as
        /// supplementary option. Otherwise make a file context.
        /// </remarks>
        public Sass(SassOptions sassOptions)
        {
            if (string.IsNullOrEmpty(sassOptions.Data))
            {
                _currentContext = new SafeSassFileContextHandle(sassOptions.InputPath);
            }
            else
            {
                _currentContext = new SafeSassDataContextHandle(sassOptions.Data);
            }

            _currentContext.SetOptions(sassOptions);
        }

        public SassResult Compile()
        {
            return _currentContext.CompileContext();
        }

        // TODO: figure out jagged array scenarios
        //public async Task<string> RequestData(string inputData)
        //{
        //    var completionSource = new TaskCompletionSource<string>();
        //    var result = Sass.RegisterCallback(s => completionSource.SetResult(s));
        //    if (result == -1)
        //    {
        //        completionSource.SetException(new SomeException("Failed to set callback"));
        //        return await completionSource.Task;
        //    }

        //    result = Sass.RequestData(inputData);
        //    if (result == -1)
        //        completionSource.SetException(new SomeException("Failed to request data"));
        //    return await completionSource.Task;
        //}
    }
}
