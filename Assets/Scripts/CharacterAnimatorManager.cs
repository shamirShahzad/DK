using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
namespace DK
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        public Animator animator;
        public CharacterManager characterManager;
        protected CharacterStatsManager characterStatsManager;
        public bool canRotate;
        protected RigBuilder rigBuilder;
        public TwoBoneIKConstraint leftHandConstraint;
        public TwoBoneIKConstraint rightHandConstraint;

        bool handleIKWeightsReset = false;

        protected virtual void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
            characterStatsManager = GetComponent<CharacterStatsManager>();
        }
        public void PlayTargetAnimation(string targetAnim, bool isInteracting,bool canRotate = false,bool mirrorAnimation  = false)
        {
            rigBuilder = GetComponent<RigBuilder>();
            animator.applyRootMotion = isInteracting;
            animator.SetBool("canRotate", canRotate);
            animator.SetBool("isInteracting", isInteracting);
            animator.SetBool("isMirrored", mirrorAnimation);
            animator.CrossFade(targetAnim, 0.2f);
            canRotate = false;
        }

        public void PlayTargetAnimationWithRootrotation(string targetAnim, bool isInteracting)
        {
            animator.applyRootMotion = isInteracting;
            animator.SetBool("isRotatingWithRootMotion", true);
            animator.SetBool("isInteracting", isInteracting);
            animator.CrossFade(targetAnim, 0.2f);
            canRotate = false;
        }
        public virtual void CanRotate()
        {
            animator.SetBool("canRotate", true);

        }

        public virtual void StopRotate()
        {
            animator.SetBool("canRotate", false);
        }

        public  virtual void EnableCombo()
        {
            animator.SetBool("canDoCombo", true);
        }
        public virtual void DisableCombo()
        {
            animator.SetBool("canDoCombo", false);
        }

        public virtual void EnableIsInvulnerable()
        {
            animator.SetBool("isInvulnerable", true);
        }
        public virtual void DisableIsInvulnerable()
        {
            animator.SetBool("isInvulnerable", false);
        }
        public virtual void EnableIsParrying()
        {
            characterManager.isParrying = true;
        }
        public virtual void DisableIsParrying()
        {
            characterManager.isParrying = false;
        }

        public virtual void EnableCanBeRiposted()
        {
            characterManager.canBeRiposted = true;
        }
        public virtual void DisableCanBeRiposted()
        {
            characterManager.canBeRiposted = false;
        }

        public virtual void TakeCriticalDamageAnimationEvent()
        {
            characterStatsManager.TakeDamageNoAnimation(characterManager.pendingCriticalDamage);
            characterManager.pendingCriticalDamage = 0;
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

            rigBuilder.Build();
        }

        public virtual void CheckHandIKWeight(RightHandRigTarget rightHandRigTarget,LeftHandRigTarget leftHandRigTarget, bool isTwoHanding)
        {
            if (characterManager.isInteracting)
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
    }
}
