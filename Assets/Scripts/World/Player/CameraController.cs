using UnityEngine;

public class CameraController : MonoBehaviour
{
    [HideInInspector] public Transform Objective;

    private void FixedUpdate()
    {
        if (Objective == null) return;

        focusObjective();
        checkRotation();
    }

    private void focusObjective()
    {
        transform.position = new Vector3(Objective.position.x, 0, Objective.position.z);
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