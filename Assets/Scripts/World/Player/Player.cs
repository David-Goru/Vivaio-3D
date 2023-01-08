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

    public LayerMask farmTileLayer;
    public LayerMask cropPositionLayer;
    public LayerMask worldItemLayer;
    public LayerMask weedLayer;
    public Transform model;

    private PlayerMovement movement;
    public PlayerAnimations Animations;
    public PlayerInventory Inventory;
    [HideInInspector] public PlayerData data;

    private void Start()
    {
        movement = new PlayerMovement(this, defaultSpeed, runSpeed);
        Animations = new PlayerAnimations(this, rightHandModel, leftHandModel);
        Inventory = new PlayerInventory(this);
        Game.Instance.cameraController.objective = transform;

        if (data != null) appearance.SetAppearance(model, data.AppearanceElements);
    }

    private void Update()
    {
        if (canMove) CheckInput();
    }

    private void FixedUpdate()
    {
        movement.FixedUpdate();
    }

    private void CheckInput()
    {
        movement.Update();

        if (Input.GetMouseButtonDown(0)) LeftClick();
        else if (Input.GetKeyDown(KeyCode.G)) Inventory.DropCurrentItem();
    }

    private void LeftClick()
    {
        if (!LeftClickFloor()) Inventory.LeftClick();
    }

    private bool LeftClickFloor()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, 100, worldItemLayer))
        {
            if (CheckDistance(hit.point)) Inventory.TryToPickUp(hit.transform.GetComponent<Item>());
            return true;
        }

        if (!Physics.Raycast(ray, out hit, 100, weedLayer)) return false;
        if (CheckDistance(hit.point)) Inventory.TryToPickUp(hit.transform.GetComponent<Weed>());
        return true;
    }

    public bool CheckDistance(Vector3 targetPosition)
    {
        return Vector3.Distance(transform.position, targetPosition) < maxDistance;
    }

    public void Block(UnityAction unblockAction = null)
    {
        canMove = false;
        movement.Block();
        onUnblockAction = unblockAction;
    }

    public void Unblock()
    {
        canMove = true;
        Animations.Set(AnimationType.NONE);
        if (onUnblockAction == null) return;
        onUnblockAction();
        onUnblockAction = null;
    }
}