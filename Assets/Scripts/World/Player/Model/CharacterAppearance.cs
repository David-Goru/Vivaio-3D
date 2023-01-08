using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Appearance", menuName = "Character/Appearance", order = 0)]
public class CharacterAppearance : ScriptableObject
{
    public AppearanceElementSelector[] appearanceElementSelectors;

    public void SetAppearance(Transform model, List<AppearanceElement> elements)
    {
        foreach (var element in elements)
        {
            if (element.optionSelected == "None") continue;
            var elementObject = model.Find(element.bodyPart.ToString()).Find(element.optionSelected).gameObject;
            elementObject.SetActive(true);
            elementObject.GetComponent<SkinnedMeshRenderer>().material.color = element.color;
        }
    }

    public void HideBodyElement(Transform model, AppearanceElement element, string option)
    {
        if (option == "None") return;

        model.Find(element.bodyPart.ToString()).Find(option).gameObject.SetActive(false);
    }

    public void ShowBodyElement(Transform model, AppearanceElement element, string option)
    {
        if (option == "None") return;

        model.Find(element.bodyPart.ToString()).Find(option).gameObject.SetActive(true);
    }
}