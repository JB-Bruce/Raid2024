using UnityEngine;
using static QuestManager;

public class GoalArea : MonoBehaviour
{
    [SerializeField]
    private string _information;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            instance.CheckQuestTrigger(QuestTriggerType.enterArea, _information);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            instance.CheckQuestTrigger(QuestTriggerType.exitArea, _information);
        }
    }
}
