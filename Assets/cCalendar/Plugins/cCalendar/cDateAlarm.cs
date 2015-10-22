#region Namespaces
using UnityEngine;
using System.Collections;

// end of Namespaces
#endregion

#region Definitions
using CallFunc = System.Func<int, int>;

// end of Definitions
#endregion

#region Date Alarm
/// <summary>
/// An alarm that goes off at a specific
/// day of the month (if non-repeating,
/// a specific year too).
/// </summary>
public class cDateAlarm : cAlarm {
	/// <summary>
	/// If the alarm has already gone off
	/// for the day.
	/// </summary>
	private bool m_AlreadyRun;
	/// <summary>
	/// The initial date that the alarm
	/// should go off.
	/// </summary>
	cDate m_Date;
	
	/// <summary>
	/// Initializes a new instance of the <see cref="cDateAlarm"/> class.
	/// </summary>
	/// <param name='name'>
	/// The name of the alarm.
	/// </param>
	/// <param name='date'>
	/// The date that the alarm should go off.
	/// </param>
	/// <param name='method'>
	/// The method to call when the alarm goes off.
	/// </param>
	public cDateAlarm(string name, cDate date, CallFunc method) : base(name, method) {
		m_Date = date;
		m_AlreadyRun = false;
	}
	
	/// <summary>
	/// Determines whether this instance is alive.
	/// </summary>
	/// <returns>
	/// <c>true</c> if this instance is alive; otherwise, <c>false</c>.
	/// </returns>
	public override bool IsAlive() {
		if(m_AlreadyRun || ((m_Date.month <= cCalendar.month && m_Date.day < cCalendar.day && m_Date.year <= cCalendar.year)
			||(m_Date.month < cCalendar.month && m_Date.day > cCalendar.day && m_Date.year <= cCalendar.year))
			){
			return false;
		}
		return true;
	}
	
	/// <summary>
	/// Set off the alarm if not already run.
	/// </summary>
	public override void Run() {
		if(!IsAlive())return;
		if(m_Date.day == cCalendar.day && m_Date.month == cCalendar.month && m_Date.year == cCalendar.year){
			base.m_Method(1);
			m_AlreadyRun = true;
		}
	}
}

// end of Date Alarm
#endregion