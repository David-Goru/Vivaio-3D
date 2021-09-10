using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float defaultSpeed = 1.25f;
    [SerializeField] private float runSpeed = 2.75f;
    [SerializeField] private float maxDistance = 1.5f;
    [SerializeField] private LayerMask worldItemLayer;
    [SerializeField] private LayerMask farmTileLayer;
    [SerializeField] private LayerMask cropPositionLayer;
    [SerializeField] private Transform rightHandModel;
    [SerializeField] private Transform leftHandModel;
    [SerializeField] private Transform model;
    [SerializeField] private CharacterAppearance appearance;

    private bool canMove = true;
    private PlayerMovement movement;
    private PlayerAnimations animations;
    private PlayerInventory inventory;
    private PlayerData data;

    public LayerMask FarmTileLayer { get => farmTileLayer; }
    public LayerMask CropPositionLayer { get => cropPositionLayer; }
    public Transform Model { get => model; }
    public PlayerData Data { get => data; set => data = value; }
    public PlayerAnimations Animations { get => animations; set => animations = value; }

    private void Start()
    {
        movement = new PlayerMovement(this, defaultSpeed, runSpeed);
        animations = new PlayerAnimations(this, rightHandModel, leftHandModel);
        inventory = new PlayerInventory(this);
        Game.Instance.CameraController.Objective = transform;

        if (data != null) appearance.SetAppearance(model, data.AppearanceElements);
    }

    private void Update()
    {
        if (canMove) checkInput();
    }

    private void FixedUpdate()
    {
        movement.FixedUpdate();
    }

    private void checkInput()
    {
        movement.Update();

        if (Input.GetMouseButtonDown(0)) leftClick();
        else if (Input.GetKeyDown(KeyCode.G)) inventory.DropCurrentItem();
    }

    private void leftClick()
    {
        if (!leftClickFloor()) inventory.LeftClick();
    }

    private bool leftClickFloor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, worldItemLayer))
        {
            if (CheckDistance(hit.point)) inventory.TryToPickUp(hit.transform.GetComponent<Item>());
            return true;
        }
        return false;
    }

    public bool CheckDistance(Vector3 targetPosition)
    {
        return Vector3.Distance(transform.position, targetPosition) < maxDistance;
    }

    public void Block()
    {
        canMove = false;
        movement.Block();
    }

    public void Unblock()
    {
        canMove = true;
        animations.Set(AnimationType.NONE);
    }
}