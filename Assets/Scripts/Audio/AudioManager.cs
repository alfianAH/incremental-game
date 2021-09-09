using System;
using System.Collections.Generic;
using UnityConfig;
using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        #region Singleton

        private static AudioManager instance;
        private const string LOG = nameof(AudioManager);

        public static AudioManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<AudioManager>();

                    if (instance == null)
                    {
                        Debug.LogError($"{LOG} not found");
                    }
                }

                return instance;
            }
        }


        #endregion
        
        #region Don't Destroy on Load
        
        /// <summary>
        /// Set instance and don't destroy on load
        /// </summary>
        private void SetInstance()
        {
            if (instance ==  null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            
            DontDestroyOnLoad(gameObject);
        }

        #endregion
        
        [ArrayElementTitle("listSound")]
        public Sound[] sounds;

        [SerializeField] private AudioSourceManager audioSourcePrefab;

        private readonly List<AudioSourceManager> audioSourcePool = new List<AudioSourceManager>();

        private void Awake()
        {
            SetInstance();
        }

        #region Audio

        /// <summary>
        /// Play audio
        /// </summary>
        /// <param name="listSound"></param>
        public void Play(ListSound listSound)
        {
            AudioSourceManager audioSourceManager = GetOrCreateAudioSource();

            Sound s = Array.Find(sounds, sound => sound.listSound == listSound);

            if (s == null)
            {
                Debug.LogError($"Audio {listSound} null");
                return;
            }

            audioSourceManager.Sound = s;
            audioSourceManager.gameObject.SetActive(true);
        }

        /// <summary>
        /// Get unused audio source in hierarchy
        /// </summary>
        /// <returns></returns>
        private AudioSourceManager GetOrCreateAudioSource()
        {
            AudioSourceManager audioSource = 
                audioSourcePool.Find(a => !a.gameObject.activeSelf);

            if (audioSource == null)
            {
                audioSource = Instantiate(audioSourcePrefab, transform);
                audioSourcePool.Add(audioSource);
            }
            
            audioSource.gameObject.SetActive(false);

            return audioSource;
        }
        
        #endregion
    }
}
