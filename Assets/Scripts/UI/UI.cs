using UnityEngine;

public class UI : MonoBehaviour
{
    private FarmUI farmUI;
    private FarmazonUI farmazonUI;
    private InventoryUI inventoryUI;
    private MailUI mailUI;
    private OptionsUI optionsUI;
    private StandUI standUI;
    private TutorialUI tutorialUI;

    private void Start()
    {
        foreach (Transform child in transform)
        {
            switch (child.name)
            {
                case "Farm":
                    farmUI = new FarmUI(child);
                    break;
                case "Farmazon":
                    farmazonUI = new FarmazonUI(child);
                    break;
                case "Inventory":
                    inventoryUI = new InventoryUI(child);
                    break;
                case "Mail":
                    mailUI = new MailUI(child);
                    break;
                case "Options":
                    optionsUI = new OptionsUI(child);
                    break;
                case "Stand":
                    standUI = new StandUI(child);
                    break;
                case "Tutorial":
                    tutorialUI = new TutorialUI(child);
                    break;
            }
        }
    }
}