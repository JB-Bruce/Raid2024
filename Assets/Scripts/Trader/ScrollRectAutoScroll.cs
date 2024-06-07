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

    private List<Selectable> _selectables = new List<Selectable>();

    [SerializeField]
    private ScrollRect _scrollRect;

    private Vector2 _nextScrollPosition = Vector2.up;

    //return the content children onEnable
    void OnEnable()
    {
        if (_scrollRect)
        {
            _scrollRect.content.GetComponentsInChildren(_selectables);
        }
    }

    //return the scrollRect
    void Awake()
    {
        _scrollRect = GetComponent<ScrollRect>();
    }

    //return the content children at start
    void Start()
    {
        if (_scrollRect)
        {
            _scrollRect.content.GetComponentsInChildren(_selectables);
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
            _scrollRect.normalizedPosition = Vector2.Lerp(_scrollRect.normalizedPosition, _nextScrollPosition, _scrollSpeed * Time.unscaledDeltaTime);
        }
        else
        {
            _nextScrollPosition = _scrollRect.normalizedPosition;
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
            selectedIndex = _selectables.IndexOf(selectedElement);
        }
        if (selectedIndex > -1)
        {
            if (quickScroll)
            {
                _scrollRect.normalizedPosition = new Vector2(0, 1 - (selectedIndex / ((float)_selectables.Count - 1)));
                _nextScrollPosition = _scrollRect.normalizedPosition;
            }
            else
            {
                _nextScrollPosition = new Vector2(0, 1 - (selectedIndex / ((float)_selectables.Count - 1)));
            }
        }
    }

    //set the mouseOver boolean to false
    public void OnPointerExit(PointerEventData eventData)
    {
        ScrollToSelected(false);
    }
}