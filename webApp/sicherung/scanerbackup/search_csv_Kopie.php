<body>
    <!-- Your HTML content -->

    <!-- Load ZXing library -->
    <script src="https://cdn.jsdelivr.net/npm/@zxing/library@latest"></script>
    
    <!-- Your custom script -->
    <script>
        function startScanner() {
            // Ensure the ZXing library is loaded here
            const codeReader = new ZXing.BrowserMultiFormatReader();
            const videoElement = document.getElementById('video');

            codeReader.decodeFromVideoDevice(null, videoElement, (result, error) => {
                if (result) {
                    document.getElementById('searchValue').value = result.text;
                    document.getElementById('scanner').style.display = 'none';
                    document.querySelector('form').submit();
                }
                if (error && !(error instanceof ZXing.NotFoundException)) {
                    console.error(error);
                }
            }).catch(err => console.error(err));
        }
    </script>
</body>
