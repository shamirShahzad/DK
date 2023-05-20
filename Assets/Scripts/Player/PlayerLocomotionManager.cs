using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class PlayerLocomotionManager : MonoBehaviour
    {


        PlayerManager player;
        public Vector3 moveDirection;



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
            player = GetComponent<PlayerManager>();
            player.isGrounded = true;
            Physics.IgnoreCollision(characterCollider, characterCollisionBlocker, true);

        }

        #region Movement
        Vector3 normalVector;
        Vector3 targetPosition;

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
                        if (player.inputHandler.sprintFlag || player.inputHandler.rollFlag)
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

        public void HandleMovement()
        {

            if (player.inputHandler.rollFlag)
                return;
            if (player.isInteracting)
                return; 

            moveDirection = player.cameraHandler.cameraObject.transform.forward * player.inputHandler.vertical;
            moveDirection += player.cameraHandler.cameraObject.transform.right * player.inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            float speed = movementSpeed;
            if (player.inputHandler.sprintFlag && player.inputHandler.moveAmount > 0.5f)
            {
                speed = sprintSpeed;
                player.isSprinting = true;
                moveDirection *= speed;
                player.playerStatsManager.DeductStamina(sprintStaminaCost);
            }
            else {
                if (player.inputHandler.moveAmount <= 0.5f)
                {
                    moveDirection *= walkingSpeed;
                    player.isSprinting = false;
                }
                else
                {
                    moveDirection *= speed;
                    player.isSprinting = false;
                }
               
            }
            
            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);

            rigidbody.velocity = projectedVelocity;
            if (player.inputHandler.lockOnFlag && player.inputHandler.sprintFlag == false)
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
                return;
            }
            if(player.playerStatsManager.currentStamina <= 0)
                return;


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

        public void HandleFalling(Vector3 moveDirection)
        {
            player.isGrounded = false;
            RaycastHit hit;
            Vector3 origin = player.transform.position;
            origin.y += groundDetectionRayStartPoint;

            if(Physics.Raycast(origin,player.transform.forward,out hit,0.4f))
            {
                moveDirection = Vector3.zero;
            }
            if(player.isInAir)
            {
                rigidbody.AddForce(-Vector3.up * fallingSpeed);
                rigidbody.AddForce(moveDirection * fallingSpeed / 10f);
            }

            Vector3 dir = moveDirection;
            dir.Normalize();
            origin = origin + dir * groundDetectionRayDistance;

            targetPosition = player.transform.position;
            Debug.DrawRay(origin, -Vector3.up * minimumDistanceNeededToFall, Color.red, 0.1f, false);
            if(Physics.Raycast(origin,-Vector3.up,out hit,minimumDistanceNeededToFall,groundLayer))
            {
                normalVector = hit.normal;
                Vector3 tp = hit.point;
                player.isGrounded = true;
                targetPosition.y = tp.y;

                if(player.isInAir)
                {
                    if(inAirTimer > 0.5f)
                    {
                        player.playerAnimatorManager.PlayTargetAnimation("Land", true);
                        inAirTimer = 0;
                    }
                    else
                    {
                        player.playerAnimatorManager.PlayTargetAnimation("Empty", false);
                        inAirTimer = 0;
                    }
                    player.isInAir = false;
                }
            }
            else
            {
                if (player.isGrounded)
                {
                    player.isGrounded = false;
                }

                if(player.isInAir == false)
                {
                    if(player.isInteracting == false)
                    {
                        player.playerAnimatorManager.PlayTargetAnimation("Falling", true);
                    }

                    Vector3 velocity = rigidbody.velocity;
                    velocity.Normalize();
                    rigidbody.velocity = velocity * (movementSpeed / 2);
                    player.isInAir = true;
                }
            }
           
                if (player.isInteracting || player.inputHandler.moveAmount > 0)
                {
                    player.transform.position = Vector3.Lerp(player.transform.position, targetPosition, Time.deltaTime/0.1f);
                }
                else
                {
                    player.transform.position = targetPosition;
                }
            
        }

        public void HandleJumping()
        {
            if (this.enabled == false || this.gameObject == null)
                return;
            if (player.isInteracting)
                return;
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
