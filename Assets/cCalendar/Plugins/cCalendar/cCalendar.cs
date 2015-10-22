#region Namespaces
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// end of Namespaces
#endregion

#region Definitions
using CallFunc = System.Func<int, int>;

// end of Definitions
#endregion

/// <summary>
/// The cCalendar class, keeps track of in-game time.  It allows the user to set the speed at which it changes during each update step.
/// By default, the in-game time starts off at January 1, 1600 12:00:00.0 and will not change until the user calls cCalendar.resume()
/// It is possible to stop cCalendar from updating the time (i.e. if you need to open a menu and pause the game) by calling cCalendar.pause()
/// </summary>
public class cCalendar : MonoBehaviour {

	#region static vars
	
	/// <summary>
	/// If isInit is false, then the init function will be run.
	/// </summary>
	private static bool isInit = false;
	
	/// <summary>
	/// If mPaused is true, then the time will not update.  The time is paused by default, so cCalendar.resume() needs to be called.
	/// </summary>
	private static bool mPaused = true;
	
	
	/// <summary>
	/// Gets a value indicating whether <see cref="cCalendar"/> is paused.
	/// </summary>
	/// <value>
	/// <c>true</c> if it is paused; otherwise, <c>false</c>.
	/// </value>
	public static bool isPaused{get{return mPaused;}}
	
	
	/// <summary>
	/// The amount to multiply Time.deltaTime by.  Calculated through: ( 60 / number of real-time minutes )
	/// If 15 is input for the number of real-time minutes, Time.deltaTime will be multiplied by 4, meaning 15 minutes of real time
	/// per one hour of in-game time.
	/// </summary>
	private static float timeSpeed = 1.0f;
	
	
	/// <summary>
	/// The amount of time that has passed in-game since the previous frame. Calculated through: Time.deltaTime * timeSpeed
	/// timeDiff can be looked at as a different Time.deltaTime, but faster, slower, or the same rate of change.
	/// </summary>
	private static float timeDiff = 0.0f;
	
	
	/// <summary>
	/// Gets the change in in-game time between frames.
	/// </summary>
	/// <value>
	/// The difference in time.
	/// </value>
	public static float deltaTime{get{return timeDiff;}}
	
	/// <summary>
	/// The list of alarms.
	/// </summary>
	private static cAlarmList mAlarms = new cAlarmList();
	
	/// <summary>
	/// If the calendar should use multithreading for the alarms.
	/// </summary>
	private static bool mMultiThreading = false;
	
	/// <summary>
	/// Gets a value indicating whether this <see cref="cCalendar"/> should use multi threading.
	/// </summary>
	/// <value>
	/// <c>true</c> if use of multithreading allowed; otherwise, <c>false</c>.
	/// </value>
	public static bool useMultiThreading{get{return mMultiThreading;}}
	
	#region Date
	
	/// <summary>
	/// The current year in in-game time.  Default is 1600.
	/// </summary>
	private static int mYear=1600,
	
	/// <summary>
	/// The current month in in-game time.  Default is the 1st month (i.e. January).
	/// </summary>
	mMonth=1,
	
	/// <summary>
	/// The current day in in-game time.  Default is the 1st day of the month.
	/// </summary>
	mDay=1;
	
	/// <summary>
	/// Gets the year of the in-game time.
	/// </summary>
	/// <value>
	/// The year.
	/// </value>
	public static int year {get{return mYear;}}
	/// <summary>
	/// Gets the month of the in-game time.
	/// </summary>
	/// <value>
	/// The month.
	/// </value>
	public static int month {get{return mMonth;}}
	/// <summary>
	/// Gets the day of the in-game time.
	/// </summary>
	/// <value>
	/// The day.
	/// </value>
	public static int day {get{return mDay;}}
	
	// end of Date
	#endregion
	
