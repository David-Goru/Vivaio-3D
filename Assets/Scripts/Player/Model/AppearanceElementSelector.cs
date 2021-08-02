using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class AppearanceElementSelector
{
    [SerializeField] private BodyPart bodyPart;
    [SerializeField] private List<Color> colors;
    [System.NonSerialized] private Transform model;
    [System.NonSerialized] private Transform uiPanel;
    [System.NonSerialized] private Text optionText;
    [System.NonSerialized] private List<string> options;

    public BodyPart BodyPart { get => bodyPart; set => bodyPart = value; }

    public void SetUpSelector(Transform model, GameObject panelPrefab, GameObject colorPrefab, Transform parent)
    {
        this.model = model;
        loadPanel(panelPrefab, parent);
        loadColors(colorPrefab);
        loadElements();

        CharacterCreation.AddSelectedAppearance(new AppearanceElement(bodyPart, "None", colors[0]));
        changeColor(colors[0]);
    }

    private void loadPanel(GameObject prefab, Transform parent)
    {
        uiPanel = Object.Instantiate(prefab).transform;
        uiPanel.transform.Find("Element name").GetComponent<Text>().text = bodyPart.ToString(); // Localization required
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
        foreach (Transform element in model.Find(bodyPart.ToString())) options.Add(element.name);
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
        AppearanceElement appearanceElement = CharacterCreation.Instance.SelectedAppearance.Find(x => x.BodyPartName == bodyPart);
        int characterBodyElementID = CharacterCreation.Instance.SelectedAppearance.IndexOf(appearanceElement);

        CharacterCreation.Instance.HideBodyElement(appearanceElement);

        int currentID = options.IndexOf(appearanceElement.OptionSelected);
        int maxID = options.Count - 1;

        currentID += increment;
        if (currentID < 0) currentID = maxID;
        else if (currentID > maxID) currentID = 0;

        CharacterCreation.Instance.SelectedAppearance[characterBodyElementID].OptionSelected = options[currentID];
        optionText.text = options[currentID];

        CharacterCreation.Instance.ShowBodyElement(appearanceElement);
    }

    private void changeColor(Color color)
    {
        foreach (Transform element in model.Find(bodyPart.ToString()))
        {
            element.GetComponent<SkinnedMeshRenderer>().material.color = color;
        }

        CharacterCreation.Instance.SelectedAppearance.Find(x => x.BodyPartName == bodyPart).Color = color;
    }
}