using System;

public class EquipementSlot : ItemSlot
{
    private Type _itemType = null;

    public void ChangeAvailability(bool availability)
    {
        _isAvailable = availability;
        if (_isAvailable)
        {
            _itemSlotSprite.color = new UnityEngine.Color(1, 1, 1, 1);
        }
        else
        {
            _itemSlotSprite.color = new UnityEngine.Color(1, 1, 1, 0);
        }
    }


    public Type ItemType { get { return _itemType; } set { _itemType = value; } }
}
