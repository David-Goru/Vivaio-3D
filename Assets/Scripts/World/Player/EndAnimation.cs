using UnityEngine;

public class EndAnimation : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player player = animator.transform.parent.GetComponent<Player>();
        player.LastAnimation = AnimationType.IDLE;
        player.UnblockMovement();
    }
}
