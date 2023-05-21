using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    [CreateAssetMenu(menuName ="Item Actions/Parry Action")]
    public class ParryAction : ItemAction
    {
        public override void PerformAction(CharacterManager character)
        {
            if (character.isInteracting)
                return;
            character.characterAnimatorManager.EraseHandIKfromWeapon();

            WeaponItem parryingWeapon = character.characterInventoryManager.currentItemBeingUsed as WeaponItem;
            if (parryingWeapon.weaponTypes == WeaponTypes.smallShield)
            {
                character.characterAnimatorManager.PlayTargetAnimation("Parry Fast", true);
            }
            else if(parryingWeapon.weaponTypes == WeaponTypes.Shield)
            {
                character.characterAnimatorManager.PlayTargetAnimation("Parry", true);
            }
        }
    }
}
