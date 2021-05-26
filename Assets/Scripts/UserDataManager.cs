using System;
using System.Collections;
using System.Text;
using Firebase.Storage;
using UnityEngine;

public static class UserDataManager
{
    private const string PROGRESS_KEY = "Progress";

    public static UserProgressData progress;
    
    /// <summary>
    /// Load user's progrress data
    /// </summary>
    public static void LoadFromLocal()
    {
        // Check if there is PROGRESS_KEY
        // If there isn't, make new data
        if (!PlayerPrefs.HasKey(PROGRESS_KEY))
        {
            progress = new UserProgressData();
            Save(true);
        }
        else // If there is, ...
        {
            string json = PlayerPrefs.GetString(PROGRESS_KEY);
            
            progress = JsonUtility.FromJson<UserProgressData>(json);
        }
    }
    
    /// <summary>
    /// Load data from cloud
    /// </summary>
    /// <param name="onComplete"></param>
    /// <returns></returns>
    public static IEnumerator LoadFromCloud(Action onComplete)
    {
        StorageReference targetStorage = GetTargetCloudStorage();

        bool isCompleted = false;
        bool isSuccessful = false;
        const long maxAllowedSize = 1024 * 1024; // Equal to 1 MB
        targetStorage.GetBytesAsync(maxAllowedSize).ContinueWith(task =>
        {
            if (!task.IsFaulted)
            {
                string json = Encoding.Default.GetString(task.Result);
                progress = JsonUtility.FromJson<UserProgressData>(json);
                isSuccessful = true;
                isCompleted = true;
            }
        });

        while (!isCompleted)
        {
            yield return null;
        }
        
        // If succesfully download, save the downloaded data
        if (isSuccessful)
        {
            Save();
        }
        else
        {
            // If there isn't data in cloud, load the data from local
            LoadFromLocal();
        }
        
        onComplete?.Invoke();
    }
    
    /// <summary>
    /// Get target cloud storage
    /// </summary>
    /// <returns></returns>
    private static StorageReference GetTargetCloudStorage()
    {
        // Use device ID as file's name which will be saved in cloud
        string deviceId = SystemInfo.deviceUniqueIdentifier;
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;

        return storage.GetReferenceFromUrl($"{storage.RootReference}/{deviceId}");
    }
    
    /// <summary>
    /// Save user's progress data
    /// </summary>
    public static void Save(bool uploadToCloud = false)
    {
        string json = JsonUtility.ToJson(progress);
        PlayerPrefs.SetString(PROGRESS_KEY, json);

        if (uploadToCloud)
        {
            AnalyticsManager.SetUserProperties("gold",progress.gold.ToString());
            
            byte[] data = Encoding.Default.GetBytes(json);
            StorageReference targetStorage = GetTargetCloudStorage();

            targetStorage.PutBytesAsync(data);
        }
    }
    
    /// <summary>
    /// Check if resources has 
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public static bool HasResources(int index)
    {
        return index + 1 <= progress.resourcesLevel.Count;
    }
}