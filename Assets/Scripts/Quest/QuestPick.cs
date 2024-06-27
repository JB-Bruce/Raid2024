using UnityEngine;

[CreateAssetMenu(fileName = "QuestItems", menuName = "ScriptableObjects/QuestAction/QuestPick", order = 1)]
[System.Serializable]
public class QuestPick : QuestAction
{
    [SerializeField]
    private Item _itemToFill;

    [SerializeField]
    private float _quantityToPick;

    [SerializeField]
    private float _quantityInInventory;

    [SerializeField]
    private string _stuffToPick;

    //call when the QuestPick is the current QuestAction to configure it
    public override bool Configure(GameObjectsList objectsToActivateAtStart)
    {
        _quantityInInventory = Mathf.Clamp(Inventory.Instance.GetContainerQuantityInInventory(name), 0, _quantityToPick);
        for (int i = 0; i < objectsToActivateAtStart.gameObjects.Count; i++)
        {
            objectsToActivateAtStart.gameObjects[i].SetActive(true);
        }
        IsFinished(0,_stuffToPick);
        return false;
    }

    //call when the QuestPick ended
    public override void OnEnd(GameObjectsList objectsToDesactivateAtTheEnd)
    {
        for (int i = 0; i < objectsToDesactivateAtTheEnd.gameObjects.Count; i++)
        {
            objectsToDesactivateAtTheEnd.gameObjects[i].SetActive(false);
        }
    }

    //return the text for the objectives
    public override string GetObjectivesText()
    {
        return "- " + LanguageManager.instance.GetText(_stuffToPick) + "   " + System.Math.Clamp(_quantityInInventory, 0, _quantityToPick) + "/" + _quantityToPick + "\n";
    }

    //check if the stuff pick is the quest stuff to pick
    private void CheckPick(float quantityPick, string stuffToPick)
    {
        if (stuffToPick == _stuffToPick)
        {
            _quantityInInventory = Mathf.Clamp(_quantityInInventory + quantityPick, 0, _quantityToPick);
        }
    }

    //return if the QuestPick is finished
    public bool IsFinished(float quantityPick, string stuffToPick)
    {
        CheckPick(quantityPick, stuffToPick);
        if (_quantityInInventory >= _quantityToPick)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}