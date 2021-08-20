using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Player : GameElement
{
    private const float defaultSpeed = 1.25f;
    private const float runSpeed = 2.75f;
    private const float maxDistance = 2.5f;

    private Vector3 position = new Vector3(0, 0, 0);
    private List<AppearanceElement> appearanceElements;
    private Item itemInHand = null;

    [System.NonSerialized] private bool canMove = true;
    [System.NonSerialized] private Vector3 lastFrameMovement;
    [System.NonSerialized] private float lastFrameSpeed;
    [System.NonSerialized] private PlayerObject playerObject;
    [System.NonSerialized] private string lastAnimation = "IDLE";

    public PlayerObject PlayerObject { get => playerObject; }
    public string LastAnimation { get => lastAnimation; set => lastAnimation = value; }
    public Item ItemInHand { get => itemInHand; set => itemInHand = value; }

    public override void Instantiate()
    {
        GameObject player = Object.Instantiate(Prefab);
        playerObject = player.GetComponent<PlayerObject>();

        if (playerObject != null)
        {
            playerObject.Data = this;
            playerObject.UpdatePosition(position);
            if (appearanceElements == null) appearanceElements = CharacterCreation.SelectedAppearance;
            if (appearanceElements != null) playerObject.SetAppearance(appearanceElements);
        }
        else Debug.Log("PlayerObject component not found in Player model.");
    }

    public override void Update()
    {
        if (!canMove)
        {
            lastFrameMovement = Vector3.zero;
            return;
        }

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            if (Input.GetButton("Run")) run();
            else walk();         
        }
        else if (lastFrameMovement != Vector3.zero)
        {
            setAnimation("IDLE");
            lastFrameMovement = Vector3.zero;            
        }

        if (Input.GetMouseButtonDown(0)) leftClick();
        else if (Input.GetKeyDown(KeyCode.G)) dropCurrentItem();
    }

    public override void FixedUpdate()
    {
        if (lastFrameMovement != Vector3.zero)
        {
            Vector3 direction = lastFrameMovement * Time.deltaTime * lastFrameSpeed;
            playerObject.Translate(direction);
            playerObject.LookAt(direction * 10);
        }
    }

    private void leftClick()
    {
        if (!leftClickFloor() && itemInHand != null) itemInHand.Use(this);
    }

    private bool leftClickFloor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, playerObject.WorldItemLayer))
        {
            if (CheckDistance(hit.point)) tryToPickUp(hit.transform.GetComponent<WorldItem>());
            return true;
        }
        return false;
    }

    private void tryToPickUp(WorldItem worldItem)
    {
        if (itemInHand != null)
        {
            if (itemInHand.ItemModel != worldItem.Item.ItemModel) return;
            else if (itemInHand.Stack >= itemInHand.ItemModel.MaxStack) return;
        }

        if (itemInHand == null)
        {
            itemInHand = worldItem.PickUp(worldItem.Item.Stack);
            playerObject.ShowCurrentItemInHand();
        }
        else
        {
            int stackToTrade = worldItem.Item.Stack;
            if (stackToTrade + itemInHand.Stack > itemInHand.ItemModel.MaxStack) stackToTrade = itemInHand.ItemModel.MaxStack - itemInHand.Stack;

            itemInHand.Stack += stackToTrade;
            worldItem.PickUp(stackToTrade);
        }

        //LastAnimation = "PICKUPITEM";
    }

    private void dropCurrentItem()
    {
        if (itemInHand == null) return;

        //LastAnimation = "DROPITEM";
        playerObject.HideCurrentItemInHand();
        playerObject.DropCurrentItemAtPlayerPosition();
        itemInHand = null;
    }

    public bool CheckDistance(Vector3 targetPosition)
    {
        return playerObject.DistanceFrom(targetPosition) < maxDistance;
    }

    private void walk()
    {
        setAnimation("WALK");
        lastFrameMovement = getLastFrameMovement();
        
        lastFrameSpeed = defaultSpeed;
    }

    private void run()
    {
        setAnimation("RUN");
        lastFrameMovement = getLastFrameMovement();
        lastFrameSpeed = runSpeed;
    }

    private Vector3 getLastFrameMovement()
    {
        Vector3 movementDirectionAndMagnitude = Vector3.right* Input.GetAxis("Horizontal") + Vector3.forward * Input.GetAxis("Vertical");
        return Vector3.ClampMagnitude(movementDirectionAndMagnitude, 1.0f); // Clamp to avoid faster diagonal movement
    }

    private void setAnimation(string newAnimation)
    {
        if (LastAnimation == newAnimation) return;

        LastAnimation = newAnimation;
        playerObject.SetAnimation(newAnimation);
    }

    public void BlockMovement()
    {
        canMove = false;
    }

    public void UnblockMovement()
    {
        canMove = true;
    }
}