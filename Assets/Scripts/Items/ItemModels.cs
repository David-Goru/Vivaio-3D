using UnityEngine;

[System.Serializable]
public class ItemModels
{
    [SerializeField] private string name;
    [SerializeField] private GameObject worldModel;
    private GameObject handModel;

    public string Name { get => name; set => name = value; }
    public GameObject WorldModel { get => worldModel; set => worldModel = value; }
    public GameObject HandModel { get => handModel; set => handModel = value; }

    public ItemModels(string name, GameObject worldModel, GameObject handModel)
    {
        this.name = name;
        this.worldModel = worldModel;
        this.handModel = handModel;
    }
}
