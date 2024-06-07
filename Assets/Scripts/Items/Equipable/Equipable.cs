using UnityEngine;

public class Equipable : Item
{
    [Header("Equipable")]
    [SerializeField] EquipementTier _equipementTier;

    [SerializeField]
    protected Sprite _worldSprite;

    public Sprite WorldSprite { get { return _worldSprite; } }
    public EquipementTier EquipementTier { get { return _equipementTier; } }
}

public enum EquipementTier
{
    A,
    B, 
    C,
    D
}
