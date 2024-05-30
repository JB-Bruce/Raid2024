using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ScrollRectAutoScroll : MonoBehaviour, IPointerExitHandler
{
    [SerializeField]
    private float _scrollSpeed = 10f;
    private bool _isGamePadUsing = false;

    private List<Selectable> _m_Selectables = new List<Selectable>();

    [SerializeField]
    private ScrollRect _m_ScrollRect;

    private Vector2 _m_NextScrollPosition = Vector2.up;

    //get the content children onEnable
    void OnEnable()
    {
        if (_m_ScrollRect)
        {
            _m_ScrollRect.content.GetComponentsInChildren(_m_Selectables);
        }
    }

    //get the scrollRect
    void Awake()
    {
        _m_ScrollRect = GetComponent<ScrollRect>();
    }

    //get the content children at start
    void Start()
    {
        if (_m_ScrollRect)
        {
            _m_ScrollRect.content.GetComponentsInChildren(_m_Selectables);
        }
        ScrollToSelected(true);
    }

    void Update()
    {
        //scroll via input.
        InputScroll();
        if ( _isGamePadUsing)
        {
            //lerp scrolling code.
            _m_ScrollRect.normalizedPosition = Vector2.Lerp(_m_ScrollRect.normalizedPosition, _m_NextScrollPosition, _scrollSpeed * Time.unscaledDeltaTime);
        }
        else
        {
            _m_NextScrollPosition = _m_ScrollRect.normalizedPosition;
        }
    }

    //scroll via input.
    void InputScroll()
    {
        if (Input.GetAxis("Vertical") != 0.0f || Input.GetAxis("Horizontal") != 0.0f || Input.GetButtonDown("Horizontal")
            || Input.GetButtonDown("Vertical") || Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            _isGamePadUsing = true;
            ScrollToSelected(false);
        }
        else
        {
            _isGamePadUsing = false;
        }
    }

    //scroll to the selected gameobject
    void ScrollToSelected(bool quickScroll)
    {
        int selectedIndex = -1;
        Selectable selectedElement = EventSystem.current.currentSelectedGameObject ? EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>() : null;

        if (selectedElement)
        {
            selectedIndex = _m_Selectables.IndexOf(selectedElement);
        }
        if (selectedIndex > -1)
        {
            if (quickScroll)
            {
                _m_ScrollRect.normalizedPosition = new Vector2(0, 1 - (selectedIndex / ((float)_m_Selectables.Count - 1)));
                _m_NextScrollPosition = _m_ScrollRect.normalizedPosition;
            }
            else
            {
                _m_NextScrollPosition = new Vector2(0, 1 - (selectedIndex / ((float)_m_Selectables.Count - 1)));
            }
        }
    }

    //set the mouseOver boolean to false
    public void OnPointerExit(PointerEventData eventData)
    {
        ScrollToSelected(false);
    }
}