	#region Time
	/// <summary>
	/// The current hour in in-game time.  Default is noon.  24-hour clock.
	/// </summary>
	private static int mHour=12,
	/// <summary>
	/// The current minute in in-game time.  Default is the beginning of the hour.
	/// </summary>
	mMinute=0;
	/// <summary>
	/// The current number of seconds in the current minute of in-game time.  Default is a new minute.
	/// </summary>
	private static float mSecond=0.0f;
	
	/// <summary>
	/// Gets the current hour in in-game time. (24-hour clock).
	/// </summary>
	/// <value>
	/// The hour.
	/// </value>
	public static int hour{get{return mHour;}}
	/// <summary>
	/// Gets the current minute in in-game time.
	/// </summary>
	/// <value>
	/// The minute.
	/// </value>
	public static int minute{get{return mMinute;}}
	/// <summary>
	/// Gets the current number of seconds in in-game time.
	/// </summary>
	/// <value>
	/// The seconds.
	/// </value>
	public static float seconds{get{return mSecond;}}
	
	// end of Time
	#endregion
	
	#region Custom Month Variables
	/// <summary>
	/// The list of custom months used by the calender system.
	/// </summary>
	private static Dictionary<ulong, cMonth> mMonthList = new Dictionary<ulong, cMonth>();
	
	/// <summary>
	/// The total number of days in the current in-game month.
	/// </summary>
	private static int mDaysThisMonth=1;
	
	/// <summary>
	/// Gets the number of in-game days during this in-game month.
	/// </summary>
	/// <value>
	/// The in-game days this in-game month.
	/// </value>
	public static int daysThisMonth{get{return mDaysThisMonth;}}
	
	/// <summary>
	/// The number of custom months in this calendar.
	/// </summary>
	private static ulong mMonthCount = 0;
	
	/// <summary>
	/// Gets the number of custom months in this calendar.
	/// </summary>
	/// <value>
	/// The number of custom months.
	/// </value>
	public static ulong monthCount {get{return mMonthCount;}}
	
	/// <summary>
	/// The name of the in-game custom month.
	/// </summary>
	private static string mMonthName="";
	
	/// <summary>
	/// Gets the name of the custom month.
	/// </summary>
	/// <value>
	/// The name of the custom month.
	/// </value>
	public static string monthName{get{return mMonthName;}}
	
	/// <summary>
	/// The abbreviation of the month (default: first three letters of the name of the month)
	/// </summary>
	private static string mMonthAbb="";
	
	/// <summary>
	/// Gets the current custom month abbreviation.
	/// </summary>
	/// <value>
	/// The custom month abbreviation.
	/// </value>
	public static string monthAbb{get{return mMonthAbb;}}
	
	// end of Custom Month Variables
	#endregion
	
	#region Leap Years
	/// <summary>
	/// If leap years are enabled, then leap years will occur in the calendar year.  Otherwise, every year will be the same.
	/// </summary>
	private static bool mLeapYearsEnabled = true;
	
	/// <summary>
	/// Gets a value indicating whether leapyears are active on the calendar.
	/// </summary>
	/// <value>
	/// <c>true</c> if leapyears enabled; otherwise, <c>false</c>.
	/// </value>
	public static bool leapyearsEnabled{get{return mLeapYearsEnabled;}}
	
	/// <summary>
	/// Toggles if leap years are enabled on the calendar or not.
	/// </summary>
	public static void toggleLeapYears(){mLeapYearsEnabled=!mLeapYearsEnabled;}
	
	/// <summary>
	/// If leap years are enabled, determines if the year is a leap year or not.
	/// </summary>
	/// <returns>
	/// <c>true</c> if it is a leap year; otherwise, <c>false</c>.
	/// </returns>
	public static bool isLeapYear(){
		if(!leapyearsEnabled)return false; // since it is normal years every year, there is no leap year
		
		return ((year%4==0) && (!(year%100==0)||(year%400==0)));
	}
	
	// end of Leap Years
	#endregion
	
	// end of static variables
	#endregion
	
