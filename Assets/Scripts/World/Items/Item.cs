using UnityEngine;

public class Item : MonoBehaviour
{
    protected ItemInfo info;
    protected ItemData data;

    public ItemInfo Info { get => info; set => info = value; }
    public ItemData Data { get => data; set => data = value; }

    public virtual void Use(Player player) { }
    public virtual void UpdateVisuals() { }

    public Item PickUp(int amount)
    {
        if (amount >= data.CurrentStack) Destroy(gameObject);
        return this;
    }
}