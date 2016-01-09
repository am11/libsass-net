using System;
using System.Runtime.InteropServices;

namespace LibSassNet
{
    public partial class Sass
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SassImporterDelegate(IntPtr currrentPath, IntPtr callback, IntPtr compiler);

        private readonly SafeSassContextHandle _internalContext;

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
                _internalContext = new SafeSassFileContextHandle(sassOptions.InputPath);
            }
            else
            {
                _internalContext = new SafeSassDataContextHandle(sassOptions.Data);
            }

            _internalContext.SetOptions(sassOptions);
        }

        public SassResult Compile()
        {
            return _internalContext.CompileContext();
        }
    }
}
