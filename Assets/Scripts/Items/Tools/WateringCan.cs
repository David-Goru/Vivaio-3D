using UnityEngine;

public class WateringCan : Item
{
    public override void Use(Player player)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, player.PlayerObject.FarmTileLayer))
        {
            if (player.CheckDistance(hit.point))
            {
                player.LastAnimation = "WATER";
                Water(hit, player);
            }
        }
    }

    public void Water(RaycastHit tile, Player player)
    {
        bool watered = tile.transform.GetComponent<FarmObject>().WaterAt(tile.point);
        if (watered)
        {
            player.BlockMovement();
            player.PlayerObject.SetAnimation("WATER");
        }
    }
}