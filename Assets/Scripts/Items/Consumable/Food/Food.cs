using UnityEngine;

[CreateAssetMenu(fileName = "Food", menuName = "ScriptableObjects/Item/Consumable/Food", order = 1)]
public class Food : Consumable
{
    [Header("Food")]
    [SerializeField]
    protected float _foodAmount;

    [SerializeField]
    protected float _drinkAmount;

    public float FoodAmount { get { return _foodAmount; } }
    public float DrinkAmount { get { return _drinkAmount; } }
}
