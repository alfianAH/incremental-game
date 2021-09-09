using System;
using UnityEngine;

namespace Audio
{
    public class AudioSourceManager : MonoBehaviour
    {
        private AudioSource audioSource;

        public Sound Sound { get; set; }

        private void OnEnable()
        {
            audioSource = GetComponent<AudioSource>();

            try
            {
                audioSource.clip = Sound.clip;
                audioSource.loop = Sound.loop;
                audioSource.pitch = Sound.pitch;
                audioSource.volume = Sound.volume;

                audioSource.PlayOneShot(Sound.clip);
            }
            catch (NullReferenceException) { }
        }

        private void Update()
        {
            if (audioSource.isPlaying == false)
            {
                gameObject.SetActive(false);
            }
        }
    }
}