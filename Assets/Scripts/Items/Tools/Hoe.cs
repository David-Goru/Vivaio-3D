using UnityEngine;

public class Hoe : Item
{
    public override void Use(Player player)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, player.PlayerObject.PlowableLayer))
        {
            if (player.CheckDistance(hit.point))
            {
                player.LastAnimation = "PLOW";
                Plow(hit, player);
            }
        }
    }

    public void Plow(RaycastHit tile, Player player)
    {
        bool plowed = tile.transform.GetComponent<FarmObject>().PlowAt(tile.point);
        if (plowed)
        {
            player.BlockMovement();
            player.PlayerObject.SetAnimation("PLOW");
        }
    }
}