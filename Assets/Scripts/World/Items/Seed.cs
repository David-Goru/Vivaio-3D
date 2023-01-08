using UnityEngine;

public class Seed : Item
{
    public override void Use(Player player)
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hit, 100, player.cropPositionLayer)) return;
        if (player.CheckDistance(hit.point)) Plant(hit.transform, player);
    }

    private void Plant(Transform cropTransform, Player player)
    {
        var planted = Game.Instance.farm.PlantAt((CropInfo)info, cropTransform.position);
        if (!planted) return;
        player.Block(() => player.Inventory.ReduceCurrentItemStack(1));
        player.Animations.Set(AnimationType.PLOW); // AnimationType.PLANT
    }
}