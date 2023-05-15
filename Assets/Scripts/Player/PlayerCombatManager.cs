using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class PlayerCombatManager : MonoBehaviour
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
        public string TH_Heavy_Attack_01 = "TH_Heavy_Attack_01";
        public string TH_Heavy_Attack_02 = "TH_Heavy_Attack_02";
        public string TH_Light_Attack_01 = "TH_Light_Attack_01";
        public string TH_Light_Attack_02 = "TH_Light_Attack_02";
        public string TH_Jumping_Attack_01 = "TH_Jumping_Attack_01";
        public string TH_Running_Attack_01 = "TH_Running_Attack_01";
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

                    int criticalDamage = player.playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.physicalDamage;
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

                    int criticalDamage = player.playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.physicalDamage;
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
    }
}
