using UnityEngine;

[System.Serializable]
public class GameElement
{
    public GameObject Prefab;
    public virtual void Instantiate() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
}