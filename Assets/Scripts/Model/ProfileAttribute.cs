using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum ProfileAttribute: int { Email, School, Hobby, FavoriteArtist, FavoriteMovie, ZipCode, SocialStatus, FavoriteFood, Pet, Job }

public class Profile : List<string>
{
	public Profile() {
//		for(int i = 0; i < TotalNumberOfAttributes(); i++)
//			this.Add (null);
	}
	
	public static int TotalNumberOfAttributes() {
		return Enum.GetNames (typeof(ProfileAttribute)).Length;
	}
	
//	public List<string> Attributes;
//
//	public string Email, School, Hobby, FavoriteArtist, FavoriteMovie, ZipCode, SocialStatus, FavoriteFood, Pet, Job;
//
//    public Profile(string email, string school, string hobby, string favoriteArtist, string favoriteMovie, string zipcode, string socialStatus, string favoriteFood, string pet, string job)
//    {
//        this.Email = email;
//        this.School = school;
//        this.Hobby = hobby;
//        this.FavoriteArtist = favoriteArtist;
//        this.FavoriteMovie = favoriteMovie;
//        this.ZipCode = zipcode;
//        this.SocialStatus = socialStatus;
//        this.FavoriteFood = favoriteFood;
//        this.Pet = pet;
//        this.Job = job;
//    }
//
//    // TODO: Fix these classes.
//    public void SetField(int field, string value)
//    {
//        switch (field)
//        {
//            case 0:
//                Email = value;
//                return;
//            case 1:
//                School = value;
//                return;
//            case 2:
//                Hobby = value;
//                return;
//            case 3:
//                FavoriteArtist = value;
//                return;
//            case 4:
//                FavoriteMovie = value;
//                return;
//            case 5:
//                ZipCode = value;
//                return;
//            case 6:
//                SocialStatus = value;
//                return;
//            case 7:
//                FavoriteFood = value;
//                return;
//            case 8:
//                Pet = value;
//                return;
//            case 9:
//                Job = value;
//                return;
//        }
//    }
//
//    public string GetField(int field)
//    {
//        switch (field)
//        {
//            case 0: return Email;
//            case 1: return School;
//            case 2: return Hobby;
//            case 3: return FavoriteArtist;
//            case 4: return FavoriteMovie;
//            case 5: return ZipCode;
//            case 6: return SocialStatus;
//            case 7: return FavoriteFood;
//            case 8: return Pet;
//            case 9: return Job;
//            default: return "Error";
//        }
//    }
}
