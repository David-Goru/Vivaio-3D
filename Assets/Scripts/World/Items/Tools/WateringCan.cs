using UnityEngine;
using System.Collections;

public class WateringCan : Item
{
    private void Start()
    {
        Transform waterObject = transform.Find("Water");
        if (waterObject != null) setWaterAmount(waterObject);
    }

    public override void Use(Player player)
    {
        if (((WateringCanData)Data).WaterAmount <= 0) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, player.FarmTileLayer) && player.CheckDistance(hit.point)) Water(hit, player);
    }

    public void Water(RaycastHit tile, Player player)
    {
        bool watered = tile.transform.GetComponent<Farm>().WaterAt(tile.point);
        if (watered)
        {
            activateParticles(player);
            player.Block();
            player.Animations.Set(AnimationType.WATER);
            player.StartCoroutine(updateWaterAmount(-1));
        }
    }

    private void setWaterAmount(Transform waterObject)
    {
        Material waterMaterial = waterObject.GetComponent<MeshRenderer>().material;
        waterMaterial.SetFloat("Fill", (float)((WateringCanData)Data).WaterAmount / (float)((WaterContainerInfo)Info).MaxWaterAmount);
    }

    private IEnumerator updateWaterAmount(int waterAmountChange)
    {
        Material material = transform.Find("Water").GetComponent<MeshRenderer>().material;
        if (material == null) yield return null;

        float timer = 0.5f;
        float tick = 0.05f;        

        int objectiveWaterAmount = ((WateringCanData)Data).WaterAmount + waterAmountChange;
        if (objectiveWaterAmount > ((WaterContainerInfo)Info).MaxWaterAmount) objectiveWaterAmount = ((WaterContainerInfo)Info).MaxWaterAmount;
        else if (objectiveWaterAmount < 0) objectiveWaterAmount = 0;

        float currentWaterAmount = ((WateringCanData)Data).WaterAmount;
        float numberOfIterations = timer / tick;
        float amountChange = (objectiveWaterAmount - currentWaterAmount) / numberOfIterations;
        while (timer > 0.0f)
        {
            timer -= tick;
            currentWaterAmount += amountChange;
            material.SetFloat("Fill", currentWaterAmount / ((WaterContainerInfo)Info).MaxWaterAmount);
            yield return new WaitForSeconds(tick);
        }

        ((WateringCanData)Data).WaterAmount = Mathf.RoundToInt(currentWaterAmount);
    }

    private void activateParticles(Player player)
    {
        Transform particles = transform.Find("Particles");
        if (particles != null) player.StartCoroutine(showParticles(particles.gameObject));
    }

    private IEnumerator showParticles(GameObject particles)
    {
        particles.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        particles.SetActive(false);
    }
}