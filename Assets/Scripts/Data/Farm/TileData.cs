using UnityEngine;

[System.Serializable]
public class TileData
{
    public Vector3 position;
    public bool watered;
    public CropData crop;
    public int ridgePrefabId;
}