using System.Collections.Generic;
using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    public static PopUpManager Instance;

    public GameObject popUpItem;
    public Transform popUpTransform;
    public bool nextPopUp = false;
    public List<ItemWithQuantity> waitingForPopUp = new List<ItemWithQuantity>();
    public int indexItemPlacementAnim = -1;
    public int customeIndex = -1;
    public int countingPopUp = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (nextPopUp)
        {
            if (indexItemPlacementAnim == 1 && waitingForPopUp.Count > 0)
            {
                ItemPopUpCustomIndex(waitingForPopUp[0].item, waitingForPopUp[0].quantityNeed, customeIndex);
                waitingForPopUp.RemoveAt(0);
                customeIndex++;
                countingPopUp++;
            }

            if (waitingForPopUp.Count == 0 || countingPopUp > 2)
            {
                customeIndex = -1;
                countingPopUp = 0;
            }

            nextPopUp = false;
        }
    }

    public void NextPopUp()
    {
        if (indexItemPlacementAnim == 1 && waitingForPopUp.Count > 0)
        {
            ItemPopUpCustomIndex(waitingForPopUp[0].item, waitingForPopUp[0].quantityNeed, customeIndex);
            waitingForPopUp.RemoveAt(0);
            customeIndex++;
            countingPopUp++;
        }

        if (waitingForPopUp.Count == 0 || countingPopUp > 2)
        {
            customeIndex = -1;
            countingPopUp = 0;
        }
    }

    public void AddPopUp(Item item, int quantity)
    {
        if (indexItemPlacementAnim > 1)
        {
            ItemWithQuantity itemWithQuantity = new ItemWithQuantity();
            itemWithQuantity.item = item;
            itemWithQuantity.quantityNeed = quantity;
            waitingForPopUp.Add(itemWithQuantity);
        }
        else if (waitingForPopUp.Count == 0)
        {
            ItemPopUp(item, quantity);
        }
    }

    public void ItemPopUp(Item item, int quantity)
    {
        GameObject popUp = Instantiate(popUpItem, popUpTransform);

        popUp.GetComponent<ItemPopUp>().SetImageNamePopUp(item.ItemSprite, quantity);
        popUp.GetComponent<ItemPopUp>().SetPosition(indexItemPlacementAnim);
        indexItemPlacementAnim += 1;
    }

    public void ItemPopUpCustomIndex(Item item, int quantity, int index)
    {
        GameObject popUp = Instantiate(popUpItem, popUpTransform);

        popUp.GetComponent<ItemPopUp>().SetImageNamePopUp(item.ItemSprite, quantity);
        popUp.GetComponent<ItemPopUp>().SetPosition(index);
        indexItemPlacementAnim += 1;
    }
}
