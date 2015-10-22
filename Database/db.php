<?
// CONNECTIONS =========================================================
$host = "mysql.hostinger.fi"; //put your host here
$user = "u863020339_uni"; //in general is root
$password = "cB7%wM5!"; //use your password here
$dbname = "u863020339_db"; //your database
$db = new PDO('mysql:host='.$host.';dbname='.$dbname.';charset=utf8', $user, $password);
// =============================================================================



// SQL INJECTION PROTECTION
function anti_injection_check($sql, $formUse = true)
{
	$sql = preg_replace("/(from|select|insert|delete|where|drop table|show tables|,|'|#|\*|--|\\\\)/i","",$sql);
	$sql = trim($sql);
	$sql = strip_tags($sql);
	if(!$formUse || !get_magic_quotes_gpc())
	  $sql = addslashes($sql);
	return $sql;
}
// =============================================================================

$unityHash = anti_injection_check($_POST["ob_hash"]);
$phpHash = "zbh564s4antyxd"; // same code in here as in your Unity game


$func = anti_injection_check($_POST["ob_function"]);
$param1 = anti_injection_check($_POST["ob_param1"]);
$param2 = anti_injection_check($_POST["ob_param2"]);
$param3 = anti_injection_check($_POST["ob_param3"]);

if ($unityHash != $phpHash){
    echo "HASH code failed.";
} else {
    try {
    	if($func == "GetAll") {
    		// Select all function 
    		$SQL = "SELECT * FROM events;";
    		$indexer = 0;
	        foreach($db->query($SQL) as $row) {
	        	if($indexer == 0) {
	            	$returnData = $row['event_id'].' '.$row['start_time'].' '.$row['end_time'].' '.$row['date'].' '.$row['location'].' '.$row['person'].' '.$row['activity'].' '.$row['reminder'];
	            } else {
	            	$returnData = $returnData . ' ' . $row['event_id'].' '.$row['start_time'].' '.$row['end_time'].' '.$row['date'].' '.$row['location'].' '.$row['person'].' '.$row['activity'].' '.$row['reminder'];
	            }
	            $indexer = $indexer + 1;
	        }
    	}
    	
    	if($func == "GetEventForBird" && $param1 != null) {
    		$SQL = "SELECT * FROM events WHERE person = '" . $param1 . "'";
    		foreach($db->query($SQL) as $row) {
	            $returnData = $row['event_id'].' '.$row['start_time'].' '.$row['end_time'].' '.$row['date'].' '.$row['location'].' '.$row['person'].' '.$row['activity'].' '.$row['reminder'];
	        }
    	}
    	
    	if($func == "PutEvent" && $param1 != null) {
    		// Parse data from param1
    		$parsedStr = explode(" ",$param1);
    		$SQL = "INSERT INTO events (start_time, end_time, date, location, person, activity, reminder) 
    				VALUES (:start_time, :end_time, :date, :location, :person, :activity, :reminder);";
    		$q = $db->prepare($SQL);
    		$q->execute(array(	':start_time'	=>		$parsedStr[0],
                  				':end_time'		=>		$parsedStr[1],
                  				':date'			=>		$parsedStr[2],
                  				':location'		=>		$parsedStr[3],
                  				':person'		=>		$parsedStr[4],
                  				':activity'		=>		$parsedStr[5],
                  				':reminder'		=>		$parsedStr[6]
                  				));
                  				
            if(!$q) {
				print_r($q->errorInfo()); 
			} else {
				$returnData = "Successfully inserted data.";
			}
    	}
    	
    	if($func == "ChangeEventAll" && $param1 != null) {
    		// Parse data from param1
    		$parsedStr = explode(" ",$param1);
    		$SQL = "UPDATE events 
    				SET start_time=:start_time, end_time=:end_time, date=:date, location=:location, person=:person, activity=:activity, reminder=:reminder
    				WHERE event_id=:event_id;";
    		$q = $db->prepare($SQL);
    		$q->execute(array(	':start_time'	=>		$parsedStr[0],
                  				':end_time'		=>		$parsedStr[1],
                  				':date'			=>		$parsedStr[2],
                  				':location'		=>		$parsedStr[3],
                  				':person'		=>		$parsedStr[4],
                  				':activity'		=>		$parsedStr[5],
                  				':reminder'		=>		$parsedStr[6],
                  				':event_id'		=>		$parsedStr[7],
                  				));
                  				
            if(!$q) {
				print_r($q->errorInfo()); 
			} else {
				$returnData = "Events all attributes changed successfully";
			}
    	}
    	
    	// param1 = event_id
    	// param2 = field
    	// param3 = value
    	if($func == "ChangeEvent" && $param1 != null && $param2 != null && $param3 != null) {
    		// query
 			$SQL = "UPDATE events SET ". $param2 . "=:value WHERE event_id=:id;";
			$q = $db->prepare($SQL);
			$q->execute(array( 	':id' 			=> 		$param1, 
								':value' 		=> 		$param3
								));
			
			if(!$q) {
				print_r($q->errorInfo()); 
			} else {
				$returnData = "Event changed successfully";
			}
    	}
    	
    	if($func == "RemoveEvent" && $param1 != null) {
    		$SQL = "DELETE FROM events WHERE event_id=:id;";
    		$q = $db->prepare($SQL);
    		$q->bindParam(':id', $param1, PDO::PARAM_INT);
    		$q->execute();
    		if(!$q) {
				echo($q->errorInfo()); 
			} else {
				$returnData = "Event id " . $param1 . " deleted successfully";
			}
    		
    	}
    	
    	
    	if($returnData == "") {
    		echo "ERROR: No data returned for " . $func;
    	} else {
    		echo $returnData;
    	}
       
    } catch(PDOException $ex) {
        //Something went wrong rollback!
        $db->rollBack();
        echo $ex->getMessage();
    }
}
?>