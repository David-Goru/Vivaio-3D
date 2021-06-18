using UnityEngine;

public class UIElement
{
    protected Transform viewer;

    public void SetViewer(Transform viewer) { this.viewer = viewer; }
    public string GetViewer() { return viewer.name; }

    public virtual void Initialize() { }
    public virtual void Update() { }
}