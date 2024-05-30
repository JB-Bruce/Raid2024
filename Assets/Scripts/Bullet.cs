using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float _damage = 0; 
    private Vector3 _velocity = Vector2.zero;
    private Vector3 _startPosition = Vector3.zero;
    private float _bulletRange = 0;
    private Transform _transform;
    public float bulletSpeed = 30;

    // Set bullet
    public void SetBullet(float damage, Vector2 direction, float bulletRange)
    {
        _transform = transform;
        _velocity = direction * bulletSpeed;
        _startPosition = _transform.position;
        _bulletRange = bulletRange;
        _damage = damage;
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
        if(collision != null && collision.TryGetComponent<Humanoid>(out Humanoid humanoid) && !collision.isTrigger) 
        {
            humanoid.TakeDamage(_damage);
        }
        if(!collision.isTrigger) 
        {
            Destroy(gameObject);
        }
    }
}
