using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class PlayerAnimatorManager : CharacterAnimatorManager
    {

        int vertical;
        int horizontal;
        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
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

            player.animator.SetFloat(vertical, v, 0.1f, Time.deltaTime);
            player.animator.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
        }

        public void DisableCollision()
        {
            player.fogWallCollider.enabled = false;
            player.fogEntryCollider.enabled = false;
        }
        public void EnableCollisison()
        {
            player.fogWallCollider.enabled = true;
            player.fogEntryCollider.enabled = true;
        }
        public void AwardSoulsOnDeath()
        {
        }

        private void OnAnimatorMove()
        {
            if (character.isInteracting == false)
                return;
            float delta = Time.deltaTime;
            player.playerLocomotion.rigidbody.drag = 0;
            Vector3 deltaPositions = player.animator.deltaPosition;
            deltaPositions.y = 0;
            Vector3 velocity = deltaPositions / delta;
            player.playerLocomotion.rigidbody.velocity = velocity;

        }
    }
}
