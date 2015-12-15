using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

public class ServerLogic : NetworkBehaviour
{
    // Global properties
    [SyncVar] public bool gameStarted;
    private const int nAttributes = 10;

    // Game properties
    private List<Profile> profiles = new List<Profile>();
    private List<KeyValuePair<int, int>> unassignedAttributes = new List<KeyValuePair<int, int>>();

    public void Awake()
    {
        // Pseudorandomnumber generator
        System.Random rnd = new System.Random();

        // Make sure players cannot move when they connect.
        gameStarted = false;

        // Initialize all the profiles.
        profiles.Add(new Profile("devries@smart.nl", "Stanislas College", "Gitaar Spelen", "B-Brave", "James Bond", "4567 GB", "Single", "Pizza", "Bobby", "Snackbar"));
        profiles.Add(new Profile("jongen34@school.nl", "Stedelijk Gymnasium", "Paard rijden", "Justin Bieber", "Star Wars", "7832 FD", "Getrouwd", "Nasi Goreng", "Rataplan", "Supermarkt"));
        profiles.Add(new Profile("concert@sea.net", "TU Delft", "Majorette", "OneDirection", "The Hobbit", "2374 AD", "In een relatie", "Hutspot", "Loebas", "Krantenwijk"));
        profiles.Add(new Profile("kees90@coolmail.com", "Hogeschool Inholland", "Boeken lezen", "K3", "SpangaS in Actie", "6709 KL", "Uit elkaar", "Pannenkoeken", "Bello", "Gras maaien"));
        profiles.Add(new Profile("sophie43@coolmail.com", "Engelbertcollege", "Voetballen", "Armin van Buren", "Finding Nemo", "1735 TR", "Ingewikkeld", "Patat", "Dexter", "Kledingwinkel"));
        profiles.Add(new Profile("ikbenhier123@home.nl", "De Regenboogschool", "Hockey", "Coldplay", "Pirates of the Caribbean", "5008 EP", "Verloofd", "Couscous", "Idefix", "Bakkerij"));
        profiles.Add(new Profile("hallodaar!@party.com", "CBS De Acker", "Tennis", "Adele", "Peter Pan", "8062 MW", "Gescheiden", "Chili con carne", "Nero", "Restaurant"));

        // Generate random pairs of <int, int> that define which attributes should be contained in the game.
        // The KeyValue Pair represents the collumn and row of the matrix above.
        for (int iAttributes = 0; iAttributes < nAttributes; iAttributes++)
            unassignedAttributes.Add(new KeyValuePair<int, int>(iAttributes, rnd.Next(0, profiles.Count-1)));
    }

    // Dynamically assigns a Player certain data.
    public void InitializePlayer(PlayerState player)
    {
        // Pseudorandomnumber generator
        System.Random rnd = new System.Random();

        // Add a Tag3D to the player
        // [Todo] Add the player name that is entered by the user itself.
        player.gameObject.AddComponent<Tag3D>();

        // Fill in the profile of the new player
        Profile profile = new Profile("_", "_", "_", "_", "_", "_", "_", "_", "_", "_");
        for (uint i = 0; i < 3; i++)
        {
            KeyValuePair<int, int> pair;
            if (unassignedAttributes.Count == 0)
                pair = new KeyValuePair<int, int>(rnd.Next(0, nAttributes), rnd.Next(0, profiles.Count-1));
            else
                pair = unassignedAttributes[rnd.Next(0, unassignedAttributes.Count-1)];
            
            // Set the field of the Profile.
            profile.SetField(pair.Key, profiles[pair.Value].GetField(pair.Key));
        }

        // Assign a profile to the player.
        player.profile = profile;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (isServer && Input.GetKeyDown(KeyCode.Return) && !gameStarted)
            gameStarted = true;
	}
}
