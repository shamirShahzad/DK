using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DK
{
    public class EnemyWeaponSlotManager : CharacterWeaponSlotManager
    {
        public WeaponItem rightHandWeapon;
        public WeaponItem leftHandWeapon;



        EnemyStatsManager enemyStatsManager;
        EnemyFXManager enemyFXManager;


        private void Awake()
        {
            enemyStatsManager = GetComponent<EnemyStatsManager>();
            enemyFXManager = GetComponent<EnemyFXManager>();
            LoadWeaponHolderSlot();
        }

        private void Start()
        {
            LoadWeaponsOnBothHands();
        }


        private void LoadWeaponHolderSlot()
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
            }
        }
        public void LoadWeaponOnSlot(WeaponItem weapon,bool isLeft)
        {
            if (isLeft)
            {
                leftHandSlot.currentWeapon = weapon;
                leftHandSlot.LoadWeaponModel(weapon);
                LoadWeaponsDamageCollider(true);
            }
            else
            {
                rightHandSlot.currentWeapon = weapon;
                rightHandSlot.LoadWeaponModel(weapon);
                LoadWeaponsDamageCollider(false);
            }
        }

        public void LoadWeaponsOnBothHands()
        {
            if (rightHandWeapon != null)
            {
                LoadWeaponOnSlot(rightHandWeapon, false);
            }
            if (leftHandWeapon != null)
            {
                LoadWeaponOnSlot(leftHandWeapon, true);
            }
        }

        public void LoadWeaponsDamageCollider(bool isLeft)
        {
            if (isLeft)
            {
                leftDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
                leftDamageCollider.characterManager = GetComponentInParent<CharacterManager>();
                enemyFXManager.leftWeaponVFX = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponVFX>();
            }
            else
            {
                rightDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
                rightDamageCollider.characterManager = GetComponentInParent<CharacterManager>();
                enemyFXManager.rightWeaponVFX = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponVFX>();
            }
        }



        #region Animation Events
        public void OpenDamageCollider()
        {
            rightDamageCollider.EnableDamageCollider();
        }

        public void CloseDamageCollider()
        {
            rightDamageCollider.DisableDamageCollider();
        }


        public void DrainStaminaLightAttack()
        {

        }
        public void DrainStaminaHeavyAttack()
        {
        }

        public void EnableCombo()
        {
            //anim.SetBool("canDoCombo", true);
        }

        public void DisableCombo()
        {
            //anim.SetBool("canDoCombo", false);
        }
        #endregion

        #region Handle Weapon Poise Bonus
        public void GrantWeaponAttackingPoiseBonus()
        {
            enemyStatsManager.totalPoiseDefense = enemyStatsManager.totalPoiseDefense + enemyStatsManager.offensivePoiseBonus;
        }

        public void ResetWeaponAttackingPoiseBonus()
        {
            enemyStatsManager.totalPoiseDefense = enemyStatsManager.armorPoisebonus;
        }

        #endregion


    }
}
