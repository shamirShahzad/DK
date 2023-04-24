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

        Animator animator;
        public WeaponItem attackingItem;
        PlayerStatsManager playerStatsManager;
        PlayerFXManager playerFXManager;
        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
            animator = GetComponent<Animator>();
            inputHandler = GetComponent<inputHandler>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerFXManager = GetComponent<PlayerFXManager>();
            LoadWeaponHolderSlots();

        }

        private void LoadWeaponHolderSlots()
        {
            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
            foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
            {
                if (weaponSlot.isLeftHandSlot)
                {
                    leftHandSlot = weaponSlot;
                }
                else if (weaponSlot.isRightHandSlot)
                {
                    rightHandSlot = weaponSlot;
                }
                else if (weaponSlot.isBackSlot)
                {
                    backSlot = weaponSlot;
                }
            }
        }

        public void LoadBothWeaponOnslot()
        {
            LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
            LoadWeaponOnSlot(playerInventoryManager.leftWeapon, true);
        }
        public void LoadWeaponOnSlot(WeaponItem weaponItem,bool isLeft)
        {
            if (weaponItem != null)
            {

                if (isLeft)
                {
                    leftHandSlot.currentWeapon = weaponItem;

                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamageCollider();
                    animator.CrossFade(weaponItem.Left_Hand_Idle, 0.2f);
                }
                else
                {
                    if (inputHandler.twoHandFlag)
                    {
                        backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                        leftHandSlot.UnloadWeaponAndDestroy();
                        animator.CrossFade(weaponItem.th_idle, 0.2f);
                    }
                    else
                    {


                        animator.CrossFade("Both Arms Empty", 0.2f);
                        backSlot.UnloadWeaponAndDestroy();
                        animator.CrossFade(weaponItem.Right_Hand_Idle, 0.2f);
                    }
                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamageCollider();
                }
            }
            else
            {
                weaponItem = unarmedWeapon;
                if (isLeft)
                {
                    animator.CrossFade("Left Arm Empty", 0.2f);
                    playerInventoryManager.leftWeapon = unarmedWeapon;
                    leftHandSlot.currentWeapon = unarmedWeapon;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamageCollider();
                }
                else
                {
                    animator.CrossFade("Right Arm Empty", 0.2f);
                    playerInventoryManager.rightWeapon = unarmedWeapon;
                    rightHandSlot.currentWeapon = unarmedWeapon;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamageCollider();
                }
            }
        }


        #region Animation Events
        private void LoadLeftWeaponDamageCollider()
        {
            leftDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            leftDamageCollider.weaponDamage = playerInventoryManager.leftWeapon.baseDamage;
            leftDamageCollider.poiseBreak = playerInventoryManager.leftWeapon.poiseBreak;
            playerFXManager.leftWeaponVFX = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponVFX>();
        }

        private void LoadRightWeaponDamageCollider()
        {
            rightDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            rightDamageCollider.weaponDamage = playerInventoryManager.rightWeapon.baseDamage;
            rightDamageCollider.poiseBreak = playerInventoryManager.rightWeapon.poiseBreak;
            playerFXManager.rightWeaponVFX = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponVFX>();
        }

        public void OpenDamageCollider()
        {
            if (playerManager.isUsingRightHand)
            {
                rightDamageCollider.EnableDamageCollider();
            }
       
            else if (playerManager.isUsingLeftHand)
            {
                leftDamageCollider.EnableDamageCollider();
            }

        }


        public void CloseDamageCollider()
        {
            if (rightDamageCollider != null)
            {
                rightDamageCollider.DisableDamageCollider();
            }
            if (leftDamageCollider != null)
            {
               leftDamageCollider.DisableDamageCollider();
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
        #endregion


        #region Handle Weapon Poise Bonus
        public void GrantWeaponAttackingPoiseBonus()
        {
            playerStatsManager.totalPoiseDefense = playerStatsManager.totalPoiseDefense + attackingItem.offensivePoiseBonus;
        }

        public void ResetWeaponAttackingPoiseBonus()
        {
            playerStatsManager.totalPoiseDefense = playerStatsManager.armorPoisebonus;
        }

        #endregion


    }


}
