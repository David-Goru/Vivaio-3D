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
        checkRotation();
    }

    private void focusObjective()
    {
        transform.position = new Vector3(objective.position.x, 0, objective.position.z);
    }

    private void checkRotation()
    {
        if (Input.GetMouseButton(2) && Input.GetAxis("Mouse X") != 0) rotate();
    }

    private void rotate()
    {
        int direction = Input.GetAxis("Mouse X") > 0 ? 1 : -1;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + direction, transform.eulerAngles.z);
    }
}