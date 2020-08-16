# Get real date & time from an API 
## Prevent cheating by changing device's time or date.

[↪Watch Video Tutorial ](https://www.youtube.com/watch?v=uJK1ajLaq6I)

### If you want to use your own API using php :
- add a folder to your server named `TimeApi` 
- create an `index.php` file inside of TimeApi/ 
```php
<?php

$datetime = date("Y-m-d\TH:i:s", time());
echo '{ "datetime": "'.$datetime.'" }';

?>
```

Now you can get time from your server:
`http://yourdomainname.com/TimeApi/`

### If you don't want to use the json format :
```php
<?php

echo date("Y-m-d\TH:i:s", time());

?>
```
But make sure to remove the `JsonUtility..` code from `WorldTimeApi.cs` in the `Assets` folder:
```c#
IEnumerator GetRealDateTimeFromAPI ( ) {
	//...
	//...
	} else {
		//success
		TimeData timeData = new TimeData( );
		timeData.datetime = webRequest.downloadHandler.text;
		//....
	}
}
```
