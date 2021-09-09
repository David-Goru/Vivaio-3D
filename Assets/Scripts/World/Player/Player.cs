using UnityEngine;
using System.Collections.Generic;

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

    private PlayerData data;
    private Item itemInHand;
    private bool canMove = true;
    private Vector3 lastFrameMovement;
    private float lastFrameSpeed;
    private AnimationType lastAnimation = AnimationType.IDLE;
    private int handLayer;
    private Transform mainHandModel;
    private Animator animator;

    public PlayerData Data { get => data; set => data = value; }
    public LayerMask FarmTileLayer { get => farmTileLayer; }
    public AnimationType LastAnimation { get => lastAnimation; set => lastAnimation = value; }
    public Animator Animator { get => animator; }
    public LayerMask CropPositionLayer { get => cropPositionLayer; }

    private void Start()
    {
        animator = model.GetComponent<Animator>();
        handLayer = animator.GetLayerIndex("MainHandInUse");
        changeMainHand(data.MainHand);
        Game.Instance.CameraController.Objective = transform;

        if (data != null) setAppearance(data.AppearanceElements);
    }

    private void Update()
    {
        if (canMove) checkInput();
        else lastFrameMovement = Vector3.zero;        
    }

    private void FixedUpdate()
    {
        if (lastFrameMovement != Vector3.zero) updatePositionAndRotation();
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
            SetAnimation(AnimationType.IDLE);
            lastFrameMovement = Vector3.zero;
        }

        if (Input.GetMouseButtonDown(0)) leftClick();
        else if (Input.GetKeyDown(KeyCode.G)) dropCurrentItem();
    }

    private void updatePositionAndRotation()
    {
        Vector3 direction = lastFrameMovement * Time.deltaTime * lastFrameSpeed;
        Vector3 position = transform.position + direction * 10000;
        Vector3 lookPosition = new Vector3(position.x, transform.position.y, position.z);
        Quaternion rotation = Quaternion.Euler(0, Game.Instance.CameraController.transform.eulerAngles.y, 0);

        transform.Translate(rotation * direction);
        model.LookAt(rotation * lookPosition);

        data.Position = transform.position;
        data.Rotation = model.eulerAngles;
    }

    private void walk()
    {
        SetAnimation(AnimationType.WALK);
        lastFrameMovement = getLastFrameMovement();
        lastFrameSpeed = defaultSpeed;
    }

    private void run()
    {
        SetAnimation(AnimationType.RUN);
        lastFrameMovement = getLastFrameMovement();
        lastFrameSpeed = runSpeed;
    }

    private Vector3 getLastFrameMovement()
    {
        Vector3 movementDirectionAndMagnitude = Vector3.right * Input.GetAxis("Horizontal") + Vector3.forward * Input.GetAxis("Vertical");
        return Vector3.ClampMagnitude(movementDirectionAndMagnitude, 1.0f); // Clamp to avoid faster diagonal movement
    }

    private float distanceFrom(Vector3 targetPosition)
    {
        return Vector3.Distance(transform.position, targetPosition);
    }

    private void setAppearance(List<AppearanceElement> appearanceElements)
    {        
        Transform model = transform.Find("Player model");
        appearance.SetAppearance(model, appearanceElements);
    }

    private void leftClick()
    {
        if (!leftClickFloor() && itemInHand != null)
        {
            itemInHand.Use(this);
            data.ItemInHand = itemInHand.Data;
        }
    }

    private bool leftClickFloor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, worldItemLayer))
        {
            if (CheckDistance(hit.point)) tryToPickUp(hit.transform.GetComponent<Item>());
            return true;
        }
        return false;
    }

    private void tryToPickUp(Item item)
    {
        if (data.ItemInHand != null)
        {
            if (data.ItemInHand.Name != item.Data.Name) return;
            else if (data.ItemInHand.CurrentStack >= itemInHand.Info.MaxStack) return;
        }

        if (data.ItemInHand == null)
        {
            item.PickUp(item.Data.CurrentStack);
            data.ItemInHand = item.Data;
            showNewItemInHand(item.Info);
        }
        else
        {
            int stackToTrade = item.Data.CurrentStack;
            if (stackToTrade + data.ItemInHand.CurrentStack > itemInHand.Info.MaxStack) stackToTrade = itemInHand.Info.MaxStack - data.ItemInHand.CurrentStack;

            data.ItemInHand.CurrentStack += stackToTrade;
            item.PickUp(stackToTrade);
        }

        if (itemInHand != null) itemInHand.Data = data.ItemInHand;        

        setHandInUse();
        //LastAnimation = "PICKUPITEM";
    }

    private void dropCurrentItem()
    {
        if (data.ItemInHand == null) return;

        //LastAnimation = "DROPITEM";
        hideCurrentItemInHand();
        dropCurrentItemAtPlayerPosition();
        data.ItemInHand = null;
        itemInHand = null;
    }

    private void changeMainHand(HandType type = HandType.RIGHT)
    {
        if (data != null) data.MainHand = type;
        animator.SetBool("LeftHand", type == HandType.LEFT);

        changeHandState(type == HandType.RIGHT ? rightHandModel : leftHandModel, true);
        changeHandState(type == HandType.RIGHT ? leftHandModel : rightHandModel, false);
    }

    private void changeHandState(Transform hand, bool newState)
    {
        if (newState == true) mainHandModel = hand;
        hand.gameObject.SetActive(newState);

        if (data != null && data.ItemInHand != null)
        {
            Transform itemInHand = hand.Find(data.ItemInHand.Name);
            if (itemInHand != null) itemInHand.gameObject.SetActive(newState);
            else hand.Find("Item").gameObject.SetActive(newState);
        }
    }

    private void showNewItemInHand(ItemInfo info)
    {
        Transform itemInHandModel = mainHandModel.Find(info.HandItemName);
        if (itemInHandModel == null) itemInHandModel = mainHandModel.Find("Item");

        itemInHandModel.gameObject.SetActive(true);
        itemInHand = itemInHandModel.GetComponent<Item>();
        itemInHand.Info = info;
    }

    private void hideCurrentItemInHand()
    {
        Transform itemInHandModel = mainHandModel.Find(itemInHand.Info.HandItemName);
        if (itemInHandModel == null) itemInHandModel = mainHandModel.Find("Item");

        itemInHandModel.gameObject.SetActive(false);
    }

    private void dropCurrentItemAtPlayerPosition()
    {
        Transform dropTransform = mainHandModel.Find(itemInHand.Info.HandItemName);
        if (dropTransform == null) dropTransform = mainHandModel.Find("Item"); 

        GameObject itemOnWorld = Instantiate(itemInHand.Info.WorldModel, dropTransform.position, dropTransform.rotation);
        itemOnWorld.GetComponent<Item>().Data = data.ItemInHand;
        itemOnWorld.GetComponent<Item>().Info = itemInHand.Info;
        unSetHandInUse();
    }

    private void setHandInUse()
    {
        changeHandWeight(1.0f);
    }

    private void unSetHandInUse()
    {
        changeHandWeight(0.0f);
    }

    private void changeHandWeight(float weightValue)
    {
        animator.SetLayerWeight(handLayer, weightValue);
    }

    public void SetAnimation(AnimationType newAnimation)
    {
        if (lastAnimation == newAnimation) return;

        lastAnimation = newAnimation;
        animator.SetTrigger(newAnimation.ToString());
    }

    public bool CheckDistance(Vector3 targetPosition)
    {
        return distanceFrom(targetPosition) < maxDistance;
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