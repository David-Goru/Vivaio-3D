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

    [HideInInspector] public FarmData Data;

    public ItemInfo WeedInfo { get => weedInfo; }
    public float CropOffsetY { get => cropOffsetY; }

    private void Start()
    {
        tiles = new List<Tile>();
        weeds = new List<Weed>();

        if (Data.Weeds == null) generateWeeds();
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

    public void PullWeed(Weed weed)
    {
        weeds.Remove(weed);
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

    private void generateWeeds()
    {
        List<Vector3> randomPositions = getRandomFarmPositions(initialWeeds);

        for (int i = 0; i < initialWeeds; i++)
        {
            float yRotation = Random.Range(0.0f, 360.0f);
            int modelId = Random.Range(0, weedPrefabs.Length);

            Weed weed = Instantiate(weedPrefabs[modelId], randomPositions[i], Quaternion.Euler(0, yRotation, 0)).GetComponent<Weed>();
            WeedData weedData = new WeedData();
            weedData.CurrentStack = 1;
            weedData.Name = "Weed";
            weedData.Position = randomPositions[i];
            weedData.YRotation = yRotation;
            weedData.ModelId = modelId;
            weed.Data = weedData;
            weed.Info = weedInfo;

            weeds.Add(weed);
        }
    }

    private List<Vector3> getRandomFarmPositions(int amountToShuffle)
    {
        int initialX = -farmSize + 1;
        int finalX = farmSize - 1;
        int initialZ = -farmSize + 1;
        int finalZ = farmSize - 1;

        int listSize = (int)Mathf.Pow(farmSize - 2, 2);
        List<Vector3> positions = new List<Vector3>(listSize);

        for (int x = initialX; x <= finalX; x++)
        {
            for (int z = initialZ; z <= finalZ; z++)
            {
                positions.Add(new Vector3(x, 0, z));
            }
        }

        return shuffle(positions, amountToShuffle);
    }

    private List<Vector3> shuffle(List<Vector3> list, int amount)
    {
        if (list.Count < amount) return list;

        for (int i = 0; i < amount; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            Vector3 temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }

        return list;
    }

    // NOT IMPLEMENTED YET
    public void Save()
    {
        Data.Tiles = new List<TileData>();
        foreach (Tile tile in tiles)
        {
            Data.Tiles.Add(tile.Data);
        }

        Data.Weeds = new List<WeedData>();
        foreach (Weed weed in weeds)
        {
            Data.Weeds.Add((WeedData)weed.Data);
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

        foreach (WeedData weedData in Data.Weeds)
        {
            Weed weed = Instantiate(weedPrefabs[weedData.ModelId], weedData.Position, Quaternion.Euler(0, weedData.YRotation, 0)).GetComponent<Weed>();
            weed.Data = weedData;
            weeds.Add(weed);
        }
    }
}