	#region Inititialization
	/// <summary>
	/// Creates the object that updates the time each frame if it hasn't been created already.
	/// </summary>
	private static void __createObject(){
		if(isInit)return; // if the object has already been created, exit.
		GameObject t_Time = new GameObject("__cCalendar");
		t_Time.AddComponent<cCalendar>();
	}
	
	/// <summary>
	/// Initializes the object that updates the time and loads the last saved time from
	/// the player preferences if needed.
	/// </summary>
	/// <param name='loadFromPlayerPrefs'>
	/// Load last saved date and time from player prefs?
	/// </param>
	public static void init(bool loadFromPlayerPrefs){
		if(isInit)return; // this has already been initialized
		__createObject();
		if(loadFromPlayerPrefs)load ();
		isInit = true;
	}
	
	/// <summary>
	/// Initialize the object that updates the time.
	/// </summary>
	public static void init(){
		init (false);
	}
	
	// end of Initialization
	#endregion
	
	#region Load/Save Date/Time
	/// <summary>
	/// Loads the saved in-game time from the Player Preferences and sets that as the current in-game time.
	/// <returns>
	/// Returns true on success; otherwise, false.
	/// </returns>
	/// </summary>
	public static bool load(){
		bool passed=true;
		passed = (passed		&&
					setYear(PlayerPrefs.GetInt ("t_Year", cCalendar.year)));
		passed = (passed		&&
					setMonth(PlayerPrefs.GetInt ("t_Month", cCalendar.month)));
		passed = (passed		&&
					setDay(PlayerPrefs.GetInt("t_Day", cCalendar.day)));
		
		passed = (passed		&&
					setHour(PlayerPrefs.GetInt ("t_Hour", cCalendar.hour)));
		passed = (passed		&&
					setMinute(PlayerPrefs.GetInt ("t_Minute", cCalendar.minute)));
		passed = (passed		&&
					setSeconds(PlayerPrefs.GetFloat("t_Seconds", cCalendar.seconds)));
		
		return passed;
	}
	
	/// <summary>
	/// Saves the current in-game time to the Player Preferences, allowing the time to be loaded through cCalendar.init(true) or cCalendar.load()
	/// <returns>
	///	Returns true if the save was a success; otherwise false.
	/// </returns>
	/// </summary>
	public static bool save(){
		PlayerPrefs.SetInt ("t_Year", mYear);
		PlayerPrefs.SetInt ("t_Month", mMonth);
		PlayerPrefs.SetInt ("t_Day", mDay);
		
		PlayerPrefs.SetInt ("t_Hour", mHour);
		PlayerPrefs.SetInt ("t_Minute", mMinute);
		PlayerPrefs.SetFloat ("t_Seconds", mSecond);
		
		
		return true;
	}
	
	// end of Load/Save Date/Time
	#endregion
	
	#region Custom Month
	
	/// <summary>
	/// Gets the number of days in the current in-game month.
	/// </summary>
	/// <returns>
	/// The total number of in-game days within the current in-game month or -1 if there are no months
	/// </returns>
	private static int getEndOfMonth(){
		if(mMonthList.Count == 0)return 0; // there are no months to return
		if(mMonth > mMonthList.Count)return 0; // it is not currently a month
		if(mMonth < 1)return 0; // it is not currently a month
		cMonth temp;
		mMonthList.TryGetValue((ulong)mMonth, out temp);
		if(temp!=null){
			return temp.days;
		}
		return 0; // could not get the days of the month, because the month doesn't exist or there was some error.
	}
	
