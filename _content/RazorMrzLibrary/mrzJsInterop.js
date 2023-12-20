export function init() {
    return new Promise((resolve, reject) => {
        let script = document.createElement('script');
        script.type = 'text/javascript';
        script.src = '_content/RazorMrzLibrary/dlr.js';
        script.onload = async () => {
            resolve();
        };
        script.onerror = () => {
            reject();
        };
        document.head.appendChild(script);
    });
}

export function setLicense(license) {
    if (!Dynamsoft) return;
    try {
        Dynamsoft.DLR.LabelRecognizer.license = license;
    }
    catch (ex) {
        console.error(ex);
    }
}

export async function loadWasm() {
    if (!Dynamsoft) return;
    try {
        await Dynamsoft.DLR.LabelRecognizer.loadWasm();
    }
    catch (ex) {
        console.error(ex);
    }
}

export async function createMrzRecognizer() {
    if (!Dynamsoft) return;

    try {
        let recognizer = await Dynamsoft.DLR.LabelRecognizer.createInstance();
        recognizer.ifSaveOriginalImageInACanvas = true;
        await recognizer.updateRuntimeSettingsFromString("MRZ");
        return recognizer;
    }
    catch (ex) {
        console.error(ex);
    }
    return null;
}

export function getSourceWidth(recognizer) {
    let canvas = recognizer.getOriginalImageInACanvas();
    return canvas.width;
}

export function getSourceHeight(recognizer) {
    let canvas = recognizer.getOriginalImageInACanvas();
    return canvas.height;
}

export function decodeBase64Image(base64) {
    return new Promise((resolve, reject) => {
        var canvas = document.createElement("canvas");
        var image = new Image();
        image.src = base64;
        image.onload = () => {
            canvas.width = image.width;
            canvas.height = image.height;
            let context = canvas.getContext('2d');
            context.drawImage(image, 0, 0, canvas.width, canvas.height);
            resolve(canvas);
        };
        image.onerror = (error) => {
            reject(error);
        };
    });
}

export async function recognizeCanvas(recognizer, canvas) {
    if (!Dynamsoft) return;

    try {
        let results = await recognizer.recognize(canvas);
        return results;
    }
    catch (ex) {
        console.error(ex);
    }
    return null;
}
