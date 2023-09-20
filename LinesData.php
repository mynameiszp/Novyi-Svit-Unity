<?php
    $servername = "localhost:3306";
    $username = "root";
    $password = "";
    $dbName = "noviy_svit_db";

    //Make connection
    $conn = new mysqli($servername, $username, $password, $dbName);
    //check
    if(!$conn){
        die("Connection Failed. ". mysqli_connect_error());
    }

    $sql = "SELECT * FROM `lines`";
    $result = mysqli_query($conn, $sql);

    if(mysqli_num_rows($result) > 0){
        while($row = mysqli_fetch_assoc($result)){
            //echo var_dump($row) . ";";
            echo "ID:".$row['ID'] . "|Scene_name:".$row['Scene_name'] . "|Speaker_name:".$row['Speaker_name'] . "|Text:".$row['Text'] . "|Avatar_link:".$row['Avatar_link'] . "|;";
        }
    }

?>