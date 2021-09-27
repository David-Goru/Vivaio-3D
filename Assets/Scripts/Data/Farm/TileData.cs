using UnityEngine;

[System.Serializable]
public class TileData
{
    public Vector3 Position;
    public bool Watered;
    public CropData Crop;
    public int RidgePrefabId;
}