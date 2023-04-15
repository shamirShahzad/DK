using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DK
{
    public class EnemyWeaponSlotManager : MonoBehaviour
    {
        public WeaponItem rightHandWeapon;
        public WeaponItem leftHandWeapon;


        WeaponHolderSlot rightHandSlot;
        WeaponHolderSlot leftHandSlot;

        DamageCollider leftHandDamageCollider;
        DamageCollider rightHandDamageCollider;

        EnemyStats enemyStats;


        private void Awake()
        {
            enemyStats = GetComponentInParent<EnemyStats>();
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
                leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
                leftHandDamageCollider.characterManager = GetComponentInParent<CharacterManager>();
            }
            else
            {
                rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
                rightHandDamageCollider.characterManager = GetComponentInParent<CharacterManager>();
            }
        }



        #region Animation Events
        public void OpenDamageCollider()
        {
            rightHandDamageCollider.EnableDamageCollider();
        }

        public void CloseDamageCollider()
        {
            rightHandDamageCollider.DisableDamageCollider();
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
            enemyStats.totalPoiseDefense = enemyStats.totalPoiseDefense + enemyStats.offensivePoiseBonus;
        }

        public void ResetWeaponAttackingPoiseBonus()
        {
            enemyStats.totalPoiseDefense = enemyStats.armorPoisebonus;
        }

        #endregion


    }
}
