using System.Collections.Generic;

[System.Serializable]
public class FarmData
{
    private List<TileData> tiles;

    public List<TileData> Tiles { get => tiles; set => tiles = value; }
}