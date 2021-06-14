using UnityEngine;

[System.Serializable]
public class AI
{
    public AI(GameObject model)
    {
        AIObject modelScript = model.GetComponent<AIObject>();
        if (modelScript != null) modelScript.Data = this;
        else Debug.Log("AIObject component not found in AI model.");
    }

    public void Update()
    {

    }
}