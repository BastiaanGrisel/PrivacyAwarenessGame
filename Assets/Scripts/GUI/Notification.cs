using UnityEngine;
using System.Collections.Generic;

public class Notification : MonoBehaviour
{
    private float alpha = 1f;
    private float speed = 0.5f;

    private List<string> notifications = new List<string>();
    private string current = "";

    public void Notify(string message)
    {
        notifications.Add(message);
    }

    void Update()
    {
        if (!Mathf.Approximately(alpha, 0))
            alpha = Mathf.MoveTowards(alpha, 0.0f, speed * Time.deltaTime);
        else if (notifications.Count != 0)
        {
            current = notifications[0];
            notifications.RemoveAt(0);
            alpha = 1.0f;
        }
        else
            current = " ";
    }
  
    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 28;
        style.alignment = TextAnchor.UpperCenter;

        int width = 200;
        int height = 100;
        Color oldColor = GUI.color;
        Color newColor = GUI.color;
        newColor.a = alpha;
        GUI.color = newColor;
        GUI.Label(new Rect(Screen.width / 2 - width/2, Screen.height / 4 - height/2, width, height), current, style);
        GUI.color = oldColor;
    }
}
