using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamProbe : MonoBehaviour
{
    ReflectionProbe probe;
    // Start is called before the first frame update
    void Start()
    {
        probe = GetComponent<ReflectionProbe>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 probeVector = new Vector3(Camera.main.transform.position.x,
                                          Camera.main.transform.position.y * -1,
                                          Camera.main.transform.position.z);

        probe.transform.position = probeVector;
        probe.RenderProbe();
    }
}
