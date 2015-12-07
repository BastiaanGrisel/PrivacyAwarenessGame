using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerSetup : NetworkBehaviour {

    [SerializeField]
	private Behaviour[] componetsToDisable;
    private Camera sceneCamera;

	public GameObject HealthUI;

	public SyncListInt keys;

	void Awake() {
		keys = new SyncListInt ();
	}

    void Start ()
    {
        if (!isLocalPlayer)
        {
            foreach (Behaviour comp in componetsToDisable){
                comp.enabled = false;
            }
        }
        else
        {
            sceneCamera = Camera.main;
            if (sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }

			GameObject ui = Instantiate(HealthUI);
			for (int i = 0; i < keys.Count; i++) {
				ui.GetComponent<HUDKeys>().KeyTexts[i].text = keys[i].ToString();
			}
        }
	}

    void OnDisable()
    {
        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }
    }

	[Command]
	public void CmdDestroyObject(NetworkInstanceId netID)
	{
		GameObject theObject = NetworkServer.FindLocalObject(netID);
		NetworkServer.Destroy (theObject);
	}
}
