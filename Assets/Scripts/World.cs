using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class World : MonoBehaviour
{
    public static World Instance;
    public static FileStream GameFile;

    [SerializeField] private List<ItemModels> itemModels = null;
    public List<ItemModels> ItemModels { get => itemModels; set => itemModels = value; }

    private GameData data;
    public GameData Data { get => data; set => data = value; }

    public GameObject UI;
    public GameObject Player;
    public GameObject Farm;
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
        Data.AI.Update();
    }

    private void FixedUpdate()
    {
        Data.Player.FixedUpdate();
    }
}