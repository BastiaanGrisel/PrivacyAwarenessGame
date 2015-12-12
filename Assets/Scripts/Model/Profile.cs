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
}
