using UnityEngine;

[System.Serializable]
public class AppearanceElement
{
    private BodyPart bodyPartName;
    private string optionSelected;
    private Color color;

    public BodyPart BodyPartName { get => bodyPartName; set => bodyPartName = value; }
    public string OptionSelected { get => optionSelected; set => optionSelected = value; }
    public Color Color { get => color; set => color = value; }

    public AppearanceElement(BodyPart bodyPartName, string optionSelected, Color color)
    {
        this.bodyPartName = bodyPartName;
        this.optionSelected = optionSelected;
        this.color = color;
    }
}