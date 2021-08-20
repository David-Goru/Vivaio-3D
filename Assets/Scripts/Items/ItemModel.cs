using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Items/Item", order = 0)]
public class ItemModel : ScriptableObject
{
    [SerializeField] private string className = "Item";
    [SerializeField] private GameObject worldModel;
    [SerializeField] private bool canBeInHand = false;
    [SerializeField] private int maxStack = 1;

    public string ClassName { get => className; set => className = value; }
    public GameObject WorldModel { get => worldModel; set => worldModel = value; }
    public bool CanBeInHand { get => canBeInHand; set => canBeInHand = value; }
    public int MaxStack { get => maxStack; set => maxStack = value; }
}