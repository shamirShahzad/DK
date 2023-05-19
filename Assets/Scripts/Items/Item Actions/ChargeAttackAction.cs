using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DK
{
    [CreateAssetMenu(menuName = "Item Actions/Charge Attack Action")]
    public class ChargeAttackAction : ItemAction
    {
        public override void PerformAction(PlayerManager player)
        {
            if (player.playerStatsManager.currentStamina <= 0)
            {
                return;
            }
            player.playerAnimatorManager.EraseHandIKfromWeapon();
            player.playerFXManager.PlayWeaponFX(false);
                if (player.isInteracting)
                    return;
                if (player.canDoCombo)
                    return;

                HandleChargeAttack(player);
            

        }
        private void HandleChargeAttack(PlayerManager player)
        {
            if (player.isUsingLeftHand)
            {
                player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.OH_Charge_Attack_01, true, false, true);
                player.playerCombatManager.lastAttack = player.playerCombatManager.OH_Charge_Attack_01;
                player.playerFXManager.PlayWeaponFX(false);
            }
            else if (player.isUsingRightHand)
            {
                if (player.inputHandler.twoHandFlag)
                {
                    player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.TH_Charge_Attack_01, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.TH_Charge_Attack_01;
                    player.playerFXManager.PlayWeaponFX(false);
                }
                else
                {
                    player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.OH_Charge_Attack_01, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.OH_Charge_Attack_01;
                    player.playerFXManager.PlayWeaponFX(false);
                }
            }


        }

    }
}
