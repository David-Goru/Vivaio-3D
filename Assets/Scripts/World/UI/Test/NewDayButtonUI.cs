using UnityEngine;
using UnityEngine.UI;

public class NewDayButtonUI : UIElement
{
    public override void Initialize()
    {
        if (viewer == null)
        {
            Debug.Log("Viewer not found on NewDayButtonUI.");
            return;
        }

        Button nextDayButton = viewer.GetComponent<Button>();
        if (nextDayButton != null) nextDayButton.onClick.AddListener(() => Game.Instance.Farm.NewDay());
        else Debug.Log("Button not found in NewDayButtonUI.");
    }
}