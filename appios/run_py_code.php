<?php
if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    // Retrieve the toolID sent from the Swift app
    $toolID = $_POST['toolID'] ?? null;

    // If toolID is missing, return an error
    if (!$toolID) {
        echo "Error: Missing toolID.";
        exit;
    }

    // Sanitize the input to prevent command injection
    $sanitizedToolID = escapeshellarg($toolID);

    // Construct the command to run the Python script
    $command = escapeshellcmd("py searchSpecificTool.py $sanitizedToolID");
    
    // Execute the Python script and capture the output
    $output = shell_exec($command);

    // Output the result or an error message
    if ($output) {
        echo $output;
    } else {
        echo "Kein passendes Werkzeug gefunden.";
    }
} else {
    echo "Invalid request method.";
}
?>

