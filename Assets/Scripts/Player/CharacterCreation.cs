using System.Collections.Generic;
using UnityEngine;

public class CharacterCreation : MonoBehaviour
{
    [SerializeField] private Transform appearanceElementsContainer;
    [SerializeField] private GameObject appearanceElementPrefab;
    [SerializeField] private GameObject appearanceColorPrefab;
    [SerializeField] private GameObject characterModelPrefab;

    public static PlayerObject CharacterObject;
    public static List<AppearanceElement> SelectedAppearance;

    private void Start()
    {
        CharacterObject = Instantiate(characterModelPrefab).GetComponent<PlayerObject>();
        initializeSelectors();
    }

    private void initializeSelectors()
    {
        AppearanceElementSelector[] appearanceElements = CharacterObject.AppearanceElementSelectors;
        SelectedAppearance = new List<AppearanceElement>();

        for (int i = 0; i < appearanceElements.Length; i++)
        {
            appearanceElements[i].SetUpSelector(appearanceElementPrefab, appearanceColorPrefab, appearanceElementsContainer);
        }
    }

    private void initializeSelector(AppearanceElementSelector element)
    {
    }

    // TEST //
    public void TestPlay()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
}