using UnityEngine;
using System.Collections.Generic;

public class UI : MonoBehaviour
{
    private List<UIElement> elements;

    private void Start()
    {
        loadElements();
    }

    public void Update()
    {
        foreach (UIElement element in elements)
        {
            element.Update();
        }
    }

    private void loadElements()
    {
        elements = new List<UIElement>();
        foreach (Transform child in transform)
        {
            UIElement element = UIElement.GetUIElementFromTransform(child);
            if (element != null)
            {
                element.Initialize();
                elements.Add(element);
            }
        }
    }
}