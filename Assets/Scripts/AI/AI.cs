using UnityEngine;

[System.Serializable]
public class AI
{
    public void Instantiate()
    {
        if (World.Instance.AI != null)
        {
            GameObject model = Object.Instantiate(World.Instance.AI);
            AIObject modelScript = model.GetComponent<AIObject>();
            if (modelScript != null) modelScript.Data = this;
            else Debug.Log("AIObject component not found in AI model.");
        }
        else Debug.Log("AI prefab not found on World instance.");
    }

    public void Update()
    {

    }
}