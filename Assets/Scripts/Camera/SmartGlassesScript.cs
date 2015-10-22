using UnityEngine;
using System.Collections;

//Basically same as the maincontroller class, but with the smartglass functions.

public class SmartGlassesScript : MonoBehaviour
{
	bool movingTowardsObject = false;
	bool rotatingTowardsObject = false;
	bool answering = false;
	public CaregiverGUIScript caregiverGUI;
	public GameObject marjatta;
	public GameObject smartGlasses;
	public Transform marjattaPath;
	Quaternion lookRotation;

	public float turnSpeed = 1f;
	public float moveSpeed = 5;
	public Component sphereleftdown;
	public Component sphereleftup;
	public Component sphereleftleft;
	public Component sphereleftright;
	public Component sphererightdown;
	public Component sphererightup;
	public Component sphererightleft;
	public Component sphererightright;
	public Component blink1;
	public Component blink2;
	public Material translucent;
	public Material ledyellow;
	bool blinking;


	Vector3 nextObjectDirection;
	public float dir;

	Transform nextWaypoint;
	
		// Use this for initialization
		void Start ()
		{

				sphereleftdown.GetComponent<Renderer>().material = translucent;
				sphereleftleft.GetComponent<Renderer>().material = translucent;
				sphereleftright.GetComponent<Renderer>().material = translucent;
				sphereleftup.GetComponent<Renderer>().material = translucent;
				sphererightup.GetComponent<Renderer>().material = translucent;
				sphererightdown.GetComponent<Renderer>().material = translucent;
				sphererightright.GetComponent<Renderer>().material = translucent;
				sphererightleft.GetComponent<Renderer>().material = translucent;

				///Can be used to choose which parts get rendered.
				//		marjatta = GameObject.Find ("Marjatta");
//				Renderer [] rend =	marjatta.GetComponentsInChildren<Renderer> () as Renderer [];
				//		Debug.Log (rend.Length);
//				foreach (Renderer renderer in rend) {
//				}

				// Set all waypoints in the same y coordinate as marjatta
				// You must manually set the children of marjattaPath to y = 0
				marjattaPath.transform.position = new Vector3 (marjattaPath.transform.position.x, 
		                                               marjatta.transform.position.y, 
		                                               marjattaPath.transform.position.z);
		
				foreach (Transform t in marjattaPath) {
						if (t.name == "Waypoint1") {
								nextWaypoint = t;
						}
				}
	
				// Start the movement routine according to the scene name
				switch (Application.loadedLevelName) {
				case "Marjattas_house":
						StartCoroutine ("MarjattasHouseRoutine");
						break;
				case "Oulu_city":
						StartCoroutine ("OuluCityRoutine");
						break;
				case "Stockmann":
						StartCoroutine ("StockmannRoutine");
						break;
				case "Marjatta_goes_Pirkko":
						StartCoroutine ("MarjattaGoesPirkkoRoutine");
						break;
				default:
						Debug.Log ("Could not find action routine for the scene");
						break;
				}

			

		}

