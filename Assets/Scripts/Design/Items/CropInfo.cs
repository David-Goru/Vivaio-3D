using UnityEngine;

[CreateAssetMenu(fileName = "CropInfo", menuName = "Items/CropInfo", order = 0)]
public class CropInfo : ItemInfo
{
    public GameObject cropModel;
    public string yieldType;
    public int maxYieldAmount;
    public int minYieldAmount;
    public int startingQuality = 50;
    public int startingStage;
    public int maxStage;
}