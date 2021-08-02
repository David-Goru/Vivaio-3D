using UnityEngine;

public class EndAnimation : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerObject playerObject = animator.transform.parent.GetComponent<PlayerObject>();
        playerObject.UnblockMovement();
    }
}
