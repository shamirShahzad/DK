using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {


        PlayerManager player;
        



        public new Rigidbody rigidbody;
        public GameObject normalCamera;



        [Header("Stats")]
        [SerializeField]
        float movementSpeed = 5;
        [SerializeField]
        float rotationSpeed = 10;
        [SerializeField]
        float sprintSpeed = 8;
        [SerializeField]
        float walkingSpeed = 6;

        [Header("Locomotion Stamina Costs")]
        [SerializeField]
        int rollStaminaCost = 15;
        [SerializeField]
        int jumpStaminaCost = 20;
        [SerializeField]
        int backStepStaminacost = 10;
        [SerializeField]
        int sprintStaminaCost = 3;


        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }
        protected override void Start()
        {
            base.Start();
        }

        #region Movement


        public void HandleRotation() {
            if (player.canRotate)
            {
                if (player.isAiming)
                {
                    Quaternion targetRotation = Quaternion.Euler(0, player.cameraHandler.cameraTransform.eulerAngles.y, 0);
                    Quaternion playerRotatin = Quaternion.Slerp(transform.rotation,targetRotation,rotationSpeed * Time.deltaTime);
                    transform.rotation = playerRotatin;
                }
                else
                {
                    if (player.inputHandler.lockOnFlag)
                    {
                        if (player.isSprinting || player.inputHandler.rollFlag)
                        {
                            Vector3 targetDir = Vector3.zero;

                            targetDir = player.cameraHandler.cameraTransform.forward * player.inputHandler.vertical;
                            targetDir += player.cameraHandler.cameraTransform.right * player.inputHandler.horizontal;
                            targetDir.Normalize();
                            targetDir.y = 0;
                            if (targetDir == Vector3.zero)
                            {
                                targetDir = transform.forward;
                            }

                            Quaternion tr = Quaternion.LookRotation(targetDir);
                            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);

                            transform.rotation = targetRotation;
                        }
                        else
                        {
                            Vector3 rotationDirection = moveDirection;
                            rotationDirection = player.cameraHandler.currentLockOnTarget.transform.position - transform.position;
                            rotationDirection.y = 0;
                            rotationDirection.Normalize();

                            Quaternion tr = Quaternion.LookRotation(rotationDirection);
                            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);
                            transform.rotation = targetRotation;
                        }

                    }
                    else
                    {
                        Vector3 targetDir = Vector3.zero;

                        float moveOverride = player.inputHandler.moveAmount;
                        targetDir = player.cameraHandler.cameraObject.transform.forward * player.inputHandler.vertical;
                        targetDir += player.cameraHandler.cameraObject.transform.right * player.inputHandler.horizontal;
                        targetDir.Normalize();
                        targetDir.y = 0;
                        if (targetDir == Vector3.zero)
                            targetDir = player.transform.forward;

                        float rs = rotationSpeed;
                        Quaternion tr = Quaternion.LookRotation(targetDir);
                        Quaternion targetRotation = Quaternion.Slerp(player.transform.rotation, tr, rs * Time.deltaTime);
                        player.transform.rotation = targetRotation;

                    }
                }
             
            }
            
        }

        public void HandleGroundedMovement()
        {

            if (player.inputHandler.rollFlag)
                return;
            if (player.isInteracting)
                return;

            if (!player.isGrounded)
                return;
            moveDirection = player.cameraHandler.transform.forward * player.inputHandler.vertical;
            moveDirection = moveDirection + player.cameraHandler.transform.right * player.inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;
            player.characterSFXManager.PlayRandomFootstepSound();
            

            if (player.isSprinting && player.inputHandler.moveAmount > 0.5f)
            {
                player.isSprinting = true;
                player.characterController.Move(moveDirection * sprintSpeed * Time.deltaTime);
                player.playerStatsManager.DeductSprintingStamina(sprintStaminaCost);
            }
            else
            {
                if(player.inputHandler.moveAmount > 0.5f)
                {
                    player.characterController.Move(moveDirection * movementSpeed * Time.deltaTime);
                }
                else if(player.inputHandler.moveAmount <= 0.5f)
                {
                    player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
                }
            }

            if (player.inputHandler.lockOnFlag && player.isSprinting == false)
            {
                player.playerAnimatorManager.updateAnimatorValues(player.inputHandler.vertical, player.inputHandler.horizontal, player.isSprinting);
            }
            else
            {
                player.playerAnimatorManager.updateAnimatorValues(player.inputHandler.moveAmount, 0, player.isSprinting);
            }
        }

        public void HandleRolling() {
            if (this.enabled == false)
                return;
            if (player.isInteracting) {
                player.inputHandler.rollFlag = false;
                return;
            }
            if(player.playerStatsManager.currentStamina <= 0)
            {
                player.inputHandler.rollFlag = false;
                return;
            }
                


            if(player.inputHandler.rollFlag)
            {
                player.inputHandler.rollFlag = false;
                moveDirection = player.cameraHandler.cameraObject.transform.forward * player.inputHandler.vertical;
                moveDirection += player.cameraHandler.cameraObject.transform.right * player.inputHandler.horizontal;
                if (player.inputHandler.moveAmount > 0)
                {
                    player.playerAnimatorManager.PlayTargetAnimation("Roll", true);
                    player.playerAnimatorManager.EraseHandIKfromWeapon();
                    moveDirection.y = 0;
                    Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                    player.transform.rotation = rollRotation;
                    player.playerStatsManager.DeductStamina(rollStaminaCost);
                }
                else {
                    player.playerAnimatorManager.PlayTargetAnimation("BackStep", true);
                    player.playerStatsManager.DeductStamina(backStepStaminacost);
                    player.playerAnimatorManager.EraseHandIKfromWeapon();
                }
            }
        }



        public void HandleJumping()
        {
            if (this.enabled == false || this.gameObject == null)
                return;
            if (player.isInteracting)
            {
                player.inputHandler.jump_input = false;
                return;
            }
            if (player.playerStatsManager.currentStamina <= 0)
                return;

            if (player.inputHandler.jump_input)
            {
                player.inputHandler.jump_input = false;
                if(player.inputHandler.moveAmount > 0)
                {
                    moveDirection = player.cameraHandler.cameraObject.transform.forward * player.inputHandler.vertical;
                    moveDirection += player.cameraHandler.cameraObject.transform.right * player.inputHandler.horizontal;
                    player.playerAnimatorManager.PlayTargetAnimation("Jump", true);
                    player.playerAnimatorManager.EraseHandIKfromWeapon();
                    moveDirection.y = 0;
                    Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
                    player.transform.rotation = jumpRotation;
                    player.playerStatsManager.DeductStamina(jumpStaminaCost);
                }
            }
        }


        #endregion


    }
}
