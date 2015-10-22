using UnityEngine;
using System.Collections;

/// <summary>
/// Waypoint script.
/// 
/// Waypoints can be placed in the ground. Name the gameobject Waypoint1, Waypoint2 etc.
/// and place them under a paren gameobject. Place the parent object in to the MainControllers variable.
/// 
/// This is basically a dummy script which only purpose is to show the gizmo.
///
/// </summary>

public class WaypointScript : MonoBehaviour {
	public Color gizmoColor = Color.black;
	public bool stopPoint = false;
	public int stopTimeInMinutes = 5;
	public bool changeSpeedPoint = false;
	public int changeSpeedWithMultiplier = 1;

	void OnDrawGizmos() {
		/// Gizmos can only be seen in the editor

		// Draw a black ball of size 1
		Gizmos.color = gizmoColor;
		Vector3 cubeCenter = new Vector3 (transform.position.x, transform.position.y + 0.5f, transform.position.z); 
		Gizmos.DrawSphere (cubeCenter, 0.5f);

		// Draw a green arrow on top of it (commented cause doesnt have a function yet
		//Vector3 arrowPos = new Vector3 (this.transform.position.x, this.transform.position.y + 1, this.transform.position.z);
		//DrawArrow.ForDebug(arrowPos, transform.forward, Color.green);
	}
}
