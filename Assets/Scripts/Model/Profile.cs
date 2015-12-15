using UnityEngine;
using System.Collections;

public class Profile
{
    public string email;
    public string school;
    public string hobby;
    public string favoriteArtist;
    public string favoriteMovie;
    public string zipcode;
    public string socialStatus;
    public string favoriteFood;
    public string pet;
    public string job;

    public Profile(string email, string school, string hobby, string favoriteArtist, string favoriteMovie, string zipcode, string socialStatus, string favoriteFood, string pet, string job)
    {
        this.email = email;
        this.school = school;
        this.hobby = hobby;
        this.favoriteArtist = favoriteArtist;
        this.favoriteMovie = favoriteMovie;
        this.zipcode = zipcode;
        this.socialStatus = socialStatus;
        this.favoriteFood = favoriteFood;
        this.pet = pet;
        this.job = job;
    }

    // TODO: Fix these classes.
    public void SetField(int field, string value)
    {
        switch (field)
        {
            case 0:
                email = value;
                return;
            case 1:
                school = value;
                return;
            case 2:
                hobby = value;
                return;
            case 3:
                favoriteArtist = value;
                return;
            case 4:
                favoriteMovie = value;
                return;
            case 5:
                zipcode = value;
                return;
            case 6:
                socialStatus = value;
                return;
            case 7:
                favoriteFood = value;
                return;
            case 8:
                pet = value;
                return;
            case 9:
                job = value;
                return;
        }
    }

    public string GetField(int field)
    {
        switch (field)
        {
            case 0: return email;
            case 1: return school;
            case 2: return hobby;
            case 3: return favoriteArtist;
            case 4: return favoriteMovie;
            case 5: return zipcode;
            case 6: return socialStatus;
            case 7: return favoriteFood;
            case 8: return pet;
            case 9: return job;
            default: return "Error";
        }
    }
}
