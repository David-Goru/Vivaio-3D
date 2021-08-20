using UnityEngine;

[System.Serializable]
public class GameElement
{
    [System.NonSerialized] public Game Game;
    public GameObject Prefab;
    public virtual void Instantiate() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
}