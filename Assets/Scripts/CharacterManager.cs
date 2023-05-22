using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class CharacterManager : MonoBehaviour
    {
        public CharacterAnimatorManager characterAnimatorManager;
        public CharacterWeaponSlotManager characterWeaponSlotManager;
        public CharacterStatsManager characterStatsManager;
        public CharacterInventoryManager characterInventoryManager;
        public CharacterFXManager characterFXManager;
        public CharacterSFXManager characterSFXManager;
        public CharacterCombatManager characterCombatManager;
        public Animator animator;
        public inputHandler inputHandler;
        [Header("Lock On Transform")]
        public Transform lockOnTransform;
        [Header("Raycast Transform")]
        public Transform criticalAttackRaycastStartPoint;

        


        [Header("Combat Flags")]
        public bool canBeRiposted;
        public bool isParrying;
        public bool isBlocking;
        public bool isFiringSpell;
        public bool isInvulnerable;
        public int pendingCriticalDamage;
        public bool canDoCombo;
        public bool isUsingRightHand;
        public bool isUsingLeftHand;
        public bool isTwoHanding;
        public bool isHoldingArrow;
        public bool isAiming;
        public bool isPerformingFullyChargedAttack;
        public bool isAttacking;
        public bool isBeingBackStabbed;
        public bool isBeingRiposted;
        public bool isPerformingBackStab;
        public bool isPerformingRiposte;
        public bool canBeParried;

        [Header("Status dead")]

        public bool isDead;


        [Header("Movement flag")]
        public bool isRotatingWithRootMotion;
        public bool canRotate;
        public bool isSprinting;
        public bool isInAir;
        public bool isGrounded;
        public bool isInteracting;


        protected virtual void Awake()
        {
            characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
            characterWeaponSlotManager = GetComponent<CharacterWeaponSlotManager>();
            characterStatsManager = GetComponent<CharacterStatsManager>();
            characterInventoryManager = GetComponent<CharacterInventoryManager>();
            characterFXManager = GetComponent<CharacterFXManager>();
            animator = GetComponent<Animator>();
            inputHandler = GetComponent<inputHandler>();
            characterSFXManager = GetComponent<CharacterSFXManager>();
            characterCombatManager = GetComponent<CharacterCombatManager>();
        }

        protected virtual void FixedUpdate()
        {
            characterAnimatorManager.CheckHandIKWeight(characterWeaponSlotManager.rightHandIKTarget, characterWeaponSlotManager.leftHandIKTarget, isTwoHanding);
        }

        public virtual void UpdateWhichHandCharacterIsUsing(bool usingRightHand)
        {
            if (usingRightHand)
            {
                isUsingLeftHand = false;
                isUsingRightHand = true;
            }
            else
            {
                isUsingRightHand = false;
                isUsingLeftHand = true;
            }
        }



    }
}
