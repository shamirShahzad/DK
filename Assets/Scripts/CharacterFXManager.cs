using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DK
{
    public class CharacterFXManager : MonoBehaviour
    {
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
    }
}
