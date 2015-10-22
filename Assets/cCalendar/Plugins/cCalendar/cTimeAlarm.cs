#region Namespaces
using UnityEngine;
using System.Collections;

// end of Namespaces
#endregion

#region Definitions
using CallFunc = System.Func<int, int>;

// end of Definitions
#endregion

#region Time Alarm
/// <summary>
/// An alarm that goes off at a specific
/// time during the day/night cycle of the
/// calendar. (seconds are not used for this
/// alarm.)
/// </summary>
public class cTimeAlarm : cAlarm {
	/// <summary>
	/// If the alarm has already gone off.
	/// </summary>
	private bool m_AlreadyRun;
	/// <summary>
	/// The time of day the alarm should go off.
	/// </summary>
	cTime m_Time;
	
	/// <summary>
	/// Initializes a new instance of the <see cref="cTimeAlarm"/> class.
	/// </summary>
	/// <param name='name'>
	/// The name of the alarm.
	/// </param>
	/// <param name='time'>
	/// The time of day the alarm should go off.
	/// </param>
	/// <param name='method'>
	/// The method to call when the alarm goes off.
	/// </param>
	public cTimeAlarm(string name, cTime time, CallFunc method) : base(name,method) {
		m_Time = time;
		m_AlreadyRun = false;
	}
	
	/// <summary>
	/// Determines whether this alarm has already gone off.
	/// </summary>
	/// <returns>
	/// <c>true</c> if this alarm has not gone off; otherwise, <c>false</c>.
	/// </returns>
	public override bool IsAlive(){
		if(m_AlreadyRun || ((m_Time.hour <= cCalendar.hour && m_Time.minute < cCalendar.minute)
		|| (m_Time.hour < cCalendar.hour && m_Time.minute > cCalendar.minute))
		){
			return false;
		}
		return true;
	}
	
	/// <summary>
	/// Sets off the alarm if it hasn't gone off already.
	/// </summary>
	public override void Run(){
		if(!IsAlive())return;
		if(cCalendar.hour == m_Time.hour && cCalendar.minute == m_Time.minute){
			base.m_Method(1);
			m_AlreadyRun = true;
		}
	}
		
}

// end of Time Alarm
#endregion