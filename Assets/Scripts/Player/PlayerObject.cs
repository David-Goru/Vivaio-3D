using UnityEngine;
using System.Collections.Generic;

public class PlayerObject : MonoBehaviour
{
    public LayerMask PlowableLayer;
    public LayerMask WorldItemLayer;
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

    private void Start()
    {
        animator = PlayerModel.GetComponent<Animator>();
        handLayer = animator.GetLayerIndex("MainHandInUse");
        ChangeMainHand(HandType.RIGHT);
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

        if (mainHandModel != null) mainHandModel.gameObject.SetActive(false);
        if (mainHandIsLeft)
        {
            mainHandModel = LeftHandModel;
            if (Data != null && Data.ItemInHand != null)
            {
                Transform itemInLeftHand = LeftHandModel.Find(Data.ItemInHand.Name);
                if (itemInLeftHand != null) itemInLeftHand.gameObject.SetActive(true);

                Transform itemInRightHand = RightHandModel.Find(Data.ItemInHand.Name);
                if (itemInRightHand != null) itemInRightHand.gameObject.SetActive(false);
            }
        }
        else
        {
            mainHandModel = RightHandModel;
            if (Data != null && Data.ItemInHand != null)
            {
                Transform itemInRightHand = RightHandModel.Find(Data.ItemInHand.Name);
                if (itemInRightHand != null) itemInRightHand.gameObject.SetActive(true);

                Transform itemInLeftHand = LeftHandModel.Find(Data.ItemInHand.Name);
                if (itemInLeftHand != null) itemInLeftHand.gameObject.SetActive(false);
            }
        }
        mainHandModel.gameObject.SetActive(true);

        animator.SetBool("LeftHand", mainHandIsLeft);
    }

    private void changeHandWeight(float weightValue)
    {
        animator.SetLayerWeight(handLayer, weightValue);
    }

    public void ShowCurrentItemInHand()
    {
        Transform itemInHand = mainHandModel.Find(Data.ItemInHand.Name);
        if (itemInHand == null) return;

        mainHandModel.Find(Data.ItemInHand.Name).gameObject.SetActive(true);
    }

    public void HideCurrentItemInHand()
    {
        Transform itemInHand = mainHandModel.Find(Data.ItemInHand.Name);
        if (itemInHand == null) return;

        mainHandModel.Find(Data.ItemInHand.Name).gameObject.SetActive(false);
    }

    public void DropCurrentItemAtPlayerPosition()
    {
        Transform transformPosition = mainHandModel.Find(Data.ItemInHand.Name);
        if (transformPosition == null) transformPosition = transform;

        Data.ItemInHand.Drop(transformPosition.position, transformPosition.rotation);
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