	/// <summary>
	/// Gets the days in a month.
	/// </summary>
	/// <returns>
	/// The days in a month.
	/// </returns>
	/// <param name='month'>
	/// The month to get the number of days
	/// </param>
	/// <param name='year'>
	/// The year to get the number of days for.
	/// </param>
	/// <exception cref='System.ArgumentException'>
	/// Is thrown when there are no months available to check.
	/// </exception>
	/// <exception cref='System.ArgumentOutOfRangeException'>
	/// Is thrown when the supplied month does not exist.
	/// </exception>
	public static int getDaysInMonth(int month, int year) {
		if(mMonthList.Count == 0){
			throw new System.ArgumentException("There are no months!");
		}
		if(month > mMonthList.Count || month < 1){
			throw new System.ArgumentOutOfRangeException("" + month + " is not an index for a month!");
		}
		cMonth temp;
		mMonthList.TryGetValue((ulong)month, out temp);
		if(temp!=null){
			if(leapyearsEnabled){
				if(year%4==0 && (year%100!=0 || year %400 ==0))
					return temp.leapyearDays;
			}
			return temp.normalDays;
		}
		Debug.LogError("Could not get the number of days for the month at index: " + month);
		return 0;
	}
	
	/// <summary>
	/// Gets the name of the custom month.
	/// </summary>
	/// <returns>
	/// The custom month's name.
	/// </returns>
	private static string getMonthName(){
		if(mMonthList.Count==0||mMonth>mMonthList.Count||mMonth<1)return ""; // there are no months, it is not a month
		cMonth temp;
		mMonthList.TryGetValue((ulong)mMonth, out temp);
		if(temp!=null){
			return temp.name;
		}
		return ""; // could not get the name of the month, because the month doesn't exist or there was some error.
	}
	
	/// <summary>
	/// Gets the abbreviation of the custom month.
	/// </summary>
	/// <returns>
	/// The abbreviation of the custom month.
	/// </returns>
	private static string getMonthAbb(){
		if(mMonthList.Count==0||mMonth>mMonthList.Count||mMonth<1)return ""; // there are no months or it is not a month
		cMonth temp;
		mMonthList.TryGetValue((ulong)mMonth, out temp);
		if(temp!=null){
			return temp.abb;
		}
		return ""; // could not get the abbreviation of the month, because the month doesn't exist or there was some error.
	}
	
	/// <summary>
	/// Adds a custom month to the calendar.
	/// </summary>
	/// <returns>
	/// The index of the month.
	/// </returns>
	/// <param name='monthName'>
	/// The name of the custom month.
	/// </param>
	/// <param name='dayCount'>
	/// The total number of days in the custom month.
	/// </param>
	public static ulong addMonth(string monthName, int dayCount){
		return addMonth(monthName, dayCount, dayCount);
	}
	
	/// <summary>
	/// Adds a custom month to the calendar.
	/// </summary>
	/// <returns>
	/// The index of the month.
	/// </returns>
	/// <param name='monthName'>
	/// The name of the custom month.
	/// </param>
	/// <param name='dayCount'>
	/// The total number of days in the custom month during a normal year.
	/// </param>
	/// <param name='leapYearDayCount'>
	/// The total number of days in the custom month during a leap year.
	/// </param>
	public static ulong addMonth(string monthName, int dayCount, int leapYearDayCount){
		return addMonth (monthName, monthName.Substring(0, 3), dayCount, leapYearDayCount);
	}
	
	/// <summary>
	/// Adds a custom month to the calendar.
	/// </summary>
	/// <returns>
	/// The index of the month.
	/// </returns>
	/// <param name='monthname'>
	/// The name of the custom month.
	/// </param>
	/// <param name='monthAbbreviation'>
	/// The abbreviation for the custom month.
	/// </param>
	/// <param name='dayCount'>
	/// The total number of days in the custom month.
	/// </param>
	public static ulong addMonth(string monthName, string monthAbbreviation, int dayCount){
		return addMonth (monthName, monthAbbreviation, dayCount, dayCount);
	}
	
