﻿using UnityEngine;

public static class UserDataManager
{
    private const string PROGRESS_KEY = "Progress";

    public static UserProgressData Progress;
    
    /// <summary>
    /// Load user's progrress data
    /// </summary>
    public static void Load()
    {
        // Check if there is PROGRESS_KEY
        // If there isn't, make new data
        if (!PlayerPrefs.HasKey(PROGRESS_KEY))
        {
            Progress = new UserProgressData();
            Save();
        }
        else // If there is, ...
        {
            string json = PlayerPrefs.GetString(PROGRESS_KEY);
            Progress = JsonUtility.FromJson<UserProgressData>(json);
        }
    }
    
    /// <summary>
    /// Save user's progress data
    /// </summary>
    public static void Save()
    {
        string json = JsonUtility.ToJson(Progress);
        PlayerPrefs.SetString(PROGRESS_KEY, json);
    }
}