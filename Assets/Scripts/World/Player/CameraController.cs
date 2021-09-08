using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform objective;

    public Transform Objective { get => objective; set => objective = value; }

    private void Update()
    {
        if (objective == null) return;
        focusObjective();
    }

    private void focusObjective()
    {
        transform.position = new Vector3(objective.position.x, 0, objective.position.z);
    }
}