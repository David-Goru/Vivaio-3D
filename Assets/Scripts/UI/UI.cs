using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class UI : GameElement
{
    private List<UIElement> elements;
    [System.NonSerialized] private UIObject uiObject;

    public override void Instantiate()
    {
        GameObject ui = Object.Instantiate(Prefab);
        uiObject = ui.GetComponent<UIObject>();

        if (uiObject != null)
        {
            uiObject.Data = this;
        }
        else Debug.Log("UIObject component not found in UI model.");

        loadElements();
    }

    public override void Update()
    {
        foreach (UIElement element in elements)
        {
            element.Update();
        }
    }

    private void loadElements()
    {
        elements = new List<UIElement>();
        foreach (Transform child in uiObject.transform)
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