using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Items/Item", order = 0)]
public class ItemInfo : ScriptableObject
{
    [SerializeField] private string className = "Item";
    [SerializeField] private int maxStack = 1;
    [SerializeField] private GameObject worldModel;

    public string ClassName { get => className; set => className = value; }
    public int MaxStack { get => maxStack; set => maxStack = value; }
    public GameObject WorldModel { get => worldModel; set => worldModel = value; }
}