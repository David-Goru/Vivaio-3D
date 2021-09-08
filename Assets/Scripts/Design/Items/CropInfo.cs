using UnityEngine;

[CreateAssetMenu(fileName = "CropInfo", menuName = "Items/CropInfo", order = 0)]
public class CropInfo : ItemInfo
{
    [SerializeField] private GameObject cropModel = null;
    [SerializeField] private string yieldType = "";
    [SerializeField] private int maxYieldAmount = 0;
    [SerializeField] private int minYieldAmount = 0;
    [SerializeField] private int startingQuality = 50;
    [SerializeField] private int startingStage = 0;
    [SerializeField] private int maxStage = 0;

    public GameObject CropModel { get => cropModel; set => cropModel = value; }
    public string YieldType { get => yieldType; set => yieldType = value; }
    public int MaxYieldAmount { get => maxYieldAmount; set => maxYieldAmount = value; }
    public int MinYieldAmount { get => minYieldAmount; set => minYieldAmount = value; }
    public int StartingQuality { get => startingQuality; set => startingQuality = value; }
    public int StartingStage { get => startingStage; set => startingStage = value; }
    public int MaxStage { get => maxStage; set => maxStage = value; }
}