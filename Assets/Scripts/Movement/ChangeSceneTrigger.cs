using UnityEngine;
using System.Collections;

/*
 *	The next scene is triggered when the named gameObject collides with the sceneTrigger object
 *	The name of the avatar and the name of the next scene need to be set for the trigger to occur
 */
public class ChangeSceneTrigger : MonoBehaviour {

	public string mainAvatarName = "Marjatta";

	public string nextSceneName = "";

	//When the avatar and the sceneTrigger object collide. Load the next scene.
	void OnTriggerEnter(Collider col) {
		if(col.gameObject.name == mainAvatarName || col.gameObject.name == "MouseOverGUI")
			Application.LoadLevel(nextSceneName);

		Debug.Log (col.gameObject.name);
	}

	public Color32 gizmoColor;
	void OnDrawGizmos() {
		/// Gizmos can only be seen in the editor

		Gizmos.color = gizmoColor;
		Gizmos.DrawCube(transform.position, new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z));

	}
}
