﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public class Tile
{
    private Vector3 position;
    private bool plowed;
    private bool watered;
    [System.NonSerialized] private GameObject ridge;

    public Vector3 Position { get => position; set => position = value; }
    public bool Watered { get => watered; set => watered = value; }

    public Tile(Vector3 position)
    {
        this.position = position;
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
        float dryValue = 0;
        while (dryValue > -1)
        {
            yield return new WaitForSeconds(0.05f);
            material.SetFloat("WetDry", dryValue);
            dryValue -= 0.05f;
        }
    }

    private void createRidge()
    {
        ridge = Object.Instantiate(Farm.GetRidgePrefab(), position, Quaternion.identity);
    }

    private void removeRidge()
    {
        MonoBehaviour.Destroy(ridge);
    }
}