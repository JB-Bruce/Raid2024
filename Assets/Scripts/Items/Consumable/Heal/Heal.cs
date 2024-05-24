using UnityEngine;

[CreateAssetMenu(fileName = "Heal", menuName = "ScriptableObjects/Item/Consumable/Heal", order = 1)]
public class Heal : Consumable
{
    [Header("Heal")]
    [SerializeField]
    protected float _healAmount;

    public float HealAmount { get { return _healAmount; } }
}
