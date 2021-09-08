using UnityEngine;
using System.Collections.Generic;

public class TestUI : UIElement
{
    private List<UIElement> subElements;

    public override void Initialize()
    {
        if (viewer == null)
        {
            Debug.Log("Viewer not found on TestUI.");
            return;
        }

        subElements = new List<UIElement>();
        foreach (Transform element in viewer)
        {
            UIElement uiElement = GetUIElementFromTransform(element, false);
            if (uiElement != null)
            {
                uiElement.Initialize();
                subElements.Add(uiElement);
            }
        }
    }
}