<?php
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: POST");
header("Access-Control-Allow-Headers: Content-Type");

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $data = json_decode($_POST['data'], true);
    $columnValue = $_POST['columnValue'];
    $searchValue = $_POST['searchValue']; // The value to search for in column 0

    if (empty($data) || empty($searchValue)) {
        echo json_encode(['success' => false, 'message' => 'Invalid input.']);
        exit();
    }

    $filePath = 'data.csv';

    // Read existing CSV data
    $rows = [];
    if (($handle = fopen($filePath, 'r')) !== FALSE) {
        $headers = fgetcsv($handle, 0, ';');
        if ($headers === FALSE) {
            echo json_encode(['success' => false, 'message' => 'Error reading CSV headers.']);
            fclose($handle);
            exit();
        }

        while (($row = fgetcsv($handle, 0, ';')) !== FALSE) {
            if ($row[0] === $searchValue) {
                // Update the specific column (22nd column, index 21)
                $row[21] = $columnValue;
            }
            $rows[] = $row;
        }
        fclose($handle);

        // Write updated data back to CSV
        if (($handle = fopen($filePath, 'w')) !== FALSE) {
            // Write the headers back to the CSV
            fputcsv($handle, $headers, ';');

            // Write all rows back to the CSV
            foreach ($rows as $row) {
                fputcsv($handle, $row, ';');
            }
            fclose($handle);

            echo json_encode(['success' => true]);
        } else {
            echo json_encode(['success' => false, 'message' => 'Error opening CSV file for writing.']);
        }
    } else {
        echo json_encode(['success' => false, 'message' => 'Error opening CSV file.']);
    }
} else {
    echo json_encode(['success' => false, 'message' => 'Invalid request method.']);
}
?>
