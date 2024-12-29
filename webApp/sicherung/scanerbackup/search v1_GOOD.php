<?php
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: GET, POST, OPTIONS");
header("Access-Control-Allow-Headers: Content-Type, Authorization");

if ($_SERVER['REQUEST_METHOD'] === 'OPTIONS') {
    header("HTTP/1.1 200 OK");
    exit();
}

if (isset($_GET['query'])) {
    $query = $_GET['query'];
    $file = fopen("data.csv", "r");

    if ($file === FALSE) {
        echo json_encode(["message" => "Error opening CSV file."]);
        exit();
    }

    $found = false;
    $result = [];
    $headers = fgetcsv($file, 0, ';');

    if ($headers === FALSE) {
        echo json_encode(["message" => "Error reading CSV headers."]);
        fclose($file);
        exit();
    }

    while (($row = fgetcsv($file, 0, ';')) !== FALSE) {
        if (in_array($query, $row)) {
            $resultRow = array_combine($headers, $row);
            $imagePath = 'images/' . $query . '.png';
            if (file_exists($imagePath)) {
                $resultRow['image'] = $imagePath;
            } else {
                $resultRow['image'] = "Kein Bild";
            }
            $result[] = $resultRow;
            $found = true;
        }
    }

    fclose($file);

    if ($found) {
        echo json_encode($result, JSON_PRETTY_PRINT);
    } else {
        echo json_encode(["message" => "No match found."]);
    }
} else {
    echo json_encode(["message" => "No query provided."]);
}
?>
