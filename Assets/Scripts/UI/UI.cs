using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class UI : GameElement
{
    private List<UIElement> elements;
    [System.NonSerialized] private UIObject uiObject;

    public override void Instantiate()
    {
        if (Game.Instance.UI != null)
        {
            GameObject ui = Object.Instantiate(Game.Instance.UI);
            uiObject = ui.GetComponent<UIObject>();

            if (uiObject != null)
            {
                uiObject.Data = this;
            }
            else Debug.Log("UIObject component not found in UI model.");

            loadElements();
        }
        else Debug.Log("UI prefab not found on Game instance.");
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
            System.Type type = System.Type.GetType(viewer.name);
            UIElement element = (UIElement)System.Activator.CreateInstance(type);
            element.SetViewer(viewer);
            return element;
        }
        catch (UnityException e) { Debug.Log("Couldn't create UI element with name " + viewer.name + ". Error: " + e); }

        return null;
    }
}