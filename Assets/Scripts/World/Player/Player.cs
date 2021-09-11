using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField] private float defaultSpeed = 1.25f;
    [SerializeField] private float runSpeed = 2.75f;
    [SerializeField] private float maxDistance = 1.5f;
    [SerializeField] private Transform rightHandModel;
    [SerializeField] private Transform leftHandModel;
    [SerializeField] private CharacterAppearance appearance;

    private bool canMove = true;
    private UnityAction onUnblockAction;

    public LayerMask FarmTileLayer;
    public LayerMask CropPositionLayer;
    public LayerMask WorldItemLayer;
    public Transform Model;

    [HideInInspector] public PlayerMovement Movement;
    [HideInInspector] public PlayerAnimations Animations;
    [HideInInspector] public PlayerInventory Inventory;
    [HideInInspector] public PlayerData Data;

    private void Start()
    {
        Movement = new PlayerMovement(this, defaultSpeed, runSpeed);
        Animations = new PlayerAnimations(this, rightHandModel, leftHandModel);
        Inventory = new PlayerInventory(this);
        Game.Instance.CameraController.Objective = transform;

        if (Data != null) appearance.SetAppearance(Model, Data.AppearanceElements);
    }

    private void Update()
    {
        if (canMove) checkInput();
    }

    private void FixedUpdate()
    {
        Movement.FixedUpdate();
    }

    private void checkInput()
    {
        Movement.Update();

        if (Input.GetMouseButtonDown(0)) leftClick();
        else if (Input.GetKeyDown(KeyCode.G)) Inventory.DropCurrentItem();
    }

    private void leftClick()
    {
        if (!leftClickFloor()) Inventory.LeftClick();
    }

    private bool leftClickFloor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, WorldItemLayer))
        {
            if (CheckDistance(hit.point)) Inventory.TryToPickUp(hit.transform.GetComponent<Item>());
            return true;
        }
        return false;
    }

    public bool CheckDistance(Vector3 targetPosition)
    {
        return Vector3.Distance(transform.position, targetPosition) < maxDistance;
    }

    public void Block(UnityAction unblockAction = null)
    {
        canMove = false;
        Movement.Block();
        onUnblockAction = unblockAction;
    }

    public void Unblock()
    {
        canMove = true;
        Animations.Set(AnimationType.NONE);
        if (onUnblockAction != null)
        {
            onUnblockAction();
            onUnblockAction = null;
        }
    }
}