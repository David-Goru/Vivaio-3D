using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public class SaveSystem
{
    public static GameData GameData;

    public static GameData GetGameData()
    {
        return GameData ?? new GameData();
    }

    public static void SetGameData()
    {
        GameData ??= new GameData();
    }

    public static bool Serialize(GameData data, string path)
    {
        var bf = GetBinaryFormatter();
        var file = File.Create(path);
        bf.Serialize(file, data);
        file.Close();

        return true;
    }

    public static GameData Deserialize(FileStream file)
    {
        var bf = GetBinaryFormatter();
        var data = (GameData)bf.Deserialize(file);
        file.Close();

        return data;
    }

    private static BinaryFormatter GetBinaryFormatter()
    {
        var bf = new BinaryFormatter();
        var surrogateSelector = new SurrogateSelector();
        surrogateSelector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), new Vector3SerializationSurrogate());
        surrogateSelector.AddSurrogate(typeof(Vector2), new StreamingContext(StreamingContextStates.All), new Vector3SerializationSurrogate());
        bf.SurrogateSelector = surrogateSelector;

        return bf;
    }
}