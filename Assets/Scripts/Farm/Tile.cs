﻿using UnityEngine;

[System.Serializable]
public class Tile
{
    private Vector3 position;
    private bool plowed;
    [System.NonSerialized] private GameObject ridge;

    public Vector3 Position { get => position; set => position = value; }
    public bool Plowed { get => plowed; set => plowed = value; }

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

        createRidge();
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

    private void createRidge()
    {
        ridge = Object.Instantiate(Farm.GetRidgePrefab(), position, Quaternion.identity);
    }

    private void removeRidge()
    {
        MonoBehaviour.Destroy(ridge);
    }
}