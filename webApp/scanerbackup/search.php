<?php
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: GET, POST, OPTIONS");
header("Access-Control-Allow-Headers: Content-Type, Authorization");

if ($_SERVER['REQUEST_METHOD'] === 'OPTIONS') {
    header("HTTP/1.1 200 OK");
    exit();
}

if ($_SERVER['REQUEST_METHOD'] === 'GET' && isset($_GET['query'])) {
    // Handle search functionality
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
} elseif ($_SERVER['REQUEST_METHOD'] === 'POST' && isset($_POST['data']) && isset($_POST['columnValue']) && isset($_POST['rowIndex'])) {
    // Handle CSV update functionality
    $data = json_decode($_POST['data'], true);
    $columnValue = $_POST['columnValue'];
    $rowIndex = (int) $_POST['rowIndex'];

    $filePath = "data.csv";
    $rows = [];

    // Read existing CSV data
    if (($handle = fopen($filePath, 'r')) !== FALSE) {
        while (($row = fgetcsv($handle, 0, ';')) !== FALSE) {
            $rows[] = $row;
        }
        fclose($handle);

        // Update the specific row and column
        if (isset($rows[$rowIndex])) {
            $rows[$rowIndex][21] = $columnValue; // Update column 22 (zero-based index)
        }

        // Write updated data back to CSV
        if (($handle = fopen($filePath, 'w')) !== FALSE) {
            foreach ($rows as $row) {
                fputcsv($handle, $row, ';');
            }
            fclose($handle);
            echo json_encode(['success' => true]);
        } else {
            echo json_encode(['success' => false, 'message' => 'Error writing to CSV file.']);
        }
    } else {
        echo json_encode(['success' => false, 'message' => 'Error opening CSV file.']);
    }
} else {
    echo json_encode(["message" => "Invalid request."]);
}
?>
