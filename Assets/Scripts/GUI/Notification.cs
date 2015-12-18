using UnityEngine;
using System.Collections;

public class Notification : MonoBehaviour
{
    private Color myColor;
    private float alpha = 1f;
    private float speed = 0.5f;
 
    private float targetAlpha = 0;
  
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log( "Fading Out" );
            alpha = 1;
        }
         
        if(!Mathf.Approximately(alpha, targetAlpha))
        {
            alpha = Mathf.MoveTowards( alpha, targetAlpha, speed * Time.deltaTime );
        }
    }
  
     void OnGUI()
     {
        GUI.Label(new Rect(500, 500, 300, 300), "Fading Text");
    }
 
     void pauseWindow (int windowID) 
     {
         myColor = GUI.color;
         myColor.a = alpha;
         GUI.color = myColor;
         GUI.Label(new Rect(100,100,100,100), "Fading Text");
         myColor.a = 1f;
         GUI.color = myColor;
         GUI.Label(new Rect (100,200,100,100), "Simple Text");
     }
}
