using UnityEngine;

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
            System.Reflection.PropertyInfo info = itemData.GetType().GetProperty(specificValues[i].VariableName);
            if (info != null) info.SetValue(itemData, specificValues[i].Value);
        }

        Item item = Instantiate(info.WorldModel, transform.position, transform.rotation).GetComponent<Item>();
        item.Info = info;
        item.Data = itemData;
    }
}

[System.Serializable]
public class SpecificValue
{
    [SerializeField] private string variableName;
    [SerializeField] private int value;

    public string VariableName { get => variableName; set => variableName = value; }
    public int Value { get => value; set => this.value = value; }
}