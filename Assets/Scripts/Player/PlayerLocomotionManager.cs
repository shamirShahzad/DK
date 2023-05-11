using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class PlayerLocomotionManager : MonoBehaviour
    {
        CameraHandler cameraHandler;
        Transform cameraObject;
        inputHandler inputHandler;
        PlayerManager playerManager;
        PlayerStatsManager playerStatsManager;
        public Vector3 moveDirection;

        [HideInInspector]
        public Transform myTransform;
        [HideInInspector]
        public PlayerAnimatorManager playerAnimatorManager;

        public new Rigidbody rigidbody;
        public GameObject normalCamera;

        [Header("Ground and air detection")]
        [SerializeField]
        float groundDetectionRayStartPoint = 0.5f;
        [SerializeField]
        float minimumDistanceNeededToFall = 1f;
        [SerializeField]
        float groundDetectionRayDistance = 0.2f;
        public LayerMask groundLayer;
        public float inAirTimer;


        [Header("Stats")]
        [SerializeField]
        float movementSpeed = 5;
        [SerializeField]
        float rotationSpeed = 10;
        [SerializeField]
        float sprintSpeed = 8;
        [SerializeField]
        float fallingSpeed = 55;
        [SerializeField]
        float walkingSpeed = 6;

        public CapsuleCollider characterCollider;
        public CapsuleCollider characterCollisionBlocker;

        [Header("Locomotion Stamina Costs")]
        [SerializeField]
        int rollStaminaCost = 15;
        [SerializeField]
        int jumpStaminaCost = 20;
        [SerializeField]
        int backStepStaminacost = 10;
        [SerializeField]
        int sprintStaminaCost = 3;



        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            inputHandler = GetComponent<inputHandler>();
            playerManager = GetComponent<PlayerManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            cameraObject = Camera.main.transform;
            myTransform = this.transform;
            cameraHandler = FindObjectOfType<CameraHandler>();
            playerManager.isGrounded = true;
            Physics.IgnoreCollision(characterCollider, characterCollisionBlocker, true);

        }

        #region Movement
        Vector3 normalVector;
        Vector3 targetPosition;

        public void HandleRotation() {
            if (playerAnimatorManager.canRotate)
            {
                if (playerManager.isAiming)
                {
                    Quaternion targetRotation = Quaternion.Euler(0, cameraHandler.cameraTransform.eulerAngles.y, 0);
                    Quaternion playerRotatin = Quaternion.Slerp(transform.rotation,targetRotation,rotationSpeed * Time.deltaTime);
                    transform.rotation = playerRotatin;
                }
                else
                {
                    if (inputHandler.lockOnFlag)
                    {
                        if (inputHandler.sprintFlag || inputHandler.rollFlag)
                        {
                            Vector3 targetDir = Vector3.zero;

                            targetDir = cameraHandler.cameraTransform.forward * inputHandler.vertical;
                            targetDir += cameraHandler.cameraTransform.right * inputHandler.horizontal;
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
                            rotationDirection = cameraHandler.currentLockOnTarget.transform.position - transform.position;
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

                        float moveOverride = inputHandler.moveAmount;
                        targetDir = cameraObject.forward * inputHandler.vertical;
                        targetDir += cameraObject.right * inputHandler.horizontal;
                        targetDir.Normalize();
                        targetDir.y = 0;
                        if (targetDir == Vector3.zero)
                            targetDir = myTransform.forward;

                        float rs = rotationSpeed;
                        Quaternion tr = Quaternion.LookRotation(targetDir);
                        Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * Time.deltaTime);
                        myTransform.rotation = targetRotation;

                    }
                }
             
            }
            
        }

        public void HandleMovement()
        {

            if (inputHandler.rollFlag)
                return;
            if (playerManager.isInteracting)
                return; 

            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            float speed = movementSpeed;
            if (inputHandler.sprintFlag && inputHandler.moveAmount > 0.5f)
            {
                speed = sprintSpeed;
                playerManager.isSprinting = true;
                moveDirection *= speed;
                playerStatsManager.TakeStaminaDamage(sprintStaminaCost);
            }
            else {
                if (inputHandler.moveAmount <= 0.5f)
                {
                    moveDirection *= walkingSpeed;
                    playerManager.isSprinting = false;
                }
                else
                {
                    moveDirection *= speed;
                    playerManager.isSprinting = false;
                }
               
            }
            
            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);

            rigidbody.velocity = projectedVelocity;
            if (inputHandler.lockOnFlag && inputHandler.sprintFlag == false)
            {
                playerAnimatorManager.updateAnimatorValues(inputHandler.vertical, inputHandler.horizontal, playerManager.isSprinting);
            }
            else
            {
                playerAnimatorManager.updateAnimatorValues(inputHandler.moveAmount, 0, playerManager.isSprinting);
            }
        }

        public void HandleRolling() {
            if (playerAnimatorManager.animator.GetBool("isInteracting")) {
                return;
            }
            if(playerStatsManager.currentStamina <= 0)
                return;


            if(inputHandler.rollFlag)
            {
                inputHandler.rollFlag = false;
                moveDirection = cameraObject.forward * inputHandler.vertical;
                moveDirection += cameraObject.right * inputHandler.horizontal;
                if (inputHandler.moveAmount > 0)
                {
                    playerAnimatorManager.PlayTargetAnimation("Roll", true);
                    playerAnimatorManager.EraseHandIKfromWeapon();
                    moveDirection.y = 0;
                    Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                    myTransform.rotation = rollRotation;
                    playerStatsManager.TakeStaminaDamage(rollStaminaCost);
                }
                else {
                    playerAnimatorManager.PlayTargetAnimation("BackStep", true);
                    playerStatsManager.TakeStaminaDamage(backStepStaminacost);
                    playerAnimatorManager.EraseHandIKfromWeapon();
                }
            }
        }

        public void HandleFalling(Vector3 moveDirection)
        {
            playerManager.isGrounded = false;
            RaycastHit hit;
            Vector3 origin = myTransform.position;
            origin.y += groundDetectionRayStartPoint;

            if(Physics.Raycast(origin,myTransform.forward,out hit,0.4f))
            {
                moveDirection = Vector3.zero;
            }
            if(playerManager.isInAir)
            {
                rigidbody.AddForce(-Vector3.up * fallingSpeed);
                rigidbody.AddForce(moveDirection * fallingSpeed / 10f);
            }

            Vector3 dir = moveDirection;
            dir.Normalize();
            origin = origin + dir * groundDetectionRayDistance;

            targetPosition = myTransform.position;
            Debug.DrawRay(origin, -Vector3.up * minimumDistanceNeededToFall, Color.red, 0.1f, false);
            if(Physics.Raycast(origin,-Vector3.up,out hit,minimumDistanceNeededToFall,groundLayer))
            {
                normalVector = hit.normal;
                Vector3 tp = hit.point;
                playerManager.isGrounded = true;
                targetPosition.y = tp.y;

                if(playerManager.isInAir)
                {
                    if(inAirTimer > 0.5f)
                    {
                        playerAnimatorManager.PlayTargetAnimation("Land", true);
                        inAirTimer = 0;
                    }
                    else
                    {
                        playerAnimatorManager.PlayTargetAnimation("Empty", false);
                        inAirTimer = 0;
                    }
                    playerManager.isInAir = false;
                }
            }
            else
            {
                if (playerManager.isGrounded)
                {
                    playerManager.isGrounded = false;
                }

                if(playerManager.isInAir == false)
                {
                    if(playerManager.isInteracting == false)
                    {
                        playerAnimatorManager.PlayTargetAnimation("Falling", true);
                    }

                    Vector3 velocity = rigidbody.velocity;
                    velocity.Normalize();
                    rigidbody.velocity = velocity * (movementSpeed / 2);
                    playerManager.isInAir = true;
                }
            }
           
                if (playerManager.isInteracting || inputHandler.moveAmount > 0)
                {
                    myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime/0.1f);
                }
                else
                {
                    myTransform.position = targetPosition;
                }
            
        }

        public void HandleJumping()
        {
            if (playerManager.isInteracting)
                return;
            if (playerStatsManager.currentStamina <= 0)
                return;

            if (inputHandler.jump_input)
            {
                inputHandler.jump_input = false;
                if(inputHandler.moveAmount > 0)
                {
                    moveDirection = cameraObject.forward * inputHandler.vertical;
                    moveDirection += cameraObject.right * inputHandler.horizontal;
                    playerAnimatorManager.PlayTargetAnimation("Jump", true);
                    playerAnimatorManager.EraseHandIKfromWeapon();
                    moveDirection.y = 0;
                    Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
                    myTransform.rotation = jumpRotation;
                    playerStatsManager.TakeStaminaDamage(jumpStaminaCost);
                }
            }
        }


        #endregion


    }
}
