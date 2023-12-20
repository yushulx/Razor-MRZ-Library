# Razor MRZ Library
A Razor Class Library built using the [Dynamsoft Label Recognizer SDK](https://www.npmjs.com/package/dynamsoft-label-recognizer?activeTab=readme), which provides APIs for scanning MRZ (Machine Readable Zone) on passports and IDs.

## Online Demo
[https://yushulx.me/Razor-MRZ-Library/](https://yushulx.me/Razor-MRZ-Library/)

## Prerequisites
- [Dynamsoft Label Recognizer License](https://www.dynamsoft.com/customer/license/trialLicense?product=dlr)

## Quick Usage
- Import and initialize the Razor MRZ Library.
    
    ```csharp
    @using RazorMrzLibrary
    
    @code {
        private MrzJsInterop? mrzJsInterop;
        
        protected override async Task OnInitializedAsync()
        {
            mrzJsInterop = new MrzJsInterop(JSRuntime);
            await mrzJsInterop.LoadJS();
        }
    }
    ```

- Set the license key and load the wasm module.
    
    ```csharp
    await mrzJsInterop.SetLicense(licenseKey);
    await mrzJsInterop.LoadWasm();
    ```

- Createa a MRZ recognizer instance.
    
    ```csharp
    MrzRecognizer recognizer = await mrzJsInterop.CreateMrzRecognizer();
    ```

- Recognize MRZ text from an image.
    
    ```csharp
    List<OcrResult> results = await recognizer.DecodeCanvas(canvas);
    ```

- Parse MRZ text.
    
    ```csharp
    string[] lines = new string[results.Count];
    for (int i = 0; i < results.Count; i++)
    {
        lines[i] = result.Text;
    }
    MrzResult mrzResult = MrzParser.Parse(lines);
    ```

## API

### OcrResult Class
Represents a line of text recognized by the OCR engine.

### MrzResult Class
Represents the result of parsing MRZ text.

### MrzParser Class
- `static MrzResult Parse(string[] lines)`: Parses MRZ text.

### MrzRecognizer Class

- `Task<List<OcrResult>> DecodeCanvas(IJSObjectReference canvas)`: Recognizes MRZ text from a canvas object.

### MrzJsInterop Class 
- `Task LoadJS()`: Loads the Dynamsoft Label Recognizer JavaScript library.
- `Task SetLicense(string license)`: Sets the license key.
- `Task LoadWasm()`: Loads the Dynamsoft Label Recognizer WebAssembly module.
- `Task<MrzRecognizer> CreateMrzRecognizer()`: Creates a MRZ recognizer instance.


## Example
- [Blazor MRZ Scanner](https://github.com/yushulx/Razor-Camera-Library/tree/main/example)

   ![Blazor MRZ Scanner](https://camo.githubusercontent.com/d09afc93937e6034834bf740b9cbc517a02c396e82d3a40c1d6c53ae38a998c6/68747470733a2f2f7777772e64796e616d736f66742e636f6d2f636f6465706f6f6c2f696d672f323032332f31322f72617a6f722d6d727a2d6c6962726172792d626c617a6f722d6170702e706e67)

## Build
```bash
cd RazorCameraLibrary
dotnet build --configuration Release
```
