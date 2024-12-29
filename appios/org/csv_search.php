<?php
header('Access-Control-Allow-Origin: *'); // Allow requests from any origin
header('Content-Type: application/json'); // Set content type to JSON

if (isset($_GET['query'])) {
    $query = strtolower($_GET['query']);
    $csvFile = fopen("testcsv.csv", "r"); // Ensure testcsv.csv is in the same folder

    $results = [];
    while (($row = fgetcsv($csvFile)) !== false) {
        if (strpos(strtolower(implode(" ", $row)), $query) !== false) {
            $results[] = $row;
        }
    }
    fclose($csvFile);

    echo json_encode($results);
}
?>
