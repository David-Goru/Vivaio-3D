using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class Game : MonoBehaviour
{
    public static Game Instance;
    public static FileStream GameFile;

    [SerializeField] private ItemModelsList itemModelsList = null;
    public List<ItemModel> ItemModels { get => itemModelsList.ItemModels; }

    private GameData data;
    public GameData Data { get => data; set => data = value; }

    public GameObject[] GameElementGameObjects;

    private void Start()
    {
        Instance = this;

        if (GameFile == null)
        {
            data = new GameData();
            data.Create(this);
        }
        else data = GameData.Deserialize(GameFile);

        data.Instantiate();
    }

    public static ItemModel GetItemModels(string name)
    {
        return Instance.ItemModels.Find(x => x.name == name);
    }

    private void Update()
    {
        foreach (GameElement gameElement in Data.GameElements)
        {
            gameElement.Update();
        }
    }

    private void FixedUpdate()
    {
        foreach (GameElement gameElement in Data.GameElements)
        {
            gameElement.FixedUpdate();
        }
    }
}