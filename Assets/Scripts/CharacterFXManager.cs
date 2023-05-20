using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DK
{
    public class CharacterFXManager : MonoBehaviour
    {
        [Header("Current Range FX")]
        public GameObject currentRangeFX;

        CharacterManager character;
        [Header("Damage FX")]
        public GameObject bloodSplatterFX;
        [Header("Weapon Effects")]
        public WeaponVFX rightWeaponVFX;
        public WeaponVFX leftWeaponVFX;
        [Header("Poison")]
        public GameObject defaultPoisonParticleFX;
        public  GameObject currentPoisonParticleFX;
        public Transform buildupTransform;
        public bool isPoisned;
        public float poisonBuildup = 0;
        public float poisonAmount = 100;
        public float defaultPoisonAmount = 100;
        public float poisonTimer = 2;
        public int poisonDamage = 1;
        float timer;


        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }
        public virtual void PlayWeaponFX(bool isLeft)
        {
            if(isLeft == false)
            {
                if (rightWeaponVFX != null)
                {
                    rightWeaponVFX.PlayWeaponFX();
                }
            }
            else
            {
                if (leftWeaponVFX != null)
                {
                    leftWeaponVFX.PlayWeaponFX();
                }
            }
        }

        public virtual void PlayBloodSplatterEffect(Vector3 bloodSplatterLocation)
        {
            GameObject blood = Instantiate(bloodSplatterFX, bloodSplatterLocation,Quaternion.identity);
            Destroy(blood, 2.5f);
        }

        protected virtual void HandlePoisonBuildup()
        {
            if (isPoisned)
                return;
            
            if(poisonBuildup >0 && poisonBuildup < 100)
            {
                poisonBuildup = poisonBuildup - 1 * Time.deltaTime;
            }
            else if (poisonBuildup >= 100)
            {
                isPoisned = true;
                poisonBuildup = 0;
                if(buildupTransform != null)
                {
                    currentPoisonParticleFX = Instantiate(defaultPoisonParticleFX, buildupTransform.transform);
                }
                else
                {
                    currentPoisonParticleFX = Instantiate(defaultPoisonParticleFX, character.transform);
                }
            }
        }

        public virtual void HAndleAllBuildupEffects()
        {
            if (character.isDead)
                return;
            HandlePoisonBuildup();
            HandleIsPoisnedEffect();
        }

        protected virtual void HandleIsPoisnedEffect()
        {
            if (isPoisned)
            {
                if(poisonAmount > 0)
                {
                    timer += Time.deltaTime;
                    if(timer >= poisonTimer)
                    {
                        character.characterStatsManager.TakePoisonDamage(poisonDamage);
                        timer = 0;
                    }
                    poisonAmount = poisonAmount - 5 * Time.deltaTime;
                }
                else
                {
                    isPoisned = false;
                    poisonAmount = defaultPoisonAmount;
                    Destroy(currentPoisonParticleFX);
                }
            }
        }
    }
}
