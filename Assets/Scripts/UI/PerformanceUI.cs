using UnityEngine;
using UnityEngine.UI;

public class PerformanceUI : UIElement
{
    /* FPS counter */
    private Text fpsCounter;
    private int frameCount = 0;
    private float deltaTime = 0.0f;
    private float fps = 0.0f;
    private float updateRate = 4.0f;  // 4 updates per second

    public override void Initialize()
    {
        if (viewer == null)
        {
            Debug.Log("Viewer not found on PerformanceUI.");
            return;
        }

        /* FPS counter */
        if (viewer.Find("Panel") && viewer.Find("Panel").Find("FPS counter")) fpsCounter = viewer.Find("Panel").Find("FPS counter").GetComponent<Text>();
        else Debug.Log("FPS counter not found in PerformanceUI.");
    }

    public override void Update()
    {
        if (viewer == null) return;

        /* FPS counter */
        if (fpsCounter != null)
        {
            frameCount++;
            deltaTime += Time.deltaTime;
            if (deltaTime > 1.0 / updateRate)
            {
                fps = frameCount / deltaTime;
                frameCount = 0;
                deltaTime -= 1.0f / updateRate;
                fpsCounter.text = fps.ToString("0.00").Replace(",", ".");
            }
        }
    }
}