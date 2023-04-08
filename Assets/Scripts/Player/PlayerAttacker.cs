using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class PlayerAttacker : MonoBehaviour
    {
        PlayerAnimatorManager animatorHandler;
        PlayerEquipmentManager playerEquipmentManager;
        PlayerManager playerManager;
        inputHandler inputHandler;
        PlayerStats playerStats;
        PlayerInventory playerInventory;
        WeaponSlotManager weaponSlotManager;
        CameraHandler cameraHandler;
        [SerializeField]
        LayerMask backStabLayer;
        [SerializeField]
        LayerMask riposteLayer;
        public string lastAttack;
        [HideInInspector]
        public bool isStabbing;
        private void Start()
        {
            animatorHandler = GetComponent<PlayerAnimatorManager>();
            inputHandler = GetComponentInParent<inputHandler>();
            playerStats = GetComponentInParent<PlayerStats>();
            weaponSlotManager = GetComponent<WeaponSlotManager>();
            playerManager = GetComponentInParent<PlayerManager>();
            playerInventory = GetComponentInParent<PlayerInventory>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            cameraHandler = FindObjectOfType<CameraHandler>();
        }
        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if (playerStats.currentStamina <= 0)
                return;
            if (inputHandler.comboFlag)
            {
                animatorHandler.anim.SetBool("canDoCombo", false);

                if (lastAttack == weapon.OH_Light_Attack_1)
                {
                    animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_2, true);
                }
                else if (lastAttack == weapon.TH_Light_Attack_01)
                {
                    animatorHandler.PlayTargetAnimation(weapon.TH_Light_Attack_02, true);
                }
            }
            
        }
        public void HandleLightAttack(WeaponItem weapon)
        {
            if (playerStats.currentStamina <= 0)
                return;
            weaponSlotManager.attackingItem = weapon;
            if (inputHandler.twoHandFlag)
            {
                animatorHandler.PlayTargetAnimation(weapon.TH_Light_Attack_01, true);
                lastAttack = weapon.TH_Light_Attack_01;
            }
            else
            {
               
                animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_1, true);
                lastAttack = weapon.OH_Light_Attack_1;
            }
           
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            if (playerStats.currentStamina <= 0 || playerManager.isInteracting)
                return;
            weaponSlotManager.attackingItem = weapon;
            if (inputHandler.twoHandFlag)
            {

            }
            else
            {
                
                animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true);
                lastAttack = weapon.OH_Heavy_Attack_1;
            }
            
        }

        public void HandleRBAction()
        {

            if (playerInventory.rightWeapon.isMeleeWeapon)
            {
                PerfromRBMeleeAction();
            }
            else if (playerInventory.rightWeapon.isSpellCaster|| 
                playerInventory.rightWeapon.isFaithCaster||
                playerInventory.rightWeapon.isPyroCaster)
            {
                PerformRBMagicAction(playerInventory.rightWeapon);
            }    
        }

        public void HandleLBAaction()
        {
            PerformLBBlockAction();
        }


        public void HandleLTAction()
        {
            if (playerInventory.leftWeapon.isShield)
            {
                PerformLTWeaponArt(inputHandler.twoHandFlag);

            }
            else if (playerInventory.leftWeapon.isMeleeWeapon)
            {

            }
        }

        private void PerfromRBMeleeAction()
        {
            if (playerManager.canDoCombo)
            {
                inputHandler.comboFlag = true;
                HandleWeaponCombo(playerInventory.rightWeapon);
                inputHandler.comboFlag = false;
            }
            else
            {
                if (playerManager.isInteracting)
                    return;
                if (playerManager.canDoCombo)
                    return;
                animatorHandler.anim.SetBool("isUsingRightHand", true);
                HandleLightAttack(playerInventory.rightWeapon);
            }
        }


        public void AttemptBackStabOrRiposte()
        {
            if (playerStats.currentStamina <= 0)
                return;
            RaycastHit hit;

            if (Physics.Raycast(inputHandler.criticalAttackRaycastStartPoint.position, 
                transform.TransformDirection(Vector3.forward),out hit,0.5f,backStabLayer))
            {
                CharacterManager enemycharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = weaponSlotManager.rightDamageCollider;
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

                    int criticalDamage = playerInventory.rightWeapon.criticalDamageMultiplier * rightWeapon.weaponDamage;
                    enemycharacterManager.pendingCriticalDamage = criticalDamage;

                    animatorHandler.PlayTargetAnimation("Back Stab",true);
                    enemycharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Back Stabbed",true);
                }
            }
            else if (Physics.Raycast(inputHandler.criticalAttackRaycastStartPoint.position,
                transform.TransformDirection(Vector3.forward), out hit, 0.7f, riposteLayer))
            {
                
                CharacterManager enemycharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = weaponSlotManager.rightDamageCollider;
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

                    int criticalDamage = playerInventory.rightWeapon.criticalDamageMultiplier * rightWeapon.weaponDamage;
                    enemycharacterManager.pendingCriticalDamage = criticalDamage;
                    animatorHandler.PlayTargetAnimation("Riposte", true);
                    enemycharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Riposted", true);
                }
            }
        }

        private void PerformRBMagicAction(WeaponItem weapon)
        {
            if (playerManager.isInteracting)
                return;
            if (weapon.isFaithCaster)
            {
                if(playerInventory.currentSpell != null && playerInventory.currentSpell.isFaithSpell)
                {
                    if(playerStats.currentFocus >= playerInventory.currentSpell.focusPointCost){
                        playerInventory.currentSpell.AttemptToCastSpell(animatorHandler, playerStats,weaponSlotManager,cameraHandler);
                    }
                    else
                    {
                        animatorHandler.PlayTargetAnimation("Failed Cast", true);
                    }
                  
                }
            }
            else if(weapon.isPyroCaster)
            {
                if (playerInventory.currentSpell != null && playerInventory.currentSpell.isPyroSpell)
                {
                    if (playerStats.currentFocus >= playerInventory.currentSpell.focusPointCost)
                    {
                        playerInventory.currentSpell.AttemptToCastSpell(animatorHandler, playerStats, weaponSlotManager,cameraHandler);
                    }
                    else
                    {
                        animatorHandler.PlayTargetAnimation("Failed Cast", true);
                    }

                }
            }
        }

        private void PerformLTWeaponArt(bool isTwoHanding)
        {
            if (playerManager.isInteracting)
                return;
            if (isTwoHanding)
            {
               
            }
            else
            {
                animatorHandler.PlayTargetAnimation(playerInventory.leftWeapon.weaponArtShield, true);

            }


        }

        private void PerformLBBlockAction()
        {
            if (playerManager.isInteracting)
                return;
            if (playerManager.isBlocking)
            {
                return;
            }
            animatorHandler.PlayTargetAnimation("Block Start", false, true);
            playerEquipmentManager.OpenBlockingCollider();
            playerManager.isBlocking = true;
        }

        private void SuccessfullycastedSpell()
        {
            playerInventory.currentSpell.SuccessfullyCastedSpell(animatorHandler, playerStats,cameraHandler,weaponSlotManager);
            animatorHandler.anim.SetBool("isFiringSpell", true);
        }
    }
}
