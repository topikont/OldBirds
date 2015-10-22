#region Namespaces
using UnityEngine;
using System.Collections;
// end of Namespaces
#endregion

#region cTime
public class cTime {
	/// <summary>
	/// The hour.
	/// </summary>
	private int mHour,
	/// <summary>
	/// The minute.
	/// </summary>
	mMinute;
	/// <summary>
	/// The number of seconds.
	/// </summary>
	private float mSeconds;
	
	/// <summary>
	/// Gets the hour.
	/// </summary>
	/// <value>
	/// The hour.
	/// </value>
	public int hour{get{return mHour;}}
	/// <summary>
	/// Gets the minute.
	/// </summary>
	/// <value>
	/// The minute.
	/// </value>
	public int minute{get{return mMinute;}}
	/// <summary>
	/// Gets the seconds.
	/// </summary>
	/// <value>
	/// The seconds.
	/// </value>
	public float seconds{get{return mSeconds;}}
	
	/// <summary>
	/// Initializes a new instance of the <see cref="cTime"/> class.
	/// </summary>
	/// <param name='hour'>
	/// The hour of the time.
	/// </param>
	/// <param name='minute'>
	/// The minute of the time.
	/// </param>
	/// <param name='seconds'>
	/// The seconds of the time.
	/// </param>
	public cTime(int hour, int minute, float seconds) {
		setHour (hour);
		setMinute (minute);
		setSeconds(seconds);
	}
	
	/// <summary>
	/// Sets the hour.
	/// </summary>
	/// <param name='hour'>
	/// The hour.
	/// </param>
	/// <exception cref='System.ArgumentOutOfRangeException'>
	/// Is thrown when the hour provided is not a known hour
	/// in a standard 24 hour day.
	/// </exception>
	public void setHour(int hour){
		if(hour < 0 || hour > 23){
			throw new System.ArgumentOutOfRangeException("" + hour + " is not an accepted hour.");
		}
		mHour = hour;
	}
	
	/// <summary>
	/// Sets the minute.
	/// </summary>
	/// <param name='minute'>
	/// The minute.
	/// </param>
	/// <exception cref='System.ArgumentOutOfRangeException'>
	/// Is thrown when the minute provided is not a known minute
	/// in a 60-minute hour.
	/// </exception>
	public void setMinute(int minute){
		if(minute < 0 || minute > 59){
			throw new System.ArgumentOutOfRangeException("" + minute + " is not an accepted minute.");
		}
		mMinute = minute;
	}
	
	/// <summary>
	/// Sets the seconds.
	/// </summary>
	/// <param name='seconds'>
	/// The seconds.
	/// </param>
	/// <exception cref='System.ArgumentOutOfRangeException'>
	/// Is thrown when the seconds provided are not a known
	/// number of seconds in a 60-second minute.
	/// </exception>
	public void setSeconds(float seconds){
		if(seconds < 0.0f || seconds >= 60.0f){
			throw new System.ArgumentOutOfRangeException("" + seconds + " is not an accepted number of seconds.");
		}
		mSeconds = seconds;
	}
}
// end of cTime
#endregion