#region Namespaces
using UnityEngine;
using System.Collections;
// end of Namespaces
#endregion

#region cDate
public class cDate : MonoBehaviour, System.IComparable<cDate> {
	/// <summary>
	/// The year of this <see cref="cDate"/> object.
	/// </summary>
	private int mYear,
	/// <summary>
	/// The month of this <see cref="cDate"/> object.
	/// </summary>
	mMonth,
	/// <summary>
	/// The day of this <see cref="cDate"/> object.
	/// </summary>
	mDay;
	/// <summary>
	/// Gets the year of this <see cref="cDate"/> object.
	/// </summary>
	/// <value>
	/// The year.
	/// </value>
	public int year{get{return mYear;}}
	/// <summary>
	/// Gets the month of this <see cref="cDate"/> object.
	/// </summary>
	/// <value>
	/// The month.
	/// </value>
	public int month{get{return mMonth;}}
	/// <summary>
	/// Gets the day of this <see cref="cDate"/> object.
	/// </summary>
	/// <value>
	/// The day.
	/// </value>
	public int day{get{return mDay;}}
	
	/// <summary>
	/// Initializes a new instance of the <see cref="cDate"/> class.
	/// </summary>
	/// <param name='year'>
	/// The year to set the date.
	/// </param>
	/// <param name='month'>
	/// The month of the date.
	/// </param>
	/// <param name='day'>
	/// The day of the date.
	/// </param>
	/// <exception cref='System.ArgumentOutOfRangeException'>
	/// Is thrown when the month or day are not consistent with
	/// the number of months or days possible.
	/// </exception>
	public cDate(int year, int month, int day) {
		setYear(year);
		setMonth(month);
		setDay (day);
	}
	
	/// <summary>
	/// Sets the day.
	/// </summary>
	/// <param name='day'>
	/// The day of the date.
	/// </param>
	/// <exception cref='System.ArgumentOutOfRangeException'>
	/// Is thrown when the day supplied is not existent for the
	/// current month.
	/// </exception>
	public void setDay(int day) {
		int days = cCalendar.getDaysInMonth(mMonth, mYear);
		if(day < 1 || day > days){
			throw new System.ArgumentOutOfRangeException("There are only " + days + " in the month!");
		}
		mDay = day;
	}
	
	/// <summary>
	/// Sets the year.
	/// </summary>
	/// <param name='year'>
	/// The year of the date.
	/// </param>
	public void setYear(int year){
		mYear = year;
	}
	
	/// <summary>
	/// Sets the month.
	/// </summary>
	/// <param name='month'>
	/// The month of the date.
	/// </param>
	/// <exception cref='System.ArgumentOutOfRangeException'>
	/// Is thrown when the month does not exist.
	/// </exception>
	public void setMonth(int month){
		if(month < 1 || (ulong)month > cCalendar.monthCount){
			throw new System.ArgumentOutOfRangeException("There are only " + cCalendar.monthCount + " months in the year!");
		}
		mMonth = month;
		int days = cCalendar.getDaysInMonth(mMonth, mYear);
		if(mDay > days){
			Debug.LogWarning("There are too many days in this month -- it is no longer a valid date.");
		}
	}
	
	public int CompareTo(cDate other) {
		return 0;
	}
}
// end of cDate
#endregion