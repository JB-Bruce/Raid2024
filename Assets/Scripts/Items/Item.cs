using UnityEngine;
using UnityEngine.UI;

public class Item : ScriptableObject
{
    [Header("Item")]
    [SerializeField]
    protected float _weight;

    [SerializeField]
    protected bool _isStackable;

    [SerializeField]
    protected int _maxStack;

    [SerializeField]
    protected string _name;

    [SerializeField]
    protected Image _itemSprite;

    public bool IsStackable {  get { return _isStackable; } }
    public int MaxStack { get { return _maxStack; } }
    public Image ItemSprite {  get { return _itemSprite; } }
}
