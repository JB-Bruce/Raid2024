using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UserInput : MonoBehaviour
{
    public static UserInput instance;

    public Vector2 MoveInput {  get; private set; }

    public bool Interract {  get; private set; }
    public bool Sprint { get; private set; }
    public bool OpenCloseInventory { get; private set; }
    public bool OpenCloseMap { get; private set; }
    public bool SelectWeapon1 { get; private set; }
    public bool SelectWeapon2 { get; private set; }
    public bool SelectWeapon3 { get; private set; }
    public bool SelectWeaponLeft { get; private set; }
    public bool SelectWeaponRight { get; private set; }
    public bool AimWeapon { get; private set; }
    public bool WeaponHit { get; private set; }

    private PlayerInput _playerInput;

    private InputAction _moveAction;
    private InputAction _interractAction;
    private InputAction _sprintAction;
    private InputAction _openCloseInvAction;
    private InputAction _openCloseMapAction;
    private InputAction _selectWeapon1Action;
    private InputAction _selectWeapon2Action;
    private InputAction _selectWeapon3Action;
    private InputAction _selectWeaponLeftAction;
    private InputAction _selectWeaponRightAction;
    private InputAction _AimWeaponAction;
    private InputAction _weaponHitAction;

    private bool _canPlay = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        _playerInput = GetComponent<PlayerInput>();
        if (SceneManager.GetActiveScene().name != "Tutorial")
        {
            StartCoroutine(CanPlay());
        }
        else
        {
            SetupInputActions();
            _canPlay = true;
        }

    }

    private void Update()
    {
        UpdateInputs();
    }

    private void SetupInputActions()
    {
        _moveAction = _playerInput.actions["Movement"];
        _interractAction = _playerInput.actions["Interract"];
        _sprintAction = _playerInput.actions["Sprint"];
        _openCloseInvAction = _playerInput.actions["Open Inventory"];
        _openCloseMapAction = _playerInput.actions["Open Map"];
        _selectWeapon1Action = _playerInput.actions["SelectFirstWeapon"];
        _selectWeapon2Action = _playerInput.actions["SelectSecondWeapon"];
        _selectWeapon3Action = _playerInput.actions["SelectThirdWeapon"];
        _selectWeaponLeftAction = _playerInput.actions["SelectLeftWeapon"];
        _selectWeaponRightAction = _playerInput.actions["SelectRightWeapon"];
        _AimWeaponAction = _playerInput.actions["AimWeapon"];
        _weaponHitAction = _playerInput.actions["WeaponHit"];
    }

    private void UpdateInputs()
    {
        if(!_canPlay) { return; }

        MoveInput = _moveAction.ReadValue<Vector2>();
        Interract = _interractAction.WasPressedThisFrame();
        Sprint = _sprintAction.IsPressed();
        OpenCloseInventory = _openCloseInvAction.WasPressedThisFrame();
        OpenCloseMap = _openCloseMapAction.WasPressedThisFrame();
        SelectWeapon1 = _selectWeapon1Action.WasPressedThisFrame();
        SelectWeapon2 = _selectWeapon2Action.WasPressedThisFrame();
        SelectWeapon3 = _selectWeapon3Action.WasPressedThisFrame();
        SelectWeaponLeft = _selectWeaponLeftAction.WasPressedThisFrame();
        SelectWeaponRight = _selectWeaponRightAction.WasPressedThisFrame();
        AimWeapon = _AimWeaponAction.IsPressed();
        WeaponHit = _weaponHitAction.WasPressedThisFrame();
    }

    // Set the Action Map to InGame when the scene is load
    private IEnumerator CanPlay() 
    {
        UnityEngine.SceneManagement.Scene loadScene = SceneManager.GetSceneByName("LoadScene"); 
        while( loadScene.name != null ) 
        {
            yield return new WaitForSeconds(0.5f);
        }
        _playerInput.SwitchCurrentActionMap("InGame");
        SetupInputActions();
        _canPlay = true;
    }
}
