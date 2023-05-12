using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DK
{
    public class CharacterWeaponSlotManager : MonoBehaviour
    {
        protected CharacterManager character;

        [Header("Weapon Items")]
        public WeaponItem unarmedWeapon;

        [Header("Damage Colliders")]
        public DamageCollider leftDamageCollider;
        public DamageCollider rightDamageCollider;

        [Header("Slots")]
        public WeaponHolderSlot leftHandSlot;
        public WeaponHolderSlot rightHandSlot;
        public WeaponHolderSlot backSlot;

        [Header("Hand IK Targets")]
        public RightHandRigTarget rightHandIKTarget;
        public LeftHandRigTarget leftHandIKTarget;
        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
            LoadWeaponHolderSlots();
        }
        protected virtual  void LoadWeaponHolderSlots()
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

        public virtual void LoadTwoHandIKTargets(bool isTwoHanding)
        {
            leftHandIKTarget = rightHandSlot.currentWeaponModel.GetComponentInChildren<LeftHandRigTarget>();
            rightHandIKTarget = rightHandSlot.currentWeaponModel.GetComponentInChildren<RightHandRigTarget>();
            character.characterAnimatorManager.SetHandIKForWeapon(rightHandIKTarget, leftHandIKTarget, isTwoHanding);
        }
        public virtual  void LoadBothWeaponOnslot()
        {
            LoadWeaponOnSlot(character.characterInventoryManager.rightWeapon, false);
            LoadWeaponOnSlot(character.characterInventoryManager.leftWeapon, true);
        }

        public virtual void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (weaponItem != null)
            {

                if (isLeft)
                {
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    
                    LoadLeftWeaponDamageCollider();
                    character.characterAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);

                }
                else
                {
                    if (character.isTwoHanding)
                    {
                        backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                        leftHandSlot.UnloadWeaponAndDestroy();
                        character.characterAnimatorManager.PlayTargetAnimation("Left Arm Empty", false, true);
                    }
                    else
                    {
                        backSlot.UnloadWeaponAndDestroy();
                    }
                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamageCollider();
                    LoadTwoHandIKTargets(character.isTwoHanding);
                    character.animator.runtimeAnimatorController = weaponItem.weaponController;
                }
            }
            else
            {
                weaponItem = unarmedWeapon;
                if (isLeft)
                {
                    character.characterInventoryManager.leftWeapon = unarmedWeapon;
                    leftHandSlot.currentWeapon = unarmedWeapon;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamageCollider();
                    character.characterAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
                }
                else
                {
                    character.characterInventoryManager.rightWeapon = unarmedWeapon;
                    rightHandSlot.currentWeapon = unarmedWeapon;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamageCollider();
                    character.animator.runtimeAnimatorController = weaponItem.weaponController;
                }
            }
        }

        protected virtual void LoadLeftWeaponDamageCollider()
        {
            leftDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            leftDamageCollider.weaponDamage = character.characterInventoryManager.leftWeapon.baseDamage;
            leftDamageCollider.characterManager = character;
            leftDamageCollider.poiseBreak = character.characterInventoryManager.leftWeapon.poiseBreak;
            leftDamageCollider.teamIdNumber = character.characterStatsManager.teamIdNumber;
            character.characterFXManager.leftWeaponVFX = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponVFX>();
        }

        protected virtual void LoadRightWeaponDamageCollider()
        {
            if (rightHandSlot.currentWeapon.weaponTypes != WeaponTypes.Bow)
            {
                rightDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
                rightDamageCollider.weaponDamage = character.characterInventoryManager.rightWeapon.baseDamage;
                rightDamageCollider.characterManager = character;
                rightDamageCollider.poiseBreak = character.characterInventoryManager.rightWeapon.poiseBreak;
                rightDamageCollider.teamIdNumber = character.characterStatsManager.teamIdNumber;
                character.characterFXManager.rightWeaponVFX = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponVFX>();
            }
        }


        public virtual void OpenDamageCollider()
        {
            if (character.isUsingRightHand)
            {
                rightDamageCollider.EnableDamageCollider();
            }

            else if (character.isUsingLeftHand)
            {
                leftDamageCollider.EnableDamageCollider();
            }

        }


        public virtual void CloseDamageCollider()
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

        public virtual void GrantWeaponAttackingPoiseBonus()
        {
            WeaponItem currentWeaponBeingUsed = character.characterInventoryManager.currentItemBeingUsed as WeaponItem;
            character.characterStatsManager.totalPoiseDefense = character.characterStatsManager.totalPoiseDefense + currentWeaponBeingUsed.offensivePoiseBonus;
        }

        public virtual void ResetWeaponAttackingPoiseBonus()
        {
            character.characterStatsManager.totalPoiseDefense = character.characterStatsManager.armorPoisebonus;
        }
    }
}
