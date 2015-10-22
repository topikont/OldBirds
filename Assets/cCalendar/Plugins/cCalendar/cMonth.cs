#region Namespaces
using UnityEngine;
using System.Collections;

// end of Namespaces
#endregion

#region Custom Month
/// <summary>
/// A Custom Month for the cCalendar calendar.
/// </summary>
/// <exception cref='ArgumentException'>
/// Is thrown when zero (0) has been input for the number of days in a month, for either the normal month
/// or a month that deals with a leap year.
/// </exception>
public class cMonth {

	/// <summary>
	/// The name of the custom month.
	/// </summary>
	private string mName,
	
	/// <summary>
	/// The abbreviation of the custom month.
	/// </summary>
	mAbb;
	
	/// <summary>
	/// The number of days in this custom month.
	/// </summary>
	private int mDays,
	
	/// <summary>
	/// The number of days in this custom month during a leap year.
	/// </summary>
	mLeapYearDays;
	
	/// <summary>
	/// Gets the name.
	/// </summary>
	/// <value>
	/// The name.
	/// </value>
	public string name{get{return mName;}}
	
	/// <summary>
	/// Gets the abbreviation.
	/// </summary>
	/// <value>
	/// The abbreviation.
	/// </value>
	public string abb{get{return mAbb;}}
	
	/// <summary>
	/// Gets the days.
	/// </summary>
	/// <value>
	/// The days.
	/// </value>
	public int days{get{return (cCalendar.isLeapYear()?mLeapYearDays:mDays);}}
	
	/// <summary>
	/// Gets the days in a leapyear year.
	/// </summary>
	/// <value>
	/// The leapyear days.
	/// </value>
	public int leapyearDays{get{return mLeapYearDays;}}
	
	/// <summary>
	/// Gets the days in a non-leapyear year.
	/// </summary>
	/// <value>
	/// The normal days.
	/// </value>
	public int normalDays{get{return mDays;}}
	
	/// <summary>
	/// Initializes a new instance of the <see cref="cCalendar.cMonth"/> class.
	/// </summary>
	/// <param name='name'>
	/// The name of the custom month
	/// </param>
	/// <param name='dayCount'>
	/// The total number of days in this month.
	/// </param>
	public cMonth(string name, int dayCount) : this(name,dayCount,dayCount) {}
	
	/// <summary>
	/// Initializes a new instance of the <see cref="cCalendar.cMonth"/> class.
	/// </summary>
	/// <param name='name'>
	/// The name of the custom month.
	/// </param>
	/// <param name='dayCount'>
	/// The total number of days in this month.
	/// </param>
	/// <param name='leapYearDayCount'>
	/// The total number of days in this month during a leap year.
	/// </param>
	public cMonth(string name, int dayCount, int leapYearDayCount) : this(name, name.Substring(0,3), dayCount, leapYearDayCount) {}
	
	/// <summary>
	/// Initializes a new instance of the <see cref="cCalendar.cMonth"/> class.
	/// </summary>
	/// <param name='name'>
	/// The name of the custom month.
	/// </param>
	/// <param name='abb'>
	/// The abbreviation for the custom month.
	/// </param>
	/// <param name='dayCount'>
	/// The total number of days in this month.
	/// </param>
	public cMonth(string name, string abb, int dayCount) : this(name, abb, dayCount, dayCount) {}
	
	/// <summary>
	/// Initializes a new instance of the <see cref="cCalendar.cMonth"/> class.
	/// </summary>
	/// <param name='name'>
	/// The name of the custom month.
	/// </param>
	/// <param name='dayCount'>
	/// The number of days this month during a normal year.
	/// </param>
	/// <param name='leapYearDayCount'>
	/// The number of days this month during a leap year.
	/// </param>
	/// <exception cref='ArgumentException'>
	/// Is thrown when dayCount or leapYearDayCount are less than or equal to zero (0).
	/// </exception>
	public cMonth(string name, string monthAbb, int dayCount, int leapYearDayCount){
		if(dayCount<=0||leapYearDayCount<=0){
			throw new System.ArgumentException("A month cannot have zero (0) days.");
		}
		
		mName			= name;
		mAbb			= monthAbb;
		mDays			= dayCount;
		mLeapYearDays	= leapYearDayCount;
	}
}

// end of Custom Month
#endregion