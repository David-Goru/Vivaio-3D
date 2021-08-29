using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    private const float defaultSpeed = 1.25f;
    private const float runSpeed = 2.75f;
    private const float maxDistance = 2.5f;

    [SerializeField] private LayerMask worldItemLayer;
    [SerializeField] private LayerMask farmTileLayer;
    [SerializeField] private Transform rightHandModel;
    [SerializeField] private Transform leftHandModel;
    [SerializeField] private Transform model;
    [SerializeField] private CharacterAppearance appearance;

    private PlayerData data;
    private Item itemInHand;
    private List<AppearanceElement> appearanceElements;
    private bool canMove = true;
    private Vector3 lastFrameMovement;
    private float lastFrameSpeed;
    private string lastAnimation = "IDLE";
    private int handLayer;
    private bool mainHandIsLeft;
    private Transform mainHandModel;
    private Animator animator;

    public PlayerData Data { get => data; set => data = value; }
    public LayerMask FarmTileLayer { get => farmTileLayer; set => farmTileLayer = value; }
    public string LastAnimation { get => lastAnimation; set => lastAnimation = value; }

    private void Start()
    {
        animator = model.GetComponent<Animator>();
        handLayer = animator.GetLayerIndex("MainHandInUse");
        changeMainHand(HandType.RIGHT);
    }

    private void Update()
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
            SetAnimation("IDLE");
            lastFrameMovement = Vector3.zero;            
        }

        if (Input.GetMouseButtonDown(0)) leftClick();
        else if (Input.GetKeyDown(KeyCode.G)) dropCurrentItem();
    }

    private void FixedUpdate()
    {
        if (lastFrameMovement != Vector3.zero)
        {
            Vector3 direction = lastFrameMovement * Time.deltaTime * lastFrameSpeed;
            Vector3 position = transform.position + direction * 10000;
            Vector3 lookPosition = new Vector3(position.x, transform.position.y, position.z);

            transform.Translate(direction);
            model.LookAt(lookPosition);

            data.Position = transform.position;
            data.Rotation = model.eulerAngles;
        }
    }

    public void SetAnimation(string newAnimation)
    {
        if (lastAnimation == newAnimation) return;

        lastAnimation = newAnimation;
        animator.SetTrigger(newAnimation);
    }

    private void leftClick()
    {
        if (!leftClickFloor() && itemInHand != null) itemInHand.Use(this);
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
        if (itemInHand != null)
        {
            if (itemInHand.Data.Name != item.Data.Name) return;
            else if (itemInHand.Data.CurrentStack >= itemInHand.Info.MaxStack) return;
        }

        if (itemInHand == null)
        {
            item.PickUp(item.Data.CurrentStack);
            data.ItemInHand = item.Data;
            showCurrentItemInHand();

            if (itemInHand != null)
            {
                itemInHand.Data = item.Data;
                itemInHand.Info = item.Info;
            }
        }
        else
        {
            int stackToTrade = item.Data.CurrentStack;
            if (stackToTrade + itemInHand.Data.CurrentStack > itemInHand.Info.MaxStack) stackToTrade = itemInHand.Info.MaxStack - itemInHand.Data.CurrentStack;

            itemInHand.Data.CurrentStack += stackToTrade;
            item.PickUp(stackToTrade);
        }

        //LastAnimation = "PICKUPITEM";
    }

    private void dropCurrentItem()
    {
        if (itemInHand == null) return;

        //LastAnimation = "DROPITEM";
        hideCurrentItemInHand();
        dropCurrentItemAtPlayerPosition();
        itemInHand = null;
        data.ItemInHand = null;
    }

    public bool CheckDistance(Vector3 targetPosition)
    {
        return distanceFrom(targetPosition) < maxDistance;
    }

    private void walk()
    {
        SetAnimation("WALK");
        lastFrameMovement = getLastFrameMovement();
        
        lastFrameSpeed = defaultSpeed;
    }

    private void run()
    {
        SetAnimation("RUN");
        lastFrameMovement = getLastFrameMovement();
        lastFrameSpeed = runSpeed;
    }

    private Vector3 getLastFrameMovement()
    {
        Vector3 movementDirectionAndMagnitude = Vector3.right* Input.GetAxis("Horizontal") + Vector3.forward * Input.GetAxis("Vertical");
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

    // PlayerHand
    private void setHandInUse()
    {
        changeHandWeight(1.0f);
    }

    private void unSetHandInUse()
    {
        changeHandWeight(0.0f);
    }

    private void changeMainHand(HandType type = HandType.DEFAULT)
    {
        if (type == HandType.LEFT) mainHandIsLeft = true;
        else if (type == HandType.RIGHT) mainHandIsLeft = false;
        else mainHandIsLeft = !mainHandIsLeft;

        if (mainHandModel != null) mainHandModel.gameObject.SetActive(false);
        if (mainHandIsLeft)
        {
            mainHandModel = leftHandModel;
            if (Data != null && Data.ItemInHand != null)
            {
                Transform itemInLeftHand = leftHandModel.Find(Data.ItemInHand.Name);
                if (itemInLeftHand != null) itemInLeftHand.gameObject.SetActive(true);

                Transform itemInRightHand = rightHandModel.Find(Data.ItemInHand.Name);
                if (itemInRightHand != null) itemInRightHand.gameObject.SetActive(false);
            }
        }
        else
        {
            mainHandModel = rightHandModel;
            if (Data != null && Data.ItemInHand != null)
            {
                Transform itemInRightHand = rightHandModel.Find(Data.ItemInHand.Name);
                if (itemInRightHand != null) itemInRightHand.gameObject.SetActive(true);

                Transform itemInLeftHand = leftHandModel.Find(Data.ItemInHand.Name);
                if (itemInLeftHand != null) itemInLeftHand.gameObject.SetActive(false);
            }
        }
        mainHandModel.gameObject.SetActive(true);

        animator.SetBool("LeftHand", mainHandIsLeft);
    }

    private void changeHandWeight(float weightValue)
    {
        animator.SetLayerWeight(handLayer, weightValue);
    }

    private void showCurrentItemInHand()
    {
        Transform itemInHandModel = mainHandModel.Find(Data.ItemInHand.Name);
        if (itemInHandModel == null) return;

        itemInHandModel.gameObject.SetActive(true);
        itemInHand = itemInHandModel.GetComponent<Item>();
    }

    private void hideCurrentItemInHand()
    {
        Transform itemInHand = mainHandModel.Find(Data.ItemInHand.Name);
        if (itemInHand == null) return;

        mainHandModel.Find(Data.ItemInHand.Name).gameObject.SetActive(false);
    }

    private void dropCurrentItemAtPlayerPosition()
    {
        Transform dropTransform = mainHandModel.Find(itemInHand.Data.Name);
        if (dropTransform == null)
        {
            dropTransform = transform;
            dropTransform.eulerAngles = model.eulerAngles;
        }

        GameObject itemOnWorld = Instantiate(itemInHand.Info.WorldModel, dropTransform.position, dropTransform.rotation);
        itemOnWorld.GetComponent<Item>().Data = itemInHand.Data;
        itemOnWorld.GetComponent<Item>().Info = itemInHand.Info;
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

public enum HandType
{
    RIGHT,
    LEFT,
    DEFAULT
}

public enum Animations
{
    IDLE,
    WALK,
    PLOW
}