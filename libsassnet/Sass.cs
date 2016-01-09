using System;
using System.Runtime.InteropServices;

namespace LibSassNet
{
    public partial class Sass
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SassImporter(IntPtr currrentPath, IntPtr callback, IntPtr compiler);

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
    }
}
