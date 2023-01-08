using UnityEngine;

[System.Serializable]
public class AppearanceElement
{
    public BodyPart bodyPart;
    public string optionSelected;
    public Color color;

    public AppearanceElement(BodyPart bodyPart, string optionSelected, Color color)
    {
        this.bodyPart = bodyPart;
        this.optionSelected = optionSelected;
        this.color = color;
    }
}