using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TileData
{
    public Vector3 Position;
    public bool Watered;
    public List<CropData> Crops;
}