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
        // Define the position relative to the target point.
        Vector3 offset = new Vector3(0, 1, 0);

        Vector3 point = Camera.main.WorldToScreenPoint(transform.position + offset);
        point.y = Screen.height - point.y;

        int xOffset = tagText.Length * 3;
        GUI.Label(new Rect(point.x - xOffset, point.y - 10, 200, 20), tagText);
    }
}