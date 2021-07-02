using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCreation : MonoBehaviour
{
    [SerializeField] private Transform appearanceElementsContainer;
    [SerializeField] private GameObject appearanceElementPrefab;
    [SerializeField] private GameObject appearanceColorPrefab;
    [SerializeField] private GameObject characterModelPrefab;

    public static PlayerObject CharacterObject;
    public static List<KeyValuePair<string, List<string>>> AppearanceElementsAndOptions;
    public static List<KeyValuePair<string, string>> SelectedAppearance;

    private void Start()
    {
        CharacterObject = Instantiate(characterModelPrefab).GetComponent<PlayerObject>();
        initializeSelectors();
    }

    private void initializeSelectors()
    {
        AppearanceElementSelector[] appearanceElements = CharacterObject.AppearanceElementSelectors;
        AppearanceElementsAndOptions = new List<KeyValuePair<string, List<string>>>();
        SelectedAppearance = new List<KeyValuePair<string, string>>();

        for (int i = 0; i < appearanceElements.Length; i++)
        {
            initializeSelector(appearanceElements[i]);
        }
    }

    private void initializeSelector(AppearanceElementSelector element)
    {
        string partName = element.BodyPart.name;
        List<string> elements = getElementsFrom(element.BodyPart.transform);
        AppearanceElementsAndOptions.Add(new KeyValuePair<string, List<string>>(partName, elements));
        SelectedAppearance.Add(new KeyValuePair<string, string>(partName, "None"));
        element.SetUpSelector(appearanceElementPrefab, appearanceColorPrefab, appearanceElementsContainer);
    }

    private List<string> getElementsFrom(Transform bodyPart)
    {
        List<string> elements = new List<string>();
        elements.Add("None");
        foreach (Transform element in bodyPart) elements.Add(element.name);
        return elements;
    }

    // TEST //
    public void TestPlay()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
}

[System.Serializable]
public class AppearanceElementSelector
{
    [SerializeField] private GameObject bodyPart;
    [SerializeField] private List<Color> colors;
    [SerializeField] private Transform uiPanel;
    [SerializeField] private Text optionText;

    public GameObject BodyPart { get => bodyPart; set => bodyPart = value; }

    public void SetUpSelector(GameObject prefab, GameObject colorPrefab, Transform parent)
    {
        uiPanel = Object.Instantiate(prefab).transform;
        uiPanel.transform.Find("Element name").GetComponent<Text>().text = bodyPart.name; // Localization required
        optionText = uiPanel.transform.Find("Option name").GetComponent<Text>();
        optionText.text = "None";
        uiPanel.transform.Find("Next option").GetComponent<Button>().onClick.AddListener(() => changeElement(1));
        uiPanel.transform.Find("Previus option").GetComponent<Button>().onClick.AddListener(() => changeElement(-1));

        if (colors.Count > 0)
        {
            Transform colorPanel = uiPanel.transform.Find("Colors").Find("Viewport").Find("Content");
            foreach (Color color in colors) createColorButton(color, colorPrefab, colorPanel);
            changeColor(colors[0]);
        }

        uiPanel.transform.SetParent(parent);
    }

    private void createColorButton(Color color, GameObject prefab, Transform parent)
    {
        GameObject colorButton = Object.Instantiate(prefab);
        colorButton.GetComponent<Image>().color = color;
        colorButton.GetComponent<Button>().onClick.AddListener(() => changeColor(color));
        colorButton.transform.SetParent(parent);
    }

    private void changeElement(int increment)
    {
        KeyValuePair<string, List<string>> bodyElement = CharacterCreation.AppearanceElementsAndOptions.Find(x => x.Key == bodyPart.name);
        KeyValuePair<string, string> characterBodyElement = CharacterCreation.SelectedAppearance.Find(x => x.Key == bodyPart.name);
        int characterBodyElementID = CharacterCreation.SelectedAppearance.IndexOf(characterBodyElement);

        CharacterCreation.CharacterObject.HideBodyElement(characterBodyElement);

        int currentID = bodyElement.Value.IndexOf(characterBodyElement.Value);
        int maxID = bodyElement.Value.Count - 1;

        currentID += increment;
        if (currentID < 0) currentID = maxID;
        else if (currentID > maxID) currentID = 0;

        characterBodyElement = new KeyValuePair<string, string>(bodyPart.name, bodyElement.Value[currentID]);
        CharacterCreation.SelectedAppearance[characterBodyElementID] = characterBodyElement;
        optionText.text = characterBodyElement.Value;

        CharacterCreation.CharacterObject.ShowBodyElement(characterBodyElement);
    }

    private void changeColor(Color color)
    {
        foreach (Transform element in bodyPart.transform)
        {
            element.GetComponent<SkinnedMeshRenderer>().material.color = color;
        }
    }
}