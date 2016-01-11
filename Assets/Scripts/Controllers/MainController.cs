using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Main controller. The main objective of this script is to move marjatta from her house
/// to stockann and simulate a phone call to her on the way there.
/// </summary>

public class MainController : MonoBehaviour {

	public CaregiverGUIScript caregiverGUI;

	public TimeController timeController;

	void Start () {
	
	}

	void Update () {

	}

	TimeController getTimeController() {
		return timeController;
	}
}
