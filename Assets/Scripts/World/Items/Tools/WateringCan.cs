using UnityEngine;
using System.Collections;

public class WateringCan : Item
{
    private static readonly int FillMaterialId = Shader.PropertyToID("Fill");
    [SerializeField] private GameObject wateringParticles;
    
    private void Start()
    {
        var waterObject = transform.Find("Water");
        if (waterObject != null) SetWaterAmount(waterObject);
    }

    public override void Use(Player player)
    {
        if (((WateringCanData)data).WaterAmount <= 0) return;

        var ray = GlobalVars.Camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, 100, player.farmTileLayer) && player.CheckDistance(hit.point)) 
            Water(hit, player);
    }

    public override void Reset()
    {
        StopAllCoroutines();
        if (wateringParticles != null) wateringParticles.SetActive(false);
    }

    private void Water(RaycastHit tile, Player player)
    {
        var watered = tile.transform.GetComponent<Farm>().WaterAt(tile.point);
        if (!watered) return;
        ActivateParticles();
        player.Block();
        player.Animations.Set(AnimationType.WATER);
        player.StartCoroutine(UpdateWaterAmount(-1));
    }

    private void SetWaterAmount(Transform waterObject)
    {
        var waterMaterial = waterObject.GetComponent<MeshRenderer>().material;
        waterMaterial.SetFloat(FillMaterialId, ((WateringCanData)data).WaterAmount / ((WaterContainerInfo)info).maxWaterAmount);
    }

    private IEnumerator UpdateWaterAmount(int waterAmountChange)
    {
        var material = transform.Find("Water").GetComponent<MeshRenderer>().material;
        if (material == null) yield return null;

        var timer = 0.5f;
        var objectiveWaterAmount = ((WateringCanData)data).WaterAmount + waterAmountChange;
        if (objectiveWaterAmount > ((WaterContainerInfo)info).maxWaterAmount) objectiveWaterAmount = ((WaterContainerInfo)info).maxWaterAmount;
        else if (objectiveWaterAmount < 0) objectiveWaterAmount = 0;

        float currentWaterAmount = ((WateringCanData)data).WaterAmount;
        var numberOfIterations = timer / GlobalVars.TimePerTick;
        var amountChange = (objectiveWaterAmount - currentWaterAmount) / numberOfIterations;
        while (timer > 0.0f)
        {
            timer -= GlobalVars.TimePerTick;
            currentWaterAmount += amountChange;
            material.SetFloat(FillMaterialId, currentWaterAmount / ((WaterContainerInfo)info).maxWaterAmount);
            yield return new WaitForSeconds(GlobalVars.TimePerTick);
        }

        ((WateringCanData)data).WaterAmount = Mathf.RoundToInt(currentWaterAmount);
    }

    private void ActivateParticles()
    {
        StartCoroutine(ShowParticles());
    }

    private IEnumerator ShowParticles()
    {
        if (wateringParticles == null) yield return null;
        
        wateringParticles.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        wateringParticles.SetActive(false);
    }
}