using UnityEngine;

public class Crop
{
    private Vector3 position;
    [System.NonSerialized] private GameObject cropObject;

    public Crop(Vector3 position)
    {
        this.position = position;

        //cropObject = Object.Instantiate(Game.GetItemModels("Carrot crop").WorldModel, position, Quaternion.identity);
        //cropObject.transform.Find("1").gameObject.SetActive(true);
    }
}