using UnityEngine;

public class Weed : MonoBehaviour
{
    [HideInInspector] public WeedData Data;

    public void Pull()
    {
        Destroy(gameObject);
    }
}