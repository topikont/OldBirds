#region Namespaces
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

// end of Namespaces
#endregion

#region Alarm List
public class cAlarmList {
	/// <summary>
	/// The alarms to be checked
	/// on updates.
	/// </summary>
	private List<cAlarm> m_Alarms;
	/// <summary>
	/// The thread start object
	/// containing the method to
	/// thread.
	/// </summary>
	ThreadStart m_ThreadStart;
	/// <summary>
	/// The thread.
	/// </summary>
	Thread m_Thread;
	
	/// <summary>
	/// Initializes a new instance of the <see cref="cAlarmList"/> class.
	/// </summary>
	public cAlarmList() {
		m_Alarms = new List<cAlarm>();
		m_ThreadStart = new ThreadStart(Run);
		m_Thread = new Thread(m_ThreadStart);
	}
	
	/// <summary>
	/// Adds an alarm to the list.
	/// </summary>
	/// <param name='alarm'>
	/// The alarm to add.
	/// </param>
	public void AddAlarm(ref cAlarm alarm) {
		m_Alarms.Add(alarm);
	}
	
	/// <summary>
	/// Begin the execution of the thread.
	/// </summary>
	public void Launch() {
		if(m_Thread.IsAlive)return;
		m_Thread.Start();
		while(!m_Thread.IsAlive);
		Thread.Sleep(1);
	}
	
	/// <summary>
	/// Abort the execution of the thread.
	/// </summary>
	public void Abort() {
		if(!m_Thread.IsAlive)return;
		m_Thread.Abort();
	}
	
	/// <summary>
	/// Run the updates for the list of alarms.
	/// </summary>
	public void Run() {
		do{
			List<cAlarm> alarmList = new List<cAlarm>(m_Alarms);
			foreach(cAlarm i in alarmList) {
				i.Run();
				if(!i.IsAlive()){
					m_Alarms.Remove(i);
				}
			}
		}while(cCalendar.useMultiThreading);
	}
	
	/// <summary>
	/// Determines whether this instance has an alarm with the specified name.
	/// </summary>
	/// <returns>
	/// <c>true</c> if this instance has the alarm with the specified name; otherwise, <c>false</c>.
	/// </returns>
	/// <param name='name'>
	/// The name of the alarm to find.
	/// </param>
	public bool HasAlarm(string name) {
		foreach(cAlarm i in m_Alarms) {
			if(i.name.Equals(name)){
				return true;
			}
		}
		return false;
	}
	
	/// <summary>
	/// Removes the alarm if it exists.
	/// </summary>
	/// <param name='name'>
	/// The name of the alarm to remove.
	/// </param>
	public void RemoveAlarm(string name) {
		if(!HasAlarm(name))return;
		List<cAlarm> alarmList = new List<cAlarm>(m_Alarms);
		foreach(cAlarm i in alarmList) {
			if(i.name.Equals(name)){
				m_Alarms.Remove(i);
				return;
			}
		}
	}
}

// end of Alarm List
#endregion