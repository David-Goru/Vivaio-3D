using UnityEngine;

public class PlayerMovement
{
    private Player player;
    private float defaultSpeed;
    private float runSpeed;
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
        checkInput();
    }

    public void FixedUpdate()
    {
        if (lastFrameMovement != Vector3.zero) updatePositionAndRotation();
    }

    public void Block()
    {
        lastFrameMovement = Vector3.zero;
    }

    private void checkInput()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            if (Input.GetButton("Run")) run();
            else walk();
        }
        else if (lastFrameMovement != Vector3.zero)
        {
            player.Animations.Set(AnimationType.IDLE);
            lastFrameMovement = Vector3.zero;
        }
    }

    private void updatePositionAndRotation()
    {
        Vector3 direction = lastFrameMovement * Time.deltaTime * lastFrameSpeed;
        Vector3 position = player.transform.position + direction * 10000;
        Vector3 lookPosition = new Vector3(position.x, player.transform.position.y, position.z);
        Quaternion rotation = Quaternion.Euler(0, Game.Instance.CameraController.transform.eulerAngles.y, 0);

        player.transform.Translate(rotation * direction);
        player.Model.LookAt(rotation * lookPosition);

        player.Data.Position = player.transform.position;
        player.Data.Rotation = player.Model.eulerAngles;
    }

    private void walk()
    {
        player.Animations.Set(AnimationType.WALK);
        lastFrameMovement = getLastFrameMovement();
        lastFrameSpeed = defaultSpeed;
    }

    private void run()
    {
        player.Animations.Set(AnimationType.RUN);
        lastFrameMovement = getLastFrameMovement();
        lastFrameSpeed = runSpeed;
    }

    private Vector3 getLastFrameMovement()
    {
        Vector3 movementDirectionAndMagnitude = Vector3.right * Input.GetAxis("Horizontal") + Vector3.forward * Input.GetAxis("Vertical");
        return Vector3.ClampMagnitude(movementDirectionAndMagnitude, 1.0f); // Clamp to avoid faster diagonal movement
    }
}