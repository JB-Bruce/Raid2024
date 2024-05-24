using UnityEngine;

public class Equipable : Item
{
    [Header("Equipable")]
    [SerializeField] EquipementTier _equipementTier;

    public EquipementTier EquipementTier { get { return _equipementTier; } }
}

public enum EquipementTier
{
    A,
    B, 
    C,
    D
}
