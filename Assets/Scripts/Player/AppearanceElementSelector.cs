using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class AppearanceElementSelector
{
    [SerializeField] private GameObject bodyPart;
    [SerializeField] private List<Color> colors;
    [SerializeField] private Transform uiPanel;
    [SerializeField] private Text optionText;
    [System.NonSerialized] private List<string> options;

    public GameObject BodyPart { get => bodyPart; set => bodyPart = value; }

    public void SetUpSelector(GameObject panelPrefab, GameObject colorPrefab, Transform parent)
    {
        loadPanel(panelPrefab, parent);
        loadColors(colorPrefab);
        loadElements();

        CharacterCreation.SelectedAppearance.Add(new AppearanceElement(bodyPart.name, "None", colors[0]));
        changeColor(colors[0]);
    }

    private void loadPanel(GameObject prefab, Transform parent)
    {
        uiPanel = Object.Instantiate(prefab).transform;
        uiPanel.transform.Find("Element name").GetComponent<Text>().text = bodyPart.name; // Localization required
        optionText = uiPanel.transform.Find("Option name").GetComponent<Text>();
        optionText.text = "None";
        uiPanel.transform.Find("Next option").GetComponent<Button>().onClick.AddListener(() => changeElement(1));
        uiPanel.transform.Find("Previus option").GetComponent<Button>().onClick.AddListener(() => changeElement(-1));
        uiPanel.transform.SetParent(parent);
    }

    private void loadColors(GameObject prefab)
    {
        Transform colorPanel = uiPanel.transform.Find("Colors").Find("Viewport").Find("Content");
        if (colors.Count > 0) foreach (Color color in colors) createColorButton(color, prefab, colorPanel);
        else createColorButton(new Color(100, 100, 100), prefab, colorPanel);
    }

    private void loadElements()
    {
        options = new List<string>();
        options.Add("None");
        foreach (Transform element in bodyPart.transform) options.Add(element.name);
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
        AppearanceElement appearanceElement = CharacterCreation.SelectedAppearance.Find(x => x.BodyPartName == bodyPart.name);
        int characterBodyElementID = CharacterCreation.SelectedAppearance.IndexOf(appearanceElement);

        CharacterCreation.CharacterObject.HideBodyElement(appearanceElement);

        int currentID = options.IndexOf(appearanceElement.OptionSelected);
        int maxID = options.Count - 1;

        currentID += increment;
        if (currentID < 0) currentID = maxID;
        else if (currentID > maxID) currentID = 0;

        CharacterCreation.SelectedAppearance[characterBodyElementID].OptionSelected = options[currentID];
        optionText.text = options[currentID];

        CharacterCreation.CharacterObject.ShowBodyElement(appearanceElement);
    }

    private void changeColor(Color color)
    {
        foreach (Transform element in bodyPart.transform)
        {
            element.GetComponent<SkinnedMeshRenderer>().material.color = color;
        }

        CharacterCreation.SelectedAppearance.Find(x => x.BodyPartName == bodyPart.name).Color = color;
    }
}