using UnityEngine;
using System.Collections;

public class Trophy : MonoBehaviour {

    [SerializeField]
    private bool isWinningTrophy;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && isWinningTrophy)
        {
            other.gameObject.GetComponent<PlayerSetup>().CmdEndGame();
        }
    }
}
