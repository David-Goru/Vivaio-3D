using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Items/Item", order = 0)]
public class ItemInfo : ScriptableObject
{
    public string ClassName = "Item";
    public string HandItemName = "Item";
    public int MaxStack = 1;
    public GameObject WorldModel;
}