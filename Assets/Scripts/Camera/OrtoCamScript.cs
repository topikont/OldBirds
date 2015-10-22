using UnityEngine;
using System.Collections;

/// <summary>
/// Main camera script.
/// </summary>
public class OrtoCamScript : MonoBehaviour {
	public bool scrollingEnabled = true;
	public float scrollSpeed = 1.0f;
	void Start () {	}
	

	void Update () {
		// keyboard scrolling
		float translationX = Input.GetAxis("Horizontal");
		float translationY = Input.GetAxis("Vertical");
		float fastTranslationX = 2 * Input.GetAxis("Horizontal");
		float fastTranslationY = 2 * Input.GetAxis("Vertical");

		if (scrollingEnabled) {

			if (Input.GetKey (KeyCode.LeftShift)) {
				// Left
				transform.parent.transform.Translate (fastTranslationX + fastTranslationY, 0, 0);	
			} else {
				// Right
				transform.parent.transform.Translate (translationX + translationY, 0, 0); 
			}

			// mouse scrolling
			float mousePosX = Input.mousePosition.x;
			float mousePosY = Input.mousePosition.y;
			int scrollDistance = 5;

			// Horizontal camera movement
			if (mousePosX < scrollDistance) { 
					//horizontal, left
					transform.parent.transform.Translate (-1 * scrollSpeed, 0, 0);
			} 
			if (mousePosX >= Screen.width - scrollDistance) { 
					// horizontal, right
					transform.parent.transform.Translate (1 * scrollSpeed, 0, 0);
			} 

			// Vertical camera movement
			if (mousePosY < scrollDistance) {
				//scrolling down
				transform.parent.transform.Translate (0, 0, -1 * scrollSpeed);
			} 
			if (mousePosY >= Screen.height - scrollDistance) {
				//scrolling up
				transform.parent.transform.Translate (0, 0, 1 * scrollSpeed);
			}

			//zooming

			if (Input.GetAxis ("Mouse ScrollWheel") > 0 && this.GetComponent<Camera>().orthographicSize > 4) {
					this.GetComponent<Camera>().orthographicSize = this.GetComponent<Camera>().orthographicSize - 6;
			}

			//
			if (Input.GetAxis ("Mouse ScrollWheel") < 0 && GetComponent<Camera>().GetComponent<Camera>().orthographicSize < 300) {
					this.GetComponent<Camera>().orthographicSize = this.GetComponent<Camera>().orthographicSize + 6;
			}
		} // /ScrollinEnabled

	}
}
