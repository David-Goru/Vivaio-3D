using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Farm : MonoBehaviour
{
    [SerializeField] private MeshFilter ground;
    [SerializeField] private GameObject[] ridgePrefabs;
    [SerializeField] private int tilesPerUnit = 0;
    [SerializeField] private float cropOffsetY = 0.0f;

    private List<Tile> tiles;

    [HideInInspector] public FarmData Data;

    public float CropOffsetY { get => cropOffsetY; }

    private void Start()
    {
        tiles = new List<Tile>();
    }

    public void NewDay()
    {
        foreach (Tile tile in tiles) tile.NewDay();
    }

    public bool PlowAt(Vector3 position)
    {
        Vector3 roundedPosition = new Vector3(Mathf.Round(position.x * tilesPerUnit) / tilesPerUnit, 0, Mathf.Round(position.z * tilesPerUnit) / tilesPerUnit);
        Tile tile = tiles.Find(x => x.Data.Position == roundedPosition);
        if (tile != null) return false;

        StartCoroutine(createRidge(roundedPosition));

        return true;
    }

    public bool WaterAt(Vector3 position)
    {
        Vector3 roundedPosition = new Vector3(Mathf.Round(position.x * tilesPerUnit) / tilesPerUnit, 0, Mathf.Round(position.z * tilesPerUnit) / tilesPerUnit);
        Tile tile = tiles.Find(x => x.Data.Position == roundedPosition);
        if (tile == null) return false;
        return tile.Water();
    }

    public bool PlantAt(CropInfo cropInfo, Vector3 cropPosition)
    {
        Tile tile = tiles.Find(x => x.Data.Position == cropPosition);
        if (tile == null) return false;
        return tile.Plant(cropInfo, cropPosition);
    }

    private IEnumerator createRidge(Vector3 position)
    {
        yield return new WaitForSeconds(0.35f);

        int ridgePrefabId = Random.Range(0, ridgePrefabs.Length);
        Tile tile = Instantiate(ridgePrefabs[ridgePrefabId], position, Quaternion.identity).GetComponent<Tile>();
        tile.Data = new TileData();
        tile.Data.Position = position;
        tile.Data.RidgePrefabId = ridgePrefabId;
        tiles.Add(tile);
    }

    // NOT IMPLEMENTED YET
    public void Save()
    {
        Data.Tiles = new List<TileData>();
        foreach (Tile tile in tiles)
        {
            Data.Tiles.Add(tile.Data);
        }
    }

    public void Load()
    {
        foreach (TileData tileData in Data.Tiles)
        {
            Tile tile = Instantiate(ridgePrefabs[tileData.RidgePrefabId], tileData.Position, Quaternion.identity).GetComponent<Tile>();
            tile.Data = tileData;
            tiles.Add(tile);
        }
    }
}