using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;
using System.Linq;
using UnityEngine.UI;

public class ServerLogic : NetworkBehaviour
{
    // Global properties
    [SyncVar] public bool GameStarted = false;

    // Game properties
    public List<Profile> Profiles = new List<Profile>();
	private List<PlayerState> Players;
	
    public void Awake()
    {
        // Initialize all the profiles.
		Profiles.Add(new Profile(){"devries@smart.nl", "Stanislas College", "Gitaar Spelen", "B-Brave", "James Bond", "4567 GB", "Single", "Pizza", "Bobby", "Snackbar"});
		Profiles.Add(new Profile(){"jongen34@school.nl", "Stedelijk Gymnasium", "Paard rijden", "Justin Bieber", "Star Wars", "7832 FD", "Getrouwd", "Nasi Goreng", "Rataplan", "Supermarkt"});
		Profiles.Add(new Profile(){"concert@sea.net", "TU Delft", "Majorette", "OneDirection", "The Hobbit", "2374 AD", "In een relatie", "Hutspot", "Loebas", "Krantenwijk"});
		Profiles.Add(new Profile(){"kees90@coolmail.com", "Hogeschool Inholland", "Boeken lezen", "K3", "SpangaS in Actie", "6709 KL", "Uit elkaar", "Pannenkoeken", "Bello", "Gras maaien"});
		Profiles.Add(new Profile(){"sophie43@coolmail.com", "Engelbertcollege", "Voetballen", "Armin van Buren", "Finding Nemo", "1735 TR", "Ingewikkeld", "Patat", "Dexter", "Kledingwinkel"});
		Profiles.Add(new Profile(){"ikbenhier123@home.nl", "De Regenboogschool", "Hockey", "Coldplay", "Pirates of the Caribbean", "5008 EP", "Verloofd", "Couscous", "Idefix", "Bakkerij"});
		Profiles.Add(new Profile(){"hallodaar!@party.com", "CBS De Acker", "Tennis", "Adele", "Peter Pan", "8062 MW", "Gescheiden", "Chili con carne", "Nero", "Restaurant"});
		Profiles.Add(new Profile(){"sophie43@coolmail.com", "Engelbertcollege", "Voetballen", "Armin van Buren", "Finding Nemo", "1735 TR", "Ingewikkeld", "Patat", "Dexter", "Kledingwinkel"});
		Profiles.Add(new Profile(){"ikbenhier123@home.nl", "De Regenboogschool", "Hockey", "Coldplay", "Pirates of the Caribbean", "5008 EP", "Verloofd", "Couscous", "Idefix", "Bakkerij"});
		Profiles.Add(new Profile(){"hallodaar!@party.com", "CBS De Acker", "Tennis", "Adele", "Peter Pan", "8062 MW", "Gescheiden", "Chili con carne", "Nero", "Restaurant"});

		Players = new List<PlayerState> ();
    }
	
    // Dynamically assigns a Player certain data.
    public void RegisterPlayer(PlayerState player)
    {
		Players.Add (player);
    }

	public void DeRegisterPlayer(PlayerState player) {
		Players.Remove (player);
	}

	// Update is called once per frame
	void Update ()
    {
        if (isServer && Input.GetKeyDown (KeyCode.Return) && !GameStarted)
			StartGame ();
	}
	
	[Server]
	void StartGame() {
		// Calculate the amount of attributes each player needs to get to ensure allt he attributes are in the game
		int AttributesPerPlayer = 3;//(int) Math.Max (3, Math.Ceiling ((double) Profile.TotalNumberOfAttributes() / (double) Players.Count));

		// Generate a list of all the attributes in random order
		List<ProfileAttribute> AllAttributes = Enum.GetValues(typeof(ProfileAttribute)).Cast<ProfileAttribute>().OrderBy(a => UnityEngine.Random.value).ToList ();
		List<ProfileAttribute> AttributesNotYetInGame = new List<ProfileAttribute> (AllAttributes);

		for (int i = 0; i < Players.Count; i++)
        {
			Players[i].ProfileIndex = i;

			for(int j = 0; j < AttributesPerPlayer; j++)
            {
				if(AttributesNotYetInGame.Any ()) {
					// Add an attribute to the player (and to the players of the other players since SelectedAttributed is a SyncList)
					Players[i].SelectedAttributes.Add ((int) AttributesNotYetInGame[0]);
					// Delete it from the list of attributes that are not yet in the game
					AttributesNotYetInGame.RemoveAt(0);
				} else {
					// Add a random attribute that that player does not already have
					Players[i].SelectedAttributes.Add ((int) AllAttributes.Find(a => !Players[i].SelectedAttributes.Contains((int) a)));
				}
			}
					
			// Assign each player a team
			Players[i].Team = i % 2;

            // Set the player Tags.
            Players[i].GetComponent<PlayerState>().RpcSetPlayerTag();
        }

		// Start the game
		GameStarted = true;

        GameObject.Find("Notification").GetComponent<Notification>().Notify("Game started!");
    }
}
