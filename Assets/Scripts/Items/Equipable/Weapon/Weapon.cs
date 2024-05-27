using UnityEngine;

public class Weapon : Equipable
{
    [Header("Weapon")]
    [SerializeField] protected float _damage;
    [SerializeField] protected float _attackSpeed;
    [SerializeField] protected float _attackRange;


    public float Damage { get { return _damage; } }
    public float AttackSpeed { get { return _attackSpeed; } }
    public float AttackRange { get { return _attackRange; } }
}
