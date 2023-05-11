using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    [CreateAssetMenu(menuName = "Item Actions/Miracle Item action")]
    public class MiracleSpellAction : ItemAction
    {
        public override void PerformAction(PlayerManager player)
        {
            if (player.isInteracting)
                return;
            WeaponItem weapon = player.playerInventoryManager.currentItemBeingUsed as WeaponItem;
            if (weapon.spellOfItem != null)
            {
                player.playerInventoryManager.currentSpell = weapon.spellOfItem;
            }
            if (player.playerInventoryManager.currentSpell != null && player.playerInventoryManager.currentSpell.isFaithSpell)
            {
                if (player.playerStatsManager.currentFocus >= player.playerInventoryManager.currentSpell.focusPointCost)
                {
                    player.playerInventoryManager.currentSpell.AttemptToCastSpell(player.playerAnimatorManager,
                        player.playerStatsManager,
                        player.playerWeaponSlotManager,
                        player.isUsingLeftHand,
                        player.cameraHandler);
                }
                else
                {
                    player.playerAnimatorManager.PlayTargetAnimation("Failed Cast", true);
                }

            }
        }
    }
}
