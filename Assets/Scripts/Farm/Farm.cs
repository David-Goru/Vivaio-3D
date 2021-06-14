using UnityEngine;

[System.Serializable]
public class Farm
{
    public Farm(GameObject model)
    {
        FarmObject modelScript = model.GetComponent<FarmObject>();
        if (modelScript != null) modelScript.Data = this;
        else Debug.Log("FarmObject component not found in Farm model.");
    }

    public void Update()
    {

    }
}