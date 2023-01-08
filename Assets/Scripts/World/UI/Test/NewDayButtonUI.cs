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

        var nextDayButton = viewer.GetComponent<Button>();
        if (nextDayButton != null) nextDayButton.onClick.AddListener(() => Game.Instance.farm.NewDay());
        else Debug.Log("Button not found in NewDayButtonUI.");
    }
}