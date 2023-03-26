using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class PlayerManager : CharacterManager
    {
        inputHandler inputHandler;
        CameraHandler cameraHandler;
        Animator anim;
        PlayerLocomotion playerLocomotion;
        InteractableUi interactableUi;
        PlayerStats playerStats;

        public GameObject interactableUiGameObject;
        public GameObject itemInteractableGameobject;

        public bool isInteracting;
        [Header("Player Flags")]
        public bool isSprinting;
        public bool isInAir;
        public bool isGrounded;
        public bool canDoCombo;
        public bool isUsingRightHand;
        public bool isUsingLeftHand;
        public bool isInvulnerable;

        void Start()
        {
            cameraHandler = FindObjectOfType<CameraHandler>();
            inputHandler = GetComponent<inputHandler>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            anim = GetComponentInChildren<Animator>();
            interactableUi = FindObjectOfType<InteractableUi>();
            playerStats = GetComponent<PlayerStats>();
            backStabCollider = GetComponentInChildren<BackStabColliders>();

        }

       
        void Update()
        {
            float delta = Time.deltaTime;

            isInteracting = anim.GetBool("isInteracting");
            canDoCombo = anim.GetBool("canDoCombo");
            isUsingRightHand = anim.GetBool("isUsingRightHand");
            isUsingLeftHand = anim.GetBool("isUsingLeftHand");
            isInvulnerable = anim.GetBool("isInvulnerable");
            anim.SetBool("isInAir", isInAir);
            anim.SetBool("isDead", playerStats.isDead);
            inputHandler.TickInput(delta);
            playerLocomotion.HandleRolling(delta);
            playerLocomotion.HandleJumping();

            playerStats.RegenarateStamina();
            playerStats.RegenarateFocus();
            CheckForInteractable();

        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;


            playerLocomotion.HandleMovement(delta);
            playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
           

        }

        private void LateUpdate()
        {
            inputHandler.rollFlag = false;
            inputHandler.rb_input = false;
            inputHandler.rt_input = false;
            inputHandler.a_input = false;
            inputHandler.jump_input = false;

            float delta = Time.deltaTime;

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotations(delta, inputHandler.mouseX, inputHandler.mouseY);
            }

            if (isInAir)
            {
                playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
            }


        }

        public void CheckForInteractable()
        {
            RaycastHit hit;

            if (Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f, cameraHandler.ignoreLayers))
            {
                if (hit.collider.tag.Equals("Interactable"))
                {
                    Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                    if(interactableObject != null)
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
                if(itemInteractableGameobject != null && inputHandler.a_input)
                {
                    itemInteractableGameobject.SetActive(false);
                }
            }
        }
    }
}
