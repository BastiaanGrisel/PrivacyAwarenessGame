using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ServerLogic : NetworkBehaviour {
    [SyncVar]
    public bool isRunning;

	// Use this for initialization
	void Start () {
        isRunning = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (isServer && Input.GetKeyDown(KeyCode.Return) && !isRunning)
        {
            isRunning = true;
        }
	}

    [Command]
    public void CmdEndGame()
    {
        isRunning = false;
    }
}
