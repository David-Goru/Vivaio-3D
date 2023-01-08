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
        player.data.itemInHand = itemInHand.data;
    }

    public void TryToPickUp(Item item)
    {
        if (player.data.itemInHand == null)
        {
            item.PickUp(item.data.currentStack);
            player.data.itemInHand = item.data;
            ShowNewItemInHand(item.info);
        }
        else
        {
            if (player.data.itemInHand.name != item.data.name) return;
            if (player.data.itemInHand.currentStack >= itemInHand.info.maxStack) return;

            var stackToTrade = item.data.currentStack;
            if (stackToTrade + player.data.itemInHand.currentStack > itemInHand.info.maxStack) stackToTrade = itemInHand.info.maxStack - player.data.itemInHand.currentStack;

            player.data.itemInHand.currentStack += stackToTrade;
            item.PickUp(stackToTrade);
        }

        if (itemInHand != null) itemInHand.data = player.data.itemInHand;

        player.Animations.SetHandInUse();
        //LastAnimation = "PICKUPITEM";
    }

    public void DropCurrentItem()
    {
        if (player.data.itemInHand == null) return;
        
        HideCurrentItemInHand();
        DropCurrentItemAtPlayerPosition();
        player.data.itemInHand = null;
        itemInHand = null;

        player.Animations.UnSetHandInUse();
        //LastAnimation = "DROPITEM";
    }

    public void ReduceCurrentItemStack(int amount)
    {
        itemInHand.data.currentStack -= amount;

        if (itemInHand.data.currentStack == 0)
        {
            HideCurrentItemInHand();
            player.Animations.UnSetHandInUse();
            itemInHand = null;
            player.data.itemInHand = null;
        }
    }

    private void ShowNewItemInHand(ItemInfo info)
    {
        var itemInHandModel = player.Animations.GetHandModel(info.handItemName);
        itemInHandModel.gameObject.SetActive(true);
        itemInHand = itemInHandModel.GetComponent<Item>();
        itemInHand.info = info;
    }

    private void HideCurrentItemInHand()
    {
        var itemInHandModel = player.Animations.GetHandModel(itemInHand.info.handItemName);
        itemInHandModel.gameObject.SetActive(false);
    }

    private void DropCurrentItemAtPlayerPosition()
    {
        var dropTransform = player.Animations.GetHandModel(itemInHand.info.handItemName);
        var itemOnWorld = Object.Instantiate(itemInHand.info.worldModel, dropTransform.position, dropTransform.rotation);
        itemOnWorld.GetComponent<Item>().data = player.data.itemInHand;
        itemOnWorld.GetComponent<Item>().info = itemInHand.info;
    }
}