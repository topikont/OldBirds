using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Main controller. The MainController acts as the main API access point for the TimeController script
///
/// </summary>

public class MainController : MonoBehaviour {

	public CaregiverGUIScript caregiverGUI;

	//And instance of the TimeController script is held in the MainController
	private static TimeController timeController;

	void Start () {
		//Create and add a TimeController to the MainController component
		timeController = gameObject.AddComponent<TimeController> ();

		/*
		 * 	Add functionality to the UI buttons in the scene. UICanvas prefab must exist in the scene.
		 */
		GameObject.Find ("PauseBtn").GetComponent<Button> ().onClick.AddListener(timeController.PauseTime);
		GameObject.Find ("PlayBtn").GetComponent<Button> ().onClick.AddListener(timeController.ResumeTime);
		GameObject.Find ("BackwardsBtn").GetComponent<Button> ().onClick.AddListener(timeController.DoubleReverseTime);
		GameObject.Find ("ForwardBtn").GetComponent<Button> ().onClick.AddListener(timeController.ForwardTime);
		GameObject.Find ("PlaybackBtn").GetComponent<Button> ().onClick.AddListener(timeController.ReverseTime);
	}

	void Update () {

	}

	public TimeController getTimeController() {
		return timeController;
	}
}
