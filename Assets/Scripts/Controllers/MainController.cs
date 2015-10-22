using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Oulucity controller. The main objective of this script is to move marjatta from her house
/// to stockann and simulate a phone call to her on the way there.
/// </summary>

public class MainController : MonoBehaviour {
	bool movingTowardsObject = false;
	bool rotatingTowardsObject = false;
	bool answering = false;
	 
	public float turnSpeed = 10f;
	public float moveSpeed = 5;
	public GameObject marjatta;
	public Transform marjattaPath;
	public CaregiverGUIScript caregiverGUI;
	
	public Transform randomWaypoints;
	// Those objects that start movement with random waypoints
	public GameObject[] randomMovementObjects;
	// Those objects that start movement with pre-determined path
	public GameObject[] determinedPathObjects;

	Transform nextWaypoint;

	Vector3 nextObjectDirection;

	Quaternion lookRotation;

	void Start () {
		// This is the path to stockmann
		// Assign marjatta to the main controller to make this work
		foreach(GameObject ob in determinedPathObjects) {
			if (ob.GetComponent<IndividualMovement>().determinedPath != null) {
				IndividualMovement im = ob.GetComponent<IndividualMovement>();
				foreach(Transform t in im.determinedPath) {
					if(t.name == "Waypoint1") {
						im.nextWaypoint = t;
						im.rotatingTowardsObject = true;
					}
				}

			} else {
				Debug.LogWarning("No path found for " + ob.name);
			}
		}
		
		if(1 == 2) {
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
			case "new_scene":
				StartCoroutine ("newSceneRoutine");
				break;
			case "Marjatta_goes_Pirkko":
				StartCoroutine ("MarjattaGoesPirkkoRoutine");
				break;
			default:
				Debug.Log ("Could not find action routine for the scene");
				break;
			}
		}
		if(randomMovementObjects != null) {
			// The random movement routine
			foreach(GameObject bird in randomMovementObjects) {
				IndividualMovement movement = bird.GetComponent<IndividualMovement>();
				// Setup waypoints
				movement.SetupRandomWaypoints(randomWaypoints);
				
				// Randomize a spawning point and add the waypoint to visited list
				int random = Random.Range(0, movement.randomWaypoints.Count);
				movement.transform.position = movement.randomWaypoints[random].position;	
				movement.visitedWaypoints.Add(movement.randomWaypoints[random]);
				
				// Set the next waypoint and start moving
				movement.SetNextRandomWaypoint(false);
			}
		}
	}

	void Update () {
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


		// Move random movers towards an object
		foreach(GameObject bird in randomMovementObjects) {
			IndividualMovement movement = bird.GetComponent<IndividualMovement>();
			// move
			if(movement.movingTowardsObject)
				movement.MoveTowardsObject();
			// Rotate towards object	
			if(movement.rotatingTowardsObject)
				movement.RotateTowardsObject();
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

	IEnumerator newSceneRoutine() {
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

}
