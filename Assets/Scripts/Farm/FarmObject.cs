using UnityEngine;

public class FarmObject : MonoBehaviour
{
    public MeshFilter Ground;

    private Farm farmScript;
    public Farm FarmScript { set => farmScript = value; }

    private void OnMouseDown()
    {
        if (farmScript != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) farmScript.PlowAt(hit.point);
        }
    }
}