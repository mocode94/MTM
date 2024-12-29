<?php
if (isset($_GET['searchedValue']) && isset($_GET['selectedPlace'])) {
    $searchedValue = $_GET['searchedValue'];
    $selectedPlace = $_GET['selectedPlace'];
    
    // Define the file path
    $filePath = 'move.txt';

    // Write to the file
    $fileContent = $searchedValue . "\n" . $selectedPlace;
    if (file_put_contents($filePath, $fileContent) !== false) {
        echo "TXT file created successfully.";
    } else {
        echo "Error: Unable to write to file.";
    }
} else {
    echo "Error: Missing parameters.";
}
// run_script.php
$output = shell_exec('py moveToolWebAPI.py 2>&1');
echo $output;
?>
