using Microsoft.JSInterop;

namespace RazorMrzLibrary
{
    public class MrzJsInterop : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        public MrzJsInterop(IJSRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./_content/RazorMrzLibrary/mrzJsInterop.js").AsTask());
        }

        public async ValueTask<string> Prompt(string message)
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<string>("showPrompt", message);
        }

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

        public async Task<MrzRecognizer> CreateMrzRecognizer()
        {
            var module = await moduleTask.Value;
            IJSObjectReference jsObjectReference = await module.InvokeAsync<IJSObjectReference>("createMrzRecognizer");
            MrzRecognizer recognizer = new MrzRecognizer(module, jsObjectReference);
            return recognizer;
        }
        public async Task LoadWasm()
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("loadWasm");
        }
    }
}