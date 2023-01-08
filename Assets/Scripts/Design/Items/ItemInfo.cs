using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Items/Item", order = 0)]
public class ItemInfo : ScriptableObject
{
    public string className = "Item";
    public string handItemName = "Item";
    public int maxStack = 1;
    public GameObject worldModel;
}