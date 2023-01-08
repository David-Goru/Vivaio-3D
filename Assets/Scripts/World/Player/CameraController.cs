using UnityEngine;

public class CameraController : MonoBehaviour
{
    [HideInInspector] public Transform objective;

    private void FixedUpdate()
    {
        if (objective == null) return;

        FocusObjective();
        CheckRotation();
    }

    private void FocusObjective()
    {
        transform.position = new Vector3(objective.position.x, 0, objective.position.z);
    }

    private void CheckRotation()
    {
        if (Input.GetMouseButton(2) && Input.GetAxis("Mouse X") != 0) Rotate();
    }

    private void Rotate()
    {
        var direction = Input.GetAxis("Mouse X") > 0 ? 1 : -1;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + direction, transform.eulerAngles.z);
    }
}