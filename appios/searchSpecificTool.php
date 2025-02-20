<?php
if ($_SERVER['REQUEST_METHOD'] === 'GET') {
    // Retrieve the toolID sent from the URL
    $toolID = $_GET['toolID'] ?? null;

    // If toolID is missing, return an error
    if (!$toolID) {
        echo "Error: Missing toolID.";
        exit;
    }


    // Construct the command to run the Python script
    $command = "py searchSpecificTool.py $toolID";

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
