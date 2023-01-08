using UnityEngine;

public class Crop
{
    [System.NonSerialized] private readonly GameObject cropObject;
    [System.NonSerialized] private GameObject currentVisuals;

    private readonly CropData data;
    private readonly CropInfo info;

    public Vector3 Position => data.position;

    public Crop(CropInfo info, Vector3 position, bool wateredOnStart)
    {
        this.info = info;
        data = new CropData
        {
            position = position,
            watered = wateredOnStart
        };

        cropObject = Object.Instantiate(info.cropModel, position + Vector3.up * Game.Instance.farm.CropOffsetY, Quaternion.identity);
        data.quality = info.startingQuality;
        data.stage = info.startingStage;
        ShowVisuals();
    }

    public void Water()
    {
        data.watered = true;
    }

    public void NewDay()
    {
        if (data.watered)
        {
            ImproveQuality(10);
            NextStage();
        }
        else WorsenQuality(10);

        data.watered = false;
        UpdateVisuals();
    }

    public Item Harvest()
    {
        var crop = currentVisuals.AddComponent<Item>();
        var cropData = new ItemData
        {
            currentStack = GetYieldAmount()
        };
        crop.data = cropData;

        if (data.quality <= 0) cropData.name = info.yieldType + " dried";
        else if (data.stage >= info.maxStage) cropData.name = info.yieldType;
        else cropData.name = "None";

        return crop;
    }

    private int GetYieldAmount()
    {
        if (info.minYieldAmount == info.maxYieldAmount) return info.minYieldAmount;
        if (data.quality < 25) return info.minYieldAmount;
        if (data.quality > 75) return info.maxYieldAmount;
        return Random.Range(info.minYieldAmount, info.maxYieldAmount + 1);
    }

    private void ImproveQuality(int amount)
    {
        data.quality += amount;
        if (data.quality > 100) data.quality = 100;
    }

    private void WorsenQuality(int amount)
    {
        data.quality -= amount;
        if (data.quality < 0) data.quality = 0;
    }

    private void NextStage()
    {
        data.stage++;
        if (data.stage > info.maxStage) data.stage = info.maxStage;
    }

    private void UpdateVisuals()
    {
        HideVisuals();
        ShowVisuals();
    }

    private void HideVisuals()
    {
        if (currentVisuals != null) currentVisuals.SetActive(false);
    }

    private void ShowVisuals()
    {
        var newCurrentVisuals = cropObject.transform.Find((data.quality <= 0 ? "Dry " : "") + data.stage);
        if (newCurrentVisuals == null) return;
        
        currentVisuals = newCurrentVisuals.gameObject;
        currentVisuals.SetActive(true);
    }
}