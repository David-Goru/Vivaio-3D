using UnityEngine;
using System.Collections.Generic;

public class PlayerObject : MonoBehaviour
{
    public LayerMask PlowableLayer;
    public Transform RightHandModel;
    public Transform LeftHandModel;
    public Transform PlayerModel;
    public CharacterAppearance Appearance;

    [System.NonSerialized] public Player Data;
    private Animator animator;

    // Hands
    private int handLayer;
    private bool mainHandIsLeft;
    private Transform mainHandModel;
    private Item currentItem;

    private void Start()
    {
        animator = PlayerModel.GetComponent<Animator>();
        handLayer = animator.GetLayerIndex("MainHandInUse");
        ChangeMainHand(HandType.RIGHT);
        
        if (Game.Instance != null) setUpItemsHandModels();
    }

    public void UpdatePosition(Vector3 position)
    {
        transform.position = position;
    }

    public void Translate(Vector3 direction)
    {
        transform.Translate(direction);
    }

    public void LookAt(Vector3 direction)
    {
        Vector3 position = transform.position + direction * 1000;
        position = new Vector3(position.x, PlayerModel.position.y, position.z);
        PlayerModel.LookAt(position);
    }

    public void SetAnimation(string animation)
    {
        animator.SetTrigger(animation);
    }

    public float DistanceFrom(Vector3 targetPosition)
    {
        return Vector3.Distance(transform.position, targetPosition);
    }
    
    public void SetAppearance(List<AppearanceElement> appearanceElements)
    {
        Transform model = transform.Find("Player model");
        Appearance.SetAppearance(model, appearanceElements);
    }

    // PlayerHand
    public void SetHandInUse()
    {
        changeHandWeight(1.0f);
    }

    public void UnSetHandInUse()
    {
        changeHandWeight(0.0f);
    }

    public void ChangeMainHand(HandType type = HandType.DEFAULT)
    {
        if (type == HandType.LEFT) mainHandIsLeft = true;
        else if (type == HandType.RIGHT) mainHandIsLeft = false;
        else mainHandIsLeft = !mainHandIsLeft;

        if (mainHandIsLeft)
        {
            mainHandModel = LeftHandModel;
            if (currentItem != null)
            {
                LeftHandModel.Find(currentItem.Name).gameObject.SetActive(true);
                RightHandModel.Find(currentItem.Name).gameObject.SetActive(false);
            }
        }
        else
        {
            mainHandModel = RightHandModel;
            if (currentItem != null)
            {
                RightHandModel.Find(currentItem.Name).gameObject.SetActive(true);
                LeftHandModel.Find(currentItem.Name).gameObject.SetActive(false);
            }
        }

        animator.SetBool("LeftHand", mainHandIsLeft);
    }

    private void changeHandWeight(float weightValue)
    {
        animator.SetLayerWeight(handLayer, weightValue);
    }

    private void setUpItemsHandModels()
    {
        foreach (ItemModels itemModels in Game.Instance.ItemModels)
        {
            if (itemModels.CanBeInHand == false) continue;

            try
            {
                itemModels.HandModel = mainHandModel.Find(itemModels.Name).gameObject;
            }
            catch (UnityException e) { Debug.LogError(string.Format("Couldn't find {0} item in player hands. Error: {1}", itemModels.Name, e)); }
        }
    }

    public void PickUpItem(Item newItem)
    {
        if (currentItem != null) return; // Check if can stack

        newItem.PickUp();
        currentItem = newItem;
    }

    public void DropCurrentItem()
    {
        if (currentItem == null) return;

        currentItem.Drop(transform.position + Vector3.up * 0.5f + Vector3.forward * 0.25f + Vector3.right * (mainHandIsLeft ? -0.5f : 0.5f), transform.Find("Player model").rotation);
        currentItem = null;
    }

    public void UnblockMovement()
    {
        Data.UnblockMovement();
    }
}

public enum HandType
{
    RIGHT,
    LEFT,
    DEFAULT
}

public enum Animations
{
    IDLE,
    WALK,
    PLOW
}