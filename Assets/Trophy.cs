using UnityEngine;
using System.Collections;

public class Trophy : MonoBehaviour {

    [SerializeField]
    public bool isWinningTrophy;
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
}
