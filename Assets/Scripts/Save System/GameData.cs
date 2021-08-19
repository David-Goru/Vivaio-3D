using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

[System.Serializable]
public class GameData
{
    private Player player;
    private Farm farm;
    private UI ui;
    private AI ai;

    public Player Player { get => player; set => player = value; }
    public Farm Farm { get => farm; set => farm = value; }
    public UI UI { get => ui; set => ui = value; }
    public AI AI { get => ai; set => ai = value; }

    public void Create()
    {
        player = new Player();
        farm = new Farm();
        ui = new UI();
        ai = new AI();
    }

    public void Instantiate()
    {
        player.Instantiate();
        farm.Instantiate();
        ui.Instantiate();
        ai.Instantiate();
    }

    public static bool Serialize(GameData data, string path)
    {
        BinaryFormatter bf = GetBinaryFormatter();
        FileStream file = File.Create(path);
        bf.Serialize(file, data);
        file.Close();

        return true;
    }

    public static GameData Deserialize(FileStream file)
    {
        BinaryFormatter bf = GetBinaryFormatter();
        GameData data = (GameData)bf.Deserialize(file);
        file.Close();

        return data;
    }

    public static BinaryFormatter GetBinaryFormatter()
    {
        BinaryFormatter bf = new BinaryFormatter();
        SurrogateSelector surrogateSelector = new SurrogateSelector();
        surrogateSelector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), new Vector3SerializationSurrogate());
        surrogateSelector.AddSurrogate(typeof(Vector2), new StreamingContext(StreamingContextStates.All), new Vector3SerializationSurrogate());
        bf.SurrogateSelector = surrogateSelector;

        return bf;
    }
}