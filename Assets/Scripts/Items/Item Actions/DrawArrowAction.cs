using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    [CreateAssetMenu(menuName ="Item Actions/Draw Arrow Action")]
    public class DrawArrowAction : ItemAction
    {
        public override void PerformAction(PlayerManager player)
        {
            if (player.isInteracting)
                return;
            if (player.isHoldingArrow)
                return;
            player.playerAnimatorManager.animator.SetBool("isHoldingArrow", true);
            player.playerAnimatorManager.PlayTargetAnimation("Bow_TH_Draw_01_R", true);
            GameObject loadedArrow = Instantiate(player.playerInventoryManager.currentAmmo.loadedItemModel, player.playerWeaponSlotManager.leftHandSlot.transform);
            player.playerFXManager.currentRangeFX = loadedArrow;
            Animator bowAnimator = player.playerWeaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();
            bowAnimator.SetBool("isDrawn", true);
            bowAnimator.Play("Bow_TH_Draw_01");
            //activate aim button
            
        }
    }
}
