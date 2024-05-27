using UnityEngine;
using UnityEngine.UI;

public class Item : ScriptableObject
{
    [SerializeField]
    protected int _weight;

    [SerializeField]
    protected bool _isStackable;

    [SerializeField]
    protected int _maxStack;

    [SerializeField]
    protected string _name;

    [SerializeField]
    protected Sprite _itemSprite;

    public bool IsStackable {  get { return _isStackable; } }
    public int MaxStack { get { return _maxStack; } }
    public Sprite ItemSprite {  get { return _itemSprite; } }
}
