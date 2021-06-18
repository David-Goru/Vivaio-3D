using UnityEngine;

[System.Serializable]
public class Farm
{
    private FarmGround ground;
    private FarmObject farmObject;

    public Farm(GameObject model)
    {
        if (ground == null) ground = new FarmGround();

        farmObject = model.GetComponent<FarmObject>();

        if (farmObject != null)
        {
            farmObject.FarmScript = this;
            farmObject.Ground.mesh = ground.Initialize(farmObject.Ground.transform.localScale);
        }
        else Debug.Log("FarmObject component not found in Farm model.");
    }

    public void PlowAt(Vector3 position)
    {
        farmObject.Ground.mesh = ground.Plow(position);
    }

    public void Update()
    {

    }
}