using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingsMenusManager : MonoBehaviour
{
    [Header("References: ")]
    [SerializeField] GameObject _graphicsMenu;
    [SerializeField] GameObject _audioMenu;
    [SerializeField] GameObject _keybindsMenu;
    [SerializeField] GameObject _settingsChooser;
    [SerializeField] PlayerInput _playerInput;

    [Header("Buttons: ")]
    [SerializeField] Button _graphicsButton;
    [SerializeField] Button _audioButton;
    [SerializeField] Button _keybindsButton;

    [Header("Settings Menus: ")]
    public GameObject settingsMenus;
    [SerializeField] GameObject _chosenMenu;

    [Header("Booleans: ")]
    public bool isInSettings;

    public static SettingsMenusManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            var canvas = transform.parent.GetComponent<Canvas>();
            canvas.sortingOrder = 50;
            canvas.worldCamera = Camera.main;

            _chosenMenu = _keybindsMenu;

            _graphicsButton.onClick.AddListener(() => ChangeChosenMenu(_graphicsMenu));
            _audioButton.onClick.AddListener(() => ChangeChosenMenu(_audioMenu));
            _keybindsButton.onClick.AddListener(() => ChangeChosenMenu(_keybindsMenu));
        }
        else
        {
            Destroy(transform.parent.parent.gameObject);
        }
        // Prevents the settings and sound managers from being destroyed on scene change
        DontDestroyOnLoad(transform.parent.parent);
    }

    private void Start()
    {
        {
            _graphicsMenu.SetActive(false);
            _audioMenu.SetActive(false);
            _keybindsMenu.SetActive(true);

            gameObject.SetActive(false);
        }
    }

    public void OpenSettings(string actionMapToSwitchTo)
    {
        Debug.Log("Attempting to switch action map to: " + actionMapToSwitchTo);
        _playerInput.SwitchCurrentActionMap(actionMapToSwitchTo);
        Debug.Log("Switched action map to: " + _playerInput.currentActionMap.name);
        isInSettings = true;
        settingsMenus.SetActive(true);
    }

    public void ChangeChosenMenu(GameObject newMenu)
    {
        if (newMenu == _chosenMenu)
        {
            return;
        }

        if (_chosenMenu != null)
        {
            _chosenMenu.SetActive(false);
        }

        _chosenMenu = newMenu;
        _chosenMenu.SetActive(true);
    }

    public void DeactivateSettingsMenus(string actionMapToSwitchTo)
    {
        Debug.Log("Attempting to switch action map to: " + actionMapToSwitchTo);
        _playerInput.SwitchCurrentActionMap(actionMapToSwitchTo);
        Debug.Log("Switched action map to: " + _playerInput.currentActionMap.name);

        settingsMenus.SetActive(false);
        isInSettings = false;
    }
}
