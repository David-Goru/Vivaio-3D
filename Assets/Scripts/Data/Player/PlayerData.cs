using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public Vector3 Position;
    public Vector3 Rotation;
    public ItemData ItemInHand;
    public HandType MainHand;
    public List<AppearanceElement> AppearanceElements;
}