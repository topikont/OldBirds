using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PointOfInterest : MonoBehaviour {
	private Image imagePanel;
	private Text textPanel;
	public Sprite image;
	public string text;
	public Transform guiPrefab;

	private Transform instance;
	public Canvas UICanvas;

	void Awake() {
		instance = Instantiate (guiPrefab);
		imagePanel = instance.Find("Image").GetComponentInChildren<Image> ();
		imagePanel.GetComponent<Image> ().sprite = image;
		
		textPanel = instance.GetComponentInChildren<Text> ();
		textPanel.text = text;

		instance.SetParent (UICanvas.transform, false);
		Vector3 guiPosition = Camera.main.WorldToScreenPoint(this.transform.position);
		guiPosition.Set (guiPosition.x, guiPosition.y + 300f, guiPosition.z);
		instance.GetComponent<RectTransform>().position = guiPosition;
		hide ();
	}

	void Update() {
		Vector3 guiPosition = Camera.main.WorldToScreenPoint (this.transform.position);
		guiPosition.Set (guiPosition.x, guiPosition.y + 100f, guiPosition.z);
		instance.GetComponent<RectTransform>().position = guiPosition;
	}

	public void show() {
		if (GetComponent<AudioSource> () != null) {
			// Plays the audio attached to the gameobject
			GetComponent<AudioSource> ().Play();

			// Adds the name of the sound to DB along with a new ID
			GameObject.Find("MainController").GetComponent<SQLHandler>().addPendingNotification(GetComponent<AudioSource> ().clip.name);
		}
		instance.gameObject.SetActive (true);
	}

	public void hide() {
		instance.gameObject.SetActive (false);
	}
}
