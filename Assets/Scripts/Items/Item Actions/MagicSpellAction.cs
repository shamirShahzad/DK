using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    [CreateAssetMenu(menuName = "Item Actions/Magic Spell Action")]
    public class MagicSpellAction : ItemAction
    {
        public override void PerformAction(PlayerManager player)
        {
            if (player.isInteracting)
                return;
            if (player.playerInventoryManager.currentSpell != null && player.playerInventoryManager.currentSpell.isMagicSpell)
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
