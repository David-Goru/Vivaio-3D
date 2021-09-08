using UnityEngine;

[System.Serializable]
public class CropData
{
    private string name;
    private Vector3 position;
    private int stage;
    private int quality;
    private bool watered;

    public string Name { get => name; set => name = value; }
    public Vector3 Position { get => position; set => position = value; }
    public int Stage { get => stage; set => stage = value; }
    public int Quality { get => quality; set => quality = value; }
    public bool Watered { get => watered; set => watered = value; }
}