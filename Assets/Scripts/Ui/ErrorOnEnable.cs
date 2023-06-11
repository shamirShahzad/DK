using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class ErrorOnEnable : MonoBehaviour
    {
        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioClip audioClip;
        
        private void OnEnable()
        {
            audioSource.PlayOneShot(audioClip, 0.5f);
        }
    }
}
