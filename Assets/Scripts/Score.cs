using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Score : NetworkBehaviour {

	[SyncVar] public int team1 = 0;
	[SyncVar] public int team2 = 0;

	// Use this for initialization
	void Start () {
		SetScore (0,0);
	}
	
	void SetScore(int t1, int t2) {
		team1 = t1;
		team2 = t2;
		GetComponent<Text>().text = t1.ToString() + " - " + t2.ToString();
	}

//	[Command]
	public void AddOnePointTo(int t) {
		switch (t) {
			case 0: SetScore (++team1, team2); break;
			case 1: SetScore (team1, ++team2); break;
		}
	}

	public int getPoints (int t)
	{
		switch (t) {
			case 0: return team1;
			case 1: return team2;
		}
		return -1;
	}
}