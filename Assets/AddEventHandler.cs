using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Handles clicks to the week area (hourly panels)
/// </summary>
public class AddEventHandler : MonoBehaviour, IPointerClickHandler {
	public TimeController tc;
	#region IPointerClickHandler implementation

	public void OnPointerClick (PointerEventData eventData)
	{
		string panelTime = transform.name.Replace("HourPanel", "");
		string clickedTime;
		if(panelTime.Length < 2) {
			clickedTime = "0" + panelTime;
		} else {
			clickedTime = panelTime;
		}
		
		// Get text from weekday 
		Transform dayTextPanel = transform.parent.parent.parent.Find("DayTextPanel");
		Text dayText = dayTextPanel.Find ("DayText").GetComponent<Text>();
		
		tc.AddNewEventClick(clickedTime, dayText.text);

	}

	#endregion

}
