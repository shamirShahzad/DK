using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DK
{
    public class CharacterFXManager : MonoBehaviour
    {
        CharacterStatsManager characterStatsManager;
        [Header("Damage FX")]
        public GameObject bloodSplatterFX;
        [Header("Weapon Effects")]
        public WeaponVFX rightWeaponVFX;
        public WeaponVFX leftWeaponVFX;
        [Header("Poison")]
        public bool isPoisned;
        public float poisnBuildup = 0;
        public float poisonAmount = 100;
        public float defaultPoisonAmount;
        public float poisonTimer = 2;
        public int poisonDamage = 1;
        float timer;


        protected virtual void Awake()
        {
            characterStatsManager = GetComponent<CharacterStatsManager>();
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
        }

        protected virtual void HandlePoisonBuildup()
        {
            if (isPoisned)
                return;
            
            if(poisnBuildup >0 && poisnBuildup < 100)
            {
                poisnBuildup = poisnBuildup - 1 * Time.deltaTime;
            }
            else if (poisnBuildup >= 100)
            {
                isPoisned = true;
                poisnBuildup = 0;
            }
        }

        public virtual void HAndleAllBuildupEffects()
        {
            if (characterStatsManager.isDead)
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
                        characterStatsManager.TakePoisonDamage(poisonDamage);
                        timer = 0;
                    }
                    poisonAmount = poisonAmount - 5 * Time.deltaTime;
                }
                else
                {
                    isPoisned = false;
                    poisonAmount = defaultPoisonAmount;
                }
            }
        }
    }
}
