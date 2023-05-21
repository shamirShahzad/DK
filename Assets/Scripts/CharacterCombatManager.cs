using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class CharacterCombatManager : MonoBehaviour
    {
        CharacterManager character;

        [SerializeField]
        LayerMask backStabLayer;
        [SerializeField]
        LayerMask riposteLayer;
        [HideInInspector]

        public string lastAttack;

        [Header("Attack Animations")]
        public string OH_Light_Attack_01 = "OH_Light_Attack_01";
        public string OH_Light_Attack_02 = "OH_Light_Attack_02";
        public string OH_Heavy_Attack_01 = "OH_Heavy_Attack_01";
        public string OH_Heavy_Attack_02 = "OH_Heavy_Attack_02";
        public string OH_Running_Attack_01 = "OH_Running_Attack_01";
        public string OH_Jumping_Attack_01 = "OH_Jumping_Attack_01";
        public string OH_Charge_Attack_01 = "OH_Charging_Attack_Charge";
        public string TH_Heavy_Attack_01 = "TH_Heavy_Attack_01";
        public string TH_Heavy_Attack_02 = "TH_Heavy_Attack_02";
        public string TH_Light_Attack_01 = "TH_Light_Attack_01";
        public string TH_Light_Attack_02 = "TH_Light_Attack_02";
        public string TH_Jumping_Attack_01 = "TH_Jumping_Attack_01";
        public string TH_Running_Attack_01 = "TH_Running_Attack_01";
        public string TH_Charge_Attack_01 = "TH_Charging_Attack_Charge";
        public string weaponArtShield = "Parry";

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }
        [Header("Attack Type")]
        public AttackType currentAttackType;


        public virtual void DrainStaminaBasedOnAttackTypes()
        {
                 //If you want Ai to have stamina as well put code in here, however fck that AI IS superior and full of Stamina 
        }

        public virtual void SetBlockingAbsorbtionsFromBlockingWeapon()
        {
            if (character.isUsingRightHand)
            {
                character.characterStatsManager.blockingPhysicalDamageAbsorbtion = character.characterInventoryManager.rightWeapon.physicalBlockingDamageAbsorbtion;
                character.characterStatsManager.blockingMagicDamageAbsorbtion = character.characterInventoryManager.rightWeapon.magicBlockingDamageAbsorbtion;
                character.characterStatsManager.blockingFireDamageAbsorbtion = character.characterInventoryManager.rightWeapon.fireBlockingDamageAbsorbtion;
                character.characterStatsManager.blockingLightningDamageAbsorbtion = character.characterInventoryManager.rightWeapon.lightningBlockingDamageAbsorbtion;
                character.characterStatsManager.blockingDarkDamageAbsorbtion = character.characterInventoryManager.rightWeapon.darkBlockingDamageAbsorbtion;
                character.characterStatsManager.blockingStability = character.characterInventoryManager.rightWeapon.stability;
            }
            else if (character.isUsingLeftHand)
            {
                character.characterStatsManager.blockingPhysicalDamageAbsorbtion = character.characterInventoryManager.leftWeapon.physicalBlockingDamageAbsorbtion;
                character.characterStatsManager.blockingMagicDamageAbsorbtion = character.characterInventoryManager.leftWeapon.magicBlockingDamageAbsorbtion;
                character.characterStatsManager.blockingFireDamageAbsorbtion = character.characterInventoryManager.leftWeapon.fireBlockingDamageAbsorbtion;
                character.characterStatsManager.blockingLightningDamageAbsorbtion = character.characterInventoryManager.leftWeapon.lightningBlockingDamageAbsorbtion;
                character.characterStatsManager.blockingDarkDamageAbsorbtion = character.characterInventoryManager.leftWeapon.darkBlockingDamageAbsorbtion;
                character.characterStatsManager.blockingStability = character.characterInventoryManager.leftWeapon.stability;
            }
        }

        public virtual void AttempBlock(DamageCollider attackingWeapon,float physicalDamage,float magicDamage,
            float fireDamage,float lightningDamage,float darkDamage,string blockAnimation)
        {
            float staminaDamageAbsorbtion = ((physicalDamage + magicDamage + fireDamage + lightningDamage + darkDamage) * attackingWeapon.guardBreakModifier) * 
                (character.characterStatsManager.blockingStability / 100);
            float staminaDamage = ((physicalDamage + magicDamage + fireDamage + lightningDamage + darkDamage) * attackingWeapon.guardBreakModifier) - staminaDamageAbsorbtion;
            character.characterStatsManager.currentStamina -=  staminaDamage;
            if(character.characterStatsManager.currentStamina <= 0)
            {
                character.isBlocking = false;
                character.characterAnimatorManager.PlayTargetAnimation("Guard Break", true);
            }
            else
            {
                character.characterAnimatorManager.PlayTargetAnimation(blockAnimation, true);
            }

        }

        public void AttemptBackStabOrRiposte()
        {
            if (character.characterStatsManager.currentStamina <= 0)
                return;
            RaycastHit hit;

            if (Physics.Raycast(character.criticalAttackRaycastStartPoint.position,
                transform.TransformDirection(Vector3.forward), out hit, 0.5f, backStabLayer))
            {
                CharacterManager enemycharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = character.characterWeaponSlotManager.rightDamageCollider;
                if (enemycharacterManager != null)
                {
                    character.transform.position = enemycharacterManager.backStabCollider.criticalDamagerStandPosition.position;
                    Vector3 rotationDirection = character.transform.root.eulerAngles;
                    rotationDirection = hit.transform.position - character.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(character.transform.rotation, tr, 500 * Time.deltaTime);
                    character.transform.rotation = targetRotation;

                    int criticalDamage = character.characterInventoryManager.rightWeapon.criticalDamageModifier * rightWeapon.physicalDamage;
                    enemycharacterManager.pendingCriticalDamage = criticalDamage;

                    character.characterAnimatorManager.PlayTargetAnimation("Back Stab", true);
                    enemycharacterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimationWithRootrotation("Back Stabbed", true);
                }
            }
            else if (Physics.Raycast(character.criticalAttackRaycastStartPoint.position,
                transform.TransformDirection(Vector3.forward), out hit, 0.7f, riposteLayer))
            {

                CharacterManager enemycharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = character.characterWeaponSlotManager.rightDamageCollider;
                if (enemycharacterManager != null && enemycharacterManager.canBeRiposted)
                {
                    character.transform.position = enemycharacterManager.riposteCollider.criticalDamagerStandPosition.position;

                    Vector3 rotationDirection = character.transform.root.eulerAngles;
                    rotationDirection = hit.transform.position - character.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(character.transform.rotation, tr, 500 * Time.deltaTime);
                    character.transform.rotation = targetRotation;

                    int criticalDamage = character.characterInventoryManager.rightWeapon.criticalDamageModifier * rightWeapon.physicalDamage;
                    enemycharacterManager.pendingCriticalDamage = criticalDamage;
                    character.characterAnimatorManager.PlayTargetAnimation("Riposte", true);
                    enemycharacterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Riposted", true);
                }
            }
        }

        private void SuccessfullycastedSpell()
        {
            character.characterInventoryManager.currentSpell.SuccessfullyCastedSpell(character);
            character.animator.SetBool("isFiringSpell", true);
        }

        
    }
}
