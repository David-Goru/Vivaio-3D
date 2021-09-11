using UnityEngine;

public class PlayerInventory
{
    private Player player;
    private Item itemInHand;

    public PlayerInventory(Player player)
    {
        this.player = player;
    }

    public void LeftClick()
    {
        if (itemInHand != null)
        {
            itemInHand.Use(player);
            player.Data.ItemInHand = itemInHand.Data;
        }
    }

    public void TryToPickUp(Item item)
    {
        if (player.Data.ItemInHand != null)
        {
            if (player.Data.ItemInHand.Name != item.Data.Name) return;
            else if (player.Data.ItemInHand.CurrentStack >= itemInHand.Info.MaxStack) return;
        }

        if (player.Data.ItemInHand == null)
        {
            item.PickUp(item.Data.CurrentStack);
            player.Data.ItemInHand = item.Data;
            showNewItemInHand(item.Info);
        }
        else
        {
            int stackToTrade = item.Data.CurrentStack;
            if (stackToTrade + player.Data.ItemInHand.CurrentStack > itemInHand.Info.MaxStack) stackToTrade = itemInHand.Info.MaxStack - player.Data.ItemInHand.CurrentStack;

            player.Data.ItemInHand.CurrentStack += stackToTrade;
            item.PickUp(stackToTrade);
        }

        if (itemInHand != null) itemInHand.Data = player.Data.ItemInHand;

        player.Animations.SetHandInUse();
        //LastAnimation = "PICKUPITEM";
    }

    public void DropCurrentItem()
    {
        if (player.Data.ItemInHand == null) return;

        //LastAnimation = "DROPITEM";
        hideCurrentItemInHand();
        dropCurrentItemAtPlayerPosition();
        player.Data.ItemInHand = null;
        itemInHand = null;
    }

    public void ReduceCurrentItemStack(int amount)
    {
        itemInHand.Data.CurrentStack -= amount;

        if (itemInHand.Data.CurrentStack == 0)
        {
            hideCurrentItemInHand();
            itemInHand = null;
            player.Data.ItemInHand = null;
        }
    }

    private void showNewItemInHand(ItemInfo info)
    {
        Transform itemInHandModel = player.Animations.GetHandModel(info.HandItemName);
        itemInHandModel.gameObject.SetActive(true);
        itemInHand = itemInHandModel.GetComponent<Item>();
        itemInHand.Info = info;
    }

    private void hideCurrentItemInHand()
    {
        Transform itemInHandModel = player.Animations.GetHandModel(itemInHand.Info.HandItemName);
        itemInHandModel.gameObject.SetActive(false);
    }

    private void dropCurrentItemAtPlayerPosition()
    {
        Transform dropTransform = player.Animations.GetHandModel(itemInHand.Info.HandItemName);
        GameObject itemOnWorld = Object.Instantiate(itemInHand.Info.WorldModel, dropTransform.position, dropTransform.rotation);
        itemOnWorld.GetComponent<Item>().Data = player.Data.ItemInHand;
        itemOnWorld.GetComponent<Item>().Info = itemInHand.Info;
        player.Animations.UnSetHandInUse();
    }
}