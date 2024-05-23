using UnityEngine;

[CreateAssetMenu(fileName = "Consumable", menuName = "ScriptableObjects/Item/Consumable", order = 1)]
public class Consumable : Item
{
    [SerializeField]
    protected float _food;

    [SerializeField]
    protected float _drink;


    public float Food { get { return _food; } }
    public float Drink { get { return _drink; } }
}
