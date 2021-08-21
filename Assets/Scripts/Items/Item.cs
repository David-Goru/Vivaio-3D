using UnityEngine;

[System.Serializable]
public class Item
{
    private string name;
    [System.NonSerialized] private GameObject worldObject;
    [System.NonSerialized] private GameObject handObject;
    [System.NonSerialized] private ItemModel itemModel;
    private int stack = 0;

    public string Name { get => name; set => name = value; }
    public GameObject HandObject { get => handObject; set => handObject = value; }
    public ItemModel ItemModel { get => itemModel; set => itemModel = value; }
    public int Stack { get => stack; set => stack = value; }

    public virtual void Use(Player player) { }

    public void Drop(Vector3 position, Quaternion rotation)
    {
        handObject = null;
        worldObject = Object.Instantiate(itemModel.WorldModel, position, rotation);
        worldObject.GetComponent<WorldItem>().Item = this;
    }
}
