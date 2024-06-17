using UnityEngine;

[CreateAssetMenu(fileName = "Ammo", menuName = "ScriptableObjects/Item/Ammo", order = 1)]
public class Ammo : Item
{
    [Header("Ammo")]
    public string BulletType;

    //public BulletType BulletType { get { return _bulletType; } }
}

/*
public enum BulletType
{
    RiffleAmmo,
    HandGunAmmo
}*/