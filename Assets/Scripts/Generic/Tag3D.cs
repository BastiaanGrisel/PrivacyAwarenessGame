﻿using UnityEngine;
using System.Collections;

public class Tag3D : MonoBehaviour
{
    public string tagText = "<Dummy Tag>";

    void Start()
    {

    }
    void OnGUI()
    {
        Vector3 offset = new Vector3(0, 3, 0); // height above the target position

        Vector3 point = Camera.main.WorldToScreenPoint(transform.position + offset);
        point.y = Screen.height - point.y;

        GUI.Label(new Rect(point.x - 35, point.y - 20, 200, 20), tagText);
    }
}
