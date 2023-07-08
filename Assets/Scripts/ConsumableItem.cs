using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class ConsumableItem : Item
    {
        [Header("Item quantity")]
        public int maxItemAmount;
        public int currentItemAmount;
        [Header("Item Model")]
        public GameObject itemModel;
        [Header("Animations")]
        public string consumableAnimation;
        public bool isInteracing;


        public virtual void AttemptToConsumeItems(PlayerAnimatorManager playerAnimatorManager,
            PlayerWeaponSlotManager weaponSlotManager,
            PlayerFXManager playerFXManager)
        {
            if(currentItemAmount > 0)
            {
                playerAnimatorManager.PlayTargetAnimation(consumableAnimation, isInteracing,true);
                playerFXManager.toBeInstantiated = true;
                playerFXManager.isDrinking = true;
                currentItemAmount--;
            }
            else
            {
                playerAnimatorManager.PlayTargetAnimation("Failed Cast", true);
                playerFXManager.toBeInstantiated = false;
            }
        }

    }
}
