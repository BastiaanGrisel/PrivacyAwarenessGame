using UnityEngine;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

public class Notification : MonoBehaviour
{
	private float StartTime;
	private float FadeSpeed = 2f;
    private float Alpha = 1f;

	private SortedDictionary<string,float> Notifications = new SortedDictionary<string,float>();
	private KeyValuePair<string,float>? CurrentNotification = null;

    public void Notify(string Message, float Duration = 1000)
    {
		if(!Notifications.ContainsKey(Message))
        	Notifications.Add(Message,Duration);
    }

    void Update()
    {
		// Show a notification if there is one
		if (!CurrentNotification.HasValue && Notifications.Any ()) {
			CurrentNotification = Notifications.First ();
			StartTime = Time.time * 1000;
			Alpha = 1f;
			Notifications.Remove(CurrentNotification.Value.Key);
		} else if(Mathf.Approximately(Alpha,0)) {
			// Remove the notification if it has become invisible
			CurrentNotification = null;
		} else if(CurrentNotification.HasValue && Time.time * 1000 - StartTime > CurrentNotification.Value.Value) {
			// If the duration of a notification is over, fade out the notification
			Alpha = Mathf.MoveTowards(Alpha, 0.0f, FadeSpeed * Time.deltaTime);
		} 
	}
  
    void OnGUI()
    {
		if (!CurrentNotification.HasValue)
			return;

        GUIStyle style = new GUIStyle();
        style.fontSize = 28;
        style.alignment = TextAnchor.UpperCenter;
		Color c = Color.white;
		c.a = Alpha;
		style.normal.textColor = c;

        int width = 200;
        int height = 100;
		GUI.Label(new Rect(Screen.width / 2 - width/2, Screen.height / 4 - height/2, width, height), CurrentNotification.Value.Key, style);
    }
}
