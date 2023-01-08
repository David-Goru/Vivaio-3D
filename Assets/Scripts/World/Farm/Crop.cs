using UnityEngine;

public class Crop
{
    [System.NonSerialized] private readonly GameObject cropObject;
    [System.NonSerialized] private GameObject currentVisuals;

    private readonly CropData data;
    private readonly CropInfo info;

    public Vector3 Position => data.Position;

    public Crop(CropInfo info, Vector3 position, bool wateredOnStart)
    {
        this.info = info;
        data = new CropData
        {
            Position = position,
            Watered = wateredOnStart
        };

        cropObject = Object.Instantiate(info.CropModel, position + Vector3.up * Game.Instance.farm.CropOffsetY, Quaternion.identity);
        data.Quality = info.StartingQuality;
        data.Stage = info.StartingStage;
        ShowVisuals();
    }

    public void Water()
    {
        data.Watered = true;
    }

    public void NewDay()
    {
        if (data.Watered)
        {
            ImproveQuality(10);
            NextStage();
        }
        else WorsenQuality(10);

        data.Watered = false;
        UpdateVisuals();
    }

    public Item Harvest()
    {
        var crop = currentVisuals.AddComponent<Item>();
        var cropData = new ItemData
        {
            CurrentStack = GetYieldAmount()
        };
        crop.data = cropData;

        if (data.Quality <= 0) cropData.Name = info.YieldType + " dried";
        else if (data.Stage >= info.MaxStage) cropData.Name = info.YieldType;
        else cropData.Name = "None";

        return crop;
    }

    private int GetYieldAmount()
    {
        if (info.MinYieldAmount == info.MaxYieldAmount) return info.MinYieldAmount;
        if (data.Quality < 25) return info.MinYieldAmount;
        if (data.Quality > 75) return info.MaxYieldAmount;
        return Random.Range(info.MinYieldAmount, info.MaxYieldAmount + 1);
    }

    private void ImproveQuality(int amount)
    {
        data.Quality += amount;
        if (data.Quality > 100) data.Quality = 100;
    }

    private void WorsenQuality(int amount)
    {
        data.Quality -= amount;
        if (data.Quality < 0) data.Quality = 0;
    }

    private void NextStage()
    {
        data.Stage++;
        if (data.Stage > info.MaxStage) data.Stage = info.MaxStage;
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
        var newCurrentVisuals = cropObject.transform.Find((data.Quality <= 0 ? "Dry " : "") + data.Stage);
        if (newCurrentVisuals == null) return;
        
        currentVisuals = newCurrentVisuals.gameObject;
        currentVisuals.SetActive(true);
    }
}