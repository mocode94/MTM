<?php
// run_script.php
$output = shell_exec('python moveToolWebAPI.py 2>&1');
echo $output;
?>