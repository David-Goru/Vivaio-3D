using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    private Vector3 position;
    private Vector3 rotation;
    private ItemData itemInHand;
    private HandType mainHand;
    private List<AppearanceElement> appearanceElements;

    public Vector3 Position { get => position; set => position = value; }
    public Vector3 Rotation { get => rotation; set => rotation = value; }
    public ItemData ItemInHand { get => itemInHand; set => itemInHand = value; }
    public HandType MainHand { get => mainHand; set => mainHand = value; }
    public List<AppearanceElement> AppearanceElements { get => appearanceElements; set => appearanceElements = value; }
}