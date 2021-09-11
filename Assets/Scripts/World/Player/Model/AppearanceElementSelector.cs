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

    public BodyPart BodyPart;

    [HideInInspector] public List<string> Options;

    public void SetUpSelector(Transform model, GameObject panelPrefab, GameObject colorPrefab, Transform parent)
    {
        this.model = model;
        loadElements();
        loadPanel(panelPrefab, parent);
        loadColors(colorPrefab);

        optionID = 0;
        index = SaveSystem.GameData.Player.AppearanceElements.Count;
        SaveSystem.GameData.Player.AppearanceElements.Add(new AppearanceElement(BodyPart, Options[optionID], colors[optionID]));

        changeColor(colors[optionID]);
    }

    private void loadPanel(GameObject prefab, Transform parent)
    {
        uiPanel = Object.Instantiate(prefab).transform;
        uiPanel.transform.Find("Element name").GetComponent<Text>().text = BodyPart.ToString(); // Localization required
        optionText = uiPanel.transform.Find("Option name").GetComponent<Text>();
        optionText.text = Options[0]; // Localization required
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
        Options = new List<string>();
        if (canBeNone) Options.Add("None");
        foreach (Transform element in model.Find(BodyPart.ToString())) Options.Add(element.name);
        if (Options.Count == 0) Options.Add("None");
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
        if (Options.Count < 2) return;

        CharacterCreation.Instance.HideBodyElement(SaveSystem.GameData.Player.AppearanceElements[index], Options[optionID]);

        int maxOptionID = Options.Count - 1;
        optionID += increment;
        if (optionID < 0) optionID = maxOptionID;
        else if (optionID > maxOptionID) optionID = 0;

        CharacterCreation.Instance.ShowBodyElement(SaveSystem.GameData.Player.AppearanceElements[index], Options[optionID]);
        SaveSystem.GameData.Player.AppearanceElements[index].OptionSelected = Options[optionID];
        optionText.text = Options[optionID];
    }

    private void changeColor(Color color)
    {
        foreach (Transform element in model.Find(BodyPart.ToString()))
        {
            element.GetComponent<SkinnedMeshRenderer>().material.color = color;
        }

        SaveSystem.GameData.Player.AppearanceElements[index].Color = color;
    }
}