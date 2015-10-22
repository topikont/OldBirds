using UnityEngine;
using System.Collections;

/// <summary>
/// Simple script to show/hide gameobjects
/// </summary>
public class ShowHide : MonoBehaviour {
	public bool targetObjHidden = true;
	public bool recursive = true;
	
	public void hideOrShow(GameObject obj) {
		if(!targetObjHidden) {
			obj.SetActive(false);
			if(recursive) {
				foreach(Transform t in transform) {
					t.gameObject.SetActive(false);
				}
			}
			targetObjHidden = true;
		} else {
			obj.SetActive(true);
			if(recursive) {
				foreach(Transform t in transform) {
					t.gameObject.SetActive(true);
				}
			}
			targetObjHidden = false;
		}
	}
	
	public void hideOrShowSelf() {
		if(!targetObjHidden) {
			this.gameObject.SetActive(false);
			if(recursive) {
				foreach(Transform t in transform) {
					t.gameObject.SetActive(false);
				}
			}
			targetObjHidden = true;
		} else {
			this.gameObject.SetActive(true);
			if(recursive) {
				foreach(Transform t in transform) {
					t.gameObject.SetActive(true);
				}
			}
			targetObjHidden = false;
		}
	}
}
