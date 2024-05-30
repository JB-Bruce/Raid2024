using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    [Header("Paremeter")]
    public GameObject firePoint;
    public GameObject rightHand;
    public GameObject leftHand;

    // Variable
    private bool _isRangeWeapon = false;
    private float _timer = 0;
    private static readonly Quaternion _zeroRotation = Quaternion.Euler(0, 0, 0);
    private static readonly Quaternion _flipRotation = Quaternion.Euler(180, 0, 0);

    // Cache
    private Weapon _equipedWeapon;
    private GameObject _handWeapon;
    private Animator _animator;
    private RangedWeapon _rangedWeapon;
    private UnitCombat _unitCombat;
    private SpriteRenderer _handWeaponSpriteRenderer;

    private Transform _transform;
    private Transform _handWeaponTransform;
    private Transform _leftHandTransform;
    private Transform _rightHandTransform;
    private Transform _firePointTransform;


    private void Start()
    {
        _transform = transform;
        _unitCombat = transform.parent.GetComponent<UnitCombat>();
        _handWeapon = transform.GetChild(0).gameObject;
        _handWeaponTransform = _handWeapon.transform;
        _animator = GetComponent<Animator>();
        _handWeaponSpriteRenderer = _handWeapon.GetComponent<SpriteRenderer>();
        _firePointTransform = firePoint.transform;
        _leftHandTransform = leftHand.transform;
        _rightHandTransform = rightHand.transform;

        EquipWeapon(_unitCombat.weapon);
    }

    private void Update()
    {
        UpdateWeaponRotation();
    }

    // Change the local rotation of the weapon according to the rotation of the weapon
    private void UpdateWeaponRotation()
    {
        var rotationZ = _transform.rotation.eulerAngles.z;
        _handWeaponTransform.localRotation = (rotationZ > 270 || rotationZ < 90) ? _zeroRotation : _flipRotation;
    }

    // Set all the caracteristique of the weapon
    public void EquipWeapon(Weapon weapon)
    {
        _equipedWeapon = weapon;
        _handWeaponSpriteRenderer.sprite = _equipedWeapon.ItemSprite;

        _rightHandTransform.localPosition = new Vector3(weapon.RightHand.x, weapon.RightHand.y, 0);
        _rightHandTransform.localRotation = Quaternion.Euler(0, 0, weapon.RotationRightHand);
        _leftHandTransform.localPosition = new Vector3(weapon.LeftHand.x, weapon.LeftHand.y, 0);
        _leftHandTransform.localRotation = Quaternion.Euler(0, 0, weapon.RotationLeftHand);

        _isRangeWeapon = weapon is RangedWeapon;
        if (_isRangeWeapon)
        {
            _rangedWeapon = _unitCombat.weapon as RangedWeapon;
            _firePointTransform.localPosition = _rangedWeapon.FirePoint;
        }
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
                FireBullet(direction);
            }
        }
        
    }

    // Instaciate and set a bullet
    private void FireBullet(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        GameObject bullet = Instantiate<GameObject>(_rangedWeapon.bullet, _firePointTransform.position, Quaternion.Euler(new Vector3(0, 0, angle)));
        bullet.GetComponent<Bullet>().SetBullet(_equipedWeapon.Damage, direction.normalized, _equipedWeapon.AttackRange);
    }

}
