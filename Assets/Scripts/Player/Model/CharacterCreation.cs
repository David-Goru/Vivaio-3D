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
    private List<AppearanceElement> selectedAppearance;
    public List<AppearanceElement> SelectedAppearance { get => selectedAppearance; set => selectedAppearance = value; }

    public static CharacterCreation Instance;

    private void Start()
    {
        Instance = this;
        model = Instantiate(characterModelPrefab).transform.Find("Player model");
        initializeSelectors();
    }

    private void initializeSelectors()
    {
        AppearanceElementSelector[] appearanceElements = characterModel.AppearanceElementSelectors;
        selectedAppearance = new List<AppearanceElement>();

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

    public static List<AppearanceElement> GetSelectedAppearance()
    {
        if (Instance == null) return null;
        return Instance.SelectedAppearance;
    }

    public static void AddSelectedAppearance(AppearanceElement appearanceElement)
    {
        if (Instance == null) return;

        if (Instance.SelectedAppearance == null) Instance.SelectedAppearance = new List<AppearanceElement>();
        Instance.SelectedAppearance.Add(appearanceElement);
    }

    // TEST
    public void TestPlay()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
}