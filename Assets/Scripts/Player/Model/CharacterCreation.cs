using System.Collections.Generic;
using UnityEngine;

public class CharacterCreation : MonoBehaviour
{
    [SerializeField] private Transform appearanceElementsContainer;
    [SerializeField] private GameObject appearanceElementPrefab;
    [SerializeField] private GameObject appearanceColorPrefab;
    [SerializeField] private GameObject characterModelPrefab;
    [SerializeField] private CharacterAppearance characterModel;

    private Transform model;
    public static List<AppearanceElement> SelectedAppearance;

    public static CharacterCreation Instance;

    private void Start()
    {
        Instance = this;
        model = Instantiate(characterModelPrefab).transform.Find("Player model");
        initializeSelectors();
    }

    private void Update()
    {
        if (Input.GetAxis("Horizontal") != 0)
        {
            float angle = -Input.GetAxis("Horizontal") / 3.0f;
            model.Rotate(Vector3.up, angle);
        }
    }

    private void initializeSelectors()
    {
        AppearanceElementSelector[] appearanceElements = characterModel.AppearanceElementSelectors;
        SelectedAppearance = new List<AppearanceElement>();

        for (int i = 0; i < appearanceElements.Length; i++)
        {
            appearanceElements[i].SetUpSelector(model, appearanceElementPrefab, appearanceColorPrefab, appearanceElementsContainer);
        }
    }

    public void HideBodyElement(AppearanceElement appearanceElement)
    {
        characterModel.HideBodyElement(model, appearanceElement);
    }

    public void ShowBodyElement(AppearanceElement appearanceElement)
    {
        characterModel.ShowBodyElement(model, appearanceElement);
    }

    public static void AddSelectedAppearance(AppearanceElement appearanceElement)
    {
        if (SelectedAppearance == null) SelectedAppearance = new List<AppearanceElement>();
        SelectedAppearance.Add(appearanceElement);
    }

    // TEST
    public void TestPlay()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
}