using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour
{
    private TileData data;
    private List<Crop> crops;

    public TileData Data { get => data; set => data = value; }

    public void Start()
    {
        crops = new List<Crop>();
        StartCoroutine(delayRidgeCreation(0.4f));
    }

    public bool Water()
    {
        if (data.Watered == true) return false;

        StartCoroutine(waterRidge());
        data.Watered = true;
        return true;
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

        timer = 5.0f;
        yObjectivePosition = -0.01f;
        float currentDryValue = 0.0f;
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

        water.gameObject.SetActive(false);
    }
}