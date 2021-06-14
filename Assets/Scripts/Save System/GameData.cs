using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

[System.Serializable]
public class GameData
{
    private UI ui;
    private Player player;
    private Farm farm;
    private AI ai;

    public Player Player { get => player; set => player = value; }
    public Farm Farm { get => farm; set => farm = value; }
    public AI AI { get => ai; set => ai = value; }
    public UI UI { get => ui; set => ui = value; }

    public void Instantiate()
    {
        if (World.Instance.UI != null) ui = Object.Instantiate(World.Instance.UI).GetComponent<UI>();
        else Debug.Log("UI prefab not found on World instance.");
        if (World.Instance.Player != null) player = new Player(Object.Instantiate(World.Instance.Player));
        else Debug.Log("Player prefab not found on World instance.");
        if (World.Instance.Farm != null) farm = new Farm(Object.Instantiate(World.Instance.Farm));
        else Debug.Log("Farm prefab not found on World instance.");
        if (World.Instance.AI != null) ai = new AI(Object.Instantiate(World.Instance.AI));
        else Debug.Log("AI prefab not found on World instance.");
    }

    public static bool Serialize(GameData data, string path)
    {
        BinaryFormatter bf = new BinaryFormatter();
        SurrogateSelector surrogateSelector = new SurrogateSelector();
        surrogateSelector.AddSurrogate(typeof(Vector2), new StreamingContext(StreamingContextStates.All), new Vector3SerializationSurrogate());
        bf.SurrogateSelector = surrogateSelector;

        FileStream file = File.Create(path);
        bf.Serialize(file, data);
        file.Close();

        return true;
    }

    public static GameData Deserialize(FileStream file)
    {
        BinaryFormatter bf = new BinaryFormatter();
        SurrogateSelector surrogateSelector = new SurrogateSelector();
        surrogateSelector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), new Vector3SerializationSurrogate());
        bf.SurrogateSelector = surrogateSelector;

        GameData data = (GameData)bf.Deserialize(file);
        file.Close();

        return data;
    }
}