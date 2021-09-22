using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCreation : MonoBehaviour
{
    [SerializeField] private float modelRotationSpeed = 0.0f;
    [SerializeField] private Color defaultButtonColor;
    [SerializeField] private Color selectedButtonColor;
    [SerializeField] private Transform appearanceElementsContainer;
    [SerializeField] private GameObject appearanceElementPrefab;
    [SerializeField] private GameObject appearanceColorPrefab;
    [SerializeField] private GameObject characterModelPrefab;
    [SerializeField] private Transform mainHandPanel;

    public CharacterAppearance CharacterModel;

    [NonSerialized] public AnimationType LastAnimation = AnimationType.IDLE;
    [NonSerialized] public Transform Model;
    [NonSerialized] public Animator Animator;

    public static CharacterCreation Instance;

    private void Awake()
    {
        Instance = this;
        SaveSystem.SetGameData();
        SaveSystem.GameData.Player = new PlayerData();
        SaveSystem.GameData.Player.AppearanceElements = new List<AppearanceElement>();

        initializeMainHand();
        initializeComponents();
        initializeSelectors();
    }

    private void FixedUpdate()
    {
        checkRotation();
    }

    private void Update()
    {
        checkMovement();
    }

    private void initializeComponents()
    {
        Model = Instantiate(characterModelPrefab).transform;
        Model.SetParent(transform);
        Animator = Model.GetComponent<Animator>();
    }

    private void initializeSelectors()
    {
        foreach (AppearanceElementSelector selector in CharacterModel.AppearanceElementSelectors)
        {
            selector.SetUpSelector(Model, appearanceElementPrefab, appearanceColorPrefab, appearanceElementsContainer);
        }
    }

    private void initializeMainHand()
    {
        mainHandPanel.Find(SaveSystem.GameData.Player.MainHand.ToString()).GetComponent<Image>().color = selectedButtonColor;
    }

    private void checkMovement() 
    {
        if (!isMovingModel()) changeAnimation(AnimationType.IDLE);
        else if (isRunning()) changeAnimation(AnimationType.RUN);
        else changeAnimation(AnimationType.WALK);
    }

    private void checkRotation()
    {
        if (isRotatingModel()) rotateModel();
    }

    private bool isRotatingModel()
    {
        return Input.GetAxis("Horizontal") != 0;
    }

    private bool isMovingModel()
    {
        return Input.GetAxis("Vertical") > 0;
    }

    private bool isRunning()
    {
        return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    }

    private void rotateModel()
    {
        float angle = -Input.GetAxis("Horizontal") * modelRotationSpeed;
        Model.Rotate(Vector3.up, angle);
    }

    private void changeAnimation(AnimationType newAnimation)
    {
        if (LastAnimation != newAnimation)
        {
            LastAnimation = newAnimation;
            Animator.SetTrigger(newAnimation.ToString());
        }
    }

    public void HideBodyElement(AppearanceElement appearanceElement, string option)
    {
        CharacterModel.HideBodyElement(Model, appearanceElement, option);
    }

    public void ShowBodyElement(AppearanceElement appearanceElement, string option)
    {
        CharacterModel.ShowBodyElement(Model, appearanceElement, option);
    }

    public void ChangeMainHand(string newMainHand)
    {
        mainHandPanel.Find(SaveSystem.GameData.Player.MainHand.ToString()).GetComponent<Image>().color = defaultButtonColor;
        mainHandPanel.Find(newMainHand).GetComponent<Image>().color = selectedButtonColor;
        SaveSystem.GameData.Player.MainHand = newMainHand == "LEFT" ? HandType.LEFT : HandType.RIGHT;
    }

    // TEST
    public void TestPlay()
    {
        Destroy(gameObject);
        Instantiate(Resources.Load<GameObject>("Game"));
    }
}