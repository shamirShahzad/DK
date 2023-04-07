using UnityEngine;

public class ResetAnimatorBool : StateMachineBehaviour
{

    public string isInteractingBool  = "isInteracting";
    public bool isInteractingStatus = false;

    public string isFiringSpellBool = "isFiringSpell";
    public bool isFiringStatus = false;

    public string canRotateBool = "canRotate";
    public bool canRotateStatus = true;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(isInteractingBool, isInteractingStatus);
        animator.SetBool(isFiringSpellBool, isFiringStatus);
        animator.SetBool(canRotateBool, canRotateStatus);
    }

}
