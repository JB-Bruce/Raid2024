using UnityEngine;

public class Weapon : Equipable
{
    [Header("Weapon")]
    [SerializeField] protected float _damage;
    [SerializeField] protected float _attackSpeed;
    [SerializeField] protected float _attackRange;
    [SerializeField] protected Vector2 _leftHandPos;
    [SerializeField] protected Vector2 _rightHandPos;
    [SerializeField] protected float _rotationRightHand;
    [SerializeField] protected float _rotationLeftHand;
    public string animAttack;
    public string attackSFX;
    public string reloadSFX;


    public float Damage { get { return _damage; } }
    public float AttackSpeed { get { return _attackSpeed; } }
    public float AttackRange { get { return _attackRange; } }
    public Vector2 LeftHand => _leftHandPos;
    public Vector2 RightHand =>_rightHandPos;
    public float RotationRightHand => _rotationRightHand;
    public float RotationLeftHand => _rotationLeftHand;

}
