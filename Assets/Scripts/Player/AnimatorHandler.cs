using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class AnimatorHandler : AnimatorManager
    {
        PlayerManager playerManager;
        inputHandler inputHandler;
        PlayerLocomotion playerLocomotion;
        int vertical;
        int horizontal;
        public bool canRotate;


        public void Initialize() {
            anim = GetComponent<Animator>();
            inputHandler = GetComponentInParent<inputHandler>();
            playerLocomotion = GetComponentInParent<PlayerLocomotion>();
            playerManager = GetComponentInParent<PlayerManager>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");

        
        }

        public void updateAnimatorValues(float verticalMovement,float horizontalMovement,bool isSprinting) 
        {
            #region Vertical
            float v = 0;

            if (verticalMovement > 0 && verticalMovement < 0.55f)
            {
                v = 0.5f;

            }
            else if (verticalMovement > 0.55f)
            {
                v = 1;
            }
            else if (verticalMovement < 0 && verticalMovement > -0.55f)
            {
                v = -0.5f;
            }
            else if (verticalMovement < -0.55f)
            {
                v = -1;
            }
            else {
                v = 0;
            }
            #endregion

            #region Horizontal
            float h = 0;

            if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            {
                h = 0.5f;

            }
            else if (horizontalMovement > 0.55f)
            {
                h = 1;
            }
            else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
            {
                h = -0.5f;
            }
            else if (horizontalMovement < -0.55f)
            {
                h = -1;
            }
            else
            {
                h = 0;
            }
            #endregion

            if (isSprinting) {
                v = 2;
                h = horizontalMovement;
            }

            anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
            anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
        }

  
        public void CanRotate()
        {
            canRotate = true;

        }

        public void StopRotate()
        {
            canRotate = false;
        }

        public void EnableCombo()
        {
            anim.SetBool("canDoCombo", true);
        }

        public void EnableIsInvulnerable()
        {
            anim.SetBool("isInvulnerable", true);
        }   
        public void DisableIsInvulnerable()
        {
            anim.SetBool("isInvulnerable", false);
        }
        public void DisableCombo()
        {
            anim.SetBool("canDoCombo", false);
        }
        private void OnAnimatorMove()
        {
            if (playerManager.isInteracting == false)
                return;
            float delta = Time.deltaTime;
            playerLocomotion.rigidbody.drag = 0;
            Vector3 deltaPositions = anim.deltaPosition;
            deltaPositions.y = 0;
            Vector3 velocity = deltaPositions / delta;
            playerLocomotion.rigidbody.velocity = velocity;

        }
    }
}
