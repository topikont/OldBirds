using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class IndividualMovement : MonoBehaviour {
	public int moveSpeed = 4;
	public float turnSpeed = 35f;
	public bool randomMovement = true;
	public Transform determinedPath = null;
	public bool rotatingTowardsObject = false;
	public bool movingTowardsObject = false;
	public Transform nextWaypoint = null;
	Vector3 nextObjectDirection;
	Quaternion lookRotation;
	
	public List<Transform> randomWaypoints;
	public List<Transform> visitedWaypoints;
	private int defaultMoveSpeed;
	private float defaultTurningSpeed;
	
	public int GetDefaultMoveSpeed() {
		return defaultMoveSpeed;
	}
	
	public float GetDefaultTurningSpeed() {
		return defaultTurningSpeed;
	}

	TimeController tc;
	void Awake() {
		randomWaypoints = new List<Transform>();
		visitedWaypoints = new List<Transform>();
		defaultMoveSpeed = moveSpeed;
		defaultTurningSpeed = turnSpeed;
		if (GameObject.Find ("MainController").GetComponent<TimeController> () != null) {
			tc = GameObject.Find ("MainController").GetComponent<TimeController> ();
		}
	}

	DateTime stoppedAtTime;
	DateTime leaveTime;
	public enum MovementStates { MOVING, STOPPED }
	public MovementStates currentState = MovementStates.MOVING;

	void Update() {
		if(nextWaypoint)
			Debug.DrawLine(transform.position, nextWaypoint.position, Color.green);
		if(lastWaypoint)
			Debug.DrawLine(transform.position, lastWaypoint.position, Color.black);

		WaypointScript nextWp = nextWaypoint.GetComponent<WaypointScript> ();
		if (nextWp.stopPoint && currentState == MovementStates.MOVING) {
			this.currentState = MovementStates.STOPPED;
			this.stoppedAtTime = tc.getCurrentDateTime();
			this.leaveTime = stoppedAtTime.AddMinutes(nextWp.stopTimeInMinutes);
			nextWp.stopPoint = false;
			if(nextWp.gameObject.GetComponent<PointOfInterest>() != null) {
				nextWp.gameObject.GetComponent<PointOfInterest>().show();
			}
		}

		if (currentState == MovementStates.STOPPED) {
			if(tc.getCurrentDateTime() >= leaveTime) {
				currentState = MovementStates.MOVING;
				if(nextWp.gameObject.GetComponent<PointOfInterest>() != null) {
					nextWp.gameObject.GetComponent<PointOfInterest>().hide();
				}
			}
		}

		// move
		if(movingTowardsObject && currentState == MovementStates.MOVING)
			MoveTowardsObject();
		// Rotate towards object	
		if(rotatingTowardsObject)
			RotateTowardsObject();

	}


	
	public int waypointIndex = 0;
	private Transform lastWaypoint;
	
	public void MoveTowardsObject ()
	{
		// Move
		
		if(moveSpeed >= 0) {
			if(nextWaypoint)
				transform.position = Vector3.MoveTowards(transform.position, nextWaypoint.position, Time.deltaTime * moveSpeed / 10);
		} else {
			if(lastWaypoint)
				transform.position = Vector3.MoveTowards(transform.position, lastWaypoint.position, Time.deltaTime * Math.Abs(moveSpeed) / 10);
		}
		
		if(moveSpeed < 0) {
			// If rewinding, go through the visited waypoints 
			if(waypointIndex > 0) {
				lastWaypoint = visitedWaypoints[waypointIndex];
			} else if(!lastWaypoint) {
				// Add next random waypoint (simulate history) if wa are at the beginning
				// Debug for the instance where user presses rewind in the beginning of the simulation
				SetNextRandomWaypoint(true);
				visitedWaypoints.Insert(0, lastWaypoint);
			}
			
			// if close enough to the last waypoint, get another from the last waypoints list 
			if(Vector3.Distance(transform.position, lastWaypoint.transform.position) < 0.01f) {
				if(waypointIndex > 0) {
					waypointIndex--;
					if(visitedWaypoints.Count > waypointIndex) {
						lastWaypoint = visitedWaypoints[waypointIndex];
					} 
				} else {
					if(randomMovement) {
						// Add next random waypoint (simulate history) if wa are at the beginning
						SetNextRandomWaypoint(true);
						visitedWaypoints.Insert(0, lastWaypoint);
					} else {
						// TODO: When in scenario mode
					}
				}
			}
		} else if(visitedWaypoints.Count > waypointIndex+1 && moveSpeed > 0) {
			// Forward moving when there's a pattern (already rewinded at some point)
			// If object near waypoint, change to next
			nextWaypoint = visitedWaypoints[waypointIndex+1];
			if(Vector3.Distance(transform.position, nextWaypoint.transform.position) < 0.01f && moveSpeed > 0) {
				waypointIndex++;
				if(randomMovement) {
					nextWaypoint = visitedWaypoints[waypointIndex];
				} else {
					SetNextWaypoint();
				}
				
				if(nextWaypoint != null) {
					rotatingTowardsObject = true;
				}
			}
		} else {
			// Normal forward movement with random waypoint generation
			if(Vector3.Distance(transform.position, nextWaypoint.transform.position) < 0.01f && moveSpeed > 0) {
				visitedWaypoints.Add(nextWaypoint);
				waypointIndex++;
				if(randomMovement) {
					SetNextRandomWaypoint(false);
				} else {
					SetNextWaypoint();
				}
				
				if(nextWaypoint != null) {
					rotatingTowardsObject = true;
				}
			}
		}
	}

	
	public void SetupRandomWaypoints(Transform _randomWaypoints) {
		if(randomWaypoints.Count == 0) {
			// Set up the list of waypoints
			randomWaypoints.Clear();
			foreach(Transform waypoint in _randomWaypoints) {
				randomWaypoints.Add(waypoint);
			}
		}
	}

	public void SetNextRandomWaypoint(bool movingBackwards) {
		

		List<Transform> tryWaypoints = new List<Transform>();
		
		foreach(Transform t in randomWaypoints) {
			tryWaypoints.Add(t);
		}
		
		bool nextFound = false;
		do {
			// Get random waypoint from list
			int random = UnityEngine.Random.Range(0, tryWaypoints.Count);
			Transform nextWP = tryWaypoints[random];
			
			// Raycast for line of sight
			float distance = Vector3.Distance(transform.position, nextWP.position);
			RaycastHit hit;
			Vector3 heading = nextWP.position - transform.position;
			bool nonWalk = false;
			if (Physics.Raycast(transform.position, heading, out hit, distance)) { 
				// Raycast hit something
				// Check if it's an Non-Walkable collider
				Debug.DrawRay(transform.position, heading);
				if(hit.transform.tag != "Non-Walkable") {
					// If not, add a waypoint
					//Debug.Log("Wasn't non-walkable");
					
					if(!movingBackwards) 
						nextWaypoint = nextWP;
					else {
						// This for the rewind movement and simulating history, if we've at the starting point
						lastWaypoint = nextWP;
					}
					nextFound = true;
				} 
			} else {
				// Raycast hit nothing
				if(!movingBackwards) 
					nextWaypoint = nextWP;
				else {
					lastWaypoint = nextWP;
				}
				nextFound = true;
			}
			// Bird cant see the next waypoint (unwalkable in the way), 
			// remove waypoint from list and repeat 
			
			tryWaypoints.RemoveAt(random);
			
		} while (nextFound == false && tryWaypoints.Count > 1);
		
		if(tryWaypoints.Count < 1) {
			Debug.LogWarning("Did not find any waypoints that did not collide with Non-Walkable");
		} else {
			// Next waypoint in sight, start moving
			movingTowardsObject = true;
			rotatingTowardsObject = true;
		}
		
	}
	
	public void RotateTowardsObject()
	{
		//find the vector pointing from our position to the target
		if(moveSpeed >= 0)
			nextObjectDirection = (nextWaypoint.position - transform.position).normalized;
		else {
			nextObjectDirection = (transform.position - lastWaypoint.position).normalized;
		}
		
		
		//create the rotation we need to be in to look at the target
		
		lookRotation = Quaternion.LookRotation(nextObjectDirection);

		//rotate us over time according to speed until we are in the required rotation
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed * 0.5f);
		
	}

	public void SetNextWaypoint ()
	{
		if(determinedPath != null) {
			// Parse the current waypoints number and add one to it
			// This is used in the determined paths, like in the marjatta scenario
			String lastTwoChars = nextWaypoint.name.Substring(nextWaypoint.name.Length - 2);
			int waypointNumber;
			if(Char.IsNumber(lastTwoChars.ToCharArray()[0])) {
				waypointNumber = int.Parse(lastTwoChars);
			} else {
				waypointNumber = int.Parse(lastTwoChars.ToCharArray()[1].ToString());
			}
			waypointNumber++;
			
			// Search for the waypoint
			foreach (Transform t in determinedPath) {
				if(t.name == "Waypoint" + waypointNumber) {
					nextWaypoint = t;
				}
			}
		} else {
			Debug.LogWarning("Determined path not assigned. Can't move " + transform.name + " object.");
		}
	}
}











