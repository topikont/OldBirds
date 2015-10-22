using UnityEngine;
using System.Collections;

public class ChangeSceneTrigger : MonoBehaviour {
	

	public string nextSceneName = "";
	void OnTriggerEnter(Collider col) {
		if(col.gameObject.name == "Marjatta" || col.gameObject.name == "MouseOverGUI")
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
