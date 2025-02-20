<?php
if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    // Retrieve values sent from Swift app
    $diameter = $_POST['radius'] ?? null;
    $typ = $_POST['typ'] ?? null;
    $tiefe = $_POST['tiefe'] ?? null;

    // If any parameter is missing, return an error
    if (!$diameter || !$typ || !$tiefe) {
        echo "Error: Missing parameters.";
        exit;
    }

    // Construct the command to run the Python script
    $command = escapeshellcmd("py Tool4Job.py $diameter $typ $tiefe");
    
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
