using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class InventoryMouseOver : MonoBehaviour, IPointerMoveHandler
{
    private Inventory _inventory;
    private bool _isUsingController = false;

    [SerializeField]
    private GameObject _overPanel;

    [SerializeField]
    private TextMeshProUGUI _itemName;

    [SerializeField]
    private TextMeshProUGUI _itemDescription;

    [SerializeField]
    private TextMeshProUGUI _protectionStat;

    [SerializeField]
    private TextMeshProUGUI _damageStat;

    [SerializeField]
    private List<GameObject> _controllerOptionsImages = new();

    [SerializeField]
    private List<GameObject> _mouseOptionsImages;

    [SerializeField]
    private GameObject _equipeGroupe;

    [SerializeField]
    private GameObject _consumeGroupe;

    [SerializeField]
    private GameObject _swapGroupe;

    [SerializeField]
    private GameObject _throwGroupe;

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
            _overPanel.SetActive(false);
            return;
        }
        if (_inventory.selectedItemSlot != null)
        {
            if (_inventory.selectedItemSlot.Item == null)
            {
                _overPanel.SetActive(false);
                return;
            }
            else
            {
                PlaceOverPanel();
                ChangeOptions();
                ChangeText();
                _overPanel.SetActive(true);
            }
        }
        else
        {
            _overPanel.SetActive(false);
        }
    }

    private void ChangeText()
    {
        _itemName.text = LanguageManager.instance.GetText(_inventory.selectedItemSlot.Item.Name);
        _itemDescription.text = '"' + LanguageManager.instance.GetText(_inventory.selectedItemSlot.Item.Description) + '"';
        _damageStat.gameObject.SetActive(false);
        _protectionStat.gameObject.SetActive(false);
        if (_inventory.selectedItemSlot.Item is Weapon weapon && weapon.Damage > 0)
        {
            _damageStat.text = LanguageManager.instance.GetText("InventoryDamage") + " : " + weapon.Damage.ToString();
            _damageStat.gameObject.SetActive(true);
        }
        else if (_inventory.selectedItemSlot.Item is Armor armor && armor.Protection > 0)
        {
            _protectionStat.text = LanguageManager.instance.GetText("InventoryProtection") + " : " + armor.Protection.ToString();
            _protectionStat.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Places the OverPanel depending on if it's too close to the borders of the screen
    /// </summary>
    private void PlaceOverPanel()
    {
        ItemSlot slot = _inventory.selectedItemSlot;
        Rect rect = _overPanel.GetComponent<RectTransform>().rect;

        Vector3 PosOffset = new Vector3(rect.width/2 + 95f/2, rect.height/2 + 95f / 2, 0);

        if (slot.gameObject.transform.position.x + 95f / 2 + rect.width * (Screen.width / 1920f) > 1920f * (Screen.width / 1920f))
        {
            PosOffset -= new Vector3(rect.width + 95f, 0, 0);
        }
        if (slot.gameObject.transform.position.y + 95f / 2 + rect.height * (Screen.height / 1080f) > 1080f * (Screen.height / 1080f))
        {
            PosOffset -= new Vector3(0, rect.height + 95f, 0);
        }

        _overPanel.transform.position = slot.gameObject.transform.position + PosOffset * (Screen.width / 1920f);
    }

    /// <summary>
    /// function to show the right controll scheme in game
    /// </summary>
    private void ChangeOptions()
    {
        if (_isUsingController)
        {
            SetActiveControllerGroupe(true);
            SetActiveMouseGroupe(false);
        }
        else
        {
            SetActiveMouseGroupe(true);
            SetActiveControllerGroupe(false);
        }

        _swapGroupe.SetActive(false);
        _consumeGroupe.SetActive(false);
        _equipeGroupe.SetActive(false);
        _throwGroupe.SetActive(true);

        if (_inventory.IsContainerSlot(_inventory.selectedItemSlot) || _inventory.selectedItemSlot is EquipementSlot)
        {
            _swapGroupe.SetActive(true);
            _throwGroupe.SetActive(false);
            _controllerOptionsImages[1].SetActive(false);
            _mouseOptionsImages[1].SetActive(false);
        }
        else if (_inventory.selectedItemSlot.Item is Consumable)
        {
            _consumeGroupe.SetActive(true);
        }
        else if (_inventory.selectedItemSlot.Item is Equipable)
        {
            _equipeGroupe.SetActive(true);
        }
        else
        {
            _controllerOptionsImages[0].SetActive(false);
            _mouseOptionsImages[0].SetActive(false);
        }
    }

    private void SetActiveMouseGroupe(bool active)
    {
        foreach (GameObject Image in _mouseOptionsImages)
        {
            Image.SetActive(active);
        }
    }

    private void SetActiveControllerGroupe(bool active)
    {
        foreach (GameObject Image in _controllerOptionsImages)
        {
            Image.SetActive(active);
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
