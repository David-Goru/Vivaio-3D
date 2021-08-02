using UnityEngine;

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
