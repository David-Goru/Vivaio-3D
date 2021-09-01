using UnityEngine;

[System.Serializable]
public class AppearanceElement
{
    private BodyPart bodyPart;
    private string optionSelected;
    private Color color;

    public BodyPart BodyPart { get => bodyPart; set => bodyPart = value; }
    public string OptionSelected { get => optionSelected; set => optionSelected = value; }
    public Color Color { get => color; set => color = value; }

    public AppearanceElement(BodyPart bodyPart, string optionSelected, Color color)
    {
        this.bodyPart = bodyPart;
        this.optionSelected = optionSelected;
        this.color = color;
    }
}