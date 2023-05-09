using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class PlayerCombatManager : MonoBehaviour
    {
        PlayerAnimatorManager playerAnimtorManager;
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
        string OH_Light_Attack_01 = "OH_Light_Attack_01";
        string OH_Light_Attack_02 = "OH_Light_Attack_02";
        string OH_Heavy_Attack_01 = "OH_Heavy_Attack_01";
        string OH_Heavy_Attack_02 = "OH_Heavy_Attack_02";
        string TH_Light_Attack_01 = "TH_Light_Attack_01";
        string TH_Light_Attack_02 = "TH_Light_Attack_02";
        string weaponArtShield = "Parry";
        private void Awake()
        {
            playerAnimtorManager = GetComponent<PlayerAnimatorManager>();
            inputHandler = GetComponent<inputHandler>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
            playerManager = GetComponent<PlayerManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerFXManager = GetComponent<PlayerFXManager>();
            cameraHandler = FindObjectOfType<CameraHandler>();
        }
        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if (playerStatsManager.currentStamina <= 0)
                return;
            if (inputHandler.comboFlag)
            {
                playerAnimtorManager.animator.SetBool("canDoCombo", false);

                if (lastAttack == OH_Light_Attack_01)
                {
                    playerAnimtorManager.PlayTargetAnimation(OH_Light_Attack_02, true);
                    playerFXManager.PlayWeaponFX(false);
                }
                else if (lastAttack == TH_Light_Attack_01)
                {
                    playerAnimtorManager.PlayTargetAnimation(TH_Light_Attack_02, true);
                }
            }
            
        }
        public void HandleLightAttack(WeaponItem weapon)
        {
            if (playerStatsManager.currentStamina <= 0)
                return;
            playerWeaponSlotManager.attackingItem = weapon;
            if (inputHandler.twoHandFlag)
            {
                playerAnimtorManager.PlayTargetAnimation(TH_Light_Attack_01, true);
                lastAttack = TH_Light_Attack_01;
                playerFXManager.PlayWeaponFX(false);
            }
            else
            {
               
                playerAnimtorManager.PlayTargetAnimation(OH_Light_Attack_01, true);
                lastAttack = OH_Light_Attack_01;
                playerFXManager.PlayWeaponFX(false);
            }
           
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            if (playerStatsManager.currentStamina <= 0 || playerManager.isInteracting)
                return;
            playerWeaponSlotManager.attackingItem = weapon;
            if (inputHandler.twoHandFlag)
            {

            }
            else
            {
                
                playerAnimtorManager.PlayTargetAnimation(OH_Heavy_Attack_01, true);
                lastAttack = OH_Heavy_Attack_01;
            }
            
        }

        public void HandleRBAction()
        {

            if (playerInventoryManager.rightWeapon.weaponTypes == WeaponTypes.StraightSword ||
                playerInventoryManager.rightWeapon.weaponTypes == WeaponTypes.Unarmed)
            {
                PerfromRBMeleeAction();
            }
            else if (playerInventoryManager.rightWeapon.weaponTypes == WeaponTypes.SpellCaster|| 
                playerInventoryManager.rightWeapon.weaponTypes == WeaponTypes.FaithCaster ||
                playerInventoryManager.rightWeapon.weaponTypes == WeaponTypes.PyromancyCaster)
            {
                PerformMagicAction(playerInventoryManager.rightWeapon,playerManager.isUsingLeftHand);
            }    
        }

        public void HandleLBAaction()
        {
           // PerformLBBlockAction();

            if (playerManager.isTwoHanding)
            {
                if (playerInventoryManager.rightWeapon.weaponTypes == WeaponTypes.Bow)
                {
                    PerformLBAimingAction();
                }
            }
            else
            {
                if (playerInventoryManager.leftWeapon.weaponTypes == WeaponTypes.Shield ||
                    playerInventoryManager.leftWeapon.weaponTypes == WeaponTypes.StraightSword)
                {
                    PerformLBBlockAction();
                }
                else if(playerInventoryManager.leftWeapon.weaponTypes == WeaponTypes.FaithCaster || 
                    playerInventoryManager.leftWeapon.weaponTypes == WeaponTypes.PyromancyCaster)
                {
                    PerformMagicAction(playerInventoryManager.leftWeapon, true);
                    playerAnimtorManager.animator.SetBool("isUsingLeftHand", true);
                }
            }
        }


        public void HandleLTAction()
        {
            if (playerInventoryManager.leftWeapon.weaponTypes == WeaponTypes.Shield ||
                playerInventoryManager.leftWeapon.weaponTypes == WeaponTypes.Unarmed)
            {
                PerformLTWeaponArt(inputHandler.twoHandFlag);

            }
            else if (playerInventoryManager.leftWeapon.weaponTypes == WeaponTypes.StraightSword)
            {

            }
        }

        private void PerfromRBMeleeAction()
        {
            if (playerManager.canDoCombo)
            {
                inputHandler.comboFlag = true;
                HandleWeaponCombo(playerInventoryManager.rightWeapon);
                inputHandler.comboFlag = false;
            }
            else
            {
                if (playerManager.isInteracting)
                    return;
                if (playerManager.canDoCombo)
                    return;
                playerAnimtorManager.animator.SetBool("isUsingRightHand", true);
                HandleLightAttack(playerInventoryManager.rightWeapon);
            }

            
        }

        public void HandleHoldRTAction()
        {
            if (playerManager.isTwoHanding)
            {
                PerformRTTRangedAction();
            }
            else
            {

            }
        }
        private void DrawArrowAction()
        {
            playerAnimtorManager.animator.SetBool("isHoldingArrow", true);
            playerAnimtorManager.PlayTargetAnimation("Bow_TH_Draw_01_R",true);
            GameObject loadedArrow = Instantiate(playerInventoryManager.currentAmmo.loadedItemModel, playerWeaponSlotManager.leftHandSlot.transform);
            Animator bowAnimator = playerWeaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();
            bowAnimator.SetBool("isDrawn", true);
            bowAnimator.Play("Bow_TH_Draw_01");
            playerFXManager.currentRangeFX = loadedArrow;
        }

        public void FireArrowAction()
        {
            ArrowInstantiationLocation arrowInstantiationLocation;
            arrowInstantiationLocation = playerWeaponSlotManager.rightHandSlot.GetComponentInChildren<ArrowInstantiationLocation>();

            Animator bowAnimator = playerWeaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();
            bowAnimator.SetBool("isDrawn", false);
            bowAnimator.Play("Bow_TH_Fire_01");
            Destroy(playerFXManager.currentRangeFX);//Destroy Loaded arrow

            playerAnimtorManager.PlayTargetAnimation("Bow_TH_Fire_01_R",true);
            playerAnimtorManager.animator.SetBool("isHoldingArrow", false);

            GameObject liveArrow = Instantiate(playerInventoryManager.currentAmmo.liveModel, arrowInstantiationLocation.transform.position, cameraHandler.cameraPivotTransform.rotation);
            Rigidbody rigidBody = liveArrow.GetComponentInChildren<Rigidbody>();
            RangedProjectileDamageCollider damageCollider= liveArrow.GetComponentInChildren<RangedProjectileDamageCollider>();

            if(cameraHandler.currentLockOnTarget != null)
            {
                Quaternion arrowRotaion = Quaternion.LookRotation(transform.forward);
                liveArrow.transform.rotation = arrowRotaion;
            }
            else
            {
                liveArrow.transform.rotation = Quaternion.Euler(cameraHandler.cameraPivotTransform.eulerAngles.x, playerManager.lockOnTransform.eulerAngles.y, 0);
            }
            rigidBody.AddForce(liveArrow.transform.forward * playerInventoryManager.currentAmmo.forwardVelocity);
            rigidBody.AddForce(liveArrow.transform.up * playerInventoryManager.currentAmmo.upWardVelocity);
            rigidBody.useGravity = playerInventoryManager.currentAmmo.useGravity;
            rigidBody.mass = playerInventoryManager.currentAmmo.ammoMass;
            liveArrow.transform.parent = null;

            damageCollider.characterManager = playerManager;
            damageCollider.ammoItem = playerInventoryManager.currentAmmo;
            damageCollider.weaponDamage = playerInventoryManager.currentAmmo.physicalDamage;


        }
        private void PerformRTTRangedAction()
        {
            if(playerStatsManager.currentStamina <= 0)
            {
                return;
            }
            playerAnimtorManager.EraseHandIKfromWeapon();
            playerAnimtorManager.animator.SetBool("isUsingRightHand", true);
            if (!playerManager.isHoldingArrow)
            {
                if (playerInventoryManager.currentAmmo != null)
                {
                    DrawArrowAction();
                }
                else
                {
                    playerAnimtorManager.PlayTargetAnimation("Failed Cast", true);
                }
            }
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

                    playerAnimtorManager.PlayTargetAnimation("Back Stab",true);
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
                    playerAnimtorManager.PlayTargetAnimation("Riposte", true);
                    enemycharacterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Riposted", true);
                }
            }
        }

        private void PerformLBAimingAction()
        {
           // playerAnimtorManager.animator.SetBool("isAiming", true);
        }

        private void PerformMagicAction(WeaponItem weapon,bool isLeftHanded)
        {
            if (playerManager.isInteracting)
                return;
            if (weapon.weaponTypes == WeaponTypes.PyromancyCaster ||
                       weapon.weaponTypes == WeaponTypes.FaithCaster
                       )
            {
                playerInventoryManager.currentSpell = weapon.spellOfItem;
            }
            else
            {
                playerInventoryManager.currentSpell = null;
            }
            if (weapon.weaponTypes == WeaponTypes.FaithCaster)
            {
                if(playerInventoryManager.currentSpell != null && playerInventoryManager.currentSpell.isFaithSpell)
                {
                    if(playerStatsManager.currentFocus >= playerInventoryManager.currentSpell.focusPointCost){
                        playerInventoryManager.currentSpell.AttemptToCastSpell(playerAnimtorManager, playerStatsManager,playerWeaponSlotManager,isLeftHanded,cameraHandler);
                    }
                    else
                    {
                        playerAnimtorManager.PlayTargetAnimation("Failed Cast", true);
                    }
                  
                }
            }
            else if(weapon.weaponTypes == WeaponTypes.PyromancyCaster)
            {
                if (playerInventoryManager.currentSpell != null && playerInventoryManager.currentSpell.isPyroSpell)
                {
                    if (playerStatsManager.currentFocus >= playerInventoryManager.currentSpell.focusPointCost)
                    {
                        playerInventoryManager.currentSpell.AttemptToCastSpell(playerAnimtorManager, playerStatsManager, playerWeaponSlotManager,isLeftHanded,cameraHandler);
                    }
                    else
                    {
                        playerAnimtorManager.PlayTargetAnimation("Failed Cast", true);
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
                playerAnimtorManager.PlayTargetAnimation(weaponArtShield, true);

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
            playerAnimtorManager.PlayTargetAnimation("Block Start", false, true);
            playerEquipmentManager.OpenBlockingCollider();
            playerManager.isBlocking = true;
        }

        private void SuccessfullycastedSpell()
        {
            playerInventoryManager.currentSpell.SuccessfullyCastedSpell(playerAnimtorManager, playerStatsManager,cameraHandler,playerWeaponSlotManager,playerManager.isUsingLeftHand);
            playerAnimtorManager.animator.SetBool("isFiringSpell", true);
        }
    }
}
