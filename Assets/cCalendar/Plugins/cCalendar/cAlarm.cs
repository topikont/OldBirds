#region Definitions
using CallFunc = System.Func<int, int>;

// end of Definitions
#endregion

#region Alarm
/// <summary>
/// The base class for all alarms.
/// </summary>
public class cAlarm {
	/// <summary>
	/// The name of the alarm.
	/// </summary>
	protected string m_Name;
	/// <summary>
	/// The method to be called when
	/// the alarm goes off.
	/// </summary>
	protected CallFunc m_Method;
	/// <summary>
	/// Gets the name of the alarm.
	/// </summary>
	/// <value>
	/// The name of the alarm.
	/// </value>
	public string name {get{return m_Name;}}
	
	/// <summary>
	/// Initializes a new instance of the <see cref="cCalendar.Alarm"/> class.
	/// </summary>
	/// <param name='name'>
	/// The name of the alarm.
	/// </param>
	/// <exception cref='System.Exception'>
	/// Is thrown when the name is an empty string.
	/// </exception>
	public cAlarm(string name, CallFunc method){
		if(string.Empty.Equals(name)){
			throw new System.Exception("Alarm name cannot be empty.");
		}
		m_Name = name;
		m_Method = method;
	}
	
	/// <summary>
	/// Determines whether the alarm has already gone off.
	/// </summary>
	/// <returns>
	/// <c>true</c> if the alarm has not gone off; otherwise, <c>false</c>.
	/// </returns>
	public virtual bool IsAlive(){
		return false;
	}
	
	/// <summary>
	/// Set off the alarm if not already set off.
	/// </summary>
	public virtual void Run() {
		
	}
}

// end of Alarm
#endregion