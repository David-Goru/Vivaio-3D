using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Farm : MonoBehaviour
{
    [SerializeField] private MeshFilter ground;
    [SerializeField] private GameObject[] ridgePrefabs;
    [SerializeField] private GameObject[] weedPrefabs;
    [SerializeField] private ItemInfo weedInfo;
    [SerializeField] private int tilesPerUnit = 0;
    [SerializeField] private float cropOffsetY = 0.0f;
    [SerializeField] private int farmSize = 0;
    [SerializeField] private int initialWeeds = 0;

    private List<Tile> tiles;
    private List<Weed> weeds;

    [HideInInspector] public FarmData data;

    public ItemInfo WeedInfo => weedInfo;
    public float CropOffsetY => cropOffsetY;

    private void Start()
    {
        tiles = new List<Tile>();
        weeds = new List<Weed>();

        if (data.weeds == null) GenerateWeeds();
    }

    public void NewDay()
    {
        foreach (var tile in tiles) tile.NewDay();
    }

    public bool PlowAt(Vector3 position)
    {
        var roundedPosition = new Vector3(Mathf.Round(position.x * tilesPerUnit) / tilesPerUnit, 0, Mathf.Round(position.z * tilesPerUnit) / tilesPerUnit);
        var tile = tiles.Find(x => x.data.position == roundedPosition);
        if (tile != null) return false;

        StartCoroutine(CreateRidge(roundedPosition));

        return true;
    }

    public bool WaterAt(Vector3 position)
    {
        var roundedPosition = new Vector3(Mathf.Round(position.x * tilesPerUnit) / tilesPerUnit, 0, Mathf.Round(position.z * tilesPerUnit) / tilesPerUnit);
        var tile = tiles.Find(x => x.data.position == roundedPosition);
        return tile != null && tile.Water();
    }

    public bool PlantAt(CropInfo cropInfo, Vector3 cropPosition)
    {
        var tile = tiles.Find(x => x.data.position == cropPosition);
        return tile != null && tile.Plant(cropInfo, cropPosition);
    }

    public void PullWeed(Weed weed)
    {
        weeds.Remove(weed);
    }

    private IEnumerator CreateRidge(Vector3 position)
    {
        yield return new WaitForSeconds(0.35f);

        int ridgePrefabId = Random.Range(0, ridgePrefabs.Length);
        Tile tile = Instantiate(ridgePrefabs[ridgePrefabId], position, Quaternion.identity).GetComponent<Tile>();
        tile.data = new TileData();
        tile.data.position = position;
        tile.data.ridgePrefabId = ridgePrefabId;
        tiles.Add(tile);
    }

    private void GenerateWeeds()
    {
        var randomPositions = GetRandomFarmPositions(initialWeeds);

        for (var i = 0; i < initialWeeds; i++)
        {
            var yRotation = Random.Range(0.0f, 360.0f);
            var modelId = Random.Range(0, weedPrefabs.Length);

            var weed = Instantiate(weedPrefabs[modelId], randomPositions[i], Quaternion.Euler(0, yRotation, 0)).GetComponent<Weed>();
            var weedData = new WeedData
            {
                currentStack = 1,
                name = "Weed",
                position = randomPositions[i],
                yRotation = yRotation,
                modelId = modelId
            };
            weed.data = weedData;
            weed.info = weedInfo;

            weeds.Add(weed);
        }
    }

    private List<Vector3> GetRandomFarmPositions(int amountToShuffle)
    {
        var initialX = -farmSize + 1;
        var finalX = farmSize - 1;
        var initialZ = -farmSize + 1;
        var finalZ = farmSize - 1;

        var listSize = (int)Mathf.Pow(farmSize - 2, 2);
        var positions = new List<Vector3>(listSize);

        for (var x = initialX; x <= finalX; x++)
        {
            for (var z = initialZ; z <= finalZ; z++)
            {
                positions.Add(new Vector3(x, 0, z));
            }
        }

        return Shuffle(positions, amountToShuffle);
    }

    private static List<Vector3> Shuffle(List<Vector3> list, int amount)
    {
        if (list.Count < amount) return list;

        for (var i = 0; i < amount; i++)
        {
            var randomIndex = Random.Range(i, list.Count);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }

        return list;
    }

    // NOT IMPLEMENTED YET
    public void Save()
    {
        data.tiles = new List<TileData>();
        foreach (var tile in tiles)
        {
            data.tiles.Add(tile.data);
        }

        data.weeds = new List<WeedData>();
        foreach (var weed in weeds)
        {
            data.weeds.Add((WeedData)weed.data);
        }
    }

    public void Load()
    {
        foreach (var tileData in data.tiles)
        {
            var tile = Instantiate(ridgePrefabs[tileData.ridgePrefabId], tileData.position, Quaternion.identity).GetComponent<Tile>();
            tile.data = tileData;
            tiles.Add(tile);
        }

        foreach (var weedData in data.weeds)
        {
            var weed = Instantiate(weedPrefabs[weedData.modelId], weedData.position, Quaternion.Euler(0, weedData.yRotation, 0)).GetComponent<Weed>();
            weed.data = weedData;
            weeds.Add(weed);
        }
    }
}