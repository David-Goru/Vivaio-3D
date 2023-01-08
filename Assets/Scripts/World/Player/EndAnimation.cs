using UnityEngine;

public class EndAnimation : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Game.Instance.player.Unblock();
    }
}