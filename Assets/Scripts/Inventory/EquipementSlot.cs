using System;

public class EquipementSlot : ItemSlot
{
    private Type _itemType = null;

    public Type ItemType { get { return _itemType; } set { _itemType = value; } }
}