		// Update is called once per frame
		void Update ()
		{

		//Get the direction for leds.
		if (nextWaypoint != null) {
						Vector3 heading = (nextWaypoint.position - marjatta.transform.position.normalized);
						dir = turningAngle (marjatta.transform.forward, heading, marjatta.transform.up);
				}

		if (blinking == true && nextWaypoint != null) {
			
			Debug.Log("Start blinking!");
			StartCoroutine("blinkLed");
		}


		// If caregiver gui associated with script
		if(caregiverGUI != null) {
			if(caregiverGUI.currentState == CaregiverGUIScript.CaregiverGUIState.CALLING ||
			   caregiverGUI.currentState == CaregiverGUIScript.CaregiverGUIState.TEXT_SENT_NOT_NOTICED) {
				StartCoroutine("AnswerPhone");
			}
			
			if(caregiverGUI.currentState == CaregiverGUIScript.CaregiverGUIState.CALL_ENDED) {
				StartCoroutine("ContinueMoving");
			}
		}
		
		
		// Move towards an object
		if (movingTowardsObject) {
			if(nextWaypoint != null)
				MoveTowardsObject(nextWaypoint);
		}
		
		// Rotate towards object
		if (rotatingTowardsObject) {
			if(nextWaypoint != null)
				RotateTowardsObject(nextWaypoint);
		}


		}

/// <summary>
/// Rotates marjatta towards object.
/// 
/// This code is mostly from:
/// http://answers.unity3d.com/questions/254130/how-do-i-rotate-an-object-towards-a-vector3-point.html
/// </summary>
void RotateTowardsObject (Transform nextWaypoint)
{
	//find the vector pointing from our position to the target
	nextObjectDirection = (nextWaypoint.position - marjatta.transform.position).normalized;
	
	//create the rotation we need to be in to look at the target
	lookRotation = Quaternion.LookRotation(nextObjectDirection);
	
	//rotate us over time according to speed until we are in the required rotation
	marjatta.transform.rotation = Quaternion.Slerp(marjatta.transform.rotation, lookRotation, Time.deltaTime * turnSpeed * 0.5f);
	
}

void MoveTowardsObject (Transform nextWaypoint)
{
	// Move
	marjatta.transform.position = Vector3.MoveTowards(marjatta.transform.position, nextWaypoint.position, Time.deltaTime * moveSpeed);
	
	// If object near waypoint, change to next
	if(Vector3.Distance(marjatta.transform.position, nextWaypoint.transform.position) < 0.01f) {
		this.nextWaypoint = GetNextWaypoint();
		if(nextWaypoint != null) {
			rotatingTowardsObject = true;
		}
	}
}

Transform GetNextWaypoint ()
{
	// Parse the current waypoints number and add one to it
	char lastChar = nextWaypoint.name[nextWaypoint.name.Length - 1];
	int waypointNumber = int.Parse (lastChar.ToString());
	waypointNumber++;
	
	// Search for the waypoint
	foreach (Transform t in marjattaPath) {
		if(t.name == "Waypoint" + waypointNumber) {
			return t;
		}
	}
	
	// If nothing is found
	return null;
}

/// <summary>
/// Oulus city action routine.
/// </summary>
IEnumerator OuluCityRoutine() {
	// Hide marjatta, so she can appear from her house
	marjatta.SetActive (false);
	// Wait 2 seconds and make marjatta visible
	yield return new WaitForSeconds(2);
	marjatta.SetActive (true);
	yield return new WaitForSeconds(1);
	rotatingTowardsObject = true;
	movingTowardsObject = true;
	
}

/// <summary>
/// Marjattases house action routine.
/// </summary>
IEnumerator MarjattasHouseRoutine() {
	yield return new WaitForSeconds(1);
	// Start movement towards first waypoint
	moveSpeed = 3;
	movingTowardsObject = true;
}

/// <summary>
/// Marjattases house action routine.
/// </summary>
IEnumerator MarjattaGoesPirkkoRoutine() {
	yield return new WaitForSeconds(1);
	// Start movement towards first waypoint
	moveSpeed = 3;
	movingTowardsObject = true;
}

/// <summary>
/// Stockmanns action routine.
/// Uses the waypoint system
/// </summary>
IEnumerator StockmannRoutine() {
	yield return new WaitForSeconds(1);
	movingTowardsObject = true;
}

/// <summary>
/// Continues the moving.
/// </summary>
/// <returns>The moving.</returns>
IEnumerator ContinueMoving() {
	// Let the caregiverGui fall back to the main menu
	answering = false;
	caregiverGUI.currentState = CaregiverGUIScript.CaregiverGUIState.MAIN;
	yield return new WaitForSeconds(1);
	movingTowardsObject = true;
}

/// <summary>
/// When the senior answer phone.
/// </summary>
/// <returns>The phone.</returns>
IEnumerator AnswerPhone() {
	if(!answering) {
		answering = true;
		// Generate a random time to answer the phone.
		yield return new WaitForSeconds(Random.Range(3, 9));
		if(caregiverGUI.currentState == CaregiverGUIScript.CaregiverGUIState.CALLING) {
			movingTowardsObject = false;
			caregiverGUI.currentState = CaregiverGUIScript.CaregiverGUIState.CALLING_ANSWERED;
		}
		
		if(caregiverGUI.currentState == CaregiverGUIScript.CaregiverGUIState.TEXT_SENT_NOT_NOTICED) {
			caregiverGUI.currentState = CaregiverGUIScript.CaregiverGUIState.TEXT_NOTICED;
			movingTowardsObject = false;
		}
		answering = false;
	}
}






