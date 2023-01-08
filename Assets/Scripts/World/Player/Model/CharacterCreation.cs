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

    public CharacterAppearance characterModel;

    [NonSerialized] public AnimationType LastAnimation = AnimationType.IDLE;
    [NonSerialized] public Transform Model;
    [NonSerialized] public Animator Animator;

    public static CharacterCreation Instance;

    private void Awake()
    {
        Instance = this;
        SaveSystem.SetGameData();
        SaveSystem.GameData.Player = new PlayerData
        {
            AppearanceElements = new List<AppearanceElement>()
        };

        InitializeMainHand();
        InitializeComponents();
        InitializeSelectors();
    }

    private void FixedUpdate()
    {
        CheckRotation();
    }

    private void Update()
    {
        CheckMovement();
    }

    private void InitializeComponents()
    {
        Model = Instantiate(characterModelPrefab).transform;
        Model.SetParent(transform);
        Animator = Model.GetComponent<Animator>();
    }

    private void InitializeSelectors()
    {
        foreach (var selector in characterModel.appearanceElementSelectors)
        {
            selector.SetUpSelector(Model, appearanceElementPrefab, appearanceColorPrefab, appearanceElementsContainer);
        }
    }

    private void InitializeMainHand()
    {
        mainHandPanel.Find(SaveSystem.GameData.Player.MainHand.ToString()).GetComponent<Image>().color = selectedButtonColor;
    }

    private void CheckMovement() 
    {
        if (!IsMovingModel()) ChangeAnimation(AnimationType.IDLE);
        else if (IsRunning()) ChangeAnimation(AnimationType.RUN);
        else ChangeAnimation(AnimationType.WALK);
    }

    private void CheckRotation()
    {
        if (IsRotatingModel()) RotateModel();
    }

    private static bool IsRotatingModel()
    {
        return Input.GetAxis("Horizontal") != 0;
    }

    private static bool IsMovingModel()
    {
        return Input.GetAxis("Vertical") > 0;
    }

    private static bool IsRunning()
    {
        return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    }

    private void RotateModel()
    {
        var angle = -Input.GetAxis("Horizontal") * modelRotationSpeed;
        Model.Rotate(Vector3.up, angle);
    }

    private void ChangeAnimation(AnimationType newAnimation)
    {
        if (LastAnimation == newAnimation) return;
        LastAnimation = newAnimation;
        Animator.SetTrigger(newAnimation.ToString());
    }

    public void HideBodyElement(AppearanceElement appearanceElement, string option)
    {
        characterModel.HideBodyElement(Model, appearanceElement, option);
    }

    public void ShowBodyElement(AppearanceElement appearanceElement, string option)
    {
        characterModel.ShowBodyElement(Model, appearanceElement, option);
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