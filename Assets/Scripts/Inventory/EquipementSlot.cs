using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipementSlot : ItemSlot
{
    private EventSystem _eventSystem;
    private Type _itemType = null;

    private void Start()
    {
        _eventSystem = EventSystem.current;
    }

    public void ChangeAvailability(bool availability)
    {
        _isAvailable = availability;
        if (_isAvailable)
        {
            _itemSlotSprite.color = new UnityEngine.Color(1, 1, 1, 1);
            gameObject.GetComponent<Button>().navigation = Navigation.defaultNavigation;
        }
        else
        {
            _itemSlotSprite.color = new UnityEngine.Color(1, 1, 1, 0);
            Navigation navigation = new Navigation();
            navigation.mode = Navigation.Mode.None;
            gameObject.GetComponent<Button>().navigation = navigation;
        }
    }


    public Type ItemType { get { return _itemType; } set { _itemType = value; } }
}
