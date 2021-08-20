using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Models List", menuName = "Items/ModelsList", order = 0)]
public class ItemModelsList : ScriptableObject
{
    [SerializeField] private List<ItemModel> itemModels = null;

    public List<ItemModel> ItemModels { get => itemModels; set => itemModels = value; }
}