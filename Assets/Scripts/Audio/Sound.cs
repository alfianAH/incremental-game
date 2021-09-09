﻿﻿using UnityEngine;

namespace Audio
{
    public enum ListSound
    {
        CollectCoin,
        AchievementUnlocked,
        ResourcesUnlocked,
        ResourcesLevelUp,
        ResourcesError
    }
    
    [System.Serializable]
    public class Sound
    {
        public ListSound listSound;
        
        public AudioClip clip;

        [Range(0, 1)]
        public float volume = 1;
        [Range(-3, 3)]
        public float pitch = 1;

        public bool loop;
    }
}