using UnityEngine;
namespace DK
{
    public class ResetAnimatorBool : StateMachineBehaviour
    {
        public string isInvulnerable = "isInvulnerable";
        public bool isInvulnerableStatus = false;
        public string isInteractingBool = "isInteracting";
        public bool isInteractingStatus = false;
        
        public string isPerformingFullyChargedAttack = "isPerformingFullyChargedAttack";
        public bool isPerformingFullyChargedAttackStatus = false;

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
            CharacterManager characterManager = animator.GetComponent<CharacterManager>();

            characterManager.isAttacking = false;
            characterManager.isUsingLeftHand = false;
            characterManager.isUsingRightHand = false;
            characterManager.isBeingBackStabbed = false;
            characterManager.isBeingRiposted = false;
            characterManager.isPerformingBackStab = false;
            characterManager.isPerformingRiposte = false;
            characterManager.canBeParried = false;
            characterManager.canBeRiposted = false;
            characterManager.characterWeaponSlotManager.CloseDamageCollider();

            characterManager.isUsingRightHand = false;
            characterManager.isUsingLeftHand = false;
            animator.SetBool(isInteractingBool, isInteractingStatus);
            animator.SetBool(isRotatingWithRootMotion, isRotatingWithRootMotionStatus);
            animator.SetBool(isFiringSpellBool, isFiringStatus);
            animator.SetBool(canRotateBool, canRotateStatus);
            animator.SetBool(isInvulnerable, isInvulnerableStatus);
            animator.SetBool(isMirrored, isMirroredStatus);
            animator.SetBool(isPerformingFullyChargedAttack, isPerformingFullyChargedAttackStatus);
        }

    }
}
