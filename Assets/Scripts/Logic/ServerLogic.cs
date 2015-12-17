using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;
using System.Linq;

public class ServerLogic : NetworkBehaviour
{
    // Global properties
    [SyncVar] public bool GameStarted = false;
    private const int nAttributes = 10;
	private List<PlayerState> Players;

    // Game properties
    private List<Profile> Profiles = new List<Profile>();
    private List<KeyValuePair<int, int>> unassignedAttributes = new List<KeyValuePair<int, int>>();

	public List<string> Categories = new List<string> {
		"Email",
		"School",
		"Hobby",
		"Muziek",
		"Film",
		"Postcode",
		"Relatie",
		"Eten",
		"Hond",
		"Restaurant"
	};

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

        // Generate random pairs of <int, int> that define which attributes should be contained in the game.
        // The KeyValue Pair represents the collumn and row of the matrix above.
//        for (int iAttributes = 0; iAttributes < nAttributes; iAttributes++)
//            unassignedAttributes.Add(new KeyValuePair<int, int>(iAttributes, rnd.Next(0, profiles.Count-1)));
    }

//	public void Start() {
//		NetMgr = GameObject.Find ("Network Manager").GetComponent<NetworkManager> ();
//	}

    // Dynamically assigns a Player certain data.
    public void RegisterPlayer(PlayerState player)
    {
		Players.Add (player);
        // Pseudorandomnumber generator
//        System.Random rnd = new System.Random();

        // Add a Tag3D to the player
        // [Todo] Add the player name that is entered by the user itself.
//        player.gameObject.AddComponent<Tag3D>();

        // Fill in the profile of the new player
//		Profile profile = new Profile ();
//        for (uint i = 0; i < 3; i++)
//        {
//            KeyValuePair<int, int> pair;
//            if (unassignedAttributes.Count == 0)
//                pair = new KeyValuePair<int, int>(rnd.Next(0, nAttributes), rnd.Next(0, profiles.Count-1));
//            else
//                pair = unassignedAttributes[rnd.Next(0, unassignedAttributes.Count-1)];
//            
//            // Set the field of the Profile.
//            profile.SetField(pair.Key, profiles[pair.Value].GetField(pair.Key));
//        }

        // Assign a profile to the player.
//        player.Profile = profile;
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
		// Assign a profile to every player
		int AttributesPerPlayer = (int) Math.Max (3, Math.Ceiling ((double) Profile.TotalNumberOfAttributes() / (double) Players.Count));
		// Generate a list of all the attributes in random order
		List<ProfileAttribute> AllAttributes = Enum.GetValues(typeof(ProfileAttribute)).Cast<ProfileAttribute>().OrderBy(a => UnityEngine.Random.value).ToList ();
		List<ProfileAttribute> AttributesNotYetInGame = new List<ProfileAttribute> (AllAttributes);

		for (int i = 0; i < Players.Count; i++) {

			Players[i].ProfileIndex = i;

			for(int j = 0; j < AttributesPerPlayer; j++) {
				if(AttributesNotYetInGame.Any ()) {
					// Add an attribute to the player
					Players[i].SelectedAttributes.Add ((int) AttributesNotYetInGame[0]);
					// Delete it from the list of attributes that are not yet in the game
					AttributesNotYetInGame.RemoveAt(0);
				} else {
					// Add a random attribute that that player does not already have
					Players[i].SelectedAttributes.Add ((int) AllAttributes.Find(a => !Players[i].SelectedAttributes.Contains((int) a)));
				}
			}

			Players[i].Team = i % 2;
		}

		// Start the game
		GameStarted = true;
	}
}
