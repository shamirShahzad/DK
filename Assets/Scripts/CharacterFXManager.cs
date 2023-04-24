using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DK
{
    public class CharacterFXManager : MonoBehaviour
    {
        [Header("Damage FX")]
        public GameObject bloodSplatterFX;
        [Header("Weapon Effects")]
        public WeaponVFX rightWeaponVFX;
        public WeaponVFX leftWeaponVFX;

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
    }
}