	/// <summary>
	/// Adds a custom month to the calendar.
	/// </summary>
	/// <returns>
	/// The index of the month.
	/// </returns>
	/// <param name='monthName'>
	/// The name of the custom month.
	/// </param>
	/// <param name='monthAbbreviation'>
	/// The abbreviation for the custom month.
	/// </param>
	/// <param name='dayCount'>
	/// The total number of days in the custom month during a normal year.
	/// </param>
	/// <param name='leapYearDayCount'>
	/// The total number of days in the custom month during a leap year.
	/// </param>
	public static ulong addMonth(string monthName, string monthAbbreviation, int dayCount, int leapYearDayCount){
		return addMonth (new cMonth(monthName, monthAbbreviation, dayCount, leapYearDayCount));
	}
	
	/// <summary>
	/// Adds a custom month to the calendar.
	/// </summary>
	/// <returns>
	/// The index of the month.
	/// </returns>
	/// <param name='aMonth'>
	/// The custom month.
	/// </param>
	public static ulong addMonth(cMonth aMonth){
		init ();
		mMonthList.Add((ulong)mMonthList.Count+1, aMonth);
		mDaysThisMonth = getEndOfMonth();
		mMonthCount++;
		return (ulong)mMonthCount;
	}
	
	/// <summary>
	/// Remove all months from the calendar.
	/// </summary>
	public static void clearMonths(){
		init ();
		mMonthList.Clear();
		mMonthCount=0;
	}
	
	/// <summary>
	/// Uses the default months (Gregorian Calendar months).
	/// </summary>
	public static void useDefaultMonths(){
		init ();
		clearMonths(); // ensure that it is cleared
		// add the gregorian calendar months
		addMonth ("January", "Jan", 31);
		addMonth ("February", "Feb", 28, 29);
		addMonth ("March", "Mar", 31);
		addMonth ("April", "Apr", 30);
		addMonth ("May", 31);
		addMonth ("June", "Jun", 30);
		addMonth ("July", "Jul", 31);
		addMonth ("August", "Aug", 31);
		addMonth ("September", "Sep", 30);
		addMonth ("October", "Oct", 31);
		addMonth ("November", "Nov", 30);
		addMonth ("December", "Dec", 31);
	}
	
	// end of Custom Month
	#endregion
	
	#region Start/Stop Time
	/// <summary>
	/// Pauses the updating of the time.
	/// </summary>
	public static void pause(){
		init ();
		mPaused=true;
	}
	
	/// <summary>
	/// Resumes the updating of the time.
	/// </summary>
	public static void resume(){
		init ();
		mPaused=false;
	}
	
	// end of Start/Stop Time
	#endregion
	
	#region Set Date/Time
	/// <summary>
	/// Sets the in-game day if the day is within the limitations of the currently set in-game month.
	/// </summary>
	/// <returns>
	/// True if the day is within the limitations of the month; otherwise, false.
	/// </returns>
	/// <param name='Day'>
	/// The current day of the in-game month.
	/// </param>
	public static bool setDay(int Day){
		init (); // if not initialized, initialize.
		if(day < 1 || day > daysThisMonth){
			return false;
		}
		mDay = Day;
		return true;
	}
	
	/// <summary>
	/// Sets the in-game hour if it is within the limitations of a 24 hour day.
	/// </summary>
	/// <returns>
	/// True if the hour is within the limitations of a 24 hour day; otherwise, false.
	/// </returns>
	/// <param name='Hour'>
	/// The current hour of the in-game day.
	/// </param>
	public static bool setHour(int Hour){
		init (); // if not initialized, intialize.
		if(hour < 0 || hour > 23){
			return false;
		}
		mHour = Hour;
		return true;
	}
	
	/// <summary>
	/// Sets the in-game minute if it is within the limitations of a 60 minute hour.
	/// </summary>
	/// <returns>
	/// True if the minute is within the limitations of a 60 minute hour; otherwise, false.
	/// </returns>
	/// <param name='Minute'>
	/// The current minute of the in-game hour.
	/// </param>
	public static bool setMinute(int Minute){
		init (); // if not initialized, initialize
		if(Minute < 0 || Minute >= 60){
			return false;
		}
		mMinute = Minute;
		return true;
	}
	