	//Determine if the the next waypoint is on left or right.
	float turningAngle (Vector3 forward, Vector3 birdDir, Vector3 up) {

		Vector3 v = Vector3.Cross (forward, birdDir);
		float dir = Vector3.Dot (v, up);


		//If left.
		if (dir < 0.0f) {
						
						if (Vector3.Distance (marjatta.transform.position, nextWaypoint.transform.position) < 8.0f) {
								Debug.Log ("Turning to right.");
								blink1 = sphereleftright;
								blink2 = sphererightright;
								blinking = true;
								
								return 1f;
						}
						//If right.
				} else if (dir > 0.0f) {
						
						if (Vector3.Distance (marjatta.transform.position, nextWaypoint.transform.position) < 8.0f) {
								Debug.Log ("Turning to left.");
								blink1 = sphereleftleft;
								blink2 = sphererightleft;
								blinking = true;
								return -1f;
						}
				}

			
		
		else {
			return 0f;
		}

		blinking = false;
		return 0f;
		}

	//TODO: Loop these in array.
	public IEnumerator blinkLed() {

		Debug.Log ("Blinking leds!");
		yield return new WaitForSeconds (0.5f);
		blink1.GetComponent<Renderer>().material = ledyellow;
		blink2.GetComponent<Renderer>().material = ledyellow;
		yield return new WaitForSeconds (0.5f);
		blink1.GetComponent<Renderer>().material = translucent;
		blink2.GetComponent<Renderer>().material = translucent;
		yield return new WaitForSeconds (0.5f);
		blink1.GetComponent<Renderer>().material = ledyellow;
		blink2.GetComponent<Renderer>().material = ledyellow;
		yield return new WaitForSeconds (0.5f);
		blink1.GetComponent<Renderer>().material = translucent;
		blink2.GetComponent<Renderer>().material = translucent;
		yield return new WaitForSeconds (0.5f);
		blink1.GetComponent<Renderer>().material = ledyellow;
		blink2.GetComponent<Renderer>().material = ledyellow;
		yield return new WaitForSeconds (0.5f);
		blink1.GetComponent<Renderer>().material = translucent;
		blink2.GetComponent<Renderer>().material = translucent;
		yield return new WaitForSeconds (1f);
		StartCoroutine ("blinkLedForward");
				}

	//TODO: Loop these in array.
	public IEnumerator blinkLedForward() {
		sphereleftup.GetComponent<Renderer>().material = ledyellow;
		sphererightup.GetComponent<Renderer>().material = ledyellow;
		yield return new WaitForSeconds(0.5f);
		sphereleftup.GetComponent<Renderer>().material = translucent;
		sphererightup.GetComponent<Renderer>().material = translucent;
		yield return new WaitForSeconds(0.5f);
		sphereleftup.GetComponent<Renderer>().material = ledyellow;
		sphererightup.GetComponent<Renderer>().material = ledyellow;
		yield return new WaitForSeconds(0.5f);
		sphereleftup.GetComponent<Renderer>().material = translucent;
		sphererightup.GetComponent<Renderer>().material = translucent;

				
				}
		

}

