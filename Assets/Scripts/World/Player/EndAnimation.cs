using UnityEngine;

public class EndAnimation : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Game.Instance.Player.Unblock();
    }
}