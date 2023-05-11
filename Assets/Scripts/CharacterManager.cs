using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class CharacterManager : MonoBehaviour
    {
        CharacterAnimatorManager characterAnimatorManager;
        CharacterWeaponSlotManager characterWeaponSlotManager;
        [Header("Lock On Transform")]
        public Transform lockOnTransform;
        [Header("Combat Colliders")]
        public CriticalDamageCollider backStabCollider;
        public CriticalDamageCollider riposteCollider;

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
