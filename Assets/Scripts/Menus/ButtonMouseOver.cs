using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private GameObject _mouseOverPanel;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _mouseOverPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _mouseOverPanel.SetActive(false);
    }
}
