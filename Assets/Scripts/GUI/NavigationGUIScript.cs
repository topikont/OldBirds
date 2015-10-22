using UnityEngine;
using System.Collections;

/// <summary>
/// Navigation GUI script for user to navigate through scenes manually.
/// </summary>
public class NavigationGUIScript : MonoBehaviour {
	private bool hide = false;

	void Update() {
		if(Input.GetKeyDown(KeyCode.H)) {
			if(!hide)
				hide = true;
			else
				hide = false;
		}
	}

	void OnGUI () {
		if(!hide) {
			// Background
			GUI.Box(new Rect(10,10,140,120), "Simulations");
			
			// Create buttons
			if(GUI.Button(new Rect(20,40,120,20), "Caregiver UI")) {
				Application.LoadLevel("Marjattas_house");
			}

			if(GUI.Button(new Rect(20,70,120,20), "Smart glasses")) {
				Application.LoadLevel("Marjatta_goes_Pirkko");
			}

			if(Application.loadedLevelName == "Marjattas_house" || Application.loadedLevelName == "Oulu_city") {
				// Skip scene button
				if(GUI.Button(new Rect(20,100,120,20), "Skip scene")) {
					Application.LoadLevel(Application.loadedLevel + 1);
				}
			}
		}
	}
}
