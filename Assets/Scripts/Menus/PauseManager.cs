using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;

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

    private void Update() //Temp Test
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (!_conditionsManager.IsAnyMenuOpen())
            {
                PauseGame();
            }
            else if (_conditionsManager.isPaused)
            {
                UnpauseGame();
            }
        }
    }

    private void OnApplicationPause(bool pause) //Pauses the game on Application quit
    {
        if (pause && !_conditionsManager.gameHasEnded || !_conditionsManager.hasDied)
        {
            PauseGame();
        }
    }

    public void PauseGame() //Pauses the game
    {
        _pauseMenu.SetActive(true);
        _pauseMenuBackgroundImage.enabled = true;
        _conditionsManager.isPaused = true;

        Time.timeScale = 0f;
    }

    public void UnpauseGame() //Unpauses the game
    {
        _pauseMenu.SetActive(false);
        _pauseMenuBackgroundImage.enabled = false;
        _conditionsManager.isPaused = false;

        Time.timeScale = 1f;
    }
}
