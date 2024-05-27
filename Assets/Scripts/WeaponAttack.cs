using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    private Weapon _equipedWeapon;
    private GameObject _handWeapon;
    private bool _isRangeWeapon = false;
    private Animator _animator;
    private RangedWeapon _rangedWeapon;
    private float _timer = 0;

    // Set all the caracteristique of the weapon
    public void EquipWeapon(UnitCombat unit)
    {
        _equipedWeapon = unit.weapon;
        _handWeapon = unit.transform.GetChild(0).GetChild(0).gameObject;

        if (_isRangeWeapon = unit.weapon.GetType() == typeof(RangedWeapon))
        {
            _rangedWeapon = unit.weapon as RangedWeapon;
        }

        //_animator = weapon.weaponSprite.gameObject.GetComponentInParent<Animator>();
    }

    // Use the equiped weapon (Attack)
    public void UseWeapon(Vector2 direction)
    {
        if(_timer < Time.time) 
        {
            _timer = Time.time + _equipedWeapon.AttackSpeed;

            //_animator.Play("Attack");
            if (_isRangeWeapon)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                GameObject bullet = Instantiate<GameObject>(_rangedWeapon.bullet, _handWeapon.transform.position , Quaternion.Euler(new Vector3(0, 0, angle)));
                bullet.GetComponent<Bullet>().SetBullet(_equipedWeapon.Damage, direction, _equipedWeapon.AttackRange);
            }
        }
        
    }

}
