using UnityEngine;

public class AppearanceElement
{
    private string bodyPartName;
    private string optionSelected;
    private Color color;

    public string BodyPartName { get => bodyPartName; set => bodyPartName = value; }
    public string OptionSelected { get => optionSelected; set => optionSelected = value; }
    public Color Color { get => color; set => color = value; }

    public AppearanceElement(string bodyPartName, string optionSelected, Color color)
    {
        this.bodyPartName = bodyPartName;
        this.optionSelected = optionSelected;
        this.color = color;
    }
}