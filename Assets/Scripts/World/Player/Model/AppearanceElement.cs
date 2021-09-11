using UnityEngine;

[System.Serializable]
public class AppearanceElement
{
    public BodyPart BodyPart;
    public string OptionSelected;
    public Color Color;

    public AppearanceElement(BodyPart bodyPart, string optionSelected, Color color)
    {
        BodyPart = bodyPart;
        OptionSelected = optionSelected;
        Color = color;
    }
}