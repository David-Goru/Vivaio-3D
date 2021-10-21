using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour
{
    private Crop crop;

    [HideInInspector] public TileData Data;

    public void Start()
    {
        StartCoroutine(delayRidgeCreation(0.4f));
    }

    public bool Plant(CropInfo cropInfo, Vector3 position)
    {
        if (crop != null) return false;

        crop = new Crop(cropInfo, position, Data.Watered);
        return true;
    }

    public bool Water()
    {
        if (Data.Watered == true) return false;

        StartCoroutine(waterRidge());
        Data.Watered = true;
        if (crop != null) crop.Water();
        return true;
    }

    public Item Harvest(Vector3 position)
    {
        if (crop == null) return new Item();
        return crop.Harvest();
    }

    public void NewDay()
    {
        if (crop != null) crop.NewDay();
        removeWater();
    }

    private IEnumerator delayRidgeCreation(float delay)
    {
        yield return new WaitForSeconds(delay);
        transform.Find("Model").gameObject.SetActive(true);
    }

    private IEnumerator waterRidge()
    {
        Material material = transform.Find("Model").GetComponent<MeshRenderer>().material;
        Transform water = transform.Find("Water");
        water.gameObject.SetActive(true);

        float timer = 1.0f;
        float tick = 0.05f;
        float yCurrentPosition = -0.01f;
        float yObjectivePosition = 0.02f;

        float numberOfIterations = timer / tick;
        float positionIncrement = (yObjectivePosition - yCurrentPosition) / numberOfIterations;
        while (timer > 0.0f)
        {
            timer -= tick;
            yCurrentPosition += positionIncrement;
            water.position += Vector3.up * positionIncrement;
            yield return new WaitForSeconds(tick);
        }

        float delay = 0.25f;
        yield return new WaitForSeconds(delay);

        timer = 10.0f;
        yObjectivePosition = 0.01f;
        float currentDryValue = 0.5f;
        float objectiveDryValue = -1.0f;

        numberOfIterations = timer / tick;
        float positionReduction = (yObjectivePosition - yCurrentPosition) / numberOfIterations;
        float dryReduction = (objectiveDryValue - currentDryValue) / numberOfIterations;
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

    private void removeWater()
    {
        StopAllCoroutines();
        Data.Watered = false;
        Material material = transform.Find("Model").GetComponent<MeshRenderer>().material;
        material.SetFloat("WetDry", 0);
        Transform water = transform.Find("Water");
        water.gameObject.SetActive(false);
        water.position = new Vector3(water.position.x, -0.01f, water.position.z);
    }
}