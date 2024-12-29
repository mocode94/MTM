<?php
if ($_SERVER['REQUEST_METHOD'] == 'POST' && isset($_FILES['file'])) {
    // Directory where the file will be uploaded
    $upload_dir = 'uploads/';
    // Path to the file to be uploaded
    $upload_file = $upload_dir . basename($_FILES['file']['name']);

    // Move the uploaded file to the designated directory
    if (move_uploaded_file($_FILES['file']['tmp_name'], $upload_file)) {
        echo "File successfully uploaded.";
    } else {
        echo "File upload failed.";
    }
} else {
    echo "No file uploaded.";
}
?>
