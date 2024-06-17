using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangedWeapon", menuName = "ScriptableObjects/Item/Equipable/Weapon/RangedWeapon", order = 1)]
public class RangedWeapon : Weapon
{
    [Header("RangedWeapon")]
    [SerializeField] protected float _spread;
    [SerializeField] protected int _maxBullet;
    [SerializeField] protected float _reloadTime;
    [SerializeField] protected List<Ammo> _ammoType;
    [SerializeField] protected Vector2 _firePoint;
    public GameObject bullet;
    public string animReload;

    public float Spread { get { return _spread; } }
    public int MaxBullet { get { return _maxBullet; } }
    public float ReloadTime { get { return _reloadTime; } }
    public List<Ammo> BulletType { get { return _ammoType; } }
    public Vector2 FirePoint => _firePoint;
}

