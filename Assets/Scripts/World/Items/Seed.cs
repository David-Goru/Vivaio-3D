using UnityEngine;

public class Seed : Item
{
    public override void Use(Player player)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, player.CropPositionLayer))
        {
            if (player.CheckDistance(hit.point)) Plant(hit.transform, player);
        }
    }

    public void Plant(Transform cropTransform, Player player)
    {
        bool planted = Game.Instance.Farm.PlantAt((CropInfo)Info, cropTransform.position);
        if (planted)
        {
            player.Block(() => player.Inventory.ReduceCurrentItemStack(1));
            player.Animations.Set(AnimationType.PLOW); // AnimationType.PLANT
        }
    }
}