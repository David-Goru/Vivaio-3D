using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class Game : MonoBehaviour
{
    public static Game Instance;
    public static FileStream GameFile;

    [SerializeField] private List<ItemModels> itemModels = null;
    public List<ItemModels> ItemModels { get => itemModels; set => itemModels = value; }

    private GameData data;
    public GameData Data { get => data; set => data = value; }

    public GameObject Player;
    public GameObject Farm;
    public GameObject UI;
    public GameObject AI;

    private void Start()
    {
        Instance = this;

        if (GameFile == null)
        {
            data = new GameData();
            data.Create();
        }
        else data = GameData.Deserialize(GameFile);

        data.Instantiate();
    }

    public static ItemModels GetItemModels(string name)
    {
        return Instance.ItemModels.Find(x => x.Name == name);
    }

    private void Update()
    {
        Data.Player.Update();
        Data.Farm.Update();
        Data.UI.Update();
        Data.AI.Update();
    }

    private void FixedUpdate()
    {
        Data.Player.FixedUpdate();
    }
}