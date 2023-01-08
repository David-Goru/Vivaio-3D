using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour
{
    private Crop crop;

    [HideInInspector] public TileData data;

    public void Start()
    {
        StartCoroutine(DelayRidgeCreation(0.4f));
    }

    public bool Plant(CropInfo cropInfo, Vector3 position)
    {
        if (crop != null) return false;

        crop = new Crop(cropInfo, position, data.watered);
        return true;
    }

    public bool Water()
    {
        if (data.watered == true) return false;

        StartCoroutine(WaterRidge());
        data.watered = true;
        crop?.Water();
        return true;
    }

    public Item Harvest(Vector3 position)
    {
        return crop?.Harvest();
    }

    public void NewDay()
    {
        crop?.NewDay();
        RemoveWater();
    }

    private IEnumerator DelayRidgeCreation(float delay)
    {
        yield return new WaitForSeconds(delay);
        transform.Find("Model").gameObject.SetActive(true);
    }

    private IEnumerator WaterRidge()
    {
        var material = transform.Find("Model").GetComponent<MeshRenderer>().material;
        var water = transform.Find("Water");
        water.gameObject.SetActive(true);

        var timer = 1.0f;
        const float tick = 0.05f;
        var yCurrentPosition = -0.01f;
        var yObjectivePosition = 0.02f;

        var numberOfIterations = timer / tick;
        var positionIncrement = (yObjectivePosition - yCurrentPosition) / numberOfIterations;
        while (timer > 0.0f)
        {
            timer -= tick;
            yCurrentPosition += positionIncrement;
            water.position += Vector3.up * positionIncrement;
            yield return new WaitForSeconds(tick);
        }

        var delay = 0.25f;
        yield return new WaitForSeconds(delay);

        timer = 10.0f;
        yObjectivePosition = 0.01f;
        var currentDryValue = 0.5f;
        var objectiveDryValue = -1.0f;

        numberOfIterations = timer / tick;
        var positionReduction = (yObjectivePosition - yCurrentPosition) / numberOfIterations;
        var dryReduction = (objectiveDryValue - currentDryValue) / numberOfIterations;
        while (timer > 0.0f)
        {
            timer -= tick;
            yCurrentPosition += positionReduction;
            water.position += Vector3.up * positionReduction;
            currentDryValue += dryReduction;
            material.SetFloat("WetDry", currentDryValue);
            yield return new WaitForSeconds(tick);
        }
    }

    private void RemoveWater()
    {
        StopAllCoroutines();
        data.watered = false;
        var material = transform.Find("Model").GetComponent<MeshRenderer>().material;
        material.SetFloat("WetDry", 0);
        var water = transform.Find("Water");
        water.gameObject.SetActive(false);
        water.position = new Vector3(water.position.x, -0.01f, water.position.z);
    }
}