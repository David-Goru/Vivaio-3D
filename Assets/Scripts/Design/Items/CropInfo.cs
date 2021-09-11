using UnityEngine;

[CreateAssetMenu(fileName = "CropInfo", menuName = "Items/CropInfo", order = 0)]
public class CropInfo : ItemInfo
{
    public GameObject CropModel;
    public string YieldType;
    public int MaxYieldAmount;
    public int MinYieldAmount;
    public int StartingQuality = 50;
    public int StartingStage;
    public int MaxStage;
}