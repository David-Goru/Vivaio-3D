using UnityEngine;

public class Hoe : Item
{
    public override void Use(Player player)
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hit, 100, player.farmTileLayer)) return;
        if (player.CheckDistance(hit.point)) Plow(hit, player);
    }

    private void Plow(RaycastHit tile, Player player)
    {
        var plowed = tile.transform.GetComponent<Farm>().PlowAt(tile.point);
        if (!plowed) return;
        player.Block();
        player.Animations.Set(AnimationType.PLOW);
    }
}