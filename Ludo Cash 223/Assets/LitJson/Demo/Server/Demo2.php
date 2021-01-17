<?php 
$get_value  = $_GET['get_value'];
$post_value = $_POST['post_value']; 

$json['str_value']        = (string)'Meruem'; 
$json['str_values'][0]    = (string)$get_value;
$json['str_values'][1]    = (string)$post_value;
$json['int_value']        = (int)101; 
$json['int_values'][0]    = (int)102; 
$json['long_value']       = (int)103; 
$json['long_values'][0]   = (int)104; 
$json['float_value']      = (float)105.1; 
$json['float_values'][0]  = (float)105.2; 
$json['double_value']     = (double)106.1; 
$json['double_values'][0] = (double)106.2; 
$json['bool_value']       = (bool)false; 
$json['bool_values'][0]   = (bool)true; 

$json['class_value']['id']   = (int)107;
$json['class_value']['name'] = (string)'BungeeGum';

$json['class_values'][0]['id']   = (int)108;
$json['class_values'][0]['name'] = (string)'Moraz';

$json['class_values'][0]['inner_class']['id']   = (int)109;
$json['class_values'][0]['inner_class']['name'] = (string)'Pokkuru';

$json['class_values'][0]['game_data'][0]['game_version']    = (int)110;
$json['class_values'][0]['game_data'][0]['parameter'][0]    = (int)512;
$json['class_values'][0]['game_data'][1]['game_version']    = (int)111;
$json['class_values'][0]['game_data'][1]['parameter'][0]    = (int)1024;

echo json_encode($json); 
?>