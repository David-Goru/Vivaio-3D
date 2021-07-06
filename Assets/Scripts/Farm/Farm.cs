using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Farm
{
    private List<Tile> tiles;
    private Vector3 size;

    [System.NonSerialized] private FarmObject farmObject;

    public void Instantiate()
    {
        if (World.Instance.Farm != null)
        {
            GameObject model = Object.Instantiate(World.Instance.Farm);
            farmObject = model.GetComponent<FarmObject>();

            if (farmObject != null)
            {
                farmObject.FarmScript = this;
                if (tiles == null) initializeTiles();
                else loadTiles();
            }
            else Debug.Log("FarmObject component not found in Farm model.");
        }
        else Debug.Log("Farm prefab not found on World instance.");
    }

    public bool PlowAt(Vector3 position)
    {
        Vector3 roundedPosition = new Vector3(Mathf.Round(position.x - 0.5f) + 0.5f, 0, Mathf.Round(position.z - 0.5f) + 0.5f);
        Tile tile = tiles.Find(x => x.Position == roundedPosition);
        if (tile == null) return false;
        return tile.Plow();
    }

    public static GameObject GetRidgePrefab()
    {
        return World.Instance.Data.Farm.RidgePrefab();
    }

    public GameObject RidgePrefab()
    {
        return farmObject.RidgePrefab;
    }

    public void Update()
    {

    }

    private void initializeTiles()
    {
        tiles = new List<Tile>();
        size = farmObject.Ground.transform.localScale;

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.z; j++)
            {
                Vector3 tilePosition = new Vector3(Mathf.Round(i - size.x / 2) + 0.5f, 0, Mathf.Round(j - size.z / 2) + 0.5f);
                tiles.Add(new Tile(tilePosition));
            }
        }
    }

    private void loadTiles()
    {
        foreach (Tile tile in tiles)
        {
            tile.Load();
        }
    }
}