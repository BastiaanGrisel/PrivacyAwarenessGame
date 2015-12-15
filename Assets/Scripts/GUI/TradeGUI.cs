using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TradeGUI : MonoBehaviour
{
    // Networking Attributes
    private NetworkManager manager;

    void Start()
    {
        gameObject.SetActive(false);
    }
}
