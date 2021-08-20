using UnityEngine;

public class CreateWorldItem : MonoBehaviour
{
    [SerializeField] private ItemModel itemModel;
    [SerializeField] private int stack = 1;

    private void Start()
    {
        System.Type type = System.Type.GetType(itemModel.ClassName);
        if (type == null) type = System.Type.GetType("Item");

        Item item = (Item)System.Activator.CreateInstance(type);
        item.Name = itemModel.name;
        item.ItemModel = itemModel;
        item.Stack = stack;

        WorldItem worldItem = Instantiate(itemModel.WorldModel, transform.position, transform.rotation).GetComponent<WorldItem>();
        if (worldItem != null) worldItem.Item = item;
    }
}