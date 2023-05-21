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

        PlayerControls inputActions;
        PlayerManager player;

        

        Vector2 movementInput;  
        Vector2 cameraInput;




        private void Start()
        {
            player = GetComponent<PlayerManager>();
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
            if (this.enabled == false || this == null)
                return;
            if (player.isDead)
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
            if(player.isHoldingArrow)
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
                if (player.playerStatsManager.currentStamina<=0)
                {
                    b_input = false; 
                    sprintFlag = false;           
                }
                if (moveAmount > 0.5 && player.playerStatsManager.currentStamina > 0)
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
                if(player.playerInventoryManager.rightWeapon.tap_RB_Action != null)
                {
                    tap_rb_input = false;
                    player.UpdateWhichHandCharacterIsUsing(true);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                    player.playerInventoryManager.rightWeapon.tap_RB_Action.PerformAction(player);
                }
                
            }
           
            
        }

        private void HandleTapRTInput()
        {
            if (tap_rt_input)
            {
                tap_rt_input = false;
                if(player.playerInventoryManager.rightWeapon.tap_RT_Action != null)
                {
                    player.UpdateWhichHandCharacterIsUsing(true);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                    player.playerInventoryManager.rightWeapon.tap_RT_Action.PerformAction(player);
                }
               
            }
        }

        private void HandleHoldRTInput()
        {
            
            if (hold_rt_Input)
            {
                if(player.playerInventoryManager.rightWeapon.hold_RT_Action != null)
                {
                    player.UpdateWhichHandCharacterIsUsing(true);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                    player.playerInventoryManager.rightWeapon.hold_RT_Action.PerformAction(player);
                }
                
            }
        }

        private void HandleHoldRBInput()
        {
            player.animator.SetBool("isCharging", hold_rb_Input);
            if (hold_rb_Input)
            {
              
                    player.UpdateWhichHandCharacterIsUsing(true);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                    
                    if (player.isTwoHanding)
                    {
                        if (player.playerInventoryManager.rightWeapon.th_hold_RB_Action != null)
                        {
                            player.playerInventoryManager.rightWeapon.th_hold_RB_Action.PerformAction(player);
                        }
                    }
                    else
                    {
                    if (player.playerInventoryManager.rightWeapon.hold_RB_Action != null)
                    {
                        player.playerInventoryManager.rightWeapon.hold_RB_Action.PerformAction(player);
                    }
                    }
                

            }
        }

        private void HandleTapLTInput()
        {
            if (tap_lt_input)
            {
                tap_lt_input = false;
                if (player.isTwoHanding)
                {
                    //lt will be right hand weapon
                    if(player.playerInventoryManager.rightWeapon.tap_LT_Action != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(true);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                        player.playerInventoryManager.rightWeapon.tap_LT_Action.PerformAction(player);
                    }
                    
                }
                else
                {
                    if(player.playerInventoryManager.leftWeapon.tap_LT_Action != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(false);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.leftWeapon;
                        player.playerInventoryManager.leftWeapon.tap_LT_Action.PerformAction(player);
                    }
                    
                }
            }
        }

        private void HandleHoldLBInput()
        {
            if (player.isInAir || player.isSprinting ||
                player.isFiringSpell)
            {
                lb_input = false;
                return;
            }

            if (lb_input)
            {
                if (player.isTwoHanding)
                {
                    if(player.playerInventoryManager.rightWeapon.hold_LB_Action != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(true);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                        player.playerInventoryManager.rightWeapon.hold_LB_Action.PerformAction(player);
                    }
                   
                }
                else
                {
                    if(player.playerInventoryManager.leftWeapon.hold_LB_Action != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(false);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.leftWeapon;
                        player.playerInventoryManager.leftWeapon.hold_LB_Action.PerformAction(player);
                    }
                    
                }
            }
            else if (lb_input == false)
            {
                if (player.isAiming)
                {
                    player.isAiming = false;
                    player.uIManager.aimCrosshair.SetActive(false);
                    player.cameraHandler.ResetAimCamRotations();
                }
                if (player.isBlocking)
                {
                    player.isBlocking = false;
                    
                }
            }
        }

        private void HandleTapLBInput()
        {
            if (tap_lb_input)
            {
                
                tap_lb_input = false;
                if (player.isTwoHanding)
                {
                    if(player.playerInventoryManager.rightWeapon.tap_LB_Action != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(true);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                        player.playerInventoryManager.rightWeapon.tap_LB_Action.PerformAction(player);
                    }
                    
                }
                else
                {
                    if (player.playerInventoryManager.leftWeapon.tap_LB_Action != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(false);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.leftWeapon;
                        player.playerInventoryManager.leftWeapon.tap_LB_Action.PerformAction(player);
                    }
                }

            }

        }

        private void HandleLockOnInput()
        {
            if(lockOnInput && lockOnFlag == false)
            {
                lockOnInput = false;
                player.cameraHandler.HandleLockOn();
                if(player.cameraHandler.nearestLockOnTarget != null)
                {
                    player.cameraHandler.currentLockOnTarget = player.cameraHandler.nearestLockOnTarget;
                    lockOnFlag = true;
                }
            }
            else if(lockOnInput && lockOnFlag == true)
            {
                lockOnInput = false;
                lockOnFlag = false;
                player.cameraHandler.ClearLockOnTarget();
            }
            if (lockOnFlag && right_Stick_Left_Input)
            {
                right_Stick_Left_Input = false;
                player.cameraHandler.HandleLockOn();
                if(player.cameraHandler.leftLockTarget != null)
                {
                    player.cameraHandler.currentLockOnTarget = player.cameraHandler.leftLockTarget;
                }
            }
            if(lockOnFlag && right_Stick_Right_Input)
            {
                right_Stick_Right_Input = false;
                player.cameraHandler.HandleLockOn();
                if(player.cameraHandler.rightLockTarget != null)
                {
                    player.cameraHandler.currentLockOnTarget = player.cameraHandler.rightLockTarget;
                }
            }

            player.cameraHandler.SetCameraHeight();
        }

        private void HandleTwoHandInput()
        {
            if(y_input)
            {
                y_input = false;
                twoHandFlag = !twoHandFlag;
                if(twoHandFlag)
                {
                    player.isTwoHanding = true;
                    player.playerWeaponSlotManager.LoadWeaponOnSlot(player.playerInventoryManager.rightWeapon, false);
                    player.playerWeaponSlotManager.LoadTwoHandIKTargets(true);
                }
                else
                {
                    player.isTwoHanding = false;
                    player.playerWeaponSlotManager.LoadWeaponOnSlot(player.playerInventoryManager.rightWeapon, false);
                    player.playerWeaponSlotManager.LoadWeaponOnSlot(player.playerInventoryManager.leftWeapon, true);
                    player.playerWeaponSlotManager.LoadTwoHandIKTargets(false);
                    
                }
            }
        }

        private void HandleConsumableInput()
        {
            if (x_input)
            {
                x_input = false;
                if (player.playerFXManager.isDrinking)
                    return;
                player.playerInventoryManager.currentConsumable.AttemptToConsumeItems(player.playerAnimatorManager, player.playerWeaponSlotManager, player.playerFXManager);
            }
        }
        
    }
}
