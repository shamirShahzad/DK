using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
namespace DK
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        public CharacterManager character;
        protected RigBuilder rigBuilder;
        public TwoBoneIKConstraint leftHandConstraint;
        public TwoBoneIKConstraint rightHandConstraint;
        public PlayerManager player;

        bool handleIKWeightsReset = false;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
            player = GetComponent<PlayerManager>();
        }
        public void PlayTargetAnimation(string targetAnim, bool isInteracting,bool canRotate = false,bool mirrorAnimation  = false)
        {
            rigBuilder = GetComponent<RigBuilder>();
            character.animator.applyRootMotion = isInteracting;
            character.animator.SetBool("canRotate", canRotate);
            character.animator.SetBool("isInteracting", isInteracting);
            character.animator.SetBool("isMirrored", mirrorAnimation);
            character.animator.CrossFade(targetAnim, 0.2f);
            canRotate = false;
        }

        public void PlayTargetAnimationWithRootrotation(string targetAnim, bool isInteracting)
        {
            character.animator.applyRootMotion = isInteracting;
            character.animator.SetBool("isRotatingWithRootMotion", true);
            character.animator.SetBool("isInteracting", isInteracting);
            character.animator.CrossFade(targetAnim, 0.2f);
            character.canRotate = false;
        }
        public virtual void CanRotate()
        {
            character.animator.SetBool("canRotate", true);

        }

        public virtual void StopRotate()
        {
            character.animator.SetBool("canRotate", false);
        }

        public  virtual void EnableCombo()
        {
            character.animator.SetBool("canDoCombo", true);
        }
        public virtual void DisableCombo()
        {
            character.animator.SetBool("canDoCombo", false);
        }

        public virtual void EnableIsInvulnerable()
        {
            character.animator.SetBool("isInvulnerable", true);
        }
        public virtual void DisableIsInvulnerable()
        {
            character.animator.SetBool("isInvulnerable", false);
        }
        public virtual void EnableIsParrying()
        {
            character.isParrying = true;
        }
        public virtual void DisableIsParrying()
        {
            character.isParrying = false;
        }

        public virtual void EnableCanBeRiposted()
        {
            character.canBeRiposted = true;
        }
        public virtual void DisableCanBeRiposted()
        {
            character.canBeRiposted = false;
        }

        public virtual void TakeCriticalDamageAnimationEvent()
        {
            character.characterStatsManager.TakeDamageNoAnimation(character.pendingCriticalDamage,0,0,0,0);
            character.pendingCriticalDamage = 0;
        }

        public virtual void SetHandIKForWeapon(RightHandRigTarget rightHandRigTarget,LeftHandRigTarget leftHandRigTarget,bool isTwoHanding)
        {
            if (isTwoHanding)
            {
                if (rightHandRigTarget != null)
                {
                    rightHandConstraint.data.target = rightHandRigTarget.transform;
                    rightHandConstraint.data.targetPositionWeight = 1;
                    rightHandConstraint.data.targetRotationWeight = 1;
                }
                
                if(leftHandRigTarget != null)
                {
                    leftHandConstraint.data.target = leftHandRigTarget.transform;
                    leftHandConstraint.data.targetPositionWeight = 1;
                    leftHandConstraint.data.targetRotationWeight = 1;
                }  
            }
            else
            {
                rightHandConstraint.data.target = null;
                leftHandConstraint.data.target = null;
            }
            if (rigBuilder != null)
            {
                rigBuilder.Build();
            }
            
        }

        public virtual void CheckHandIKWeight(RightHandRigTarget rightHandRigTarget,LeftHandRigTarget leftHandRigTarget, bool isTwoHanding)
        {
            if (character.isInteracting)
            {
                return;
            }
            if (handleIKWeightsReset)
            {
                handleIKWeightsReset = false;

                if (rightHandConstraint.data.target != null)
                {
                    rightHandConstraint.data.target = rightHandRigTarget.transform;
                    rightHandConstraint.data.targetPositionWeight = 1;
                    rightHandConstraint.data.targetRotationWeight = 1;
                }
                if (leftHandConstraint.data.target != null)
                {
                    leftHandConstraint.data.target = leftHandRigTarget.transform;
                    leftHandConstraint.data.targetPositionWeight = 1;
                    leftHandConstraint.data.targetRotationWeight = 1;
                }
            }
        }
        public virtual void EraseHandIKfromWeapon()
        {
            handleIKWeightsReset = true;
            if(rightHandConstraint.data.target != null)
            {
                rightHandConstraint.data.targetPositionWeight = 0;
                rightHandConstraint.data.targetRotationWeight = 0;
            }
            if(leftHandConstraint.data.target != null)
            {
                leftHandConstraint.data.targetPositionWeight = 0;
                leftHandConstraint.data.targetRotationWeight = 0;
            }
            
        }

        protected virtual void OnAnimatorMove()
        {
            if (character.isInteracting == false)
                return;
            Vector3 velocity = character.animator.deltaPosition;
            character.characterController.Move(velocity);
            character.transform.rotation *= character.animator.deltaRotation;
        }
    }
}
