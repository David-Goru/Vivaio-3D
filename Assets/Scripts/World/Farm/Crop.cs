using UnityEngine;

public class Crop
{
    [System.NonSerialized] private GameObject cropObject;
    [System.NonSerialized] private GameObject currentVisuals;

    private CropData data;
    private CropInfo info;

    public Vector3 Position { get => data.Position; }

    public Crop(CropInfo info, Vector3 position, bool wateredOnStart)
    {
        this.info = info;
        data = new CropData();
        data.Position = position;
        data.Watered = wateredOnStart;

        cropObject = Object.Instantiate(info.CropModel, position, Quaternion.identity);
        data.Quality = info.StartingQuality;
        data.Stage = info.StartingStage;
        showVisuals();
    }

    public void Water()
    {
        data.Watered = true;
    }

    public void NewDay()
    {
        if (data.Watered)
        {
            improveQuality(10);
            nextStage();
        }
        else worsenQuality(10);

        data.Watered = false;
        updateVisuals();
    }

    public Item Harvest()
    {
        Item crop = new Item();
        ItemData cropData = new ItemData();
        cropData.CurrentStack = getYieldAmount();
        crop.Data = cropData;

        if (data.Quality <= 0) cropData.Name = info.YieldType + " dried";
        else if (data.Stage >= info.MaxStage) cropData.Name = info.YieldType;
        else cropData.Name = "None";

        return crop;
    }

    private int getYieldAmount()
    {
        if (info.MinYieldAmount == info.MaxYieldAmount) return info.MinYieldAmount;
        if (data.Quality < 25) return info.MinYieldAmount;
        if (data.Quality > 75) return info.MaxYieldAmount;
        return Random.Range(info.MinYieldAmount, info.MaxYieldAmount + 1);
    }

    private void improveQuality(int amount)
    {
        data.Quality += amount;
        if (data.Quality > 100) data.Quality = 100;
    }

    private void worsenQuality(int amount)
    {
        data.Quality -= amount;
        if (data.Quality < 0) data.Quality = 0;
    }

    private void nextStage()
    {
        data.Stage++;
        if (data.Stage > info.MaxStage) data.Stage = info.MaxStage;
    }

    private void updateVisuals()
    {
        hideVisuals();
        showVisuals();
    }

    private void hideVisuals()
    {
        if (currentVisuals != null) currentVisuals.SetActive(false);
    }

    private void showVisuals()
    {
        Transform newCurrentVisuals = cropObject.transform.Find((data.Quality <= 0 ? "Dry " : "") + data.Stage);
        if (newCurrentVisuals != null)
        {
            currentVisuals = newCurrentVisuals.gameObject;
            currentVisuals.SetActive(true);
        }
    }
}