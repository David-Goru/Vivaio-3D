using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class AppearanceElementSelector
{
    [SerializeField] private BodyPart bodyPart;
    [SerializeField] private bool canBeNone = false;
    [SerializeField] private List<Color> colors;

    private int index;
    private int optionID;
    private Transform model;
    private Transform uiPanel;
    private Text optionText;
    private List<string> options;

    public BodyPart BodyPart { get => bodyPart; set => bodyPart = value; }
    public List<string> Options { get => options; }

    public void SetUpSelector(Transform model, GameObject panelPrefab, GameObject colorPrefab, Transform parent)
    {
        this.model = model;
        loadElements();
        loadPanel(panelPrefab, parent);
        loadColors(colorPrefab);

        optionID = 0;
        index = SaveSystem.GameData.Player.AppearanceElements.Count;
        SaveSystem.GameData.Player.AppearanceElements.Add(new AppearanceElement(bodyPart, options[optionID], colors[optionID]));

        changeColor(colors[optionID]);
    }

    private void loadPanel(GameObject prefab, Transform parent)
    {
        uiPanel = Object.Instantiate(prefab).transform;
        uiPanel.transform.Find("Element name").GetComponent<Text>().text = bodyPart.ToString(); // Localization required
        optionText = uiPanel.transform.Find("Option name").GetComponent<Text>();
        optionText.text = options[0]; // Localization required
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
        if (canBeNone) options.Add("None");
        foreach (Transform element in model.Find(bodyPart.ToString())) options.Add(element.name);
        if (options.Count == 0) options.Add("None");
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
        if (options.Count < 2) return;

        CharacterCreation.Instance.HideBodyElement(SaveSystem.GameData.Player.AppearanceElements[index], options[optionID]);

        int maxOptionID = options.Count - 1;
        optionID += increment;
        if (optionID < 0) optionID = maxOptionID;
        else if (optionID > maxOptionID) optionID = 0;

        CharacterCreation.Instance.ShowBodyElement(SaveSystem.GameData.Player.AppearanceElements[index], options[optionID]);
        SaveSystem.GameData.Player.AppearanceElements[index].OptionSelected = options[optionID];
        optionText.text = options[optionID];
    }

    private void changeColor(Color color)
    {
        foreach (Transform element in model.Find(bodyPart.ToString()))
        {
            element.GetComponent<SkinnedMeshRenderer>().material.color = color;
        }

        SaveSystem.GameData.Player.AppearanceElements[index].Color = color;
    }
}