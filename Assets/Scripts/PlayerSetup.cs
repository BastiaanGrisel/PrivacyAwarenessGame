using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerSetup : NetworkBehaviour {

    [SerializeField]
	private Behaviour[] componetsToDisable;
    private Camera sceneCamera;

	public GameObject HealthUI;
	
	public List<int> keys = new List<int> (); 

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

			System.Random rnd = new System.Random();
			keys.Add(rnd.Next(1, 4));
			keys.Add(rnd.Next(1, 4));
			keys.Add(rnd.Next(1, 4));

			for (int i = 0; i < keys.Count; i++) {
				HealthUI.GetComponent<HUDKeys>().KeyTexts[i].text = keys[i].ToString();
			}

			Instantiate(HealthUI);
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
