using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class PlayerCombatManager : CharacterCombatManager
    {

        PlayerManager player;
        [SerializeField]
        LayerMask backStabLayer;
        [SerializeField]
        LayerMask riposteLayer;
        public string lastAttack;
        [HideInInspector]
        public bool isStabbing;

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


        private void Awake()
        {
            player = GetComponent<PlayerManager>();
        }
               

        public void AttemptBackStabOrRiposte()
        {
            if (player.playerStatsManager.currentStamina <= 0)
                return;
            RaycastHit hit;

            if (Physics.Raycast(player.inputHandler.criticalAttackRaycastStartPoint.position, 
                transform.TransformDirection(Vector3.forward),out hit,0.5f,backStabLayer))
            {
                CharacterManager enemycharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = player.playerWeaponSlotManager.rightDamageCollider;
                if (enemycharacterManager != null)
                {
                    isStabbing = true;
                    player.transform.position = enemycharacterManager.backStabCollider.criticalDamagerStandPosition.position;
                    Vector3 rotationDirection = player.transform.root.eulerAngles;
                    rotationDirection = hit.transform.position - player.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(player.transform.rotation, tr, 500 * Time.deltaTime);
                    player.transform.rotation = targetRotation;

                    int criticalDamage = player.playerInventoryManager.rightWeapon.criticalDamageModifier * rightWeapon.physicalDamage;
                    enemycharacterManager.pendingCriticalDamage = criticalDamage;

                    player.playerAnimatorManager.PlayTargetAnimation("Back Stab",true);
                    enemycharacterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimationWithRootrotation("Back Stabbed",true);
                }
            }
            else if (Physics.Raycast(player.inputHandler.criticalAttackRaycastStartPoint.position,
                transform.TransformDirection(Vector3.forward), out hit, 0.7f, riposteLayer))
            {
                
                CharacterManager enemycharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = player.playerWeaponSlotManager.rightDamageCollider;
                if (enemycharacterManager != null && enemycharacterManager.canBeRiposted)
                {
                    player.transform.position = enemycharacterManager.riposteCollider.criticalDamagerStandPosition.position;

                    Vector3 rotationDirection = player.transform.root.eulerAngles;
                    rotationDirection = hit.transform.position - player.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(player.transform.rotation, tr, 500 * Time.deltaTime);
                    player.transform.rotation = targetRotation;

                    int criticalDamage = player.playerInventoryManager.rightWeapon.criticalDamageModifier * rightWeapon.physicalDamage;
                    enemycharacterManager.pendingCriticalDamage = criticalDamage;
                    player.playerAnimatorManager.PlayTargetAnimation("Riposte", true);
                    enemycharacterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Riposted", true);
                }
            }
        }

        private void SuccessfullycastedSpell()
        {
            player.playerInventoryManager.currentSpell.SuccessfullyCastedSpell(player);
            player.animator.SetBool("isFiringSpell", true);
        }

        public override void DrainStaminaBasedOnAttackTypes()
        {
            if (player.isUsingRightHand)
            {
                int baseStamina = player.playerInventoryManager.rightWeapon.baseStaminaCost;
                if (currentAttackType == AttackType.Light)
                {
                    player.playerStatsManager.DeductStamina(baseStamina * player.playerInventoryManager.rightWeapon.lightStaminaModifier);
                }
                else if(currentAttackType == AttackType.Light2)
                {
                    player.playerStatsManager.DeductStamina(baseStamina * player.playerInventoryManager.rightWeapon.lightStaminaModifier + 10);
                }
                else if(currentAttackType == AttackType.Heavy)
                {
                    player.playerStatsManager.DeductStamina(baseStamina * player.playerInventoryManager.rightWeapon.heavyStaminaModifier);
                }
                else if(currentAttackType == AttackType.Heavy2)
                {
                    player.playerStatsManager.DeductStamina(baseStamina * player.playerInventoryManager.rightWeapon.heavyStaminaModifier + 20);
                }
                else if(currentAttackType == AttackType.Jumping)
                {
                    player.playerStatsManager.DeductStamina(baseStamina * player.playerInventoryManager.rightWeapon.jumppingStaminaModifier);
                }
                else if(currentAttackType == AttackType.Running)
                {
                    player.playerStatsManager.DeductStamina(baseStamina * player.playerInventoryManager.rightWeapon.runningStaminaModifier);
                }
                else if(currentAttackType == AttackType.Critical)
                {
                    player.playerStatsManager.DeductStamina(baseStamina * player.playerInventoryManager.rightWeapon.criticalStaminaModifier);
                }
            }
            else if(player.isUsingLeftHand)
            {
                int baseStamina = player.playerInventoryManager.leftWeapon.baseStaminaCost;
                if (currentAttackType == AttackType.Light)
                {
                    player.playerStatsManager.DeductStamina(baseStamina * player.playerInventoryManager.leftWeapon.lightStaminaModifier);
                }
                else if (currentAttackType == AttackType.Light2)
                {
                    player.playerStatsManager.DeductStamina(baseStamina * player.playerInventoryManager.leftWeapon.lightStaminaModifier + 10);
                }
                else if (currentAttackType == AttackType.Heavy)
                {
                    player.playerStatsManager.DeductStamina(baseStamina * player.playerInventoryManager.leftWeapon.heavyStaminaModifier);
                }
                else if (currentAttackType == AttackType.Heavy2)
                {
                    player.playerStatsManager.DeductStamina(baseStamina * player.playerInventoryManager.leftWeapon.heavyStaminaModifier + 20);
                }
                else if (currentAttackType == AttackType.Jumping)
                {
                    player.playerStatsManager.DeductStamina(baseStamina * player.playerInventoryManager.leftWeapon.jumppingStaminaModifier);
                }
                else if (currentAttackType == AttackType.Running)
                {
                    player.playerStatsManager.DeductStamina(baseStamina * player.playerInventoryManager.leftWeapon.runningStaminaModifier);
                }
                else if (currentAttackType == AttackType.Critical)
                {
                    player.playerStatsManager.DeductStamina(baseStamina * player.playerInventoryManager.leftWeapon.criticalStaminaModifier);
                }
            }
        }

    }
}
