using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class CharacterSFXManager : MonoBehaviour
    {
        CharacterManager character;
        public AudioSource audioSource;
        float soundVolume = 0.4f;
        
        [Header("Taking Damaage Sounds")]
        public AudioClip[] takingDamageSounds;
        private int lastGoreSound = -1;
        private int lastSwooshSound = -1;

        [Header("Footstep Sound")]
        private int lastFootstepSound;
        public AudioClip[] footsteps;
        int randomFootstep;

        [Header("BackStabORRiposte")]
        public AudioClip backStabOrRiposte;


        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            character = GetComponent<CharacterManager>();
            soundVolume = PlayerPrefs.GetFloat("FXVolume" + FirebaseManager.instance.User.DisplayName);
        }

        public virtual void PlayRandomDamageSoundEffect()
        {
            
            int randomValue = Random.Range(0, takingDamageSounds.Length);
            if(randomValue == lastGoreSound)
            {
                PlayRandomDamageSoundEffect();
            }
            else
            {
                audioSource.PlayOneShot(takingDamageSounds[randomValue],soundVolume);
                lastGoreSound = randomValue;
            }
        }

        public virtual void PlayRandomFootstepSound()
        {
            int randomValue = Random.Range(0, 12);
            if(character.animator.GetFloat("Vertical") >0.1 && character.animator.GetFloat("Vertical") <=0.5 || character.animator.GetFloat("Vertical") <-0.1 && character.animator.GetFloat("Vertical") >=-0.5)
            audioSource.PlayOneShot(footsteps[randomValue],soundVolume);
            if(character.animator.GetFloat("Horizontal") < -0.1 && character.animator.GetFloat("Horizontal") >=-0.5 || character.animator.GetFloat("Horizontal") > 0.1 && character.animator.GetFloat("Horizontal") <=0.5)
                audioSource.PlayOneShot(footsteps[randomValue], soundVolume);
        }
        public virtual void PlayRandomFootstepRunningSound()
        {
            int randomValue = Random.Range(0, 12);
            if (character.animator.GetFloat("Vertical") > 0.6 || character.animator.GetFloat("Vertical") < -0.5)
                audioSource.PlayOneShot(footsteps[randomValue], soundVolume);
            if (character.animator.GetFloat("Horizontal") < -0.5||character.animator.GetFloat("Horizontal")>0.5)
                audioSource.PlayOneShot(footsteps[randomValue], soundVolume);
        }
        public virtual void PlayRandomFootstepSprintingSound()
        {
            int randomValue = Random.Range(0, 12);
            if(character.animator.GetFloat("Vertical") >1 && character.animator.GetFloat("Vertical") <=2 )
                audioSource.PlayOneShot(footsteps[randomValue], soundVolume+0.15f);
        }


        public virtual void PlayRandomWeaponSwoosh()
        {
            int randomValue;
            if (character.isUsingRightHand)
            {
                randomValue = Random.Range(0, character.characterInventoryManager.rightWeapon.weaponWooshes.Length);
                if(randomValue == lastSwooshSound)
                {
                    PlayRandomWeaponSwoosh();
                }
                else
                {
                    lastSwooshSound = randomValue;
                    audioSource.PlayOneShot(character.characterInventoryManager.rightWeapon.weaponWooshes[randomValue], soundVolume);
                }
            }
            else if (character.isUsingLeftHand)
            {
                randomValue = Random.Range(0, character.characterInventoryManager.leftWeapon.weaponWooshes.Length);
                if (randomValue == lastSwooshSound)
                {
                    PlayRandomWeaponSwoosh();
                }
                else
                {
                    lastSwooshSound = randomValue;
                    audioSource.PlayOneShot(character.characterInventoryManager.leftWeapon.weaponWooshes[randomValue], soundVolume);
                }
            }
        }


    }
}
