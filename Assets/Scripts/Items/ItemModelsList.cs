using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Models List", menuName = "Items/ModelsList", order = 0)]
public class ItemModelsList : ScriptableObject
{
    [SerializeField] private List<ItemModels> itemModels = null;

    public List<ItemModels> ItemModels { get => itemModels; set => itemModels = value; }
}