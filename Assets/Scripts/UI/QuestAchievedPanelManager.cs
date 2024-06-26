using UnityEngine;

public class QuestAchievedPanelManager : MonoBehaviour
{
    public static QuestAchievedPanelManager Instance;

    [SerializeField]
    private GameObject _questAchievedPanel;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void PlayQuestAchievedAnimation()
    {
        _questAchievedPanel.SetActive(true);
    }
}
