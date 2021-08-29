using UnityEngine;

public class UIElement
{
    protected Transform viewer;

    public void SetViewer(Transform viewer) { this.viewer = viewer; }
    public string GetViewer() { return viewer.name; }

    public virtual void Initialize() { }
    public virtual void Update() { }

    public static UIElement GetUIElementFromTransform(Transform viewer, bool showErrorMessage = true)
    {
        try
        {
            System.Type type = System.Type.GetType(viewer.name);

            if (type == null)
            {
                if (showErrorMessage) Debug.Log(string.Format("Couldn't create UI element with name {0}. Error: type not found.", viewer.name));
                return null;
            }

            UIElement element = (UIElement)System.Activator.CreateInstance(type);
            element.SetViewer(viewer);
            return element;
        }
        catch (UnityException e) { if (showErrorMessage) Debug.Log(string.Format("Couldn't create UI element with name {0}. Error: {1}", viewer.name, e)); }

        return null;
    }
}