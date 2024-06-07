using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;

    [SerializeField] private PlayerInput _playerInput;

    [Header("Menu objets: ")]
    [SerializeField] GameObject _pauseMenu;
    [SerializeField] Image _pauseMenuBackgroundImage;

    ConditionsManager _conditionsManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _conditionsManager = ConditionsManager.instance;
    }

    private void OnApplicationPause(bool pause) //Pauses the game on Application quit
    {
        if (_conditionsManager != null)
        {
            if (pause && _playerInput.actions.FindActionMap("InGame").enabled && (!_conditionsManager.gameHasEnded || !_conditionsManager.hasDied))
            {
                PauseGame();
            }
        }
    }

    public void PauseGame() //Pauses the game
    {
        _playerInput.SwitchCurrentActionMap("Pause");
        GetComponent<ControllerMenus>().SelectFirstButton();
        Cursor.visible = true;
        _pauseMenu.SetActive(true);
        _pauseMenuBackgroundImage.enabled = true;
        _conditionsManager.isPaused = true;

        Time.timeScale = 0f;
    }

    public void UnpauseGame() //Unpauses the game
    {
        _playerInput.SwitchCurrentActionMap("InGame");
        _pauseMenu.SetActive(false);
        _pauseMenuBackgroundImage.enabled = false;
        _conditionsManager.isPaused = false;

        Time.timeScale = 1f;
    }
}
