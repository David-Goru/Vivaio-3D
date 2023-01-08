using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public List<ItemData> itemsOnWorld;
    public PlayerData player;
    public FarmData farm;
    public AIData ai;

    public GameData()
    {
        itemsOnWorld = new List<ItemData>();
        player = new PlayerData();
        farm = new FarmData();
        ai = new AIData();
    }
}