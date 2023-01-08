using UnityEngine;
using System.Collections;

public class WateringCan : Item
{
    private void Start()
    {
        var waterObject = transform.Find("Water");
        if (waterObject != null) SetWaterAmount(waterObject);
    }

    public override void Use(Player player)
    {
        if (((WateringCanData)data).WaterAmount <= 0) return;

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, 100, player.farmTileLayer) && player.CheckDistance(hit.point)) 
            Water(hit, player);
    }

    private void Water(RaycastHit tile, Player player)
    {
        var watered = tile.transform.GetComponent<Farm>().WaterAt(tile.point);
        if (!watered) return;
        ActivateParticles(player);
        player.Block();
        player.Animations.Set(AnimationType.WATER);
        player.StartCoroutine(UpdateWaterAmount(-1));
    }

    private void SetWaterAmount(Transform waterObject)
    {
        var waterMaterial = waterObject.GetComponent<MeshRenderer>().material;
        waterMaterial.SetFloat("Fill", ((WateringCanData)data).WaterAmount / ((WaterContainerInfo)info).maxWaterAmount);
    }

    private IEnumerator UpdateWaterAmount(int waterAmountChange)
    {
        var material = transform.Find("Water").GetComponent<MeshRenderer>().material;
        if (material == null) yield return null;

        var timer = 0.5f;
        const float tick = 0.05f;        

        var objectiveWaterAmount = ((WateringCanData)data).WaterAmount + waterAmountChange;
        if (objectiveWaterAmount > ((WaterContainerInfo)info).maxWaterAmount) objectiveWaterAmount = ((WaterContainerInfo)info).maxWaterAmount;
        else if (objectiveWaterAmount < 0) objectiveWaterAmount = 0;

        float currentWaterAmount = ((WateringCanData)data).WaterAmount;
        var numberOfIterations = timer / tick;
        var amountChange = (objectiveWaterAmount - currentWaterAmount) / numberOfIterations;
        while (timer > 0.0f)
        {
            timer -= tick;
            currentWaterAmount += amountChange;
            material.SetFloat("Fill", currentWaterAmount / ((WaterContainerInfo)info).maxWaterAmount);
            yield return new WaitForSeconds(tick);
        }

        ((WateringCanData)data).WaterAmount = Mathf.RoundToInt(currentWaterAmount);
    }

    private void ActivateParticles(Player player)
    {
        var particles = transform.Find("Particles");
        if (particles != null) player.StartCoroutine(ShowParticles(particles.gameObject));
    }

    private IEnumerator ShowParticles(GameObject particles)
    {
        particles.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        particles.SetActive(false);
    }
}