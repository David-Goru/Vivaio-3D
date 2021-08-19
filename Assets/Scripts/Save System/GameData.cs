using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    private List<GameElement> gameElements;

    public List<GameElement> GameElements { get => gameElements; set => gameElements = value; }

    public void Create()
    {
        foreach (GameObject gameElementGameObject in Game.Instance.GameElementGameObjects)
        {
            try
            {
                gameElements.Add(createGameElementFromGameObject(gameElementGameObject));
            }
            catch (UnityException e) { Debug.LogError(string.Format("Couldn't create {0} class. Error: {1}", gameElementGameObject.name, e));  }
        }
    }

    public void Instantiate()
    {
        foreach (GameElement gameElement in gameElements)
        {
            gameElement.Instantiate();
        }
    }

    private GameElement createGameElementFromGameObject(GameObject gameObject)
    {
        System.Type type = System.Type.GetType(gameObject.name);
        GameElement gameElement = (GameElement)System.Activator.CreateInstance(type);
        gameElement.Prefab = gameObject;

        return gameElement;
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