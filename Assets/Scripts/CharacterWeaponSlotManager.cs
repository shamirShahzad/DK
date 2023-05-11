using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DK
{
    public class CharacterWeaponSlotManager : MonoBehaviour
    {
        protected CharacterManager characterManager;
        protected CharacterStatsManager characterStatsManager;
        protected CharacterFXManager characterFXManager;
        protected CharacterInventoryManager characterInventoryManager;
        protected CharacterAnimatorManager characterAnimatorManager;

        

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
            characterManager = GetComponent<CharacterManager>();
            characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
            characterInventoryManager = GetComponent<CharacterInventoryManager>();
            characterStatsManager = GetComponent<CharacterStatsManager>();
            characterFXManager = GetComponent<CharacterFXManager>();
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
            characterAnimatorManager.SetHandIKForWeapon(rightHandIKTarget, leftHandIKTarget, isTwoHanding);
        }
        public virtual  void LoadBothWeaponOnslot()
        {
            LoadWeaponOnSlot(characterInventoryManager.rightWeapon, false);
            LoadWeaponOnSlot(characterInventoryManager.leftWeapon, true);
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
                    characterAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);

                }
                else
                {
                    if (characterManager.isTwoHanding)
                    {
                        backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                        leftHandSlot.UnloadWeaponAndDestroy();
                        characterAnimatorManager.PlayTargetAnimation("Left Arm Empty", false, true);
                    }
                    else
                    {
                        backSlot.UnloadWeaponAndDestroy();
                    }
                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamageCollider();
                    LoadTwoHandIKTargets(characterManager.isTwoHanding);
                    characterAnimatorManager.animator.runtimeAnimatorController = weaponItem.weaponController;
                }
            }
            else
            {
                weaponItem = unarmedWeapon;
                if (isLeft)
                {
                    characterInventoryManager.leftWeapon = unarmedWeapon;
                    leftHandSlot.currentWeapon = unarmedWeapon;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamageCollider();
                    characterAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
                }
                else
                {
                    characterInventoryManager.rightWeapon = unarmedWeapon;
                    rightHandSlot.currentWeapon = unarmedWeapon;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamageCollider();
                    characterAnimatorManager.animator.runtimeAnimatorController = weaponItem.weaponController;
                }
            }
        }

        protected virtual void LoadLeftWeaponDamageCollider()
        {
            leftDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            leftDamageCollider.weaponDamage = characterInventoryManager.leftWeapon.baseDamage;
            leftDamageCollider.characterManager = characterManager;
            leftDamageCollider.poiseBreak = characterInventoryManager.leftWeapon.poiseBreak;
            leftDamageCollider.teamIdNumber = characterStatsManager.teamIdNumber;
            characterFXManager.leftWeaponVFX = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponVFX>();
        }

        protected virtual void LoadRightWeaponDamageCollider()
        {
            if (rightHandSlot.currentWeapon.weaponTypes != WeaponTypes.Bow)
            {
                rightDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
                rightDamageCollider.weaponDamage = characterInventoryManager.rightWeapon.baseDamage;
                rightDamageCollider.characterManager = characterManager;
                rightDamageCollider.poiseBreak = characterInventoryManager.rightWeapon.poiseBreak;
                rightDamageCollider.teamIdNumber = characterStatsManager.teamIdNumber;
                characterFXManager.rightWeaponVFX = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponVFX>();
            }
        }


        public virtual void OpenDamageCollider()
        {
            if (characterManager.isUsingRightHand)
            {
                rightDamageCollider.EnableDamageCollider();
            }

            else if (characterManager.isUsingLeftHand)
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
            WeaponItem currentWeaponBeingUsed = characterInventoryManager.currentItemBeingUsed as WeaponItem;
            characterStatsManager.totalPoiseDefense = characterStatsManager.totalPoiseDefense + currentWeaponBeingUsed.offensivePoiseBonus;
        }

        public virtual void ResetWeaponAttackingPoiseBonus()
        {
            characterStatsManager.totalPoiseDefense = characterStatsManager.armorPoisebonus;
        }
    }
}
