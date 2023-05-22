using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class ResetAnimatorBoolAI : ResetAnimatorBool
    {

        public string isPhaseShifting = "isPhaseShifting";
        public bool isPhaseShiftingStatus = false;
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //CharacterManager characterManager = animator.GetComponent<CharacterManager>();
            //characterManager.isBeingBackStabbed = false;
            //characterManager.isBeingRiposted = false;
            //characterManager.isPerformingBackStab = false;
            //characterManager.isPerformingRiposte = false;
            //characterManager.canBeRiposted = false;
            //characterManager.canBeParried = false;
            //characterManager.isAttacking = false;

            base.OnStateEnter(animator, stateInfo, layerIndex);
            animator.SetBool(isPhaseShifting, isPhaseShiftingStatus);
        }

    }
}
