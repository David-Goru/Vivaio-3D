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
        
        if (World.Instance != null) setUpItemsHandModels();
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

    private void setUpItemsHandModels()
    {
        foreach (ItemModels itemModels in World.Instance.ItemModels)
        {
            itemModels.HandModel = mainHandModel.Find(itemModels.Name).gameObject;
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
}

public enum HandType
{
    RIGHT,
    LEFT,
    DEFAULT
}

[System.Serializable]
public class Item
{
    private string name;
    [System.NonSerialized] private GameObject worldObject;
    [System.NonSerialized] private ItemModels itemModels;

    public string Name { get => name; set => name = value; }

    public Item(string name, ItemModels itemModels)
    {
        this.name = name;
        this.itemModels = itemModels;
    }

    public void PickUp()
    {
        if (worldObject != null) Object.Destroy(worldObject);
        itemModels.HandModel.SetActive(true);
    }

    public void Drop(Vector3 position, Quaternion rotation)
    {
        worldObject = Object.Instantiate(itemModels.WorldModel, position, rotation);
        worldObject.GetComponent<WorldItem>().Item = this;
        itemModels.HandModel.SetActive(false);
    }
}

[System.Serializable]
public class ItemModels
{
    [SerializeField] private string name;
    [SerializeField] private GameObject worldModel;
    private GameObject handModel;

    public string Name { get => name; set => name = value; }
    public GameObject WorldModel { get => worldModel; set => worldModel = value; }
    public GameObject HandModel { get => handModel; set => handModel = value; }

    public ItemModels(string name, GameObject worldModel, GameObject handModel)
    {
        this.name = name;
        this.worldModel = worldModel;
        this.handModel = handModel;
    }
}