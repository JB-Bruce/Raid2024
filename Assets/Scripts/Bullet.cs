using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float _damage = 0; 
    private Vector3 _velocity = Vector2.zero;
    private Vector3 _startPosition = Vector3.zero;
    private float _bulletRange = 0;
    private Transform _transform;
    public float bulletSpeed = 30;
    private Faction _ownerFaction;

    public string bulletHit;

    [SerializeField] AudioSource _bulletAudioSource;

    private SoundManager _soundManager;

    private void Awake()
    {
        _bulletAudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _soundManager = SoundManager.instance;
    }

    // Set bullet
    public void SetBullet(float damage, Vector2 direction, float bulletRange, Faction _faction)
    {
        _transform = transform;
        _velocity = direction * bulletSpeed;
        _startPosition = _transform.position;
        _bulletRange = bulletRange;
        _damage = damage;
        _ownerFaction = _faction;
    }

    private void Update()
    {
        transform.position += _velocity * Time.deltaTime;
        if(Vector2.Distance(_transform.position, _startPosition) > _bulletRange)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent == null)
            return;
        if (collision.transform.parent.parent == null)
            return;
        if (collision != null && collision.transform.parent.parent.TryGetComponent<Humanoid>(out Humanoid humanoid) && !collision.isTrigger) 
        {
            if (humanoid.faction == _ownerFaction)
            {
                return;
            }
            humanoid.TakeDamage(_damage, _ownerFaction, transform.right);
            _soundManager.PlaySFX(bulletHit, _bulletAudioSource);
        }

        if(!collision.isTrigger && collision!=null) 
        {
            Destroy(gameObject);
        }
    }
}

