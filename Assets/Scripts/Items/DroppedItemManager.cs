using System.Collections.Generic;
using UnityEngine;

public class DroppedItemManager : MonoBehaviour
{
    public static DroppedItemManager Instance;
    public List<DroppedItem> droppedItems;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
