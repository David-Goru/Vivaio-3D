using UnityEngine;

public class ReflectionUpdate : MonoBehaviour
{
    ReflectionProbe probe;
    Transform cameraTransform;

    void Start()
    {
        probe = GetComponent<ReflectionProbe>();
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    void Update()
    {
        probe.transform.position = new Vector3(cameraTransform.position.x, -cameraTransform.position.y, cameraTransform.position.z);
        probe.RenderProbe();
    }
}