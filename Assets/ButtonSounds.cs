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
            soundVolume = PlayerPrefs.GetFloat("FXVolume" + FirebaseManager.instance.User.DisplayName);
        }
        public void PlaySound(AudioClip clip)
        {
            audioSource.PlayOneShot(clip, soundVolume);
        }

    }
}
