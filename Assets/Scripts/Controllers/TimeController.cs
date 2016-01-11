using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Time controller.
/// 
///	Controls the time simulations of the game and calendar handling
/// </summary>
public class TimeController : MonoBehaviour {
	// speed that time changes
	private float timeSpeed;

	// time
	private int day, month, year, minute, hour;
	
	private int weekOffSet = 0;
	
	// leap years enabled
	private bool leapyear;
	
	// multithreaded alarms enabled
	private bool multithreadingEnabled;
	private bool midNightUpdate = false;
	private MainController mainController;
	private GameObject[] RMBirds;
	private GameObject[] DMBirds;
	private List<BirdEvent> events;
	
	public Text GUICalendarMonday;
	public Text GUICalendarTuesday;
	public Text GUICalendarWednesday;
	public Text GUICalendarThursday;
	public Text GUICalendarFriday;
	public Text GUICalendarSaturday;
	public Text GUICalendarSunday;
	
	public ArrayList GUICalDays = new ArrayList();
	
	public Text dateText;
	public Text timeText;
	public Text statusText;
	public int startingDay = DateTime.Now.Day;
	public int startingHour = 16;
	
	public RectTransform eventPanel;
	public RectTransform eventControlPanel;
	
	private List<RectTransform> eventPanelArray;

	// If this is other than 0, event with this id will be deleted on next save button press
	private int currentEventId = 0;
	
	private Color originalColor;
	
	// Use this for initialization
	void Start () {
		GUICalDays.Add(GUICalendarMonday);
		GUICalDays.Add(GUICalendarTuesday);
		GUICalDays.Add(GUICalendarWednesday);
		GUICalDays.Add(GUICalendarThursday);
		GUICalDays.Add(GUICalendarFriday);
		GUICalDays.Add(GUICalendarSaturday);
		GUICalDays.Add(GUICalendarSunday);
	
		eventPanelArray = new List<RectTransform>();
		if (this.GetComponent<MainController> ()) {
			mainController = this.GetComponent<MainController>();
		}

		timeSpeed = 1f;	
		
		// start the simulated time 
		cCalendar.setYear(DateTime.Now.Year);
		cCalendar.setDay(startingDay);
		cCalendar.setMonth(DateTime.Now.Month);
		cCalendar.setHour(startingHour);
		cCalendar.setSpeed(timeSpeed);
		cCalendar.resume ();

		UpdateCalendar();
	}
	
	// Update is called once per frame
	void Update () {
		// day will sometimes go over the current month, so update it to the current day
		// Might need optimizing (not to call every frame)

		if(day > cCalendar.daysThisMonth)day=cCalendar.day;
		hour = cCalendar.hour;
		minute = cCalendar.minute;
		// Let's add that zero if minutes or hours are single digit
		string hours;
		string minutes;

		if(minute < 10)
			minutes = "0" + minute;
		else
			minutes = minute.ToString();
		
		if(hour < 10)
			hours = "0" + hour;
		else
			hours = hour.ToString();
		
		// Add them to the UI text	
		timeText.text = hours + ":" + minutes;
		
		// And suffixes to the day numbers
		string dayText;
		day = cCalendar.day;
		if(day == 1) {
			dayText = day + "st";
		} else if(day == 2) {
			dayText = day + "nd";
		} else {
			dayText = day + "th";
		}
		
		// Set the date to the UI
		dateText.text = dayText + " of " + cCalendar.monthName + ", " + cCalendar.year;
		
		// Update GUI if day is changed (midnight)
		// Wont update if the game is paused exactly at midnight
		if(midNightUpdate == false) {
			if(hours == "00" && minutes == "00" && timeSpeed > 0f) {
				UpdateCalendar();
				midNightUpdate = true;
			}
			
			if(hours == "23" && minutes == "59" && timeSpeed < 0f) {
				UpdateCalendar();
				midNightUpdate = true;
			}
		} else if(!(hours == "00" && minutes == "00") &&
		          !(hours == "23" && minutes == "59") &&
					midNightUpdate == true){
			midNightUpdate = false;
		}
	}

