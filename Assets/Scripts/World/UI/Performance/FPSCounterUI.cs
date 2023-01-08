using UnityEngine;
using UnityEngine.UI;

public class FPSCounterUI : UIElement
{
    private Text counterText;
    private int frameCount = 0;
    private float deltaTime = 0.0f;
    private float fps = 0.0f;
    private const float UpdatesPerSecond = 5.0f;

    public override void Initialize()
    {
        if (viewer == null)
        {
            Debug.Log("Viewer not found on FPSCounterUI.");
            return;
        }

        if (viewer.Find("FPS counter")) counterText = viewer.Find("FPS counter").GetComponent<Text>();
        else Debug.Log("FPS counter text not found in FPSCounterUI.");
    }

    public override void Update()
    {
        if (viewer == null) return;
        if (counterText == null) return;
        
        frameCount++;
        deltaTime += Time.deltaTime;
        if (!(deltaTime > 1.0 / UpdatesPerSecond)) return;
        
        fps = frameCount / deltaTime;
        frameCount = 0;
        deltaTime -= 1.0f / UpdatesPerSecond;
        counterText.text = fps.ToString("0.00").Replace(",", ".");
    }
}