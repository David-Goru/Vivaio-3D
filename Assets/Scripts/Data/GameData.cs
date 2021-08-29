using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    private List<ItemData> itemsOnWorld;
    private PlayerData player;
    private FarmData farm;
    private AIData ai;

    public List<ItemData> ItemsOnWorld { get => itemsOnWorld; set => itemsOnWorld = value; }
    public PlayerData Player { get => player; set => player = value; }
    public FarmData Farm { get => farm; set => farm = value; }
    public AIData AI { get => ai; set => ai = value; }

    public GameData()
    {
        itemsOnWorld = new List<ItemData>();
        player = new PlayerData();
        farm = new FarmData();
        ai = new AIData();
    }
}