using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class CharacterLocomotionManager : MonoBehaviour
    {
        CharacterManager character;

        
        public Vector3 moveDirection;
        public LayerMask groundLayer;
        

        [Header("Gravity Settings")]
        public float inAirTimer;
        [SerializeField] protected Vector3 yVelocity;
        [SerializeField] protected float groundedYVelocity = -20f;
        [SerializeField] protected float fallStartYVelocity = -7f;
        [SerializeField] protected float gravityForce = -20f;
        [SerializeField] float groundCheckSphereRadius = 1;
        protected bool fallingVelocitySet = false;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }
        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {
            character.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer);
            character.animator.SetBool("isGrounded", character.isGrounded);
            HandleGroundCheck();
        }

        public virtual void HandleGroundCheck()
        {
            if (character.isGrounded)
            {
                if (yVelocity.y < 0)
                {
                    inAirTimer = 0;
                    fallingVelocitySet = false;
                    yVelocity.y = groundedYVelocity;
                }
            }
            else
            {
                if (!fallingVelocitySet)
                {
                    fallingVelocitySet = true;
                    yVelocity.y = fallStartYVelocity; 
                }
                inAirTimer = inAirTimer + Time.deltaTime;
                yVelocity.y += gravityForce * Time.deltaTime;
            }
            character.animator.SetFloat("isInAirTimer", inAirTimer);

            character.characterController.Move(yVelocity * Time.deltaTime);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(transform.position, groundCheckSphereRadius);
        }

    }
}
