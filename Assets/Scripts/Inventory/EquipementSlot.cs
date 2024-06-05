using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipementSlot : ItemSlot
{
    private Type _itemType = null;

    public void ChangeAvailability(bool availability)
    {
        _isAvailable = availability;
        if (_isAvailable)
        {
            _itemSlotSprite.color = new UnityEngine.Color(0.5f, 0.5f, 0.5f, 1f);
            gameObject.GetComponent<Button>().navigation = Navigation.defaultNavigation;
        }
        else
        {
            _itemSlotSprite.color = new UnityEngine.Color(0.25f, 0.25f, 0.25f, 0.5f);
            Navigation navigation = new Navigation();
            navigation.mode = Navigation.Mode.None;
            gameObject.GetComponent<Button>().navigation = navigation;
        }
    }


    public Type ItemType { get { return _itemType; } set { _itemType = value; } }
}
