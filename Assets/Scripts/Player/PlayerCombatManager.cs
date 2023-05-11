using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class PlayerCombatManager : MonoBehaviour
    {
        PlayerAnimatorManager playerAnimatorManager;
        PlayerEquipmentManager playerEquipmentManager;
        PlayerManager playerManager;
        inputHandler inputHandler;
        PlayerStatsManager playerStatsManager;
        PlayerInventoryManager playerInventoryManager;
        PlayerWeaponSlotManager playerWeaponSlotManager;
        PlayerFXManager playerFXManager;
        CameraHandler cameraHandler;
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
        public string TH_Heavy_Attack_01 = "TH_Heavy_Attack_01";
        public string TH_Heavy_Attack_02 = "TH_Heavy_Attack_02";
        public string TH_Light_Attack_01 = "TH_Light_Attack_01";
        public string TH_Light_Attack_02 = "TH_Light_Attack_02";
        public string TH_Jumping_Attack_01 = "TH_Jumping_Attack_01";
        public string TH_Running_Attack_01 = "TH_Running_Attack_01";
        public string weaponArtShield = "Parry";


        private void Awake()
        {
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            inputHandler = GetComponent<inputHandler>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
            playerManager = GetComponent<PlayerManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerFXManager = GetComponent<PlayerFXManager>();
            cameraHandler = FindObjectOfType<CameraHandler>();
        }
               

        public void AttemptBackStabOrRiposte()
        {
            if (playerStatsManager.currentStamina <= 0)
                return;
            RaycastHit hit;

            if (Physics.Raycast(inputHandler.criticalAttackRaycastStartPoint.position, 
                transform.TransformDirection(Vector3.forward),out hit,0.5f,backStabLayer))
            {
                CharacterManager enemycharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = playerWeaponSlotManager.rightDamageCollider;
                if (enemycharacterManager != null)
                {
                    isStabbing = true;
                    playerManager.transform.position = enemycharacterManager.backStabCollider.criticalDamagerStandPosition.position;
                    Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                    rotationDirection = hit.transform.position - playerManager.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                    playerManager.transform.rotation = targetRotation;

                    int criticalDamage = playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.weaponDamage;
                    enemycharacterManager.pendingCriticalDamage = criticalDamage;

                    playerAnimatorManager.PlayTargetAnimation("Back Stab",true);
                    enemycharacterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimationWithRootrotation("Back Stabbed",true);
                }
            }
            else if (Physics.Raycast(inputHandler.criticalAttackRaycastStartPoint.position,
                transform.TransformDirection(Vector3.forward), out hit, 0.7f, riposteLayer))
            {
                
                CharacterManager enemycharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = playerWeaponSlotManager.rightDamageCollider;
                if (enemycharacterManager != null && enemycharacterManager.canBeRiposted)
                {
                    playerManager.transform.position = enemycharacterManager.riposteCollider.criticalDamagerStandPosition.position;

                    Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                    rotationDirection = hit.transform.position - playerManager.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                    playerManager.transform.rotation = targetRotation;

                    int criticalDamage = playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.weaponDamage;
                    enemycharacterManager.pendingCriticalDamage = criticalDamage;
                    playerAnimatorManager.PlayTargetAnimation("Riposte", true);
                    enemycharacterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Riposted", true);
                }
            }
        }

        private void SuccessfullycastedSpell()
        {
            playerInventoryManager.currentSpell.SuccessfullyCastedSpell(playerAnimatorManager, playerStatsManager,cameraHandler,playerWeaponSlotManager,playerManager.isUsingLeftHand);
            playerAnimatorManager.animator.SetBool("isFiringSpell", true);
        }
    }
}
