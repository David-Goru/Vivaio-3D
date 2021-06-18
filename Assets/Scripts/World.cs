using UnityEngine;
using System.IO;

public class World : MonoBehaviour
{
    public static World Instance;
    public static FileStream GameFile;

    private GameData data;
    public GameData Data { get => data; set => data = value; }

    public GameObject UI;
    public GameObject Player;
    public GameObject Farm;
    public GameObject AI;

    private void Start()
    {
        Instance = this;

        if (GameFile == null) data = new GameData();
        else data = GameData.Deserialize(GameFile);

        data.Instantiate();
    }

    private void Update()
    {
        Data.UI.Update();
        Data.Player.Update();
        Data.Farm.Update();
        Data.AI.Update();
    }

    private void FixedUpdate()
    {
        Data.Player.FixedUpdate();
    }
}