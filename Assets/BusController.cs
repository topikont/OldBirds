using UnityEngine;
using System.Collections;
using System;

public class BusController : MonoBehaviour {
	public int leaveAtHour = 0;
	public int leaveAtMinute = 0;
	public Animator leavingAnim;

	bool bLeaving {
		get;
		set;
	}

	public TimeController timeController;

	void Start () {
		bLeaving = false;
	}
	
	void Update () {
		DateTime currentTime = timeController.getCurrentDateTime();
		if(leaveAtHour <= currentTime.Hour 
			&& leaveAtMinute <= currentTime.Minute) {
			if(!bLeaving) {
				leavingAnim.Play("Bus_leaving");
				bLeaving = true;
			}
		}
		
		if(leavingAnim.GetCurrentAnimatorStateInfo(0).IsName("animation_finished")) {
			// Hide bus when it's out of the area
			this.transform.parent.gameObject.SetActive(false);
			Application.LoadLevel("Bus scenario p2");
		}
	}
}
