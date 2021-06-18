using UnityEngine;
using System;
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
            UIElement element = getUIElement(child);
            if (element != null)
            {
                element.Initialize();
                elements.Add(element);
            }
        }
    }

    private UIElement getUIElement(Transform viewer)
    {
        try
        {
            Type type = Type.GetType(viewer.name);
            UIElement element = (UIElement)Activator.CreateInstance(type);
            element.SetViewer(viewer);
            return element;
        }
        catch (UnityException e) { Debug.Log("Couldn't create UI element with name " + viewer.name + ". Error: " + e); }

        return null;
    }
}