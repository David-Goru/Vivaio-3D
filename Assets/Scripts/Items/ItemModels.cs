using UnityEngine;

[System.Serializable]
public class ItemModels
{
    [SerializeField] private string name;
    [SerializeField] private GameObject worldModel;
    [SerializeField] private bool canBeInHand = false;
    private GameObject handModel;

    public string Name { get => name; set => name = value; }
    public GameObject WorldModel { get => worldModel; set => worldModel = value; }
    public GameObject HandModel { get => handModel; set => handModel = value; }
    public bool CanBeInHand { get => canBeInHand; set => canBeInHand = value; }
}
