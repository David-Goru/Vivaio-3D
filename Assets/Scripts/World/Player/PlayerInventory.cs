using UnityEngine;

public class PlayerInventory
{
    private readonly Player player;
    private Item itemInHand;

    public bool InventoryIsEmpty => itemInHand == null;

    public PlayerInventory(Player player)
    {
        this.player = player;
    }

    public void LeftClick()
    {
        if (itemInHand == null) return;
        itemInHand.Use(player);
        player.data.ItemInHand = itemInHand.data;
    }

    public void TryToPickUp(Item item)
    {
        if (player.data.ItemInHand == null)
        {
            item.PickUp(item.data.CurrentStack);
            player.data.ItemInHand = item.data;
            ShowNewItemInHand(item.info);
        }
        else
        {
            if (player.data.ItemInHand.Name != item.data.Name) return;
            if (player.data.ItemInHand.CurrentStack >= itemInHand.info.MaxStack) return;

            var stackToTrade = item.data.CurrentStack;
            if (stackToTrade + player.data.ItemInHand.CurrentStack > itemInHand.info.MaxStack) stackToTrade = itemInHand.info.MaxStack - player.data.ItemInHand.CurrentStack;

            player.data.ItemInHand.CurrentStack += stackToTrade;
            item.PickUp(stackToTrade);
        }

        if (itemInHand != null) itemInHand.data = player.data.ItemInHand;

        player.Animations.SetHandInUse();
        //LastAnimation = "PICKUPITEM";
    }

    public void DropCurrentItem()
    {
        if (player.data.ItemInHand == null) return;
        
        HideCurrentItemInHand();
        DropCurrentItemAtPlayerPosition();
        player.data.ItemInHand = null;
        itemInHand = null;

        player.Animations.UnSetHandInUse();
        //LastAnimation = "DROPITEM";
    }

    public void ReduceCurrentItemStack(int amount)
    {
        itemInHand.data.CurrentStack -= amount;

        if (itemInHand.data.CurrentStack == 0)
        {
            HideCurrentItemInHand();
            player.Animations.UnSetHandInUse();
            itemInHand = null;
            player.data.ItemInHand = null;
        }
    }

    private void ShowNewItemInHand(ItemInfo info)
    {
        var itemInHandModel = player.Animations.GetHandModel(info.HandItemName);
        itemInHandModel.gameObject.SetActive(true);
        itemInHand = itemInHandModel.GetComponent<Item>();
        itemInHand.info = info;
    }

    private void HideCurrentItemInHand()
    {
        var itemInHandModel = player.Animations.GetHandModel(itemInHand.info.HandItemName);
        itemInHandModel.gameObject.SetActive(false);
    }

    private void DropCurrentItemAtPlayerPosition()
    {
        var dropTransform = player.Animations.GetHandModel(itemInHand.info.HandItemName);
        var itemOnWorld = Object.Instantiate(itemInHand.info.WorldModel, dropTransform.position, dropTransform.rotation);
        itemOnWorld.GetComponent<Item>().data = player.data.ItemInHand;
        itemOnWorld.GetComponent<Item>().info = itemInHand.info;
    }
}