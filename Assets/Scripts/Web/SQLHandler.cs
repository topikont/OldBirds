using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using UnityEngine.UI;

/**
// This is what the PHP eats:
// Gets all data
ob_hash:zbh564s4antyxd ob_function:GetAll 

// Gets all the events for one person
ob_hash:zbh564s4antyxd ob_function:GetEventForBird ob_param1:personname 

// From left to right: start_time, end_time, date, location, person, activity, reminder. Separate with spaces.
ob_hash:zbh564s4antyxd ob_function:PutEvent ob_param1:"00:01:00 00:02:00 2015-02-25 testloc marjatta trafficking 10" 

// Changes a field with a value, according to the id in database
ob_hash:zbh564s4antyxd ob_function:ChangeEvent ob_param1:personID ob_param2:field ob_param3:value

*/

public class SQLHandler : MonoBehaviour {
	private String formText = ""; //this field is where the messages sent by PHP script will be in
	private String formFunction = "getAll";
	
	String URL = "http://obdb.esy.es/db.php"; //
	String hash = "zbh564s4antyxd"; // This has to be the same in here and in the php file
	bool fetching = false;
	enum FunctionTypes { NONE, GET_ALL, PUT_EVENT, GHANGE_EVENT, GET_BIRD_EVENT, DELETE_EVENT, PUT_PEND_NOTIF, GET_PEND_NOTIF };
	int currentFunctionType = (int)FunctionTypes.NONE;
	public Transform loadingPanel;
	WWWForm form;
	
	List<BirdEvent> events;
	
	void Start() {
		events = new List<BirdEvent>();
		form = new WWWForm();
		form.AddField("ob_hash", hash);
	}
	
	public void updateAllEvents() {
		currentFunctionType = (int)FunctionTypes.GET_ALL;
		form.AddField("ob_function", "GetAll");
		StartCoroutine("AccessDatabase");
	}
	
	/// <summary>
	/// Adds the event.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void addEvent(BirdEvent eventData) {
		currentFunctionType = (int)FunctionTypes.PUT_EVENT;
		
		String putString =  eventData.StartTime.Hour + ":" + eventData.StartTime.Minute + ":00" + " " +
							eventData.EndTime.Hour + ":" + eventData.EndTime.Minute + ":00" + " " +
							eventData.Date.Date.ToString("yyyy-MM-dd") + " " + 
							eventData.Location.Replace(" ", "_") + " " + 
							eventData.Person.Replace(" ", "_") + " " + 
							eventData.Activity.Replace(" ", "_") + " " + 
							eventData.Reminder;
		
		form.AddField( "ob_function", "PutEvent" );
		form.AddField( "ob_param1", putString );
		StartCoroutine("AccessDatabase");
	}

	public void addPendingNotification(String sound) { 
		currentFunctionType = (int)FunctionTypes.PUT_PEND_NOTIF;
		form.AddField( "ob_function", "AddNotification" );
		form.AddField ("ob_param1", sound);
		Debug.Log ("Adding a new notification to the DB with sound name " + sound);
		StartCoroutine("AccessDatabase");
	}
	
	public void changeEvent(BirdEvent eventData) {
		currentFunctionType = (int)FunctionTypes.GHANGE_EVENT;
		form.AddField( "ob_function", "ChangeEventAll" );
		
		String putString =  eventData.StartTime.Hour + ":" + eventData.StartTime.Minute + ":00" + " " +
							eventData.EndTime.Hour + ":" + eventData.EndTime.Minute + ":00" + " " +
							eventData.Date.Date.ToString("yyyy-MM-dd") + " " + 
							eventData.Location.Replace(" ", "_") + " " + 
							eventData.Person.Replace(" ", "_") + " " + 
							eventData.Activity.Replace(" ", "_") + " " + 
							eventData.Reminder + " " +
							eventData.EventID;
		form.AddField( "ob_param1", putString);
		StartCoroutine("AccessDatabase");
	}
	
	public void updateEventForBird(String birdName) {
		currentFunctionType = (int)FunctionTypes.GET_BIRD_EVENT;
		form.AddField( "ob_function", "GetEventForBird" );
		form.AddField( "ob_param1", birdName );
		StartCoroutine("AccessDatabase");
	}
	
	public void deleteEvent(String event_id) {
		currentFunctionType = (int)FunctionTypes.DELETE_EVENT;
		form.AddField( "ob_function", "RemoveEvent" );
		form.AddField( "ob_param1", event_id );
		StartCoroutine("AccessDatabase");
	}
	
	IEnumerator AccessDatabase() {
		loadingPanel.GetComponent<ShowHide>().hideOrShow(loadingPanel.gameObject);
		Text loadingText = loadingPanel.Find ("Text").GetComponent<Text>();
		loadingText.text = "Working...";
		Debug.Log ("SQLHandler is working...");
		WWW w = new WWW(URL, form); //here we create a var called 'w' and we sync with our URL and the form
		yield return w; 			//we wait for the form to check the PHP file, so our game dont just hang
		if (w.error != null) {
			Debug.Log(w.error); 	//if there is an error, tell us
		} else {
			Debug.Log(w.data); 
			formText = w.data; 		//here we return the data our PHP told us
			w.Dispose(); 			//clear our form in game
		}
		Debug.Log ("SQLHandler completed");
		// Handle data
		String[] splittedData = formText.Split(' ');
		if(currentFunctionType == (int)FunctionTypes.GET_ALL) {
			int rows = splittedData.Length / 8;
			int rowIndexer = 0; 
			for(int i = 0; i < rows; i++) {
				// There's total of 8 fields per row
				// id, starttime, endtime, date, location, person, activity, reminder
				BirdEvent bEvent = new BirdEvent();
				bEvent.EventID = int.Parse(splittedData[0 + rowIndexer]);
				bEvent.StartTime = DateTime.ParseExact(	splittedData[1 + rowIndexer], 
														"HH:mm:ss", 
														System.Globalization.CultureInfo.InvariantCulture);
				bEvent.EndTime = DateTime.ParseExact( splittedData[2 + rowIndexer], 
				                                      "HH:mm:ss", 
				                                      System.Globalization.CultureInfo.InvariantCulture);
				bEvent.Date = DateTime.ParseExact(	splittedData[3 + rowIndexer], 
				                                   "yyyy-MM-dd", 
				                                   System.Globalization.CultureInfo.InvariantCulture);
				bEvent.Location = splittedData[4 + rowIndexer].Replace("_", " ");
				bEvent.Person = splittedData[5 + rowIndexer].Replace("_", " ");
				bEvent.Activity = splittedData[6 + rowIndexer].Replace("_", " ");
				bEvent.Reminder = int.Parse(splittedData[7 + rowIndexer]);
				events.Add(bEvent);
				rowIndexer += 8;
			}
			this.GetComponent<TimeController>().UpdateEventList(events);
			events.Clear();
		}
		loadingPanel.GetComponent<ShowHide>().hideOrShow(loadingPanel.gameObject);
		
		if(	currentFunctionType == (int)FunctionTypes.PUT_EVENT ||
			currentFunctionType == (int)FunctionTypes.DELETE_EVENT || 
		   	currentFunctionType == (int)FunctionTypes.GHANGE_EVENT) {
			this.updateAllEvents();
			this.GetComponent<TimeController>().eventControlPanel.GetComponent<ShowHide>().hideOrShowSelf();
		}
	}
}













