using UnityEngine;
namespace DK
{
    public class ResetAnimatorBool : StateMachineBehaviour
    {
        public string isInvulnerable = "isInvulnerable";
        public bool isInvulnerableStatus = false;
        public string isInteractingBool = "isInteracting";
        public bool isInteractingStatus = false;

        public string isFiringSpellBool = "isFiringSpell";
        public bool isFiringStatus = false;

        public string canRotateBool = "canRotate";
        public bool canRotateStatus = true;

        public string isRotatingWithRootMotion = "isRotatingWithRootMotion";
        public bool isRotatingWithRootMotionStatus = false;

        public string isUsingRightHand = "isUsingRightHand";
        public bool isUsingRightHandStatus = false;

        public string isUsingLeftHand = "isUsingLeftHand";
        public bool isUsingLeftHandStatus = false;

        public string isMirrored = "isMirrored";
        public bool isMirroredStatus = false;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(isInteractingBool, isInteractingStatus);
            animator.SetBool(isRotatingWithRootMotion, isRotatingWithRootMotionStatus);
            animator.SetBool(isFiringSpellBool, isFiringStatus);
            animator.SetBool(canRotateBool, canRotateStatus);
            animator.SetBool(isInvulnerable, isInvulnerableStatus);
            animator.SetBool(isUsingRightHand, isUsingRightHandStatus);
            animator.SetBool(isUsingLeftHand, isUsingLeftHandStatus);
            animator.SetBool(isMirrored, isMirroredStatus);
        }

    }
}
