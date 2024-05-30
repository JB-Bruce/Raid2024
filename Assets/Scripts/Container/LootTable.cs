using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LootTable", menuName = "ScriptableObjects/LootTable", order = 1)]
public class LootTable : ScriptableObject
{
    [SerializeField]
    private List<Loot> _loots = new List<Loot>();


    /// <summary>
    /// Generates an item with an amount and returns it
    /// </summary>
    public ContainerItem GenerateItem()
    {
        ContainerItem containerItem = new ContainerItem();
        if (_loots.Count > 0)
        {
            float randomPourcentage = UnityEngine.Random.Range(0.0f, 100.0f);
            float currentPourcentage = 0f;
            Loot loot = _loots[0];
            for (int i = 0; i < _loots.Count; i++)
            {
                loot = _loots[i];
                currentPourcentage += loot.dropRate;
                if (randomPourcentage <= currentPourcentage)
                {
                    containerItem.item = loot.item;
                    containerItem.amount = 1;
                    break;
                }
            }
            if (loot.dropNumber.Count > 0)
            {
                float randomDropPourcentage = UnityEngine.Random.Range(0.0f, 100.0f);
                float currentDropPourcentage = 0f;
                for (int i = 0; i < loot.dropNumber.Count; i++)
                {
                    DropNumber dropNumber = loot.dropNumber[i];
                    currentDropPourcentage += dropNumber.pourcentage;
                    if (randomDropPourcentage <= currentDropPourcentage)
                    {
                        containerItem.amount = dropNumber.amount;
                        break;
                    }
                }
            }
        }
        return containerItem;
    }
}

[System.Serializable]
struct Loot
{
    public Enum resourceType;
    public Item item;
    public float dropRate;
    public List<DropNumber> dropNumber;
}

[System.Serializable]
struct DropNumber
{
    public int amount;
    public float pourcentage;
}