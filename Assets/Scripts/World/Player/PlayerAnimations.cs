using UnityEngine;

public class PlayerAnimations
{
    private Player player;
    private Transform rightHandModel;
    private Transform leftHandModel;
    private AnimationType lastAnimation = AnimationType.IDLE;
    private Animator animator;
    private Transform mainHandModel;
    private int handLayer;

    public PlayerAnimations(Player player, Transform rightHandModel, Transform leftHandModel)
    {
        this.player = player;
        this.rightHandModel = rightHandModel;
        this.leftHandModel = leftHandModel;

        animator = player.Model.GetComponent<Animator>();
        handLayer = animator.GetLayerIndex("MainHandInUse");
        ChangeMainHand(player.Data.MainHand);
    }

    public void Set(AnimationType newAnimation)
    {
        if (lastAnimation == newAnimation) return;

        lastAnimation = newAnimation;
        if (newAnimation == AnimationType.NONE) return;

        animator.SetTrigger(newAnimation.ToString());
    }

    public Transform GetHandModel(string itemName)
    {
        Transform itemInHandModel = mainHandModel.Find(itemName);
        if (itemInHandModel == null) itemInHandModel = mainHandModel.Find("Item");

        return itemInHandModel;
    }

    public void ChangeMainHand(HandType type = HandType.RIGHT)
    {
        if (player.Data != null) player.Data.MainHand = type;
        animator.SetBool("LeftHand", type == HandType.LEFT);

        changeHandState(type == HandType.RIGHT ? rightHandModel : leftHandModel, true);
        changeHandState(type == HandType.RIGHT ? leftHandModel : rightHandModel, false);
    }

    public void SetHandInUse()
    {
        changeHandWeight(1.0f);
    }

    public void UnSetHandInUse()
    {
        changeHandWeight(0.0f);
    }

    private void changeHandState(Transform hand, bool newState)
    {
        if (newState == true) mainHandModel = hand;
        hand.gameObject.SetActive(newState);

        if (player.Data != null && player.Data.ItemInHand != null)
        {
            Transform itemInHand = hand.Find(player.Data.ItemInHand.Name);
            if (itemInHand != null) itemInHand.gameObject.SetActive(newState);
            else hand.Find("Item").gameObject.SetActive(newState);
        }
    }

    private void changeHandWeight(float weightValue)
    {
        animator.SetLayerWeight(handLayer, weightValue);
    }
}