using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Appearance", menuName = "Character/Appearance", order = 0)]
public class CharacterAppearance : ScriptableObject
{
    public AppearanceElementSelector[] AppearanceElementSelectors;

    public void SetAppearance(Transform model, List<AppearanceElement> elements)
    {
        foreach (AppearanceElement element in elements)
        {
            if (element.OptionSelected == "None") continue;
            GameObject elementObject = model.Find(element.BodyPartName.ToString()).Find(element.OptionSelected).gameObject;
            elementObject.SetActive(true);
            elementObject.GetComponent<SkinnedMeshRenderer>().material.color = element.Color;
        }
    }

    public void HideBodyElement(Transform model, AppearanceElement element)
    {
        if (element.OptionSelected == "None") return;

        model.Find(element.BodyPartName.ToString()).Find(element.OptionSelected).gameObject.SetActive(false);
    }

    public void ShowBodyElement(Transform model, AppearanceElement element)
    {
        if (element.OptionSelected == "None") return;

        model.Find(element.BodyPartName.ToString()).Find(element.OptionSelected).gameObject.SetActive(true);
    }
}

public enum BodyPart
{
    BODY,
    EYES,
    HAIR,
    SHIRTS,
    TROUSERS,
    SHOES
}