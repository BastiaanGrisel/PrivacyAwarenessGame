using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {

    [SerializeField]
	private Behaviour[] componetsToDisable;
    private Camera sceneCamera;

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
        }
	}

    void OnDisable()
    {
        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }
    }
}
