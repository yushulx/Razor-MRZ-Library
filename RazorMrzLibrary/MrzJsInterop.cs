using Microsoft.JSInterop;

namespace RazorMrzLibrary
{
    /// <summary>
    /// Provides JavaScript interop functionalities for MRZ recognition.
    /// </summary>
    public class MrzJsInterop : IAsyncDisposable
    {
        // Holds a task that resolves to a JavaScript module reference.
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        /// <summary>
        /// Initializes a new instance of the MrzJsInterop class.
        /// </summary>
        /// <param name="jsRuntime">The JS runtime to use for invoking JavaScript functions.</param>
        public MrzJsInterop(IJSRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./_content/RazorMrzLibrary/mrzJsInterop.js").AsTask());
        }


        /// <summary>
        /// Releases unmanaged resources asynchronously.
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                var module = await moduleTask.Value;
                await module.DisposeAsync();
            }
        }

        /// <summary>
        /// Loads and initializes the JavaScript module.
        /// </summary>
        public async Task LoadJS()
        {
            var module = await moduleTask.Value;
            await module.InvokeAsync<object>("init");
        }

        /// <summary>
        /// Sets the license key for the Dynamsoft Label Recognizer SDK.
        /// </summary>
        /// <param name="license">The license key.</param>
        public async Task SetLicense(string license)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("setLicense", license);
        }

        /// <summary>
        /// Creates a new MrzRecognizer instance.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result is a new MrzRecognizer instance.</returns>
        public async Task<MrzRecognizer> CreateMrzRecognizer()
        {
            var module = await moduleTask.Value;
            IJSObjectReference jsObjectReference = await module.InvokeAsync<IJSObjectReference>("createMrzRecognizer");
            MrzRecognizer recognizer = new MrzRecognizer(module, jsObjectReference);
            return recognizer;
        }

        /// <summary>
        /// Loads the WebAssembly for MRZ recognition.
        /// </summary>
        public async Task LoadWasm()
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("loadWasm");
        }
    }
}