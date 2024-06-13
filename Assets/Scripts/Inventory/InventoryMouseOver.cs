using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InventoryMouseOver : MonoBehaviour, IPointerMoveHandler
{
    private Inventory _inventory;
    private bool _isUsingController = false;

    [SerializeField]
    private GameObject _OverPanel;

    [SerializeField]
    private GameObject _ControllerOptions;

    [SerializeField]
    private GameObject _MouseOptions;

    private void Start()
    {
        _inventory = Inventory.Instance;
    }

    /// <summary>
    /// Activates or Deactivates the OverPanel depending on the situation
    /// if it activates it, also calls PlaceOverPanel
    /// </summary>
    private void Update()
    {
        if (!_inventory.isInventoryOpen && !_inventory.isHalfInvenoryOpen)
        {
            _OverPanel.SetActive(false);
            return;
        }
        if (_inventory.selectedItemSlot != null)
        {
            if (_inventory.selectedItemSlot.Item == null)
            {
                _OverPanel.SetActive(false);
                return;
            }
            else
            {
                PlaceOverPanel();
                ChangeOptions();
                _OverPanel.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Places the OverPanel depending on if it's too close to the borders of the screen
    /// </summary>
    private void PlaceOverPanel()
    {
        ItemSlot slot = _inventory.selectedItemSlot;
        Rect rect = _OverPanel.GetComponent<RectTransform>().rect;
        Vector3 PosOffset = new Vector3(rect.width/2, rect.height/2, 0);

        if (slot.gameObject.transform.localPosition.x + rect.width > 1920/2)
        {
            PosOffset -= new Vector3(rect.width, 0, 0);
        }
        if (slot.gameObject.transform.localPosition.y + rect.height > 1080/2)
        {
            PosOffset -= new Vector3(0, rect.height, 0);
        }

        _OverPanel.transform.localPosition = slot.gameObject.transform.localPosition + PosOffset;
    }

    /// <summary>
    /// function to show the right controll scheme in game
    /// </summary>
    private void ChangeOptions()
    {
        if (_isUsingController)
        {
            _ControllerOptions.SetActive(true);
            _MouseOptions.SetActive(false);
        }
        else
        {
            _MouseOptions.SetActive(true);
            _ControllerOptions.SetActive(false);
        }
    }

    public void ControllerMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _isUsingController = true;
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (_inventory.isInventoryOpen || _inventory.isHalfInvenoryOpen)
        {
            _isUsingController = false;
        }
    }
}
