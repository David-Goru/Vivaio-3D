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
        Vector3 cropPosition = cropTransform.position;
        Vector3 tilePosition = cropTransform.parent.position;

        bool planted = Game.Instance.Farm.PlantAt((CropInfo)info, tilePosition, cropPosition);
        if (planted)
        {
            player.BlockMovement();
            player.SetAnimation(AnimationType.PLOW); // AnimationType.PLANT
        }
    }
}