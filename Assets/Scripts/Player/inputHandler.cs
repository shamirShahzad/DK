using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DK
{
    public class inputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        [Header("Input Flags")]
        public bool b_input;
        public bool a_input;
        public bool y_input;
        public bool x_input;
        public bool tap_rb_input;
        public bool lb_input;
        public bool tap_lb_input;
        public bool tap_rt_input;
        public bool tap_lt_input;
        public bool hold_rt_Input;
        public bool hold_rb_Input;
        public bool jump_input;
        public bool lockOnInput;
        public bool right_Stick_Right_Input;
        public bool right_Stick_Left_Input;

        public bool lockOnFlag;
        public bool twoHandFlag;
        public bool fireFlag;
        public bool rollFlag;
        public bool sprintFlag;
        public bool comboFlag;
        public float rollInputTimer;

        public Transform criticalAttackRaycastStartPoint;


        PlayerControls inputActions;
        PlayerCombatManager playerCombatManager;
        PlayerInventoryManager playerInventoryManager;
        PlayerManager playerManager;
        PlayerFXManager playerFXManager;
        PlayerWeaponSlotManager weaponSlotManager;
        BlockingCollider blockingCollider;
        PlayerStatsManager playerStatsManager;
        CameraHandler cameraHandler;
        PlayerAnimatorManager playerAnimatorManager;
        public UIManager uIManager;
        

        Vector2 movementInput;  
        Vector2 cameraInput;




        private void Start()
        {

            playerCombatManager = GetComponent<PlayerCombatManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerFXManager = GetComponent<PlayerFXManager>();
            playerManager = GetComponent<PlayerManager>();
            uIManager = FindObjectOfType<UIManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            cameraHandler = FindObjectOfType<CameraHandler>();
            weaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            blockingCollider = GetComponentInChildren<BlockingCollider>();
        }

        public void OnEnable()
        {

            if (inputActions == null) {

                inputActions = new PlayerControls();

                inputActions.PlayerMovement.Movement.performed+=inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed+=i=> cameraInput= i.ReadValue<Vector2>();
                
                inputActions.PlayerActions.RB.performed += i => tap_rb_input = true;
                inputActions.PlayerActions.HoldRB.performed += i => hold_rb_Input = true;
                inputActions.PlayerActions.HoldRB.canceled += i => hold_rb_Input = false;
                inputActions.PlayerActions.TapLB.performed += i => tap_lb_input = true;
                inputActions.PlayerActions.LB.performed += i => lb_input = true;
                inputActions.PlayerActions.LB.canceled+= i => lb_input = false;
                inputActions.PlayerActions.RT.performed += i => tap_rt_input = true;
                inputActions.PlayerActions.HoldRT.performed += i => hold_rt_Input = true;
                inputActions.PlayerActions.HoldRT.canceled += i => hold_rt_Input = false;
                inputActions.PlayerActions.LT.performed += i => tap_lt_input = true;
                inputActions.PlayerActions.A.performed += i => a_input = true;
                inputActions.PlayerActions.X.performed += i => x_input = true;
                inputActions.PlayerActions.Roll.performed += i => b_input = true;
                inputActions.PlayerActions.Roll.canceled += i => b_input = false;
                inputActions.PlayerActions.Jump.performed += i => jump_input = true;
                inputActions.PlayerActions.LockOn.performed += i => lockOnInput = true;
                inputActions.PlayerMovement.LockOnTargetRight.performed += i => right_Stick_Right_Input = true;
                inputActions.PlayerMovement.LockOnTargetLeft.performed += i => right_Stick_Left_Input = true;
                inputActions.PlayerActions.Y.performed += i => y_input = true;
                //inputActions.PlayerActions.CriticalAttack.performed += i => criticalAttackInput = true;
            }

            inputActions.Enable();
            
        }

        public void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput()
        {
            if (playerStatsManager.isDead)
                return;
            MoveInput();
            HandleRollingInput();

            HandleHoldRTInput();

            

            HandleTapRBInput();
            HandleHoldRBInput();
            HandleTapRTInput();
            HandleTapLTInput();
            HandleTapLBInput();

            HandleHoldLBInput();
            HandleLockOnInput();
            HandleTwoHandInput();
            HandleConsumableInput();

        }

        private void MoveInput()
        {
            if(playerManager.isHoldingArrow)
            {
                horizontal = movementInput.x;
                vertical = movementInput.y;

                moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical)) ;

                if(moveAmount > 0.5f)
                {
                    moveAmount = 0.5f;
                }
                mouseX = cameraInput.x;
                mouseY = cameraInput.y;
            }
            else
            {
                horizontal = movementInput.x;
                vertical = movementInput.y;

                moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

                mouseX = cameraInput.x;
                mouseY = cameraInput.y;
            }
          


        }


        private void HandleRollingInput() {

            
            if (b_input) {
                rollInputTimer += Time.deltaTime;
                if (playerStatsManager.currentStamina<=0)
                {
                    b_input = false; 
                    sprintFlag = false;           
                }
                if (moveAmount > 0.5 && playerStatsManager.currentStamina > 0)
                {
                    sprintFlag = true;
                }

            }
            else
            {
                sprintFlag = false;
                if (rollInputTimer >0  && rollInputTimer<0.5f)
                {
                    rollFlag = true;
                }
                rollInputTimer = 0;
            }
        }

        private void HandleTapRBInput()
        {
            if (tap_rb_input)
            {
                if(playerInventoryManager.rightWeapon.tap_RB_Action != null)
                {
                    tap_rb_input = false;
                    playerManager.UpdateWhichHandCharacterIsUsing(true);
                    playerInventoryManager.currentItemBeingUsed = playerInventoryManager.rightWeapon;
                    playerInventoryManager.rightWeapon.tap_RB_Action.PerformAction(playerManager);
                }
                
            }
           
            
        }

        private void HandleTapRTInput()
        {
            if (tap_rt_input)
            {
                tap_rt_input = false;
                if(playerInventoryManager.rightWeapon.tap_RT_Action != null)
                {
                    playerManager.UpdateWhichHandCharacterIsUsing(true);
                    playerInventoryManager.currentItemBeingUsed = playerInventoryManager.rightWeapon;
                    playerInventoryManager.rightWeapon.tap_RT_Action.PerformAction(playerManager);
                }
               
            }
        }

        private void HandleHoldRTInput()
        {
            if (hold_rt_Input)
            {
                if(playerInventoryManager.rightWeapon.hold_RT_Action != null)
                {
                    playerManager.UpdateWhichHandCharacterIsUsing(true);
                    playerInventoryManager.currentItemBeingUsed = playerInventoryManager.rightWeapon;
                    playerInventoryManager.rightWeapon.hold_RT_Action.PerformAction(playerManager);
                }
                
            }
        }

        private void HandleHoldRBInput()
        {
            if (hold_rb_Input)
            {
                if (playerInventoryManager.rightWeapon.hold_RB_Action != null)
                {
                    playerManager.UpdateWhichHandCharacterIsUsing(true);
                    playerInventoryManager.currentItemBeingUsed = playerInventoryManager.rightWeapon;
                    playerInventoryManager.rightWeapon.hold_RB_Action.PerformAction(playerManager);
                }

            }
        }

        private void HandleTapLTInput()
        {
            if (tap_lt_input)
            {
                tap_lt_input = false;
                if (playerManager.isTwoHanding)
                {
                    //lt will be right hand weapon
                    if(playerInventoryManager.rightWeapon.tap_LT_Action != null)
                    {
                        playerManager.UpdateWhichHandCharacterIsUsing(true);
                        playerInventoryManager.currentItemBeingUsed = playerInventoryManager.rightWeapon;
                        playerInventoryManager.rightWeapon.tap_LT_Action.PerformAction(playerManager);
                    }
                    
                }
                else
                {
                    if(playerInventoryManager.leftWeapon.tap_LT_Action != null)
                    {
                        playerManager.UpdateWhichHandCharacterIsUsing(false);
                        playerInventoryManager.currentItemBeingUsed = playerInventoryManager.leftWeapon;
                        playerInventoryManager.leftWeapon.tap_LT_Action.PerformAction(playerManager);
                    }
                    
                }
            }
        }

        private void HandleHoldLBInput()
        {
            if (playerManager.isInAir || playerManager.isSprinting ||
                playerManager.isFiringSpell)
            {
                lb_input = false;
                return;
            }

            if (lb_input)
            {
                if (playerManager.isTwoHanding)
                {
                    if(playerInventoryManager.rightWeapon.hold_LB_Action != null)
                    {
                        playerManager.UpdateWhichHandCharacterIsUsing(true);
                        playerInventoryManager.currentItemBeingUsed = playerInventoryManager.rightWeapon;
                        playerInventoryManager.rightWeapon.hold_LB_Action.PerformAction(playerManager);
                    }
                   
                }
                else
                {
                    if(playerInventoryManager.leftWeapon.hold_LB_Action != null)
                    {
                        playerManager.UpdateWhichHandCharacterIsUsing(false);
                        playerInventoryManager.currentItemBeingUsed = playerInventoryManager.leftWeapon;
                        playerInventoryManager.leftWeapon.hold_LB_Action.PerformAction(playerManager);
                    }
                    
                }
            }
            else if (lb_input == false)
            {
                if (playerManager.isAiming)
                {
                    playerManager.isAiming = false;
                    uIManager.aimCrosshair.SetActive(false);
                    cameraHandler.ResetAimCamRotations();
                }
                if (blockingCollider.blockingBoxCollider.enabled)
                {
                    playerManager.isBlocking = false;
                    blockingCollider.DisableBlockingCollider();
                }
            }
        }

        private void HandleTapLBInput()
        {
            if (tap_lb_input)
            {
                
                tap_lb_input = false;
                if (playerManager.isTwoHanding)
                {
                    if(playerInventoryManager.rightWeapon.tap_LB_Action != null)
                    {
                        playerManager.UpdateWhichHandCharacterIsUsing(true);
                        playerInventoryManager.currentItemBeingUsed = playerInventoryManager.rightWeapon;
                        playerInventoryManager.rightWeapon.tap_LB_Action.PerformAction(playerManager);
                    }
                    
                }
                else
                {
                    if (playerInventoryManager.leftWeapon.tap_LB_Action != null)
                    {
                        playerManager.UpdateWhichHandCharacterIsUsing(false);
                        playerInventoryManager.currentItemBeingUsed = playerInventoryManager.leftWeapon;
                        playerInventoryManager.leftWeapon.tap_LB_Action.PerformAction(playerManager);
                    }
                }

            }

        }

        private void HandleLockOnInput()
        {
            if(lockOnInput && lockOnFlag == false)
            {
                lockOnInput = false;
                cameraHandler.HandleLockOn();
                if(cameraHandler.nearestLockOnTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
                    lockOnFlag = true;
                }
            }
            else if(lockOnInput && lockOnFlag == true)
            {
                lockOnInput = false;
                lockOnFlag = false;
                cameraHandler.ClearLockOnTarget();
            }
            if (lockOnFlag && right_Stick_Left_Input)
            {
                right_Stick_Left_Input = false;
                cameraHandler.HandleLockOn();
                if(cameraHandler.leftLockTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.leftLockTarget;
                }
            }
            if(lockOnFlag && right_Stick_Right_Input)
            {
                right_Stick_Right_Input = false;
                cameraHandler.HandleLockOn();
                if(cameraHandler.rightLockTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.rightLockTarget;
                }
            }

            cameraHandler.SetCameraHeight();
        }

        private void HandleTwoHandInput()
        {
            if(y_input)
            {
                y_input = false;
                twoHandFlag = !twoHandFlag;
                if(twoHandFlag)
                {
                    playerManager.isTwoHanding = true;
                    weaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
                    weaponSlotManager.LoadTwoHandIKTargets(true);
                }
                else
                {
                    playerManager.isTwoHanding = false;
                    weaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
                    weaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.leftWeapon, true);
                    weaponSlotManager.LoadTwoHandIKTargets(false);
                    
                }
            }
        }

        private void HandleConsumableInput()
        {
            if (x_input)
            {
                x_input = false;
                if (playerFXManager.isDrinking)
                    return;
                playerInventoryManager.currentConsumable.AttemptToConsumeItems(playerAnimatorManager, weaponSlotManager, playerFXManager);
            }
        }
        
    }
}
