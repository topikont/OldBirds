using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BusDoorTrigger : MonoBehaviour {
	public List<GameObject> l_OnBoard;
	public bool changed = false;
	// Use this for initialization
	void Start () {
		l_OnBoard = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other) {
		// Sets bird inactive (hides it) and adds it to the
		// on board list
		if(other.transform.tag == "Bird") {
			other.gameObject.SetActive(false);
			l_OnBoard.Add(other.gameObject);
			changed = true;
		}
	}
}
