<?php
// Set the content type header for plain text output
header('Content-Type: text/plain; charset=utf-8'); // Make sure it's text/plain

// Check if the necessary GET parameters are set
if (isset($_GET['searchedValue']) && isset($_GET['selectedPlace'])) {
    $searchedValue = $_GET['searchedValue'];
    $selectedPlace = $_GET['selectedPlace'];

    // Define the file path
    $filePath = 'move.txt';

    // Prepare the file content to write
    $fileContent = $searchedValue . "\n" . $selectedPlace;
    
    // Attempt to write to the file
    if (file_put_contents($filePath, $fileContent) !== false) {
        echo "Befehl weitergeben\n";  // Acknowledge file write success
    } else {
        // If file write fails, provide error message
        echo "Error: Unable to write to file.\n";
    }

    // Execute the Python script and capture the output
    $output = shell_exec('py moveToolWebAPI.py 2>&1');
    
    // Split the output into lines
    $lines = explode("\n", $output);

    // Output each line as plain text
    foreach ($lines as $line) {
        echo $line . "\n"; // Ensure it's plain text
    }
} else {
    // Return error if parameters are missing
    echo "Error: Missing parameters.\n";
}
?>
