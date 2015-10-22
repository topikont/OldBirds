using UnityEngine;
using System.Collections;

/// <summary>
/// Selection tool for selecting object for i.e. multicall purposes
/// 
/// How to use: Add this to your main controller gameobject, add the main controller gameobject to the GUI objects sel tool.
/// This will only select gameobjects with the tag "bird".
/// </summary>
public class SelectionTool : MonoBehaviour {
	/// <summary>
	/// Shader taken from unity wiki
	/// Author: User:AnomalousUnderdog.
	/// Content is available under Creative Commons Attribution Share Alike.
	/// </summary>
	public Shader silhouette;
	private ArrayList selectedBirds;
	private GameObject[] selectedBirdObjects;
	void Start () {
		selectedBirds = new ArrayList();
	}

	void Update () {
		if( Input.GetMouseButtonDown(0) )
		{
			Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
			RaycastHit hit;
			if( Physics.Raycast( ray, out hit, 2000 ) )
			{
				// Accept only transforms tagged with "Bird"
				if(hit.transform.tag == "Bird") {
					// Check if gameobject found in list already
					bool found = false;
					foreach(GameObject selection in selectedBirds) {
						if(selection == hit.transform.gameObject) {
							found = true;
						}
					}
					
					// Close info window
					updateBirdList();
					foreach(GameObject bird in selectedBirdObjects) {
						bird.GetComponent<InformationTextScript>().setShow(false);
					}
					
					if(!found) {
						// Add to list and change shader for the silhouette
						selectedBirds.Add(hit.transform.gameObject);
						foreach(Transform child in hit.transform) {
							if(child.name == "Body") {
								// Add silhouette only to the body object
								child.GetComponent<Renderer>().material.shader = silhouette;
								child.GetComponent<Renderer>().material.SetColor ("_OutlineColor", Color.green);
							}
						}
					} else {
						// Remove from list and remove silhouette
						selectedBirds.Remove(hit.transform.gameObject);
						foreach(Transform child in hit.transform) {
							if(child.name == "Body") {
								child.GetComponent<Renderer>().material.shader = Shader.Find ("Diffuse");
							
							}
						}
					}
					
					found = false;
				}


			}
		}
	}

	public ArrayList getSelectionList() {
		return this.selectedBirds;
	}

	public void addToSelectionList(GameObject bird) {
		selectedBirds.Add(bird);
		foreach(Transform child in bird.transform) {
			if(child.name == "Body") {
				child.GetComponent<Renderer>().material.shader = silhouette;
				child.GetComponent<Renderer>().material.SetColor ("_OutlineColor", Color.green);
			}
		}
	}
	
	public void updateBirdList() {
		selectedBirdObjects = new GameObject[selectedBirds.Count];
		for(int i = 0; i < selectedBirds.Count; i++) {
			selectedBirdObjects[i] = (GameObject)selectedBirds.ToArray()[i];
		}
	}
	
	public void showInfoWindow() {
		updateBirdList();
		
		foreach(GameObject bird in selectedBirdObjects) {
			bird.GetComponent<InformationTextScript>().show();
		}
	}


}
