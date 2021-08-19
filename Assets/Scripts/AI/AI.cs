using UnityEngine;

[System.Serializable]
public class AI : GameElement
{
    public override void Instantiate()
    {
        GameObject model = Object.Instantiate(Prefab);
        AIObject modelScript = model.GetComponent<AIObject>();
        if (modelScript != null) modelScript.Data = this;
        else Debug.Log("AIObject component not found in AI model.");
    }
}