using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum ProfileAttribute: int { Email, School, Hobby, FavoriteArtist, FavoriteMovie, ZipCode, SocialStatus, FavoriteFood, Pet, Job }

// Add some extra methods to ProfileAttribute
public static class ProfileAttributeExt 
{
	public static string ToFriendlyString(this ProfileAttribute attr)
	{
		switch (attr) 
		{
			case ProfileAttribute.Email: 			return "Email";
			case ProfileAttribute.School: 			return "School";
			case ProfileAttribute.Hobby: 			return "Hobby";
			case ProfileAttribute.FavoriteArtist: 	return "Artist";
			case ProfileAttribute.FavoriteMovie: 	return "Movie";
			case ProfileAttribute.ZipCode: 			return "Zip Code";
			case ProfileAttribute.SocialStatus:		return "Social Status";
			case ProfileAttribute.FavoriteFood: 	return "Food";
			case ProfileAttribute.Pet: 				return "Pet";
			case ProfileAttribute.Job: 				return "Job";
			default: return "Unknown";
		}
	}
}

public class Profile : List<string>
{	
	public static int NumberOfAttributes = Enum.GetNames (typeof(ProfileAttribute)).Length;
}