	/// <summary>
	/// Updates the events. After SQLHandler has fetched data, it calls the UpdateEventList method.
	/// </summary>
	/// <param name="hidden">Hidden.</param>
	public void UpdateEvents(ShowHide hidden) {
		if(!hidden.targetObjHidden) {
			this.GetComponent<SQLHandler>().updateAllEvents();
		}
	}
	
	/// <summary>
	/// Updates the event list. SQLHandler calls this when the data is returned.
	/// </summary>
	/// <param name="events">Events.</param>
	public void UpdateEventList(List<BirdEvent> _events) {
		this.events = new List<BirdEvent>(_events);
		UpdateCalendar();
	}
	
	int sundayTrick = 0;
	
	/// <summary>
	/// Updates the calendar days when scrolling from week to week. Also initiates the day texts.
	/// </summary>
	public void UpdateCalendar ()
	{
		// Clear eventPanels if any
		foreach(RectTransform rt in eventPanelArray) {
			Destroy (rt.gameObject);
		}
		eventPanelArray.Clear();
	
		// Get current day of the week		
		String mString;
		String dayString;
		// Need to adjust the leading zero again
		if(cCalendar.month < 10) {
			mString = "0" + cCalendar.month;
		} else {
			mString = cCalendar.month.ToString();
		}
		
		if(cCalendar.day < 10) {
			dayString = "0" + cCalendar.day;
		} else {
			dayString = cCalendar.day.ToString();
		}
		
		// Get the current weekday from the simulated time
		String dString = dayString + "." + mString + "." + cCalendar.year;
		DateTime date = DateTime.ParseExact(dString, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
		int currentDayOfWeek = (int)date.DayOfWeek;
		int index = 0;
		foreach(Text t in GUICalDays) {
			// Get the date for current iteration
			DateTime dt1 = date;
			
			if(date.DayOfWeek == DayOfWeek.Sunday) {
				// A trick to avoid a bug in calendars where sunday is the first day of week
				sundayTrick = 7;
			}
			DateTime dt2 = dt1.AddDays(-currentDayOfWeek - sundayTrick + index + 1 + (weekOffSet * 7));
			t.text = t.text.Substring(0,3) + " " + dt2.Day + "/" + dt2.Month;
			
			// Add events for day
			if(events != null) {
				foreach(BirdEvent bEvent in events) {
					// If there's an event for current day
					if(bEvent.Date.Date.CompareTo(dt2.Date) == 0) {
						// Now draw a panel from the top of the ending time
						// to the top of the starting time
						CreateEventPanel(bEvent, t);
					}
				}
			} else {
				//Debug.LogError("Event list not updated");
			}
			
			Color highlightColor = new Color(0.22f, 0.8f, 0.1f, 1.0f);
			Image bgImage = t.transform.parent.GetComponent<Image>();
			if(t.text.Substring(0,3) == date.DayOfWeek.ToString().Substring(0,3)) {
				// Highlight today in calendar
				if(weekOffSet == 0) {
					// If it's the current week
					if(bgImage.color != highlightColor)
						originalColor = bgImage.color;
					bgImage.color = new Color(0.22f, 0.8f, 0.1f, 1.0f); 
				}
				else {
					bgImage.color = originalColor;
				}
			} else {
				bgImage.color = originalColor;
			}
			index++;
		}
	}

	/// <summary>
	/// Creates the event panel.
	/// </summary>
	/// <param name="bEvent">Bird event.</param>
	/// <param name="t">Text object</param> 
	void CreateEventPanel (BirdEvent bEvent, Text t)
	{
		// The panel that contains hour list
		// Pronbably a bad way to do stuff
		Transform contentPanel = t.transform.parent.parent.FindChild("Scroll View").FindChild("Content Panel");

		foreach(Transform tr in contentPanel) {
			
			String hour = tr.name.Replace("HourPanel", "");
			if(hour.Length < 2) {
				hour = "0" + hour;
			}
			DateTime dtHour = DateTime.ParseExact(hour, "HH", System.Globalization.CultureInfo.InvariantCulture);
			                        
			                        

			// When we're at the end hour
			if(bEvent.EndTime.Hour.CompareTo(dtHour.Hour) == 0) {
				// The panel we want to create
				RectTransform ePanel = (RectTransform)Instantiate(eventPanel);
				ePanel.name = "EventPanel";
			
				// We want to go from the start of end hour to the start of start hour
				// to avoid z-index issues
				ePanel.transform.SetParent(tr.transform, false);
				
				// This should be changed then going from hours to minutes accuracy
				int hoursBetween = bEvent.EndTime.Hour - bEvent.StartTime.Hour;
				
				ePanel.offsetMax = new Vector2(ePanel.offsetMax.x, ePanel.offsetMax.y + (28 * hoursBetween));
				Text timeText = ePanel.transform.Find("TimeText").GetComponent<Text>();
				Text activityText = ePanel.transform.Find("ActivityText").GetComponent<Text>();
				
				String startHour = bEvent.StartTime.ToString("HH");
				String startMinute = bEvent.StartTime.ToString("mm");
				
				String endHour = bEvent.EndTime.ToString("HH");
				String endMinute = bEvent.EndTime.ToString("mm");
				
				String startTimeStr = startHour + ":" + startMinute;
				String endTimeStr = endHour + ":" + endMinute;
				
				timeText.text = startTimeStr + " - " + endTimeStr;
				activityText.text = bEvent.Activity;
				ePanel.GetComponent<EventAttributes>().data = bEvent;
				eventPanelArray.Add(ePanel);
			}

		}
		
	}

	public void PauseTime() {
		// Pauses the time
		int speed = 0;
		SetGameSpeed(speed);
	}
	
	/// <summary>
	/// Resumes the time. I.e. makes the time go at normal speed.
	/// </summary>
	public void ResumeTime() {
		timeSpeed = 1;
		int speed = 1;
		SetGameSpeed(speed);
	}
	
	/// <summary>
	/// Forwards the time.
	/// </summary>
	public void ForwardTime() {
		if(timeSpeed < 0) {
			timeSpeed = 1;
		}
		int speed = 2;
		SetGameSpeed(speed);
	}
	
	/// <summary>
	/// Reverses the time at normal speed.
	/// </summary>
	public void ReverseTime() {
		timeSpeed = 1;
		int speed = -1;
		SetGameSpeed(speed);
	}
	
	/// <summary>
	/// Doubles the reverse time speed.
	/// </summary>
	public void DoubleReverseTime() {
		if(timeSpeed > 0) {
			timeSpeed = -1;
		}
		int speed = 2;
		SetGameSpeed(speed);
	}
	
	/// <summary>
	/// Sets the game speed.
	/// </summary>
	/// <param name="speed">Speed.</param>
	private void SetGameSpeed(int speed) {
		if(timeSpeed < Math.Abs(1024)) {
			timeSpeed = timeSpeed * speed;

			cCalendar.setSpeed(timeSpeed);
			statusText.text = timeSpeed + "x";
		}
	}
	
	/// <summary>
	/// GUI calendar button functionalities
	/// </summary>
	public void GUINextWeek() {
		weekOffSet = weekOffSet + 1;
		UpdateCalendar();
	}
	
	public void GUIPreviousWeek() {
		weekOffSet = weekOffSet - 1;
		UpdateCalendar();
	}
	
	public void GUIThisWeek() {
		if(weekOffSet != 0) {
			weekOffSet = 0;
			UpdateCalendar();
		}
		
	}
	
	public void ChangeEventClick (BirdEvent eventData) {
		// Open panel according to the click
		
		InputField dateTxt = eventControlPanel.transform.Find("DateField").GetComponent<InputField>();
		InputField startTimeTxt = eventControlPanel.transform.Find("StartTimeField").GetComponent<InputField>();
		InputField endTimeTxt = eventControlPanel.transform.Find("EndTimeField").GetComponent<InputField>();
		InputField activityTxt = eventControlPanel.transform.Find("ActivityField").GetComponent<InputField>();
		InputField personTxt = eventControlPanel.transform.Find("PersonField").GetComponent<InputField>();
		InputField reminderTxt = eventControlPanel.transform.Find("AlarmField").GetComponent<InputField>();
		InputField locationTxt = eventControlPanel.transform.Find("LocationField").GetComponent<InputField>();
		
		dateTxt.text = eventData.Date.ToString("dd.MM.yyyy");
		startTimeTxt.text = eventData.StartTime.ToString("HH:mm");
		endTimeTxt.text = eventData.EndTime.ToString("HH:mm");
		activityTxt.text = eventData.Activity.ToString();
		personTxt.text = eventData.Person.ToString();
		reminderTxt.text = eventData.Reminder.ToString();
		locationTxt.text = eventData.Location.ToString();
		
		// Save id of current modified event
		currentEventId = eventData.EventID;	
		
		// Open the EventControlPanel
		eventControlPanel.GetComponent<ShowHide>().hideOrShowSelf();
		
		eventControlPanel.transform.Find("ChangeButton").gameObject.SetActive(true);
		eventControlPanel.transform.Find("RemoveButton").gameObject.SetActive(true);
		eventControlPanel.transform.Find("SaveButton").gameObject.SetActive(false);
	}
	
	public void AddNewEventClick (string clickedTime, string clickedWeekday)
	{
		// Change texts to the event control panel according to the click
		InputField dateTxt = eventControlPanel.transform.Find("DateField").GetComponent<InputField>();
		InputField startTimeTxt = eventControlPanel.transform.Find("StartTimeField").GetComponent<InputField>();
		InputField endTimeTxt = eventControlPanel.transform.Find("EndTimeField").GetComponent<InputField>();
		InputField activityTxt = eventControlPanel.transform.Find("ActivityField").GetComponent<InputField>();
		InputField personTxt = eventControlPanel.transform.Find("PersonField").GetComponent<InputField>();
		InputField reminderTxt = eventControlPanel.transform.Find("AlarmField").GetComponent<InputField>();
		InputField locationTxt = eventControlPanel.transform.Find("LocationField").GetComponent<InputField>();
		
		
		Debug.Log(eventControlPanel.transform.Find("ChangeButton").gameObject.activeSelf);
		
		eventControlPanel.transform.Find("ChangeButton").gameObject.SetActive(false);
		eventControlPanel.transform.Find("RemoveButton").gameObject.SetActive(false);
		eventControlPanel.transform.Find("SaveButton").gameObject.SetActive(true);
		
		Debug.Log(eventControlPanel.transform.Find("ChangeButton").gameObject.activeSelf);
		
		activityTxt.text = "New_activity";
		reminderTxt.text = "Reminder_in_minutes";
		locationTxt.text = "New_location";
		startTimeTxt.text = clickedTime + ":00";
		personTxt.text = "New_person";
		
		int endTime = int.Parse(clickedTime) + 1;
		string endTimeStr;
		if(endTime < 10) {
			// Leading zero check
			endTimeStr = "0" + endTime;
		} else {
			endTimeStr = "" + endTime;
		}
		
		endTimeTxt.text = endTimeStr + ":00";
		
		String stripWeekDay = clickedWeekday.Substring(4, clickedWeekday.Length - 4);
		String[] date = stripWeekDay.Split('/');
		
		if(date[1].Length < 2) {
			date[1] = "0" + date[1];
		}
		dateTxt.text = date[0] + "." + date[1] + "." + cCalendar.year;
		
		// Open the EventControlPanel
		eventControlPanel.GetComponent<ShowHide>().hideOrShowSelf();
		
		eventControlPanel.transform.Find("ChangeButton").gameObject.SetActive(false);
		eventControlPanel.transform.Find("RemoveButton").gameObject.SetActive(false);
		eventControlPanel.transform.Find("SaveButton").gameObject.SetActive(true);
	}
	
	public void AddNewEvent() {
		// Get all the fields from UI
		String dateTxt = eventControlPanel.transform.Find("DateField").GetComponent<InputField>().text;
		String startTimeTxt = eventControlPanel.transform.Find("StartTimeField").GetComponent<InputField>().text;
		String endTimeTxt = eventControlPanel.transform.Find("EndTimeField").GetComponent<InputField>().text;
		String activityTxt = eventControlPanel.transform.Find("ActivityField").GetComponent<InputField>().text;
		String personTxt = eventControlPanel.transform.Find("PersonField").GetComponent<InputField>().text;
		String alarmTxt = eventControlPanel.transform.Find("AlarmField").GetComponent<InputField>().text;
		String locationTxt = eventControlPanel.transform.Find("LocationField").GetComponent<InputField>().text;
		
		
		// Add a new bird event according to them
		BirdEvent newEvent = new BirdEvent();
		
		String[] fixText = dateTxt.Split('.');
		if(fixText[0].Length < 2) fixText[0] = "0" + fixText[0];
		if(fixText[1].Length < 2) fixText[1] = "0" + fixText[1];
		dateTxt = fixText[0] + "." + fixText[1] + "." + fixText[2];
		
		Debug.Log(dateTxt + " " + startTimeTxt + " " + endTimeTxt);
		newEvent.Date = DateTime.ParseExact(dateTxt, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
		newEvent.StartTime = DateTime.ParseExact(startTimeTxt, "HH:mm", System.Globalization.CultureInfo.InvariantCulture);
		newEvent.EndTime = DateTime.ParseExact(endTimeTxt, "HH:mm", System.Globalization.CultureInfo.InvariantCulture);
		newEvent.Activity = activityTxt;
		newEvent.Person = personTxt;
		try{
			newEvent.Reminder = int.Parse(alarmTxt);
		} catch (Exception e) {
			newEvent.Reminder = 0;
		}
		newEvent.Location = locationTxt;
		
		this.GetComponent<SQLHandler>().addEvent(newEvent);
		UpdateCalendar();
	}
	
	public void ChangeEvent() {
		BirdEvent eventData = new BirdEvent();
		
		// Get all the fields from UI
		String dateTxt = eventControlPanel.transform.Find("DateField").GetComponent<InputField>().text;
		String startTimeTxt = eventControlPanel.transform.Find("StartTimeField").GetComponent<InputField>().text;
		String endTimeTxt = eventControlPanel.transform.Find("EndTimeField").GetComponent<InputField>().text;
		String activityTxt = eventControlPanel.transform.Find("ActivityField").GetComponent<InputField>().text;
		String personTxt = eventControlPanel.transform.Find("PersonField").GetComponent<InputField>().text;
		String alarmTxt = eventControlPanel.transform.Find("AlarmField").GetComponent<InputField>().text;
		String locationTxt = eventControlPanel.transform.Find("LocationField").GetComponent<InputField>().text;
		
		String[] fixText = dateTxt.Split('.');
		if(fixText[0].Length < 2) fixText[0] = "0" + fixText[0];
		if(fixText[1].Length < 2) fixText[1] = "0" + fixText[1];
		dateTxt = fixText[0] + "." + fixText[1] + "." + fixText[2];
		
		Debug.Log(dateTxt + " " + startTimeTxt + " " + endTimeTxt);
		
		eventData.EventID = currentEventId;
		eventData.Date = DateTime.ParseExact(dateTxt, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
		eventData.StartTime = DateTime.ParseExact(startTimeTxt, "HH:mm", System.Globalization.CultureInfo.InvariantCulture);
		eventData.EndTime = DateTime.ParseExact(endTimeTxt, "HH:mm", System.Globalization.CultureInfo.InvariantCulture);
		eventData.Activity = activityTxt;
		eventData.Person = personTxt;
		eventData.Reminder = int.Parse(alarmTxt);
		eventData.Location = locationTxt;
		
		this.GetComponent<SQLHandler>().changeEvent(eventData);
		UpdateCalendar();
		
		currentEventId = 0;
	}
	
	public void CancelButtonClick() {
		// Remove deletion mark
		currentEventId = 0;
	}
	
	public void DeleteButtonClick() {
		if(currentEventId != 0) {
			this.GetComponent<SQLHandler>().deleteEvent(currentEventId.ToString());
		} else {
			Debug.Log("Event had no ID");
		}
		
		currentEventId = 0;
		
	}
	
	public DateTime getCurrentDateTime() {
		return new DateTime(cCalendar.year, cCalendar.month, cCalendar.day, cCalendar.hour, cCalendar.minute, (int)cCalendar.seconds);
	}

	public float getTimeSpeed() {
		return timeSpeed;
	}
}
