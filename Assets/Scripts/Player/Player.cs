using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Player
{
    public const float defaultSpeed = 1.25f;
    public const float runSpeed = 2.75f;

    private Vector3 position = new Vector3(0, 0, 0);
    private List<KeyValuePair<string, string>> characterBodyElements;

    [System.NonSerialized] private bool canMove = true;
    [System.NonSerialized] private Vector3 lastFrameMovement;
    [System.NonSerialized] private float lastFrameSpeed;
    [System.NonSerialized] private PlayerObject playerObject;

    public void Instantiate()
    {
        if (World.Instance.Player != null)
        {
            GameObject model = Object.Instantiate(World.Instance.Player);
            playerObject = model.GetComponent<PlayerObject>();

            if (playerObject != null)
            {
                playerObject.Data = this;
                playerObject.UpdatePosition(position);
                if (characterBodyElements == null && CharacterCreation.SelectedAppearance != null) characterBodyElements = CharacterCreation.SelectedAppearance;
                if (characterBodyElements != null) playerObject.SetModel(characterBodyElements);
            }
            else Debug.Log("PlayerObject component not found in Player model.");
        }
        else Debug.Log("Player prefab not found on World instance.");
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
            playerObject.SetAnimation("WALK");
        }
        else
        {
            lastFrameMovement = Vector3.zero;
            playerObject.SetAnimation("IDLE");
        }

        if (Input.GetMouseButtonDown(0))
        {
            // if has tool in hands, Tool.Use();
            TryPlow();
        }
    }

    public void FixedUpdate()
    {
        if (lastFrameMovement != Vector3.zero)
        {
            Vector3 direction = lastFrameMovement * Time.deltaTime * lastFrameSpeed;
            playerObject.Translate(direction);
            playerObject.LookAt(direction);
        }
    }

    // Hoe.Try():
    public void TryPlow()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, playerObject.PlowableLayer))
        {
            if (CheckDistance(hit.point)) Plow(hit);
        }
    }

    // How.CheckDistance():
    public bool CheckDistance(Vector3 targetPosition)
    {
        float maxDistance = 2.5f;
        return playerObject.DistanceFrom(targetPosition) < maxDistance;
    }

    // Hoe.Use():
    public void Plow(RaycastHit tile)
    {
        bool plowed = tile.transform.GetComponent<FarmObject>().PlowAt(tile.point);
        if (plowed)
        {
            // Plowing animation
        }
    }
}