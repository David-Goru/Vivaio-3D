using UnityEngine;
using System.Collections.Generic;

public class Farm : MonoBehaviour
{
    [SerializeField] private MeshFilter ground;
    [SerializeField] private GameObject ridgePrefab;

    private FarmData data;
    private List<Tile> tiles;

    public FarmData Data { get => data; set => data = value; }

    private void Start()
    {
        tiles = new List<Tile>();
    }

    public bool PlowAt(Vector3 position)
    {
        Vector3 roundedPosition = new Vector3(Mathf.Round(position.x - 0.5f) + 0.5f, 0, Mathf.Round(position.z - 0.5f) + 0.5f);
        Tile tile = tiles.Find(x => x.Data.Position == roundedPosition);
        if (tile != null) return false;

        tile = Instantiate(ridgePrefab, roundedPosition, Quaternion.identity).GetComponent<Tile>();
        tile.Data = new TileData();
        tile.Data.Position = roundedPosition;
        tiles.Add(tile);

        return true;
    }

    public bool WaterAt(Vector3 position)
    {
        Vector3 roundedPosition = new Vector3(Mathf.Round(position.x - 0.5f) + 0.5f, 0, Mathf.Round(position.z - 0.5f) + 0.5f);
        Tile tile = tiles.Find(x => x.Data.Position == roundedPosition);
        if (tile == null) return false;
        return tile.Water();
    }

    // NOT IMPLEMENTED YET
    public void Save()
    {
        data.Tiles = new List<TileData>();
        foreach (Tile tile in tiles)
        {
            data.Tiles.Add(tile.Data);
        }
    }

    public void Load()
    {
        foreach (TileData tileData in data.Tiles)
        {
            Tile tile = Instantiate(ridgePrefab, tileData.Position, Quaternion.identity).GetComponent<Tile>();
            tile.Data = tileData;
            tiles.Add(tile);
        }
    }
}