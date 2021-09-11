using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public List<ItemData> ItemsOnWorld;
    public PlayerData Player;
    public FarmData Farm;
    public AIData AI;

    public GameData()
    {
        ItemsOnWorld = new List<ItemData>();
        Player = new PlayerData();
        Farm = new FarmData();
        AI = new AIData();
    }
}