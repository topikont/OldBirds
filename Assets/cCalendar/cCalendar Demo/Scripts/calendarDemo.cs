using UnityEngine;
using System;
using System.Collections;

public class calendarDemo : MonoBehaviour {
	
	private Rect todaysDate, todaysTime, settingsMenu, menuBackground;
	
	// time editor locations
	
	// speed that time changes
	private float speed;
	
	// time
	private int day, month, year, minute, hour;
	
	// leap years enabled
	private bool leapyear;
	
	// multithreaded alarms enabled
	private bool multithreadingEnabled;
	
	
	// Use this for initialization
	void Start () {
		// location of the current calendar time display
		todaysDate = new Rect(10, 0, 200, 20);
		todaysTime = new Rect(10, 20, 200, 20);
		
		// location of the time settings/change display
		settingsMenu = new Rect(250, 5, 300, 215);
		menuBackground = new Rect(0,0,300,215);
		
		speed = 15.0f; // default 15 minutes of real-time to an hour of calendar time
		
		// default settings of the calendar
		day = cCalendar.day;
		month = cCalendar.month;
		year = cCalendar.year;
		minute = cCalendar.minute;
		hour = cCalendar.hour;
		leapyear = cCalendar.leapyearsEnabled;
		multithreadingEnabled = cCalendar.useMultiThreading;
		cCalendar.setSpeed(speed);
		
		
		// start the calendar
		cCalendar.resume ();
	}
	
	// Update is called once per frame
	void Update () {
		// day will sometimes go over the current month, so update it to the current day
		if(day > cCalendar.daysThisMonth)day=cCalendar.day;
	}
	
	void OnGUI(){
	
		// display the time
		
		
		GUI.Label(todaysDate, "Today's Date: " + cCalendar.day + " " + cCalendar.monthAbb + ", " + cCalendar.year);
		GUI.Label(todaysTime, "The Time: " +
								((cCalendar.hour<10)?"0"+cCalendar.hour:cCalendar.hour.ToString())+
								":" +
								((cCalendar.minute<10)?"0"+cCalendar.minute:cCalendar.minute.ToString())+
								":" +
								((cCalendar.seconds < 10)?"0"+cCalendar.seconds.ToString().Substring(0,1):cCalendar.seconds.ToString().Substring(0,2)));
		
		
		// display the settings to change the time
		
		GUI.BeginGroup (settingsMenu);
			GUI.Box (menuBackground, "");
			GUILayout.BeginVertical();
				GUILayout.BeginHorizontal();
					GUILayout.Label("Minutes Per Hour: ");
					speed = GUILayout.HorizontalSlider (speed, -60.0f, 60.0f, GUILayout.MinWidth(100));
					GUILayout.Label("Selected: " + ((speed<10f&&speed>=0f)?"0"+speed.ToString().Substring(0,1):
																		(speed<0f&&speed>-10f)?speed.ToString().Substring(0,2):
																								(speed<-10f)?speed.ToString().Substring(0,3):
																												speed.ToString().Substring(0,2)),
									GUILayout.MinWidth (90));
									
				GUILayout.EndHorizontal();
				try{
				
				
					GUILayout.BeginHorizontal();
						GUILayout.Label ("   Day: ", GUILayout.MinWidth(70));
						day = (int)GUILayout.HorizontalSlider ((float)day, 1.0f, (float)cCalendar.daysThisMonth, GUILayout.MinWidth(100));
						GUILayout.Label("Selected: " + ((day<10)?"0"+day:""+day));
					GUILayout.EndHorizontal();
					
					
					GUILayout.BeginHorizontal();
						GUILayout.Label("Month: ", GUILayout.MinWidth(70));
						month = (int)GUILayout.HorizontalSlider((float)month, 1.0f, (float)cCalendar.monthCount, GUILayout.MinWidth(100));
						GUILayout.Label("Selected: " + ((month<10)?"0"+month:""+month));
					GUILayout.EndHorizontal();
					
					
					GUILayout.BeginHorizontal();
						GUILayout.Label("  Year: ", GUILayout.MaxWidth(70));
						year = Int32.Parse(GUILayout.TextField(""+year, GUILayout.MaxWidth(200)));
					GUILayout.EndHorizontal();
					
					
					GUILayout.BeginHorizontal();
						GUILayout.Label("Minute: ", GUILayout.MinWidth(70));
						minute = (int)GUILayout.HorizontalSlider ((float)minute, 0.0f, 59.0f, GUILayout.MinWidth(100));
						GUILayout.Label("Selected: " + ((minute<10)?"0"+minute:""+minute));
					GUILayout.EndHorizontal();
					
					GUILayout.BeginHorizontal();
						GUILayout.Label("Hour: ", GUILayout.MinWidth(70));
						hour = (int)GUILayout.HorizontalSlider ((float)hour, 0.0f, 23.0f, GUILayout.MinWidth(100));
						GUILayout.Label("Selected: " + ((hour<10)?"0"+hour:""+hour));
					GUILayout.EndHorizontal();
				}catch(FormatException Fe){
					Debug.Log(Fe.Message);
				}catch(OverflowException Oe){
					Debug.Log(Oe.Message);
				}
				leapyear = GUILayout.Toggle(leapyear, "Enable Leap Years?");
				multithreadingEnabled = GUILayout.Toggle(multithreadingEnabled, "Enable MultiThreading?");
				if(GUILayout.Button("Change Settings", GUILayout.MaxWidth(300))){
					cCalendar.setSpeed(speed);
					cCalendar.setYear (year);
					cCalendar.setMonth(month);
					cCalendar.setDay(day);
					cCalendar.setHour (hour);
					cCalendar.setMinute (minute);
					cCalendar.setSeconds (0f);
					if(leapyear&&!cCalendar.leapyearsEnabled)cCalendar.toggleLeapYears();
					if(!leapyear&&cCalendar.leapyearsEnabled)cCalendar.toggleLeapYears();
					if(multithreadingEnabled&&!cCalendar.useMultiThreading)cCalendar.enableMultiThreading();
					if(!multithreadingEnabled&&cCalendar.useMultiThreading)cCalendar.disableMultiThreading();
				}
			GUILayout.EndVertical ();
		GUI.EndGroup ();
	}
}
