using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Farm : GameElement
{
    private List<Tile> tiles;
    private Vector3 size;

    [System.NonSerialized] private FarmObject farmObject;

    public override void Instantiate()
    {
        if (Game.Instance.Farm != null)
        {
            GameObject model = Object.Instantiate(Game.Instance.Farm);
            farmObject = model.GetComponent<FarmObject>();

            if (farmObject != null)
            {
                farmObject.FarmScript = this;
                if (tiles == null) initializeTiles();
                else loadTiles();
            }
            else Debug.Log("FarmObject component not found in Farm model.");
        }
        else Debug.Log("Farm prefab not found on Game instance.");
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
        return Game.Instance.Data.Farm.RidgePrefab();
    }

    public GameObject RidgePrefab()
    {
        return farmObject.RidgePrefab;
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