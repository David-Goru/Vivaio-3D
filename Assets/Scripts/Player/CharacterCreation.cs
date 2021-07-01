using System.Collections.Generic;
using UnityEngine;

public class CharacterCreation : MonoBehaviour
{
    public GameObject CharacterModelPrefab;

    private PlayerObject characterObject;
    [SerializeField] private List<KeyValuePair<string, List<string>>> bodyElements;

    public static List<KeyValuePair<string, string>> CharacterBodyElements;

    private void Start()
    {
        characterObject = Instantiate(CharacterModelPrefab).GetComponent<PlayerObject>();

        GameObject[] bodyParts = characterObject.BodyParts;
        bodyElements = new List<KeyValuePair<string, List<string>>>();
        CharacterBodyElements = new List<KeyValuePair<string, string>>();
        for (int i = 0; i < bodyParts.Length; i++)
        {
            string partName = bodyParts[i].name;
            List<string> elements = getElementsFrom(bodyParts[i].transform);
            bodyElements.Add(new KeyValuePair<string, List<string>>(partName, elements));
            CharacterBodyElements.Add(new KeyValuePair<string, string>(partName, "None"));
        }
    }

    private List<string> getElementsFrom(Transform bodyPart)
    {
        List<string> elements = new List<string>();
        elements.Add("None");
        foreach (Transform element in bodyPart) elements.Add(element.name);
        return elements;
    }

    public void NextElement(string bodyPart)
    {
        KeyValuePair<string, List<string>> bodyElement = bodyElements.Find(x => x.Key == bodyPart);
        KeyValuePair<string, string> characterBodyElement = CharacterBodyElements.Find(x => x.Key == bodyPart);
        int characterBodyElementID = CharacterBodyElements.IndexOf(characterBodyElement);

        characterObject.HideBodyElement(characterBodyElement);

        int currentID = bodyElement.Value.IndexOf(characterBodyElement.Value);
        int maxID = bodyElement.Value.Count;

        currentID++;
        if (currentID >= maxID) currentID = 0;
        characterBodyElement = new KeyValuePair<string, string>(bodyPart, bodyElement.Value[currentID]);
        CharacterBodyElements[characterBodyElementID] = characterBodyElement;

        characterObject.ShowBodyElement(characterBodyElement);
    }

    public void PreviusElement(string bodyPart)
    {
        KeyValuePair<string, List<string>> bodyElement = bodyElements.Find(x => x.Key == bodyPart);
        KeyValuePair<string, string> characterBodyElement = CharacterBodyElements.Find(x => x.Key == bodyPart);
        int characterBodyElementID = CharacterBodyElements.IndexOf(characterBodyElement);

        characterObject.HideBodyElement(characterBodyElement);

        int currentID = bodyElement.Value.IndexOf(characterBodyElement.Value);
        int maxID = bodyElement.Value.Count;

        currentID--;
        if (currentID < 0) currentID = maxID;
        characterBodyElement = new KeyValuePair<string, string>(bodyPart, bodyElement.Value[currentID]);
        CharacterBodyElements[characterBodyElementID] = characterBodyElement;

        characterObject.ShowBodyElement(characterBodyElement);
    }

    public void TestPlay()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
}