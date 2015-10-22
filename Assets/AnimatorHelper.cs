using UnityEngine;
using System.Collections;

public class AnimatorHelper : MonoBehaviour {
	Animator anim;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnEnable() {
		anim.Play("loading_button");
	}
}
