using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverSelect : MonoBehaviour, IPointerEnterHandler
{
    public MenuButtonTracker menuButtonTracker;
    private Animator animator;

    private void Start()
    {
        if (menuButtonTracker == null)
        {
            menuButtonTracker = FindObjectOfType<MenuButtonTracker>();
        }

        animator = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (menuButtonTracker != null)
        {
            if (menuButtonTracker.eventSystem.currentSelectedGameObject != gameObject)
            {
                menuButtonTracker.SetLastGameObjectSelected(gameObject);

            }
        }
    }
}
