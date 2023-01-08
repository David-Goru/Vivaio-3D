using UnityEngine;

public class Item : MonoBehaviour
{
    [HideInInspector] public ItemInfo info;
    [HideInInspector] public ItemData data;

    public virtual void Use(Player player) { }
    public virtual void UpdateVisuals() { }

    public virtual Item PickUp(int amount)
    {
        if (amount >= data.CurrentStack) Destroy(gameObject);
        return this;
    }
}