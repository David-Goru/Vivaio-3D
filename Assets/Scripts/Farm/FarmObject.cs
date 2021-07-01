using UnityEngine;

public class FarmObject : MonoBehaviour
{
    public GameObject RidgePrefab;
    public MeshFilter Ground;

    private Farm farmScript;
    public Farm FarmScript { set => farmScript = value; }

    public bool PlowAt(Vector3 position)
    {
        if (farmScript != null) return farmScript.PlowAt(position);
        return false;
    }
}