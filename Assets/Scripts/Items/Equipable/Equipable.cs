using UnityEngine;

public class Equipable : Item
{
    [Header("Equipable")]
    [SerializeField] RomanNumeral _equipementTier;

    [SerializeField]
    protected Sprite _worldSprite;

    [SerializeField]
    protected bool _flipable;
    public Sprite WorldSprite { get { return _worldSprite; } }
    public RomanNumeral EquipementTier { get { return _equipementTier; } }

    public bool Flipable {  get { return _flipable; } }
}
