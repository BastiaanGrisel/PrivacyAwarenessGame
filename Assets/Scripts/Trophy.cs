using UnityEngine;
using System.Collections;

public class Trophy : MonoBehaviour
{
	public int Number;

    public void Start()
    {
        gameObject.AddComponent<Tag3D>();
        gameObject.GetComponent<Tag3D>().tagText = "Trophy " + Number.ToString();
        gameObject.GetComponent<Tag3D>().color = Color.yellow;
    }

    public void showTag()
    {
        
    }
}
