using UnityEngine;

[System.Serializable]
public class Farm
{
    private FarmMeshes meshes;
    private FarmObject farmObject;

    public void Instantiate()
    {
        if (meshes == null) meshes = new FarmMeshes();

        if (World.Instance.Farm != null)
        {
            GameObject model = Object.Instantiate(World.Instance.Farm);
            farmObject = model.GetComponent<FarmObject>();

            if (farmObject != null)
            {
                farmObject.FarmScript = this;
                meshes.Initialize(farmObject.Ground.transform.localScale);
                farmObject.Ground.mesh = meshes.Ground;
            }
            else Debug.Log("FarmObject component not found in Farm model.");
        }
        else Debug.Log("Farm prefab not found on World instance.");
    }

    public bool PlowAt(Vector3 position)
    {
        if (meshes.Plow(position))
        {
            farmObject.Ground.mesh = meshes.Ground;
            return true;
        }
        return false;
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
}