	/// <summary>
	/// Sets the current in-game month if it is within the the number of months in one in-game year.
	/// </summary>
	/// <returns>
	/// True if the month is within the number of months in one in-game year; otherwise, false.
	/// </returns>
	/// <param name='Month'>
	/// The current month of the in-game year.
	/// </param>
	public static bool setMonth(int Month){
		init (); // if not initialized, initialize
		//if(Month < 1 || Month > (int)mMonthCount){
		if(Month < 1 || Month > 12){
			Debug.Log("Couldn't set month");
			return false;
		}
		mMonth = Month;
		mDaysThisMonth = getEndOfMonth();
		mMonthName = getMonthName();
		mMonthAbb = getMonthAbb();
		return true;
	}
	
	/// <summary>
	/// Sets the current in-game seconds if it is within the limitation of a 60 second minute.
	/// </summary>
	/// <returns>
	/// True if the seconds are within the limitations of a 60 second minute; otherwise, false.
	/// </returns>
	/// <param name='Seconds'>
	/// The current number of seconds of the in-game minute.
	/// </param>
	public static bool setSeconds(float Seconds){
		init (); // if not initialized, initialize
		if(Seconds < 0.0f || Seconds >= 60.0f){
			return false;
		}
		mSecond = Seconds;
		return true;
	}
	
	/// <summary>
	/// Sets the speed that the in-game time changes.
	/// </summary>
	/// <param name='minutesPerHour'>
	/// Real-Time Minutes per One Hour of In-Game Time.
	/// </param>
	public static void setSpeed(float minutesPerHour){
		
		init (); // if not initialized, initialize
		
		// check for possible division by zero and prevent it
		if(minutesPerHour==0){
			timeSpeed = 0f;
			return;
		}
		
		timeSpeed = minutesPerHour;
	}
	
	/// <summary>
	/// Sets the current in-game year.
	/// </summary>
	/// <returns>
	/// Always returns true.
	/// </returns>
	/// <param name='Year'>
	/// The current in-game year.
	/// </param>
	public static bool setYear(int Year){
		init (); // if not initialized, initialize
		mYear = Year;
		return true;
	}
	
	// end of Set Date/Time
	#endregion
	
	#region Alarms
	/// <summary>
	/// Sets the alarm.
	/// </summary>
	/// <returns>
	/// 1 on success, 0 on failure.
	/// </returns>
	/// <param name='alarmName'>
	/// The name of the alarm.
	/// </param>
	/// <param name='date'>
	/// The date that the alarm should
	/// go off on.
	/// </param>
	/// <param name='method'>
	/// The method to call when the 
	/// alarm goes off.
	/// </param>
	public static int setAlarm(string alarmName, cDate date, CallFunc method){
		cAlarm alarm = new cDateAlarm(alarmName,date,method);
		mAlarms.AddAlarm(ref alarm);
		return 1;
	}
	
	/// <summary>
	/// Sets the alarm.
	/// </summary>
	/// <returns>
	/// 1 on success, 0 on failure.
	/// </returns>
	/// <param name='alarmName'>
	/// The name of the alarm.
	/// </param>
	/// <param name='time'>
	/// The time that the alarm should
	/// go off on.
	/// </param>
	/// <param name='method'>
	/// The method to call when the 
	/// alarm goes off.
	/// </param>
	public static int setAlarm(string alarmName, cTime time, CallFunc method){
		cAlarm alarm = new cTimeAlarm(alarmName, time, method);
		mAlarms.AddAlarm(ref alarm);
		return 1;
	}
	
	/// <summary>
	/// Enables the use of multithreading for
	/// alarms.  WARNING: Can cause desynchronization
	/// of times and dates to occur.
	/// </summary>
	public static void enableMultiThreading(){
		mMultiThreading = true;
	}
	
	public static void disableMultiThreading(){
		mMultiThreading = false;
	}
	
