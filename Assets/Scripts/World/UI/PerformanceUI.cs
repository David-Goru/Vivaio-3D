using UnityEngine;
using System.Collections.Generic;

public class PerformanceUI : UIElement
{
    private List<UIElement> subElements;

    public override void Initialize()
    {
        if (viewer == null)
        {
            Debug.Log("Viewer not found on PerformanceUI.");
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

    public override void Update()
    {
        if (viewer == null) return;

        foreach (UIElement uiElement in subElements)
        {
            uiElement.Update();
        }
    }
}