using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Tile
{
    private Vector3 position;
    private bool plowed;
    private bool watered;
    private List<Crop> crops;
    [System.NonSerialized] private GameObject ridge;

    public Vector3 Position { get => position; set => position = value; }
    public bool Watered { get => watered; set => watered = value; }

    public Tile(Vector3 position)
    {
        this.position = position;
        crops = new List<Crop>();
    }

    public void Load()
    {
        if (plowed) createRidge();
    }

    public bool Plow()
    {
        if (plowed == true) return false;

        Game.Instance.StartCoroutine(delayRidgeCreation(0.4f));
        plowed = true;
        return true;
    }

    public bool UnPlow()
    {
        if (plowed == false) return false;

        removeRidge();
        plowed = false;
        return true;
    }

    public bool Water()
    {
        if (plowed == false || watered == true) return false;

        Game.Instance.StartCoroutine(waterRidge());
        watered = true;
        return true;
    }

    private IEnumerator delayRidgeCreation(float delay)
    {
        yield return new WaitForSeconds(delay);
        createRidge();
    }

    private IEnumerator waterRidge()
    {
        Material material = ridge.transform.Find("Model").GetComponent<MeshRenderer>().material;
        Transform water = ridge.transform.Find("Water");
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

    private void createRidge()
    {
        ridge = Object.Instantiate(Farm.GetRidgePrefab(), position, Quaternion.identity);
        crops.Add(new Crop(position + Vector3.right * 0.25f + Vector3.forward * 0.25f + Vector3.up * 0.0925f));
        crops.Add(new Crop(position + Vector3.left * 0.25f + Vector3.forward * 0.25f + Vector3.up * 0.0925f));
        crops.Add(new Crop(position + Vector3.right * 0.25f - Vector3.forward * 0.25f + Vector3.up * 0.0925f));
        crops.Add(new Crop(position + Vector3.left * 0.25f - Vector3.forward * 0.25f + Vector3.up * 0.0925f));
    }

    private void removeRidge()
    {
        MonoBehaviour.Destroy(ridge);
    }
}