	// end of Alarms
	#endregion
	
	#region Updates
	
	/// <summary>
	/// Updates the in-game seconds.
	/// </summary>
	private void updateSeconds(){
		if(isPaused)return; // don't update time if paused.
		mSecond += timeDiff; // update the number of seconds that have passed in in-game time.
		if(mSecond>=60.0f){ // if a minute has passed, reset seconds to 0
			#if UNITY_IPHONE || UNITY_ANDROID || UNITY_WEBPLAYER
			mMinute++;
			mSecond=0.0f;
			#else
			mMinute += (int)(mSecond / 60);
			mSecond = mSecond % 60.0f;
			#endif
		}else if(mSecond < 0.0f){
			mMinute--;
			mSecond=60.0f+(mSecond % 60); // get the remainder of the division and set that to the current seconds
		}
	}
	
	/// <summary>
	/// Updates the in-game minutes.
	/// </summary>
	private void updateMinutes(){
		if(isPaused)return; // don't update if paused.
		if(mMinute>=60){ // if an hour has passed, reset minutes to 0
			mHour++;
			mMinute=mMinute-60;
		}else if(mMinute<0){
			mHour--;
			mMinute=60+mMinute; // adding mMinute, because mMinute is currently negative or 0
		}
	}
	
	/// <summary>
	/// Updates the in-game hours.
	/// </summary>
	private void updateHours(){
		if(isPaused)return; // don't update if paused.
		if(mHour>=24){ // if a day has passed, reset hours to 0.
			mDay++;
			mHour=mHour-24;
		}else if(mHour < 0){
			mDay--;
			mHour=24+mHour; // adding mHour because mHour is currently negative or 0
		}
	}
	
	/// <summary>
	/// Updates the in-game days if there are any months in the calendar.
	/// </summary>
	private void updateDays(){
		if(isPaused||mMonthList.Count==0)return; // don't update if paused or if there are no months.
		if(mDay>mDaysThisMonth){ // the month has completed. It's the 1st of the next month now.
			mMonth++;
			mDay = mDay - mDaysThisMonth;
			mDaysThisMonth = getEndOfMonth();
		}else if(mDay < 1){
			mMonth--;
			mDay=getEndOfMonth()+mDay; // adding mDay because mDay is currently either 0 or negative
		}
	}
	
	/// <summary>
	/// Updates the in-game months if there are any months in the calendar.
	/// </summary>
	private void updateMonths(){
		if(isPaused||mMonthList.Count==0)return; // don't update if paused or if there are no months.
		if(mMonth>mMonthList.Count){
			mYear++;
			mMonth=1;
		}else if(mMonth < 1){
			mYear--;
			mMonth=(int)mMonthCount+mMonth; // adding mMonth because mMonth is currently either 0 or negative
			mDay = getEndOfMonth();
		}
		mDaysThisMonth=getEndOfMonth();
		mMonthName = getMonthName();
		mMonthAbb = mMonthName.Substring(0, 3);
	}
	
	// end of Updates
	#endregion
	
	#region MonoBehaviour Funcs
	void Awake(){
		DontDestroyOnLoad(this); // don't destroy this thing when loading other levels.
	}
	
	void Start(){
		// check if any months have been added, if not, add the default months (gregorian calendar months)
		if(mMonthList.Count == 0){
			useDefaultMonths();
		}
	}
	
	/// <summary>
	/// Updates the time and sets the change in time determinant to the speed of change.
	/// </summary>
	void Update () {
		if(isPaused)return; // if it is paused, do not update the time.
		
		timeDiff = Time.deltaTime * timeSpeed; // update the change in time
		
		// update the calendar
		updateSeconds();
		updateMinutes();
		updateHours();
		updateDays();
		updateMonths();
		
		if(useMultiThreading){
			mAlarms.Launch();
		}else{
			mAlarms.Run();
		}
	}
	
	// end of MonoBehaviour Funcs
	#endregion
}
