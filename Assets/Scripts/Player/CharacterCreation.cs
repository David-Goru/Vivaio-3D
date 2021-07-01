using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCreation : MonoBehaviour
{
    [SerializeField] private Transform appearanceElementsContainer;
    [SerializeField] private GameObject appearanceElementPrefab;
    [SerializeField] private GameObject characterModelPrefab;

    public static PlayerObject CharacterObject;
    public static List<KeyValuePair<string, List<string>>> AppearanceElementsAndOptions;
    public static List<KeyValuePair<string, string>> SelectedAppearance;

    private void Start()
    {
        CharacterObject = Instantiate(characterModelPrefab).GetComponent<PlayerObject>();

        GameObject[] appearanceElements = CharacterObject.AppearanceElements;
        AppearanceElementsAndOptions = new List<KeyValuePair<string, List<string>>>();
        SelectedAppearance = new List<KeyValuePair<string, string>>();
        for (int i = 0; i < appearanceElements.Length; i++)
        {
            string partName = appearanceElements[i].name;
            List<string> elements = getElementsFrom(appearanceElements[i].transform);
            AppearanceElementsAndOptions.Add(new KeyValuePair<string, List<string>>(partName, elements));
            SelectedAppearance.Add(new KeyValuePair<string, string>(partName, "None"));
        }

        setUpAppearanceUI();
    }

    private List<string> getElementsFrom(Transform bodyPart)
    {
        List<string> elements = new List<string>();
        elements.Add("None");
        foreach (Transform element in bodyPart) elements.Add(element.name);
        return elements;
    }

    private void setUpAppearanceUI()
    {
        foreach (KeyValuePair<string, List<string>> element in AppearanceElementsAndOptions)
        {
            AppearanceElementSelector selector = new AppearanceElementSelector();
            selector.SetUpSelector(element.Key, Instantiate(appearanceElementPrefab), appearanceElementsContainer);
        }
    }

    // TEST //
    public void TestPlay()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
}

public class AppearanceElementSelector
{
    private string bodyPart;
    private Text optionText;

    public void SetUpSelector(string bodyPart, GameObject uiPanel, Transform parent)
    {
        this.bodyPart = bodyPart;

        uiPanel.transform.Find("Element name").GetComponent<Text>().text = bodyPart;
        optionText = uiPanel.transform.Find("Option name").GetComponent<Text>();
        optionText.text = "None";
        uiPanel.transform.Find("Next option").GetComponent<Button>().onClick.AddListener(() => changeElement(1));
        uiPanel.transform.Find("Previus option").GetComponent<Button>().onClick.AddListener(() => changeElement(-1));

        uiPanel.transform.SetParent(parent);
    }

    private void changeElement(int increment)
    {
        KeyValuePair<string, List<string>> bodyElement = CharacterCreation.AppearanceElementsAndOptions.Find(x => x.Key == bodyPart);
        KeyValuePair<string, string> characterBodyElement = CharacterCreation.SelectedAppearance.Find(x => x.Key == bodyPart);
        int characterBodyElementID = CharacterCreation.SelectedAppearance.IndexOf(characterBodyElement);

        CharacterCreation.CharacterObject.HideBodyElement(characterBodyElement);

        int currentID = bodyElement.Value.IndexOf(characterBodyElement.Value);
        int maxID = bodyElement.Value.Count - 1;

        currentID += increment;
        if (currentID < 0) currentID = maxID;
        else if (currentID > maxID) currentID = 0;

        characterBodyElement = new KeyValuePair<string, string>(bodyPart, bodyElement.Value[currentID]);
        CharacterCreation.SelectedAppearance[characterBodyElementID] = characterBodyElement;
        optionText.text = characterBodyElement.Value;

        CharacterCreation.CharacterObject.ShowBodyElement(characterBodyElement);
    }
}