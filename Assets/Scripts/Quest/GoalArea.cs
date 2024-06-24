using UnityEngine;
using static QuestManager;

public class GoalArea : MonoBehaviour
{
    [SerializeField]
    private string _information;

    private bool _canInteract = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            instance.CheckQuestTrigger(QuestTriggerType.enterArea, _information);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && _canInteract)
        {
            instance.CheckQuestTrigger(QuestTriggerType.exitArea, _information);
        }
    }

    private void OnDisable()
    {
        _canInteract = false;
    }

    private void OnEnable()
    {
        _canInteract = true;
    }
}
