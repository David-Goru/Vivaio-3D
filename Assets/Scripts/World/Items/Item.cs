using UnityEngine;

public class Item : MonoBehaviour
{
    [HideInInspector] public ItemInfo Info;
    [HideInInspector] public ItemData Data;

    public virtual void Use(Player player) { }
    public virtual void UpdateVisuals() { }

    public virtual Item PickUp(int amount)
    {
        if (amount >= Data.CurrentStack) Destroy(gameObject);
        return this;
    }
}