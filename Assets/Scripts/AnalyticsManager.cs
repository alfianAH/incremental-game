using Firebase.Analytics;

public class AnalyticsManager
{
    /// <summary>
    /// Event log
    /// </summary>
    /// <param name="eventName">Event name</param>
    /// <param name="parameters"></param>
    private static void LogEvent(string eventName, params Parameter[] parameters)
    {
        FirebaseAnalytics.LogEvent(eventName, parameters);
    }
    
    /// <summary>
    /// Log upgrade resource events
    /// </summary>
    /// <param name="resourceIndex">Resource's index</param>
    /// <param name="level">Resource's level</param>
    public static void LogUpgradeEvent(int resourceIndex, int level)
    {
        // We use Event and Parameter from Firebase
        // so, it can appear as data report in Firebase Analytics
        LogEvent(FirebaseAnalytics.EventLevelUp, 
            new Parameter(FirebaseAnalytics.ParameterIndex, resourceIndex.ToString()), 
            new Parameter(FirebaseAnalytics.ParameterLevel, level));
    }
    
    /// <summary>
    /// Log unlock resource events
    /// </summary>
    /// <param name="resourceIndex">Resource's index</param>
    public static void LogUnlockEvent(int resourceIndex)
    {
        LogEvent(FirebaseAnalytics.EventUnlockAchievement, 
            new Parameter(FirebaseAnalytics.ParameterIndex, resourceIndex.ToString()));
    }

    /// <summary>
    /// Set user's properties
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public static void SetUserProperties(string name, string value)
    {
        FirebaseAnalytics.SetUserProperty(name, value);
    }
}
