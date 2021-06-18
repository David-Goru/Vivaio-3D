using UnityEngine;

[System.Serializable]
public class Player
{
    public const float defaultSpeed = 1.0f;
    public const float runSpeed = 2.0f;

    private Vector3 position = new Vector3(0, 0, 0);

    [System.NonSerialized] private bool canMove = true;
    [System.NonSerialized] private Vector3 lastFrameMovement;
    [System.NonSerialized] private float lastFrameSpeed;
    [System.NonSerialized] private GameObject model;

    public Player(GameObject model)
    {
        this.model = model;
        model.transform.position = position;

        PlayerObject modelScript = model.GetComponent<PlayerObject>();
        if (modelScript != null) modelScript.Data = this;
        else Debug.Log("PlayerObject component not found in Player model.");
    }

    public void Update()
    {
        if (!canMove)
        {
            lastFrameMovement = Vector3.zero;
            return;
        }

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            lastFrameMovement = Vector3.ClampMagnitude(Vector3.right * Input.GetAxis("Horizontal") + Vector3.forward * Input.GetAxis("Vertical"), 1.0f); // Clamp to avoid faster diagonal movement
            lastFrameSpeed = Input.GetButton("Run") ? runSpeed : defaultSpeed;

            // Set animation
        }
        else
        {
            lastFrameMovement = Vector3.zero;
            // Stop animation
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Physics.CheckSphere(mousePos, 1 << LayerMask.NameToLayer("Ground")))
            {

            }
        }
    }

    public void FixedUpdate()
    {
        if (lastFrameMovement != Vector3.zero) model.transform.Translate(lastFrameMovement * Time.deltaTime * lastFrameSpeed);
    }
}