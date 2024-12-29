<?php
// Initialize variables
$searchValue = '';
$result = '';

// Check if the form is submitted
if ($_SERVER["REQUEST_METHOD"] == "POST") {
    if (isset($_POST['searchValue'])) {
        // Get the search value from the form input
        $searchValue = trim($_POST['searchValue']);
        
        // Set the path to your CSV file
        $csvFile = 'TEDI.csv';

        // Check if the file exists
        if (file_exists($csvFile)) {
            // Open the CSV file
            if (($file = fopen($csvFile, 'r')) !== FALSE) {
                // Loop through CSV rows
                while (($data = fgetcsv($file, 1000, ';')) !== FALSE) {
                    // Check if the search value matches the first column
                    if (strcasecmp($data[0], $searchValue) == 0) {
                        // Display the value from the 21st column (index 20)
                        $result = isset($data[21]) ? htmlspecialchars($data[21]) : 'No value in column 21';
                        break;
                    }
                }
                fclose($file);
                if ($result === '') {
                    $result = 'Value not found';
                }
            } else {
                $result = 'Error opening file';
            }
        } else {
            $result = 'File not found';
        }
    }

    // Check if the scan button is pressed
    if (isset($_POST['scanValue'])) {
        // Process scan result if needed
        $searchValue = trim($_POST['scanValue']);
    }
}
?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>CSV Search and Scanner</title>
    <style>
        table {
            width: 100%;
            border-collapse: collapse;
        }
        table, th, td {
            border: 1px solid black;
        }
        th, td {
            padding: 8px;
            text-align: left;
        }
        .result {
            margin-top: 20px;
            font-weight: bold;
        }
        #scanner {
            display: none;
        }
    </style>
</head>
<body>
    <h1>Search CSV File</h1>
    <form method="post" action="">
        <label for="searchValue">Search Value:</label>
        <input type="text" id="searchValue" name="searchValue" value="<?php echo htmlspecialchars($searchValue); ?>">
        <button type="submit">Search</button>
        <button type="button" onclick="startScanner()">Scan</button>
    </form>

    <div class="result">
        <?php echo $result; ?>
    </div>

    <div id="scanner">
        <video id="video" width="300" height="200"></video>
    </div>

    <!-- Include ZXing library -->
    <script src="https://cdn.jsdelivr.net/npm/@zxing/browser@latest/dist/index.umd.min.js"></script>

    <script>
        function startScanner() {
            document.getElementById('scanner').style.display = 'block';
            const codeReader = new ZXing.BrowserMultiFormatReader();
            const videoElement = document.getElementById('video');

            codeReader.decodeFromVideoDevice(null, videoElement, (result, error) => {
                if (result) {
                    document.getElementById('searchValue').value = result.text;
                    document.getElementById('scanner').style.display = 'none';
                    // Optionally submit the form
                    document.querySelector('form').submit();
                }
                if (error && !(error instanceof ZXing.NotFoundException)) {
                    console.error(error);
                }
            }).catch(err => console.error(err));
        }
    </script>
</body>
</html>
