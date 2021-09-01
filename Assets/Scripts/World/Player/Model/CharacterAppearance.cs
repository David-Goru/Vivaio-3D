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
            GameObject elementObject = model.Find(element.BodyPart.ToString()).Find(element.OptionSelected).gameObject;
            elementObject.SetActive(true);
            elementObject.GetComponent<SkinnedMeshRenderer>().material.color = element.Color;
        }
    }

    public void HideBodyElement(Transform model, AppearanceElement element, string option)
    {
        if (option == "None") return;

        model.Find(element.BodyPart.ToString()).Find(option).gameObject.SetActive(false);
    }

    public void ShowBodyElement(Transform model, AppearanceElement element, string option)
    {
        if (option == "None") return;

        model.Find(element.BodyPart.ToString()).Find(option).gameObject.SetActive(true);
    }
}