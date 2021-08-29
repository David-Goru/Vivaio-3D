using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TileData
{
    private Vector3 position;
    private bool watered;
    private List<CropData> crops;

    public Vector3 Position { get => position; set => position = value; }
    public bool Watered { get => watered; set => watered = value; }
    public List<CropData> Crops { get => crops; set => crops = value; }
}