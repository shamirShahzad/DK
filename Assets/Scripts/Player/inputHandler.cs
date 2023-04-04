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
        public bool rb_input;
        public bool lb_input;
        public bool rt_input;
        public bool lt_input;
        public bool criticalAttackInput;    
        public bool jump_input;
        public bool lockOnInput;
        public bool right_Stick_Right_Input;
        public bool right_Stick_Left_Input;

        public bool lockOnFlag;
        public bool twoHandFlag;
        public bool rollFlag;
        public bool sprintFlag;
        public bool comboFlag;
        public float rollInputTimer;

        public Transform criticalAttackRaycastStartPoint;


        PlayerControls inputActions;
        PlayerAttacker playerAttacker;
        PlayerInventory playerInventory;
        PlayerManager playerManager;
        WeaponSlotManager weaponSlotManager;
        PlayerStats playerStats;
        CameraHandler cameraHandler;
        PlayerAnimatorManager animatorHandler;
        

        Vector2 movementInput;  
        Vector2 cameraInput;




        private void Start()
        {
            playerAttacker = GetComponentInChildren<PlayerAttacker>();
            playerInventory = GetComponent<PlayerInventory>();
            playerManager = GetComponent<PlayerManager>();
            playerStats = GetComponent<PlayerStats>();
            cameraHandler = FindObjectOfType<CameraHandler>();
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
            animatorHandler = GetComponentInChildren<PlayerAnimatorManager>();
        }

        public void OnEnable()
        {

            if (inputActions == null) {

                inputActions = new PlayerControls();

                inputActions.PlayerMovement.Movement.performed+=inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed+=i=> cameraInput= i.ReadValue<Vector2>();
                
                inputActions.PlayerActions.RB.performed += i => rb_input = true;
                inputActions.PlayerActions.LB.performed += i => lb_input = true;
                inputActions.PlayerActions.RT.performed += i => rt_input = true;
                inputActions.PlayerActions.LT.performed += i => lt_input = true;
                inputActions.PlayerActions.A.performed += i => a_input = true;
                inputActions.PlayerActions.Roll.performed += i => b_input = true;
                inputActions.PlayerActions.Roll.canceled += i => b_input = false;
                inputActions.PlayerActions.Jump.performed += i => jump_input = true;
                inputActions.PlayerActions.LockOn.performed += i => lockOnInput = true;
                inputActions.PlayerMovement.LockOnTargetRight.performed += i => right_Stick_Right_Input = true;
                inputActions.PlayerMovement.LockOnTargetLeft.performed += i => right_Stick_Left_Input = true;
                inputActions.PlayerActions.Y.performed += i => y_input = true;
                inputActions.PlayerActions.CriticalAttack.performed += i => criticalAttackInput = true;
            }

            inputActions.Enable();
            
        }

        public void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            MoveInput(delta);
            HandleRollingInput(delta);
            HandleCombatInput(delta);
            HandleLockOnInput();
            HandleTwoHandInput();
            HandleCriticalAttackInput();

        }

        private void MoveInput(float delta)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;

            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

            mouseX = cameraInput.x;
            mouseY = cameraInput.y;


        }


        private void HandleRollingInput(float delta) {

            
            if (b_input) {
                rollInputTimer += delta;
                if (playerStats.currentStamina<=0)
                {
                    b_input = false; 
                    sprintFlag = false;           
                }
                if (moveAmount > 0.5 && playerStats.currentStamina > 0)
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

        private void HandleCombatInput(float delta)
        {
            

           //rb_input =  inputActions.PlayerActions.RB.IsPressed();
           //rt_input =  inputActions.PlayerActions.RT.IsPressed();

            if (rb_input)
            {
                playerAttacker.HandleRBAction();   
            }
            if (rt_input)
            {
                playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
            }
            if (lb_input)
            {
                //do block
            }
            if (lt_input)
            {
                if (twoHandFlag)
                {

                }
                else
                {
                    playerAttacker.HandleLTAction();
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
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
                }
                else
                {
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftWeapon, true);
                }
            }
        }

        private void HandleCriticalAttackInput()
        {
            if (criticalAttackInput)
            {
                criticalAttackInput = false;
               playerAttacker.AttemptBackStabOrRiposte();
            }

        }
        
    }
}
