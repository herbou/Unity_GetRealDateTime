using UnityEngine;
using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine.Networking;

public class WorldTimeAPI : MonoBehaviour {
#region Singleton class: WorldTimeAPI

	public static WorldTimeAPI Instance;

	void Awake ( ) {
		if ( Instance == null ) {
			Instance = this;
			DontDestroyOnLoad ( this.gameObject );
		} else {
			Destroy ( this.gameObject );
		}
	}

#endregion

	//json container
	struct TimeData {
		//public string client_ip;
		//...
		public string datetime;
		//..
	}

	const string API_URL = "http://worldtimeapi.org/api/ip";

	[HideInInspector] public bool IsTimeLodaed = false;

	private DateTime _currentDateTime = DateTime.Now;

	void Start ( ) {
		StartCoroutine ( GetRealDateTimeFromAPI ( ) );
	}

	public DateTime GetCurrentDateTime ( ) {
		//here we don't need to get the datetime from the server again
		// just add elapsed time since the game start to _currentDateTime

		return _currentDateTime.AddSeconds ( Time.realtimeSinceStartup );
	}

	IEnumerator GetRealDateTimeFromAPI ( ) {
		UnityWebRequest webRequest = UnityWebRequest.Get ( API_URL );
		Debug.Log ( "getting real datetime..." );

		yield return webRequest.Send ( );

		if ( webRequest.isError ) {
			//error
			Debug.Log ( "Error: " + webRequest.error );

		} else {
			//success
			TimeData timeData = JsonUtility.FromJson<TimeData> ( webRequest.downloadHandler.text );
			//timeData.datetime value is : 2020-08-14T15:54:04+01:00

			_currentDateTime = ParseDateTime ( timeData.datetime );
			IsTimeLodaed = true;

			Debug.Log ( "Success." );
		}
	}
	//datetime format => 2020-08-14T15:54:04+01:00
	DateTime ParseDateTime ( string datetime ) {
		//match 0000-00-00
		string date = Regex.Match ( datetime, @"^\d{4}-\d{2}-\d{2}" ).Value;

		//match 00:00:00
		string time = Regex.Match ( datetime, @"\d{2}:\d{2}:\d{2}" ).Value;

		return DateTime.Parse ( string.Format ( "{0} {1}", date, time ) );
	}
}


/* API (json)
{
	"abbreviation" : "+01",
	"client_ip"    : "190.107.125.48",
	"datetime"     : "2020-08-14T15:544:04+01:00",
	"dst"          : false,
	"dst_from"     : null,
	"dst_offset"   : 0,
	"dst_until"    : null,
	"raw_offset"   : 3600,
	"timezone"     : "Asia/Brunei",
	"unixtime"     : 1595601262,
	"utc_datetime" : "2020-08-14T15:54:04+00:00",
	"utc_offset"   : "+01:00"
}

We only need "datetime" property.
*/