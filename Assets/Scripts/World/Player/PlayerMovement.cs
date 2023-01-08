using UnityEngine;

public class PlayerMovement
{
    private readonly Player player;
    private readonly float defaultSpeed;
    private readonly float runSpeed;
    private Vector3 lastFrameMovement;
    private float lastFrameSpeed;

    public PlayerMovement(Player player, float defaultSpeed, float runSpeed)
    {
        this.player = player;
        this.defaultSpeed = defaultSpeed;
        this.runSpeed = runSpeed;
    }

    public void Update()
    {
        CheckInput();
    }

    public void FixedUpdate()
    {
        if (lastFrameMovement != Vector3.zero) UpdatePositionAndRotation();
    }

    public void Block()
    {
        lastFrameMovement = Vector3.zero;
    }

    private void CheckInput()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            if (Input.GetButton("Run")) Run();
            else Walk();
        }
        else if (lastFrameMovement != Vector3.zero)
        {
            player.Animations.Set(AnimationType.IDLE);
            lastFrameMovement = Vector3.zero;
        }
    }

    private void UpdatePositionAndRotation()
    {
        var newPosition = player.transform.position + lastFrameMovement * 1000000;
        var lookPosition = new Vector3(newPosition.x, player.transform.position.y, newPosition.z);
        var cameraRotation = Quaternion.Euler(0, Game.Instance.cameraController.transform.eulerAngles.y, 0);

        var targetRotation = Quaternion.LookRotation(cameraRotation * lookPosition);
        player.model.rotation = Quaternion.RotateTowards(player.model.rotation, targetRotation, 540.0f * Time.deltaTime);
        player.transform.Translate(cameraRotation * lastFrameMovement * Time.deltaTime * lastFrameSpeed);

        player.data.Position = player.transform.position;
        player.data.Rotation = player.model.eulerAngles;
    }

    private void Walk()
    {
        player.Animations.Set(AnimationType.WALK);
        lastFrameMovement = GetLastFrameMovement();
        lastFrameSpeed = defaultSpeed;
    }

    private void Run()
    {
        player.Animations.Set(AnimationType.RUN);
        lastFrameMovement = GetLastFrameMovement();
        lastFrameSpeed = runSpeed;
    }

    private static Vector3 GetLastFrameMovement()
    {
        var movementDirectionAndMagnitude = Vector3.right * Input.GetAxis("Horizontal") + Vector3.forward * Input.GetAxis("Vertical");
        return Vector3.ClampMagnitude(movementDirectionAndMagnitude, 1.0f); // Clamp to avoid faster diagonal movement
    }
}