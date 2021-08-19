using UnityEngine;

[System.Serializable]
public class AI : GameElement
{
    public override void Instantiate()
    {
        if (Game.Instance.AI != null)
        {
            GameObject model = Object.Instantiate(Game.Instance.AI);
            AIObject modelScript = model.GetComponent<AIObject>();
            if (modelScript != null) modelScript.Data = this;
            else Debug.Log("AIObject component not found in AI model.");
        }
        else Debug.Log("AI prefab not found on Game instance.");
    }
}