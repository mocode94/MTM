// script.js
document.addEventListener("DOMContentLoaded", function() {
    const scanButton = document.getElementById('scanButton');
    const resultContainer = document.getElementById('result');
    let html5QrcodeScanner;

    scanButton.addEventListener('click', function() {
        if (!html5QrcodeScanner) {
            // Initialize the QR Code Scanner
            html5QrcodeScanner = new Html5Qrcode("qr-reader");

            // Define the function to handle the scan results
            const onScanSuccess = (decodedText, decodedResult) => {
                resultContainer.innerText = `QR Code Detected: ${decodedText}`;
                html5QrcodeScanner.clear();
            };

            // Define the function to handle errors
            const onScanError = (errorMessage) => {
                console.error(`QR Code Scan Error: ${errorMessage}`);
            };

            // Start the QR code scanning process
            html5QrcodeScanner.start(
                { facingMode: "environment" }, // Camera configuration
                { fps: 10 }, // Frame per second
                onScanSuccess, // Success callback
                onScanError // Error callback
            ).catch(err => {
                console.error("Failed to start scanning:", err);
            });
        } else {
            // If already initialized, clear previous scan and restart
            html5QrcodeScanner.clear();
        }
    });
});
