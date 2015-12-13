using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

public class ServerLogic : NetworkBehaviour
{
    // Global properties
    [SyncVar] public bool gameStarted;

    // [Refactor] Game properties
    private List<int> keys = new List<int>();
    private List<int> unusedKeys = new List<int>();

    // [Updated] Game properties
    private List<Profile> profiles = new List<Profile>();

    public void Awake()
    {
        gameStarted = false;

        // [Refactor] Add keys;
        keys.Add(1);
        keys.Add(2);
        keys.Add(3);

        // Initialize all the profiles.
        profiles.Add(new Profile("devries@smart.nl", "Stanislas College", "Gitaar Spelen", "B-Brave", "James Bond", "4567 GB", "Single", "Pizza", "Bobby", "Snackbar"));
        profiles.Add(new Profile("jongen34@school.nl", "Stedelijk Gymnasium", "Paard rijden", "Justin Bieber", "Star Wars", "7832 FD", "Getrouwd", "Nasi Goreng", "Rataplan", "Supermarkt"));
        profiles.Add(new Profile("concert@sea.net", "TU Delft   ", "Majorette", "OneDirection", "The Hobbit", "2374 AD", "In een relatie", "Hutspot", "Loebas", "Krantenwijk"));
        profiles.Add(new Profile("kees90@coolmail.com", "Hogeschool Inholland", "Boeken lezen", "K3", "SpangaS in Actie", "6709 KL", "Uit elkaar", "Pannenkoeken", "Bello", "Gras maaien"));
        profiles.Add(new Profile("sophie43@coolmail.com", "Engelbertcollege", "Voetballen", "Armin van Buren", "Finding Nemo", "1735 TR", "Ingewikkeld", "Patat", "Dexter", "Kledingwinkel"));
        profiles.Add(new Profile("ikbenhier123@home.nl", "De Regenboogschool", "Hockey", "Coldplay", "Pirates of the Caribbean", "5008 EP", "Verloofd", "Couscous", "Idefix", "Bakkerij"));
        profiles.Add(new Profile("hallodaar!@party.com", "De Regenboogschool", "Tennis", "Adele", "Peter Pan", "8062 MW", "Gescheiden", "Chili con carne", "Nero", "Restaurant"));
    }

    // Dynamically assigns a Player certain data.
    public void InitializePlayer(GameObject player)
    {
        // [Refactor] Assign the Keys to the player.
        System.Random rnd = new System.Random();

        for (int i = 0; i < 3; i++)
        {
            if (unusedKeys.Count > 0)
            {
                int index = rnd.Next(0, unusedKeys.Count - 1);
                player.GetComponent<PlayerState>().keys.Add(unusedKeys[index]);
                unusedKeys.RemoveAt(index);
            }
            else
                player.GetComponent<PlayerState>().keys.Add(keys[rnd.Next(0, keys.Count - 1)]);
        }

        // Assign a profile to the player.
        // [TODO]: Edit code to take action when the profiles are exhausted.
        player.GetComponent<PlayerState>().SetPlayerProfile(profiles[0]);
        profiles.RemoveAt(0);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (isServer && Input.GetKeyDown(KeyCode.Return) && !gameStarted)
            gameStarted = true;
	}
}
