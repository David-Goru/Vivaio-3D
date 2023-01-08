using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public Vector3 position;
    public Vector3 rotation;
    public ItemData itemInHand;
    public HandType mainHand;
    public List<AppearanceElement> appearanceElements;
}