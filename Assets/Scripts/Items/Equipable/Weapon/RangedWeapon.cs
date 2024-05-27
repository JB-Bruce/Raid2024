using UnityEngine;

[CreateAssetMenu(fileName = "RangedWeapon", menuName = "ScriptableObjects/Item/Equipable/Weapon/RangedWeapon", order = 1)]
public class RangedWeapon : Weapon
{
    [Header("RangedWeapon")]
    [SerializeField] protected float _spread;
    [SerializeField] protected int _maxBullet;
    [SerializeField] protected float _reloadTime;
    [SerializeField] protected BulletType _ammoType;
    [SerializeField] protected Vector2 _firePoint;
    public GameObject bullet;

    public float Spread { get { return _spread; } }
    public int MaxBullet { get { return _maxBullet; } }
    public float ReloadTime { get { return _reloadTime; } }
    public BulletType BulletType { get { return _ammoType; } }
    public Vector2 FirePoint => _firePoint;
}

