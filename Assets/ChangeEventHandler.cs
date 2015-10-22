using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ChangeEventHandler : MonoBehaviour, IPointerClickHandler {
	public TimeController tc;
	#region IPointerClickHandler implementation
	
	public void OnPointerClick (PointerEventData eventData)
	{
		this.tc = GameObject.Find("MainController").GetComponent<TimeController>();
		EventAttributes eAtr = this.GetComponent<EventAttributes>();
		tc.ChangeEventClick(eAtr.data);
		
	}
	
	#endregion
	
}
