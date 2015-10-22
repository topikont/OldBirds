using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// User calendar event.
/// 00:01:00 00:02:00 2015-02-25 testloc marjatta trafficking 10
/// </summary>
public class BirdEvent {

	private int eventID;
	private DateTime startTime;
	private DateTime endTime;
	private DateTime date;
	private String location;
	private String person;
	private String activity;
	private int reminder;
	
	public int EventID {
		get {
			return this.eventID;
		}
		set {
			eventID = value;
		}
	}
	
	public DateTime StartTime {
		get {
			return this.startTime;
		}
		set {
			startTime = value;
		}
	}

	public DateTime EndTime {
		get {
			return this.endTime;
		}
		set {
			endTime = value;
		}
	}

	public DateTime Date {
		get {
			return this.date;
		}
		set {
			date = value;
		}
	}

	public string Location {
		get {
			return this.location;
		}
		set {
			location = value;
		}
	}

	public string Person {
		get {
			return this.person;
		}
		set {
			person = value;
		}
	}

	public string Activity {
		get {
			return this.activity;
		}
		set {
			activity = value;
		}
	}

	public int Reminder {
		get {
			return this.reminder;
		}
		set {
			reminder = value;
		}
	}
	
	public BirdEvent ()
	{
		
	}
}
