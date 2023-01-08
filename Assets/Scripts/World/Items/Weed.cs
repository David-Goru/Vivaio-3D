public class Weed : Item
{
    public override Item PickUp(int amount)
    {
        Game.Instance.farm.PullWeed(this);
        return base.PickUp(amount);
    }
}