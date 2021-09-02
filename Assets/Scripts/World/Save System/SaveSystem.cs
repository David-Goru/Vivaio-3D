using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public class SaveSystem
{
    public static GameData GameData;

    public static GameData GetGameData()
    {
        if (GameData != null) return GameData;
        return new GameData();
    }

    public static void SetGameData()
    {
        if (GameData == null) GameData = new GameData();
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