using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BusMouseOver : MonoBehaviour {
	public bool showInfoBox = false	;
	public RectTransform busPanel;
	public BusDoorTrigger doorTrigger;
	public GameObject textListObject;
	public float top = 100f;
	
	void Start() {
		busPanel.gameObject.SetActive(false);
	}
	
	void Update() {
		if(doorTrigger.changed) {
			resetOnBoardUI();
			doorTrigger.changed = false;
		}
	}
	
	void OnMouseOver() {
		busPanel.gameObject.SetActive(true);
		createItems();
	}
	
	void OnMouseExit() {
		destroyItems();
		busPanel.gameObject.SetActive(false);
	}
	
	public void resetOnBoardUI() {
		destroyItems();
		createItems();
	}
	
	void destroyItems() {
		foreach(Transform t in busPanel) {
			Destroy(t.gameObject);
		}
	}
	
	void createItems() {
		Vector3 guiPosition = Camera.main.WorldToScreenPoint(this.transform.position);
		guiPosition.Set (guiPosition.x, guiPosition.y + top, guiPosition.z);
		busPanel.position = guiPosition;
		if(doorTrigger.l_OnBoard != null && busPanel.childCount < doorTrigger.l_OnBoard.Count) {
			foreach(GameObject obj in doorTrigger.l_OnBoard) {
				GameObject text = Instantiate(textListObject);
				text.transform.SetParent(busPanel.transform, false);
				text.GetComponent<Text>().text = obj.name;
			}
		}
	}
}
