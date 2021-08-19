using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameElement
{
    public virtual void Instantiate() { }
    public virtual void Update() { }
}