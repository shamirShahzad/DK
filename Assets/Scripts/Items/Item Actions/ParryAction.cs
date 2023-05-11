using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    [CreateAssetMenu(menuName ="Item Actions/Parry Action")]
    public class ParryAction : ItemAction
    {
        public override void PerformAction(PlayerManager player)
        {
            if (player.isInteracting)
                return;
            player.playerAnimatorManager.EraseHandIKfromWeapon();

            WeaponItem parryingWeapon = player.playerInventoryManager.currentItemBeingUsed as WeaponItem;
            if (parryingWeapon.weaponTypes == WeaponTypes.smallShield)
            {
                player.playerAnimatorManager.PlayTargetAnimation("Parry Fast", true);
            }
            else if(parryingWeapon.weaponTypes == WeaponTypes.Shield)
            {
                player.playerAnimatorManager.PlayTargetAnimation("Parry", true);
            }
        }
    }
}
