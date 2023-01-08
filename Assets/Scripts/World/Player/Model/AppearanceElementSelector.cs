using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class AppearanceElementSelector
{
    [SerializeField] private bool canBeNone = false;
    [SerializeField] private List<Color> colors;

    private int index;
    private int optionID;
    private Transform model;
    private Transform uiPanel;
    private Text optionText;

    public BodyPart bodyPart;

    [HideInInspector] public List<string> options;

    public void SetUpSelector(Transform model, GameObject panelPrefab, GameObject colorPrefab, Transform parent)
    {
        this.model = model;
        LoadElements();
        LoadPanel(panelPrefab, parent);
        LoadColors(colorPrefab);

        optionID = 0;
        index = SaveSystem.GameData.player.appearanceElements.Count;
        SaveSystem.GameData.player.appearanceElements.Add(new AppearanceElement(bodyPart, options[optionID], colors[optionID]));

        ChangeColor(colors[optionID]);
    }

    private void LoadPanel(GameObject prefab, Transform parent)
    {
        uiPanel = Object.Instantiate(prefab).transform;
        uiPanel.transform.Find("Element name").GetComponent<Text>().text = bodyPart.ToString(); // Localization required
        optionText = uiPanel.transform.Find("Option name").GetComponent<Text>();
        optionText.text = options[0]; // Localization required
        uiPanel.transform.Find("Next option").GetComponent<Button>().onClick.AddListener(() => ChangeElement(1));
        uiPanel.transform.Find("Previus option").GetComponent<Button>().onClick.AddListener(() => ChangeElement(-1));
        uiPanel.transform.SetParent(parent);
    }

    private void LoadColors(GameObject prefab)
    {
        var colorPanel = uiPanel.transform.Find("Colors").Find("Viewport").Find("Content");
        if (colors.Count > 0) foreach (Color color in colors) CreateColorButton(color, prefab, colorPanel);
        else CreateColorButton(new Color(100, 100, 100), prefab, colorPanel);
    }

    private void LoadElements()
    {
        options = new List<string>();
        if (canBeNone) options.Add("None");
        foreach (Transform element in model.Find(bodyPart.ToString())) options.Add(element.name);
        if (options.Count == 0) options.Add("None");
    }

    private void CreateColorButton(Color color, GameObject prefab, Transform parent)
    {
        var colorButton = Object.Instantiate(prefab);
        colorButton.GetComponent<Image>().color = color;
        colorButton.GetComponent<Button>().onClick.AddListener(() => ChangeColor(color));
        colorButton.transform.SetParent(parent);
    }

    private void ChangeElement(int increment)
    {
        if (options.Count < 2) return;

        CharacterCreation.Instance.HideBodyElement(SaveSystem.GameData.player.appearanceElements[index], options[optionID]);

        var maxOptionID = options.Count - 1;
        optionID += increment;
        if (optionID < 0) optionID = maxOptionID;
        else if (optionID > maxOptionID) optionID = 0;

        CharacterCreation.Instance.ShowBodyElement(SaveSystem.GameData.player.appearanceElements[index], options[optionID]);
        SaveSystem.GameData.player.appearanceElements[index].optionSelected = options[optionID];
        optionText.text = options[optionID];
    }

    private void ChangeColor(Color color)
    {
        foreach (Transform element in model.Find(bodyPart.ToString()))
        {
            element.GetComponent<SkinnedMeshRenderer>().material.color = color;
        }

        SaveSystem.GameData.player.appearanceElements[index].color = color;
    }
}