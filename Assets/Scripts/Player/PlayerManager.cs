using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class PlayerManager : CharacterManager
    {
        public CameraHandler cameraHandler;
        public PlayerLocomotionManager playerLocomotion;
        public PlayerAnimatorManager playerAnimatorManager;
        public PlayerWeaponSlotManager playerWeaponSlotManager;
        public PlayerFXManager playerFXManager;
        public InteractableUi interactableUi;
        public PlayerStatsManager playerStatsManager;
        public PlayerCombatManager playerCombatManager;
        public PlayerInventoryManager playerInventoryManager;
        public PlayerEquipmentManager playerEquipmentManager;
        public UIManager uIManager;
        
            
        public GameObject interactableUiGameObject;
        public GameObject itemInteractableGameobject;
        
        public BoxCollider fogWallCollider,fogEntryCollider;


        protected override void Awake()
        {
            base.Awake();
            cameraHandler = FindObjectOfType<CameraHandler>();
            inputHandler = GetComponent<inputHandler>();
            playerLocomotion = GetComponent<PlayerLocomotionManager>();
            interactableUi = FindObjectOfType<InteractableUi>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
             playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerFXManager = GetComponent<PlayerFXManager>();
            playerCombatManager = GetComponent<PlayerCombatManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            uIManager = FindObjectOfType<UIManager>();
        }       
        void Update()
        {
            if (playerLocomotion == null)
                return;
            isInteracting = animator.GetBool("isInteracting");
            canDoCombo = animator.GetBool("canDoCombo");
            canRotate = animator.GetBool("canRotate");
            isInvulnerable = animator.GetBool("isInvulnerable");
            isFiringSpell = animator.GetBool("isFiringSpell");
            isHoldingArrow = animator.GetBool("isHoldingArrow");
            isPerformingFullyChargedAttack = animator.GetBool("isPerformingFullyChargedAttack");
            animator.SetBool("isBlocking",isBlocking);
            animator.SetBool("isTwoHanding", isTwoHanding);
            animator.SetBool("isDead", isDead);
            inputHandler.TickInput();
            playerLocomotion.HandleRolling();
            playerLocomotion.HandleJumping();

            playerStatsManager.RegenarateStamina();
            playerStatsManager.RegenarateFocus();
            playerLocomotion.HandleGroundedMovement();
            playerLocomotion.HandleRotation();
            CheckForInteractable();
            if (cameraHandler == null)
                return;
            

        }

        protected override void FixedUpdate()
        {
            if (playerLocomotion == null || playerLocomotion.enabled == false)
                return;
            base.FixedUpdate();
            
            playerFXManager.HAndleAllBuildupEffects();
            
           

        }

        private void LateUpdate()
        {
            
            inputHandler.a_input = false;

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget();
                cameraHandler.HandleCameraRotation();
            }
            
        }

        public void OpenChestInteraction(Transform playerStandsHereWhenOpeningChest)
        {
            transform.position = playerStandsHereWhenOpeningChest.transform.position;
            playerAnimatorManager.PlayTargetAnimation("Open Chest", true);
        }

        public void CheckForInteractable()
        {
            RaycastHit hit;

            if (Physics.SphereCast(transform.position, 0.15f, transform.forward, out hit, 0.7f, cameraHandler.ignoreLayers))
            {
                if (hit.collider.tag.Equals("Interactable"))
                {
                    Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                    if (interactableObject != null)
                    {
                        if (interactableObject.isIntro)
                        {
                            string interactableText = interactableObject.interactableText;
                            interactableUi.interactableText.text = interactableText;
                            interactableUiGameObject.SetActive(true);
                        }
                        else
                        {
                            string interactableText = interactableObject.interactableText;
                            interactableUi.interactableText.text = interactableText;
                            interactableUiGameObject.SetActive(true);
                        }
                        if (inputHandler.a_input)
                        {
                            hit.collider.GetComponent<Interactable>().Interact(this);
                        }
                    }
                }
            }
            else
            {
                if (interactableUiGameObject != null)
                {
                    interactableUiGameObject.SetActive(false);
                }
                if (itemInteractableGameobject != null && inputHandler.a_input)
                {
                    itemInteractableGameobject.SetActive(false);
                }
            }
        }

        public void PassThroughtFoggWallInteraction(Transform fogWallTransform,
            BoxCollider fogEnterCollider,BoxCollider fogBlockCollider)
        {
            playerLocomotion.rigidbody.velocity = Vector3.zero;
            Vector3 rotationDirection = fogWallTransform.transform.forward;
            Quaternion turnRotation = Quaternion.LookRotation(rotationDirection);
            transform.rotation = turnRotation;
            fogWallCollider = fogBlockCollider;
            fogEntryCollider = fogEnterCollider;
            //fogEntryCollider.enabled = false;
            //fogWallCollider.enabled = false;
            playerAnimatorManager.PlayTargetAnimation("Pass Through Wall", true);     
        }



    }
}
