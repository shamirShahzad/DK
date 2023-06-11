using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class ButtonSounds : MonoBehaviour
    {
        [SerializeField] AudioSource audioSource;
        float soundVolume;


        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            
        }
        public void PlaySound(AudioClip clip)
        {
            soundVolume = PlayerPrefs.GetFloat("FXVolume" + FirebaseManager.instance.User.DisplayName);
            audioSource.PlayOneShot(clip, soundVolume);
        }
        public void PlaySoundFullVolume(AudioClip clip)
        {
            audioSource.PlayOneShot(clip, 1);
        }

    }
}
