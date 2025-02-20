<?php
// Set the content type header for plain text output
header('Content-Type: text/plain; charset=utf-8'); // Make sure it's text/plain

if (isset($_GET['searchedValue'])) {
    $searchedValue = $_GET['searchedValue'];
    $inputValue = isset($_GET['inputValue']) ? $_GET['inputValue'] : null;

    $filePath = 'measure.txt';
    $fileContent = $searchedValue;

    if (file_put_contents($filePath, $fileContent) !== false) {


        if ($inputValue !== null) {
            // Second run with inputValue
            $command = escapeshellcmd("py measureIOSAPP.py") . " " . escapeshellarg($inputValue);
            $output = shell_exec($command . " 2>&1");
        } else {
            // First run without inputValue
            $command = escapeshellcmd("py measureIOSAPP.py");
            $output = shell_exec($command . " 2>&1");
            
        }

        if ($output === null) {
            echo "Error: Failed to execute Python script.\n";
            exit(1);
        }

        $lines = explode("\n", $output);
        foreach ($lines as $line) {
            echo $line . "\n";
        }
    } else {
        echo "Error: Unable to write to file.\n";
        exit(1);
    }
} else {
    echo "Error: Missing parameters.\n";
}
?>
