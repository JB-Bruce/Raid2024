using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

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
    private static readonly Vector3 _normalScale = new Vector3(0.2f,0.2f, 0.2f);
    public Vector3 _flipScale = new Vector3(-0.2f, 0.2f, 0.2f);

    // Cache
    private Weapon _equipedWeapon;


    [SerializeField]
    private Transform _AnimTransform;
    private Animator _animator;
    private RangedWeapon _rangedWeapon;
    private UnitCombat _unitCombat;
    private NavMeshAgent _navMeshAgent;
    private SoundManager _soundManager;
    private UnitSoundPlayer _unitSoundPlayer;
    private Humanoid _humanoid;

    [SerializeField]
    private SpriteRenderer _handWeaponSpriteRenderer;

    [SerializeField]
    private Transform _transform;

    [SerializeField]
    private Transform _handWeaponTransform;

    [SerializeField]
    private Transform _leftHandTransform;

    [SerializeField]
    private Transform _rightHandTransform;

    [SerializeField]
    private Transform _firePointTransform;

    public UnityEvent<Weapon> weaponChange = new UnityEvent<Weapon>();

    private Camera _camera;

    private void Start()
    {
        _soundManager = SoundManager.instance;
        _unitSoundPlayer = UnitSoundPlayer.instance;
    }

    public void Init()
    {
        _unitCombat = transform.parent.parent.GetComponent<UnitCombat>();
        _animator = GetComponent<Animator>();
        _navMeshAgent = transform.parent.parent.GetComponent<NavMeshAgent>();
        _camera = Camera.main;
    }

    private void Update()
    {
        UpdateWeaponRotation();
    }

    // Change the local rotation of the weapon according to the rotation of the weapon
    public void UpdateWeaponRotation()
    {
        var rotationZ = _transform.rotation.eulerAngles.z;
        
        if(rotationZ > 270 || rotationZ < 90)
        {
            if(!_equipedWeapon.Flipable) 
            {
                _handWeaponTransform.localRotation = _zeroRotation;
            }
            else
            {
                _handWeaponTransform.localRotation = _zeroRotation;
                _AnimTransform.localScale = _normalScale; 
            }

        }
        else
        {
            if (!_equipedWeapon.Flipable)
            {
                _handWeaponTransform.localRotation = _flipRotation;
            }
            else
            {
                _handWeaponTransform.localRotation = _flipRotation;
                _AnimTransform.localScale = _flipScale;
            }
        }
    }

    // Set all the caracteristique of the weapon
    public void EquipWeapon(Weapon weapon)
    {
        _equipedWeapon = weapon;
        weaponChange.Invoke(_equipedWeapon);
        _handWeaponSpriteRenderer.sprite = _equipedWeapon.WorldSprite;
        //_AnimTransform.localScale = _normalScale;

        if(_navMeshAgent != null)
        {
            _navMeshAgent.speed = weapon.MoveSpeed;
        }

        _rightHandTransform.localPosition = new Vector3(weapon.RightHand.x, weapon.RightHand.y, 0);
        _rightHandTransform.localRotation = Quaternion.Euler(0, 0, weapon.RotationRightHand);
        _leftHandTransform.localPosition = new Vector3(weapon.LeftHand.x, weapon.LeftHand.y, 0);
        _leftHandTransform.localRotation = Quaternion.Euler(0, 0, weapon.RotationLeftHand);

        _isRangeWeapon = weapon is RangedWeapon;
        if (_isRangeWeapon)
        {
            _rangedWeapon = _equipedWeapon as RangedWeapon;
            _firePointTransform.localPosition = _rangedWeapon.FirePoint;
        }
    }

    // Use the equiped weapon (Attack)
    public void UseWeapon(Vector2 direction, Faction _faction)
    {
        if (_timer < Time.time) 
        {
            _timer = Time.time + _equipedWeapon.AttackSpeed;

            _animator.Play(_equipedWeapon.animAttack, 0, 0);
            _soundManager.PlaySFX(_equipedWeapon.attackSFX, _unitSoundPlayer.unitAudioSource);
            if (_isRangeWeapon)
            {
                direction = rotateVector2(direction, Random.Range(-(_rangedWeapon.Spread/ 2), _rangedWeapon.Spread / 2));
                FireBullet(direction, _faction);
            }
        }
        
    }

    // Give damage to the nearrest player
    public void CaCAttack()
    {
        bool _isAI = _unitCombat != null;

        Humanoid _enemy = _isAI ? _unitCombat.nearestEnemy : GetFrontEnemy();

        if (_enemy != null)
        {
            _enemy.TakeDamage(_equipedWeapon.Damage, _isAI ? _unitCombat.GetFaction() : Faction.Player, (_enemy.transform.position - transform.position).normalized);
            _soundManager.PlaySFX(_equipedWeapon.hitSFX, _unitSoundPlayer.unitAudioSource);
        }

    }

    //Get the enemy front of the unit
    private Humanoid GetFrontEnemy()
    {
        Vector3 direction = -(_transform.position - _camera.ScreenToWorldPoint(Input.mousePosition)).normalized;

        RaycastHit2D _hit =  Physics2D.Raycast(_transform.position, direction, _equipedWeapon.AttackRange);

        if(_hit.collider != null  && _hit.collider.transform.parent != null && _hit.collider.transform.parent.parent != null 
            && _hit.collider.transform.parent.parent.TryGetComponent<Humanoid>(out Humanoid humanoid))
        {
            return humanoid;
        }
        return null;
    }

    // Instaciate and set a bullet
    private void FireBullet(Vector2 direction, Faction _faction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        GameObject bullet = Instantiate<GameObject>(_rangedWeapon.bullet, _firePointTransform.position, Quaternion.Euler(new Vector3(0, 0, angle)));
        bullet.GetComponent<Bullet>().SetBullet(_equipedWeapon.Damage, direction.normalized, _equipedWeapon.AttackRange, _faction);
    }

    // Function for rotate a Vector2D
    Vector2 rotateVector2(Vector2 vec, float angle)
    {

        const float PI = 3.141592f;

        float dirAngle = Mathf.Atan2(vec.y, vec.x);

        dirAngle *= 180 / PI;

        float newAngle = (dirAngle + angle) * PI / 180;

        Vector2 newDir = new Vector2(Mathf.Cos(newAngle), Mathf.Sin(newAngle));

        return newDir.normalized;

    }

    public bool IsRangedWeapon => _isRangeWeapon;
    public RangedWeapon RangedWeapon => _rangedWeapon;
    public Animator Animator => _animator;
    public float Timer 
    { 
        get { return _timer; }
        set {  _timer = value; }
    }

}
