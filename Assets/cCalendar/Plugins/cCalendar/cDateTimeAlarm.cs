#region Namespaces
using UnityEngine;
using System.Collections;

// end of Namespaces
#endregion

#region Defines
using CallFunc = System.Func<int,int>;

// end of Defines
#endregion

#region Date-Time Alarm
/// <summary>
/// A date-time alarm.  Sets an alarm off
/// on a specific date at a specific time.
/// </summary>
public class cDateTimeAlarm : cAlarm {
	/// <summary>
	/// If the alarm has already been set off.
	/// </summary>
	private bool m_AlreadyRun;
	/// <summary>
	/// The date the alarm should go off.
	/// </summary>
	private cDate m_Date;
	/// <summary>
	/// The time the alarm should go off.
	/// </summary>
	private cTime m_Time;
	
	public cDateTimeAlarm(string name, cDate date, cTime time, CallFunc method) : base(name, method) {
		m_AlreadyRun = false;
		m_Date = date;
		m_Time = time;
	}
	
	/// <summary>
	/// Determines whether this alarm has already gone off.
	/// </summary>
	/// <returns>
	/// <c>true</c> if this alarm has not gone off+; otherwise, <c>false</c>.
	/// </returns>
	public override bool IsAlive ()
	{
		if(m_AlreadyRun || 
			((m_Date.day <= cCalendar.day && m_Date.month <= cCalendar.month && m_Date.year <= cCalendar.year && m_Time.hour <= cCalendar.hour && m_Time.minute < cCalendar.minute)||
			(m_Date.day <= cCalendar.day && m_Date.month <= cCalendar.month && m_Date.year <= cCalendar.year && m_Time.hour < cCalendar.hour && m_Time.minute > cCalendar.minute)||
			(m_Date.day > cCalendar.day && m_Date.month < cCalendar.month && m_Date.year <= cCalendar.year && m_Time.hour <= cCalendar.hour && m_Time.minute < cCalendar.minute)||
			(m_Date.day > cCalendar.day && m_Date.month < cCalendar.month && m_Date.year <= cCalendar.year && m_Time.hour < cCalendar.hour && m_Time.minute > cCalendar.minute)||
			(m_Date.day > cCalendar.day && m_Date.month < cCalendar.month && m_Date.year <= cCalendar.year))){
			return false;
		}
		
		return true;
	}
	
	/// <summary>
	/// Set off the alarm if it has not already gone off.
	/// </summary>
	public override void Run ()
	{
		if(!IsAlive())return;
		if(m_Date.day == cCalendar.day && m_Date.month == cCalendar.month && m_Date.year == cCalendar.year && m_Time.hour == cCalendar.hour && m_Time.minute == cCalendar.minute){
			base.m_Method(1);
			m_AlreadyRun = true;
		}
	}
}

// end of Date-Time Alarm
#endregion