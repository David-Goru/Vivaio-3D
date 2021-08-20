using UnityEngine;

public class WorldItem : MonoBehaviour
{
    private Item item;
    public Item Item { get => item; set => item = value; }

    public Item PickUp(int amount)
    {
        if (amount >= item.Stack) Destroy(gameObject);
        return item;
    }
}