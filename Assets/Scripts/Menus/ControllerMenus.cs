using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ControllerMenus : MonoBehaviour
{
    [SerializeField]
    private GameObject _firstMenuButton;

    [SerializeField]
    private GameObject _secondMenuButton;

    private EventSystem _eventSystem;

    private void Awake()
    {
        _eventSystem = EventSystem.current;
    }

    public void MoveInMenuAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_eventSystem.currentSelectedGameObject == null)
            {
                if (_firstMenuButton.activeInHierarchy)
                {
                    _eventSystem.SetSelectedGameObject(_firstMenuButton);
                }
                else if (_secondMenuButton.activeInHierarchy)
                {
                    _eventSystem.SetSelectedGameObject(_secondMenuButton);
                }
            }
        }
    }
}
