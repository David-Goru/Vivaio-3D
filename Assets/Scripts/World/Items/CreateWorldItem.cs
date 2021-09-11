using UnityEngine;
using System.Reflection;

public class CreateWorldItem : MonoBehaviour
{
    [SerializeField] private ItemInfo info;
    [SerializeField] private int startingStack = 1;
    [SerializeField] private SpecificValue[] specificValues;

    private void Start()
    {
        System.Type dataType = System.Type.GetType(info.ClassName + "Data");
        if (dataType == null) dataType = System.Type.GetType("ItemData");
        ItemData itemData = (ItemData)System.Activator.CreateInstance(dataType);

        itemData.Name = info.name;
        itemData.CurrentStack = startingStack;

        for (int i = 0; i < specificValues.Length; i++)
        {
            FieldInfo field = itemData.GetType().GetField(specificValues[i].VariableName);
            if (field != null) field.SetValue(itemData, specificValues[i].Value);
        }

        Item item = Instantiate(info.WorldModel, transform.position, transform.rotation).GetComponent<Item>();
        item.Info = info;
        item.Data = itemData;

        Destroy(gameObject);
    }
}

[System.Serializable]
public class SpecificValue
{
    public string VariableName;
    public int Value;
}