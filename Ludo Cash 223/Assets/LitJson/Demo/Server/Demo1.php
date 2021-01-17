<?php 
$id = $_POST['id']; 

$json['user_id']     = (int)5555; 
$json['user_name']   = (string)$id; 
$json['power_ratio'] = (float)1.4; 

$json['game_data']['game_version']    = (int)2;
$json['game_data']['parameter'][0]    = (int)256;

echo json_encode($json); 
?>