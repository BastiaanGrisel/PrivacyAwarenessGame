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
	public List<PlayerState> Players { get; private set; }
	public GameObject ScoreBoardInstance;

    public void Awake()
    {
        // Initialize all the profiles.
		Profiles.Add(new Profile(){"johnsmith@smart.com", "Stanford University", "Playing Guitar", "Robbie Williams", "James Bond", "4567 GB", "Single", "Pizza", "Parrot", "Snackbar"});

		Profiles.Add(new Profile(){"freddy34@school.com", "Waterloo University", "Fishing", "Justin Bieber", "Pirates of the Caribbean", "7832 FD", "Married", "Sushi", "Goldfish", "Elderly Assistance"});

		Profiles.Add(new Profile(){"concert@sea.net", "TU Delft", "Orchestra", "One Direction", "The Hobbit", "2374 AD", "In Relationship", "Fish and Chips", "Platypus", "Paper Round"});

		Profiles.Add(new Profile(){"hpotter@hogwarts.com", "Hogwarts University", "Reading", "K3", "Harry Potter", "6709 KL", "In Love", "Pancakes", "Owl", "Cleaning"});

        Profiles.Add(new Profile(){"sophie43@coolmail.com", "Engelbert College", "Cricket", "Armin van Buren", "Fast and the Furious", "1735 TR", "It's Complicated..", "Fries", "Cat", "Supermarket"});

        Profiles.Add(new Profile(){"iamhere123@home.com", "Harverd University", "Hockey", "The Beatles", "Guardians of the Galaxy", "5008 EP", "Engaged", "Couscous", "Dog", "ICT"});

        Profiles.Add(new Profile(){"bobby@hotmail.com", "Open University", "Tennis", "The Fray", "Saving Private Ryan", "8062 MW", "Distance Relationship", "Chili Con Carne", "Guinea Pig", "Tomato Picker"});

        Profiles.Add(new Profile(){"sophie43@coolmail.com", "LUC The Hague", "Football", "The Rolling Stones", "Finding Nemo", "1735 TR", "Not Interested", "Pasta", "Stick Insects", "Clothing Store"});

        Profiles.Add(new Profile(){"achmed.p@school.com", "ROC Groningen", "Water Polo", "Coldplay", "Star Wars", "5008 EP", "Engaged", "Spaghetti", "Snake", "Bakery"});

        Profiles.Add(new Profile(){"heythere@party.com", "The Rainbow", "Volleybal", "Adele", "Peter Pan", "8062 MW", "Gescheiden", "Kebab", "Mouse", "Restaurant"});

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

		// Set the number of attributes in the game
		int NumberOfAttributes = (int) Math.Max (Math.Floor (Players.Count / 2.0) * 3, 3);

		// Generate a list of all the attributes in random order
		List<ProfileAttribute> AllAttributes = Enum.GetValues(typeof(ProfileAttribute)).Cast<ProfileAttribute>()/*.OrderBy(a => UnityEngine.Random.value)*/.Take(NumberOfAttributes).ToList ();
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
        }

		// Initialize doors
		foreach(GameObject d in GameObject.FindGameObjectsWithTag ("Door")) {
			d.GetComponent<UnlockableDoor>().InitializeDoor(NumberOfAttributes);
		}

		// Start the game
		GameStarted = true;

        Players[0].CmdBroadcastNotification("Game started!");
    }

	public void revealTeamMember (int Team)
	{
        GameObject notification = GameObject.Find("Notification");
        if (Players.Exists(p => p.Team == Team && !p.Revealed))
        {
            PlayerState player = Players.First(p => p.Team == Team && !p.Revealed);
            player.Reveal(Team);
            notification.GetComponent<Notification>().Notify(player.username + " belongs to your team!", 1000);
        }

        if (Players.Exists(p => p.Team != Team && !p.Revealed))
        {
            PlayerState player = Players.First(p => p.Team != Team && !p.Revealed);
            player.Reveal(Team);
            notification.GetComponent<Notification>().Notify(player.username + " belongs to the enemy team!", 1000);
        }
	}
}
