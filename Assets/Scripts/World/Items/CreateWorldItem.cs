using UnityEngine;
using System.Reflection;

public class CreateWorldItem : MonoBehaviour
{
    [SerializeField] private ItemInfo info;
    [SerializeField] private int startingStack = 1;
    [SerializeField] private SpecificValue[] specificValues;

    private void Start()
    {
        var dataType = System.Type.GetType(info.className + "Data") ?? System.Type.GetType("ItemData");
        var itemData = (ItemData)System.Activator.CreateInstance(dataType);

        itemData.name = info.name;
        itemData.currentStack = startingStack;

        foreach (var specificValue in specificValues)
        {
            var field = itemData.GetType().GetField(specificValue.variableName);
            if (field != null) field.SetValue(itemData, specificValue.value);
        }

        var item = Instantiate(info.worldModel, transform.position, transform.rotation).GetComponent<Item>();
        item.info = info;
        item.data = itemData;

        Destroy(gameObject);
    }
}

[System.Serializable]
public class SpecificValue
{
    public string variableName;
    public int value;
}