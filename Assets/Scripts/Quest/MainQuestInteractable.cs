using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainQuestInteractable : Interactable
{
    public override void Interact()
    {
        QuestManager.instance.CheckQuestTrigger(QuestManager.QuestTriggerType.dialogue, "_pnjHideName");
    }
}