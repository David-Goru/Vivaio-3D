using UnityEngine;

public class PlayerAnimations
{
    private readonly Player player;
    private readonly Transform rightHandModel;
    private readonly Transform leftHandModel;
    private AnimationType lastAnimation = AnimationType.IDLE;
    private readonly Animator animator;
    private Transform mainHandModel;
    private readonly int handLayer;

    public PlayerAnimations(Player player, Transform rightHandModel, Transform leftHandModel)
    {
        this.player = player;
        this.rightHandModel = rightHandModel;
        this.leftHandModel = leftHandModel;

        animator = player.model.GetComponent<Animator>();
        handLayer = animator.GetLayerIndex("MainHandInUse");
        ChangeMainHand(player.data.mainHand);
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
        var itemInHandModel = mainHandModel.Find(itemName);
        if (itemInHandModel == null) itemInHandModel = mainHandModel.Find("Item");

        return itemInHandModel;
    }

    private void ChangeMainHand(HandType type = HandType.RIGHT)
    {
        if (player.data != null) player.data.mainHand = type;
        animator.SetBool("LeftHand", type == HandType.LEFT);

        ChangeHandState(type == HandType.RIGHT ? rightHandModel : leftHandModel, true);
        ChangeHandState(type == HandType.RIGHT ? leftHandModel : rightHandModel, false);
    }

    public void SetHandInUse()
    {
        ChangeHandWeight(1.0f);
    }

    public void UnSetHandInUse()
    {
        ChangeHandWeight(0.0f);
    }

    private void ChangeHandState(Transform hand, bool newState)
    {
        if (newState == true) mainHandModel = hand;
        hand.gameObject.SetActive(newState);

        if (player.data?.itemInHand == null) return;
        var itemInHand = hand.Find(player.data.itemInHand.name);
        if (itemInHand != null) itemInHand.gameObject.SetActive(newState);
        else hand.Find("Item").gameObject.SetActive(newState);
    }

    private void ChangeHandWeight(float weightValue)
    {
        animator.SetLayerWeight(handLayer, weightValue);
    }
}