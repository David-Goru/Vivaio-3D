public class Weed : Item
{
    public override Item PickUp(int amount)
    {
        Game.Instance.Farm.PullWeed(this);
        return base.PickUp(amount);
    }
}