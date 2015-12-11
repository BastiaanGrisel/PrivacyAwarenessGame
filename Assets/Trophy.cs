using UnityEngine;
using System.Collections;

public class Trophy : MonoBehaviour {

    [SerializeField]
    private bool isWinningTrophy;
    private ServerLogic serverLogic;

	// Use this for initialization
	void Start () {

	}

    void Awake()
    {
        serverLogic = GameObject.FindObjectOfType<ServerLogic>();
    }

	// Update is called once per frame
	void Update () {

	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && isWinningTrophy)
        {
            serverLogic.CmdEndGame();
        }
    }
}
