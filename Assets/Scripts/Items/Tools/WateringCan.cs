using UnityEngine;
using System.Collections;

[System.Serializable]
public class WateringCan : Item
{
    private int maxWaterAmount = 10;
    private int waterAmount = 10;

    public override void Use(Player player)
    {
        if (waterAmount <= 0) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, player.PlayerObject.FarmTileLayer))
        {
            if (player.CheckDistance(hit.point))
            {
                player.LastAnimation = "WATER";
                Water(hit, player);
            }
        }
    }

    public override void UpdateVisuals()
    {
        GameObject objectToSetVisuals = worldObject != null ? worldObject : HandObject;
        if (objectToSetVisuals == null) return;

        Transform waterObject = objectToSetVisuals.transform.Find("Water");
        if (waterObject != null) setWaterAmount(waterObject);
    }

    public void Water(RaycastHit tile, Player player)
    {
        bool watered = tile.transform.GetComponent<FarmObject>().WaterAt(tile.point);
        if (watered)
        {
            activateParticles(player);
            player.BlockMovement();
            player.PlayerObject.SetAnimation("WATER");
            player.PlayerObject.StartCoroutine(updateWaterAmount(-1));
        }
    }

    private void setWaterAmount(Transform waterObject)
    {
        Material waterMaterial = waterObject.GetComponent<MeshRenderer>().material;
        waterMaterial.SetFloat("Fill", waterAmount / maxWaterAmount);
    }

    private IEnumerator updateWaterAmount(int waterAmountChange)
    {
        GameObject objectToSetVisuals = worldObject != null ? worldObject : HandObject;
        if (objectToSetVisuals == null) yield return null;

        Material material = objectToSetVisuals.transform.Find("Water").GetComponent<MeshRenderer>().material;
        if (material == null) yield return null;

        float timer = 0.5f;
        float tick = 0.05f;        

        int objectiveWaterAmount = waterAmount + waterAmountChange;
        if (objectiveWaterAmount > maxWaterAmount) objectiveWaterAmount = maxWaterAmount;
        else if (objectiveWaterAmount < 0) objectiveWaterAmount = 0;

        float currentWaterAmount = waterAmount;
        float numberOfIterations = timer / tick;
        float amountChange = (objectiveWaterAmount - currentWaterAmount) / numberOfIterations;
        while (timer > 0.0f)
        {
            timer -= tick;
            currentWaterAmount += amountChange;
            material.SetFloat("Fill", currentWaterAmount / maxWaterAmount);
            yield return new WaitForSeconds(tick);
        }

        waterAmount = Mathf.RoundToInt(currentWaterAmount);
    }

    private void activateParticles(Player player)
    {
        Transform particles = HandObject.transform.Find("Particles");
        if (particles != null) player.PlayerObject.StartCoroutine(showParticles(particles.gameObject));
    }

    private IEnumerator showParticles(GameObject particles)
    {
        particles.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        particles.SetActive(false);
    }
}