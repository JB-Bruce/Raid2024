using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainQuestInteractable : Interactable
{
    [SerializeField]
    private QuestManager.QuestTriggerType _questTriggerType;

    [SerializeField]
    private string _information;

    [SerializeField]
    private List<Item> _itemsToGive;

    [SerializeField]
    private List<Item> _itemsTo;

    protected override void Interact()
    {
        QuestManager.instance.CheckQuestTrigger(_questTriggerType, _information);
    }

    private void GiveItems()
    {

    }
}