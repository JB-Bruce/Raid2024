using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HighLightGUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _highlight;
    public bool isOver = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _highlight.SetActive(true);
        isOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _highlight.SetActive(false);
        isOver = false;
    }
}
