using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class PlayerManager : CharacterManager
    {
        public inputHandler inputHandler;
        public CameraHandler cameraHandler;
        Animator animator;
        public PlayerLocomotionManager playerLocomotion;
        public PlayerAnimatorManager playerAnimatorManager;
        public PlayerWeaponSlotManager playerWeaponSlotManager;
        public PlayerFXManager playerFXManager;
        public InteractableUi interactableUi;
        public PlayerStatsManager playerStatsManager;
        public PlayerCombatManager playerCombatManager;
        public PlayerInventoryManager playerInventoryManager;
        public PlayerEquipmentManager playerEquipmentManager;
            
        public GameObject interactableUiGameObject;
        public GameObject itemInteractableGameobject;
        
        public BoxCollider fogWallCollider,fogEntryCollider;


        protected override void Awake()
        {
            base.Awake();
            cameraHandler = FindObjectOfType<CameraHandler>();
            inputHandler = GetComponent<inputHandler>();
            playerLocomotion = GetComponent<PlayerLocomotionManager>();
            animator = GetComponent<Animator>();
            interactableUi = FindObjectOfType<InteractableUi>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            backStabCollider = GetComponentInChildren<CriticalDamageCollider>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerFXManager = GetComponent<PlayerFXManager>();
            playerCombatManager = GetComponent<PlayerCombatManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        }       
        void Update()
        {
            float delta = Time.deltaTime;

            isInteracting = animator.GetBool("isInteracting");
            canDoCombo = animator.GetBool("canDoCombo");
            isInvulnerable = animator.GetBool("isInvulnerable");
            isFiringSpell = animator.GetBool("isFiringSpell");
            isHoldingArrow = animator.GetBool("isHoldingArrow");
            animator.SetBool("isBlocking",isBlocking);
            animator.SetBool("isInAir", isInAir);
            animator.SetBool("isTwoHanding", isTwoHanding);
            animator.SetBool("isDead", playerStatsManager.isDead);
            playerAnimatorManager.canRotate = animator.GetBool("canRotate");
            inputHandler.TickInput();
            playerLocomotion.HandleRolling();
            playerLocomotion.HandleJumping();

            playerStatsManager.RegenarateStamina();
            playerStatsManager.RegenarateFocus();
            CheckForInteractable();

        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            playerLocomotion.HandleFalling(playerLocomotion.moveDirection);
            playerLocomotion.HandleMovement();
            playerLocomotion.HandleRotation();
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

            if (isInAir)
            {
                playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
            }


        }



        public void OpenChestInteraction(Transform playerStandsHereWhenOpeningChest)
        {
            playerLocomotion.rigidbody.velocity = Vector3.zero;
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
                        string interactableText = interactableObject.interactableText;
                        interactableUi.interactableText.text = interactableText;
                        interactableUiGameObject.SetActive(true);

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
