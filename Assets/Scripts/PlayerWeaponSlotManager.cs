using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class PlayerWeaponSlotManager : CharacterWeaponSlotManager
    {
   

        PlayerManager player;

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
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
                   // player.playerAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false,true);
                }
                else
                {
                    if (player.inputHandler.twoHandFlag)
                    {
                        backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                        leftHandSlot.UnloadWeaponAndDestroy();
                        player.playerAnimatorManager.PlayTargetAnimation("Left Arm Empty", false, true);
                    }
                    else
                    {
                        backSlot.UnloadWeaponAndDestroy();
                    }
                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamageCollider();
                    player.animator.runtimeAnimatorController = weaponItem.weaponController;
                }
            }
            else
            {
                weaponItem = unarmedWeapon;
                if (isLeft)
                {
                    player.playerInventoryManager.leftWeapon = unarmedWeapon;
                    leftHandSlot.currentWeapon = unarmedWeapon;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamageCollider();
                   // player.playerAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
                }
                else
                {
                    player.playerInventoryManager.rightWeapon = unarmedWeapon;
                    rightHandSlot.currentWeapon = unarmedWeapon;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamageCollider();
                    player.animator.runtimeAnimatorController = weaponItem.weaponController;
                }
            }
        }
    }


}
