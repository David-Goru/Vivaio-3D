using UnityEngine;

public class Item : MonoBehaviour
{
    [HideInInspector] public ItemInfo info;
    [HideInInspector] public ItemData data;

    public virtual void Use(Player player) { }
    public virtual void UpdateVisuals() { }
    public virtual void Reset() { }

    public virtual Item PickUp(int amount)
    {
        if (amount >= data.currentStack) Destroy(gameObject);
        return this;
    }
}