<?php
// Set the path to your CSV file
$csvFile = 'TEDI.csv';

// Check if the file exists
if (!file_exists($csvFile)) {
    die('File not found');
}

// Open the CSV file
$file = fopen($csvFile, 'r');

// Start HTML output
echo '<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>CSV Viewer</title>
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
    </style>
</head>
<body>
    <h1>CSV Viewer</h1>
    <table>';

// Read the CSV file and output as an HTML table
$isHeader = true;
while (($data = fgetcsv($file, 1000, ';')) !== FALSE) {
    echo '<tr>';
    foreach ($data as $cell) {
        if ($isHeader) {
            echo '<th>' . htmlspecialchars($cell) . '</th>';
        } else {
            echo '<td>' . htmlspecialchars($cell) . '</td>';
        }
    }
    echo '</tr>';
    $isHeader = false;
}

// Close the file
fclose($file);

// End HTML output
echo '</table>
</body>
</html>';
?>
