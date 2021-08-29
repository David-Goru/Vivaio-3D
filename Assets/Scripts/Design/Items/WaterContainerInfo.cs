using UnityEngine;

[CreateAssetMenu(fileName = "WaterContainer", menuName = "Items/WaterContainer", order = 0)]
public class WaterContainerInfo : ItemInfo
{
    [SerializeField] private int maxWaterAmount = 1;

    public int MaxWaterAmount { get => maxWaterAmount; set => maxWaterAmount = value; }
}