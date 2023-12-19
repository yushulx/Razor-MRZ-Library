using Microsoft.JSInterop;
using System.Text.Json;

namespace RazorMrzLibrary
{
    public class MrzRecognizer
    {
        // Fields to hold JavaScript object references.
        private IJSObjectReference _module;
        private IJSObjectReference _jsObjectReference;

        // Public properties to store source dimensions.
        public int SourceWidth, SourceHeight;

        /// <summary>
        /// Initializes a new instance of the MrzRecognizer class.
        /// </summary>
        /// <param name="module">A reference to the JavaScript module.</param>
        /// <param name="recognizer">A reference to the JavaScript object for MRZ recognition.</param>
        public MrzRecognizer(IJSObjectReference module, IJSObjectReference recognizer)
        {
            _module = module;
            _jsObjectReference = recognizer;
        }

        /// <summary>
        /// Asynchronously recognize MRZ from a canvas object.
        /// </summary>
        /// <param name="canvas">A reference to the JavaScript object representing the canvas with the MRZ image.</param>
        /// <returns>A task that represents the asynchronous recognize operation. The task result contains MRZ results.</returns>
        public async Task<List<OcrResult>> DecodeCanvas(IJSObjectReference canvas)
        {
            JsonElement? result = await _module.InvokeAsync<JsonElement>("recognizeCanvas", _jsObjectReference, canvas);
            SourceWidth = await _module.InvokeAsync<int>("getSourceWidth", _jsObjectReference);
            SourceHeight = await _module.InvokeAsync<int>("getSourceHeight", _jsObjectReference);
            return OcrResult.WrapResult(result);
        }
    }
}
