using UnityEngine;

public class Armor : Equipable
{
    [Header("Armor")]
    [SerializeField] protected float _protection;

    public float Protection { get { return _protection; } }
}
