using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class PlayerWeaponSlotManager : CharacterWeaponSlotManager
    {
   
        inputHandler inputHandler;
        PlayerManager playerManager;
        PlayerInventoryManager playerInventoryManager;
        PlayerAnimatorManager playerAnimatorManager;
        PlayerStatsManager playerStatsManager;
        PlayerFXManager playerFXManager;
        protected override void Awake()
        {
            base.Awake();
            playerManager = GetComponent<PlayerManager>();
            inputHandler = GetComponent<inputHandler>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerFXManager = GetComponent<PlayerFXManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            LoadWeaponHolderSlots();

        }

        public override void LoadWeaponOnSlot(WeaponItem weaponItem,bool isLeft)
        {
            if (weaponItem != null)
            {

                if (isLeft)
                {
                    leftHandSlot.currentWeapon = weaponItem;

                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamageCollider();
                    playerAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false,true);
                }
                else
                {
                    if (inputHandler.twoHandFlag)
                    {
                        backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                        leftHandSlot.UnloadWeaponAndDestroy();
                        playerAnimatorManager.PlayTargetAnimation("Left Arm Empty", false, true);
                    }
                    else
                    {
                        backSlot.UnloadWeaponAndDestroy();
                    }
                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamageCollider();
                    playerAnimatorManager.animator.runtimeAnimatorController = weaponItem.weaponController;
                }
            }
            else
            {
                weaponItem = unarmedWeapon;
                if (isLeft)
                {
                    playerInventoryManager.leftWeapon = unarmedWeapon;
                    leftHandSlot.currentWeapon = unarmedWeapon;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamageCollider();
                    playerAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
                }
                else
                {
                    playerInventoryManager.rightWeapon = unarmedWeapon;
                    rightHandSlot.currentWeapon = unarmedWeapon;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamageCollider();
                    playerAnimatorManager.animator.runtimeAnimatorController = weaponItem.weaponController;
                }
            }
        }
        public void DrainStaminaLightAttack()
        {
            playerStatsManager.TakeStaminaDamage(Mathf.RoundToInt(attackingItem.baseStaminaCost * attackingItem.lightAttackMultiplier));
        }
        public void DrainStaminaHeavyAttack()
        {
            playerStatsManager.TakeStaminaDamage(Mathf.RoundToInt(attackingItem.baseStaminaCost * attackingItem.heavyAttackMultiplier));
        }





    }


}
