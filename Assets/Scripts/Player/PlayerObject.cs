using UnityEngine;
using System.Collections.Generic;

public class PlayerObject : MonoBehaviour
{
    public AppearanceElementSelector[] AppearanceElementSelectors;
    public LayerMask PlowableLayer;
    public Transform RightHandModel;
    public Transform LeftHandModel;
    public Transform PlayerModel;

    [System.NonSerialized] public Player Data;
    private string lastAnimation = "IDLE";
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
        if (lastAnimation == animation) return;

        animator.SetTrigger(animation);
        lastAnimation = animation;
    }

    public float DistanceFrom(Vector3 targetPosition)
    {
        return Vector3.Distance(transform.position, targetPosition);
    }

    public void SetModel(List<AppearanceElement> elements)
    {
        foreach (AppearanceElement element in elements)
        {
            if (element.OptionSelected == "None") continue;
            GameObject elementObject = PlayerModel.Find(element.BodyPartName).Find(element.OptionSelected).gameObject;
            elementObject.SetActive(true);
            elementObject.GetComponent<SkinnedMeshRenderer>().material.color = element.Color;
        }
    }

    public void HideBodyElement(AppearanceElement element)
    {
        if (element.OptionSelected == "None") return;

        Transform model = transform.Find("Player model");
        model.Find(element.BodyPartName).Find(element.OptionSelected).gameObject.SetActive(false);
    }

    public void ShowBodyElement(AppearanceElement element)
    {
        if (element.OptionSelected == "None") return;

        Transform model = transform.Find("Player model");
        model.Find(element.BodyPartName).Find(element.OptionSelected).gameObject.SetActive(true);
    }

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

    public void PickUpItem(Item newItem)
    {
        if (currentItem != null) return; // Check if can stack

        newItem.PickUp();
        mainHandModel.transform.Find(newItem.Name).gameObject.SetActive(true);

        currentItem = newItem;
    }

    public void DropCurrentItem()
    {
        currentItem.Drop(transform.position);
        mainHandModel.transform.Find(currentItem.Name).gameObject.SetActive(false);
    }
}

public enum HandType
{
    RIGHT,
    LEFT,
    DEFAULT
}

public class Item
{
    private string name;
    private GameObject model;

    public string Name { get => name; }
    public GameObject Model { get => model; }

    public Item(GameObject model)
    {
        name = model.name;
        this.model = model;
    }

    public void PickUp()
    {
        MonoBehaviour.Destroy(model);
    }

    public void Drop(Vector3 playerPosition)
    {
        // Instantiate model on floor?
    }
}